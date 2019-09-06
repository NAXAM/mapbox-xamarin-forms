using System;
using System.Collections.Generic;
using Foundation;
using Mapbox;
using Naxam.Mapbox.Platform.iOS;
using Naxam.Controls.Forms;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Diagnostics;
using Naxam.Mapbox.Annotations;
using System.Collections.Specialized;
using Naxam.Mapbox;
using System.Linq;
using Naxam.Mapbox.Platform.iOS.Extensions;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer
    {
        void AddAnnotation(Annotation annotation)
        {
            var shape = ShapeFromAnnotation(annotation);
            if (shape != null)
            {
                map.AddAnnotation(shape);
                annotation.Id = shape.Handle.ToInt64().ToString();
            }
        }

        void AddAnnotations(Annotation[] annotations)
        {
            var annots = new List<MGLShape>();
            foreach (Annotation at in annotations)
            {
                var shape = ShapeFromAnnotation(at);
                if (shape == null)
                {
                    continue;
                }
                annots.Add(shape);
            }
            map.AddAnnotations(annots.ToArray());
            for (int i = 0; i < annots.Count; i++)
            {
                annotations[i].Id = annots[i].Id();
            }
        }

        [Export("mapView:calloutViewForAnnotation:")]
        public IMGLCalloutView MapView_CalloutViewForAnnotation(MGLMapView mapView, IMGLAnnotation annotation)
        {
            var id = annotation.Handle;
            if (mapView.Annotations != null)
            {
                var bindingContext = Element.Annotations.FirstOrDefault(a => a.NativeHandle == id);
                UIView calloutContent = Element.InfoWindowTemplate.DataTemplateToNativeView(bindingContext, Element);
                return new MGLCustomCalloutView(null, calloutContent);
            }

            return null;
        }

        [Export("mapView:viewForAnnotation:")]
        public MGLAnnotationView MapView_ViewForAnnotation(MGLMapView mapView, IMGLAnnotation annotation)
        {
            var fannotation = Element.Annotations.FirstOrDefault(x => x.NativeHandle == annotation.Handle);

            switch (fannotation)
            {
                case SymbolAnnotation symbol:
                    if (symbol.IconImage?.Source != null)
                    {
                        return null;
                    }
                    break;
            }

            var annotationView = mapView.DequeueReusableAnnotationViewWithIdentifier("draggablePoint");
            if (annotationView != null) return annotationView;
            var view = new DraggableAnnotationView(reuseIdentifier: "draggablePoint", size: 24);
            view.DragFinished += (sender, e) =>
            {
                var point = new SymbolAnnotation();
                point.NativeHandle = annotation.Handle;
                point.Coordinates = annotation.Coordinate.ToLatLng();
                Element.DragFinishedCommand?.Execute(point);
            };

            return view;
        }

        [Export("mapView:imageForAnnotation:")]
        public MGLAnnotationImage MapView_ImageForAnnotation(MGLMapView mapView, IMGLAnnotation annotation)
        {
            var fannotation = Element.Annotations.FirstOrDefault(x => x.NativeHandle == annotation.Handle);

            switch (fannotation)
            {
                case SymbolAnnotation symbol:
                    switch (symbol.IconImage.Source)
                    {
                        case FileImageSource fileImageSource:
                            var cachedImage = mapView.DequeueReusableAnnotationImageWithIdentifier(fileImageSource.File);

                            if (cachedImage != null) return cachedImage;

                            var fileImageSourceHandler = new FileImageSourceHandler();
                            var image = fileImageSourceHandler.LoadImageAsync(fileImageSource).Result;

                            if (image == null) return null;

                            return MGLAnnotationImage.AnnotationImageWithImage(image, fileImageSource.File);
                    }
                    break;
            }

            return null;
        }

        void RemoveAnnotations(Annotation[] annotations)
        {
            var currentAnnotations = map.Annotations;
            if (currentAnnotations == null)
            {
                return;
            }
            var annots = new List<MGLShape>();
            foreach (Annotation at in annotations)
            {
                foreach (NSObject curAnnot in currentAnnotations)
                {
                    if (curAnnot is MGLShape shape)
                    {
                        if (string.IsNullOrEmpty(shape.Id()))
                        {
                            continue;
                        }
                        if (shape.Id() == at.Id)
                        {
                            annots.Add(shape);
                        }
                    }
                }
            }
            map.RemoveAnnotations(annots.ToArray());
        }

        void RemoveAllAnnotations()
        {
            if (map == null || map.Annotations == null) return;

            map.RemoveAnnotations(map.Annotations);
        }

        private void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var annotations = new List<MGLShape>();
                foreach (Annotation annotation in e.NewItems)
                {
                    var shape = ShapeFromAnnotation(annotation);
                    if (shape == null) continue;

                    annotations.Add(shape);
                }
                map.AddAnnotations(annotations.ToArray());
                for (int i = 0; i < annotations.Count; i++)
                {
                    if (e.NewItems[i] is Annotation an)
                    {
                        an.Id = annotations[i].Id();
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var items = new List<Annotation>();
                foreach (Annotation annot in e.OldItems)
                {
                    items.Add(annot);
                }
                RemoveAnnotations(items.ToArray());
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset) //The content of the collection was cleared.
            {
                RemoveAllAnnotations();
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var itemsToRemove = new List<Annotation>();
                foreach (Annotation annotation in e.OldItems)
                {
                    itemsToRemove.Add(annotation);
                }
                RemoveAnnotations(itemsToRemove.ToArray());
                var annots = new List<MGLShape>();
                foreach (Annotation annotation in e.NewItems)
                {
                    var shape = ShapeFromAnnotation(annotation);
                    if (shape != null)
                    {
                        annots.Add(shape);
                    }
                }
                map.AddAnnotations(annots.ToArray());
                for (int i = 0; i < annots.Count; i++)
                {
                    if (e.NewItems[i] is Annotation an)
                    {
                        an.Id = annots[i].Handle.ToString();
                    }
                }
            }
        }

        MGLShape ShapeFromAnnotation(Annotation annotation)
        {
            MGLShape shape = null;
            if (annotation is SymbolAnnotation symbol)
            {
                shape = new MGLPointAnnotation
                {
                    Coordinate = symbol.Coordinates.ToCLCoordinate(),
                };
            }
            else if (annotation is LineAnnotation line)
            {
                shape = PolyLineWithCoordinates(line.Coordinates.ToArray());
                //if (polyline.Coordinates is INotifyCollectionChanged notifiyCollection)
                //{
                //    notifiyCollection.CollectionChanged += (sender, e) =>
                //    {
                //        //TODO Move to a separated method
                //        if (e.Action == NotifyCollectionChangedAction.Add)
                //        {
                //            foreach (FormsMB.Point pos in e.NewItems)
                //            {
                //                var coord = TypeConverter.FromPositionToCoordinate(pos);
                //                (shape as MGLPolyline).AppendCoordinates(ref coord, 1);
                //            }
                //        }
                //        else if (e.Action == NotifyCollectionChangedAction.Remove)
                //        {
                //            (shape as MGLPolyline).RemoveCoordinatesInRange(new NSRange(e.OldStartingIndex, e.OldItems.Count));
                //        }
                //    };
                //}

            }
            else if (annotation is FillAnnotation polyline)
            {
                //if (polyline.Coordinates == null || polyline.Coordinates.Length == 0)
                //{
                //    return null;
                //}
                //var lines = new MGLPolyline[polyline.Coordinates.Length];
                //for (var i = 0; i < polyline.Coordinates.Length; i++)
                //{
                //    if (polyline.Coordinates[i].Length == 0)
                //    {
                //        continue;
                //    }
                //    lines[i] = PolyLineWithCoordinates(polyline.Coordinates[i]);
                //}
                //shape = MGLMultiPolyline.MultiPolylineWithPolylines(lines);
            }

            if (shape != null)
            {
                if (annotation.Title != null)
                {
                    shape.Title = annotation.Title;
                }
                if (annotation.SubTitle != null)
                {
                    shape.Subtitle = annotation.SubTitle;
                }
                if (!string.IsNullOrEmpty(annotation.Id))
                {
                    shape.SetId(annotation.Id);
                }

                annotation.NativeHandle = shape.Handle;
            }

            return shape;
        }

        MGLPolyline PolyLineWithCoordinates(LatLng[] positions)
        {
            if (positions == null || positions.Length == 0)
            {
                return null;
            }
            var first = positions[0].ToCLCoordinate();
            var output = MGLPolyline.PolylineWithCoordinates(ref first, 1);
            var i = 1;
            while (i < positions.Length)
            {
                var coord = positions[i].ToCLCoordinate();
                output.AppendCoordinates(ref coord, 1);
                i++;
            }
            return output;
        }

        [Export("mapView:annotationCanShowCallout:"),]
        protected virtual bool AnnotationCanShowCallout(MGLMapView mapView, NSObject annotation)
        {
            if (annotation is MGLShape && Element.CanShowCalloutChecker != null)
            {
                return Element.CanShowCalloutChecker.Invoke(((MGLShape)annotation).Id());
            }
            return true;
        }

        [Export("mapView:tapOnCalloutForAnnotation:")]
        protected virtual void MapView_TapOnCalloutForAnnotation(MGLMapView mapView, NSObject annotation)
        {
            if (annotation is MGLShape shape)
            {
                Element.DidTapOnCalloutViewCommand?.Execute(shape.Id());
            }
            else
            {
                Element.DidTapOnCalloutViewCommand?.Execute(null);
            }
        }

    }
}
