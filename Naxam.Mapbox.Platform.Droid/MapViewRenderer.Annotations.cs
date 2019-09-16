using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Plugins.Annotation;
using Com.Mapbox.Mapboxsdk.Utils;
using Java.Interop;
using Java.Lang;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Annotations;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NxAnnotation = Naxam.Mapbox.Annotations.Annotation;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer : IOnSymbolClickListener
    {
        SymbolManager symbolManager;
        CircleManager circleManager;

        private void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddAnnotations(e.NewItems.Cast<NxAnnotation>().ToArray());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveAnnotations(e.OldItems.Cast<NxAnnotation>().ToArray());
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveAllAnnotations();
                    AddAnnotations(Element.Annotations.ToList());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    var itemsToRemove = new List<NxAnnotation>();
                    foreach (NxAnnotation annotation in e.OldItems)
                    {
                        itemsToRemove.Add(annotation);
                    }
                    RemoveAnnotations(itemsToRemove.ToArray());

                    var itemsToAdd = new List<NxAnnotation>();
                    foreach (NxAnnotation annotation in e.NewItems)
                    {
                        itemsToRemove.Add(annotation);
                    }
                    AddAnnotations(itemsToAdd.ToArray());
                    break;
            }
        }

        void Element_AnnotationsChanged(object sender, AnnotationChangedEventArgs e)
        {
            if (mapReady)
            {
                RemoveAllAnnotations();
                AddAnnotations(Element?.Annotations?.ToArray());
            }

            if (e.OldAnnotations is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= OnAnnotationsCollectionChanged;
            }

            if (e.NewAnnotations is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += OnAnnotationsCollectionChanged;
            }
        }

        void RemoveAnnotations(IList<NxAnnotation> annotations)
        {
            if (map == null)
                return;

            for (int i = 0; i < annotations.Count; i++)
            {
                switch (annotations[i])
                {
                    case SymbolAnnotation symbolAnnotation:
                        {
                            if (symbolManager == null) continue;
                            Java.Lang.Object symbol = null;
                            try
                            {
                                symbol = new Java.Lang.Object(
                                    symbolAnnotation.NativeHandle,
                                    Android.Runtime.JniHandleOwnership.DoNotTransfer
                                    );
                                symbolManager.Delete(symbol);
                            }
                            finally
                            {
                                symbol?.Dispose();
                            }
                        }
                        break;
                    case CircleAnnotation circleAnnotation:
                        {
                            // TODO
                        }
                        break;
                }
            }
        }

        void AddAnnotations(IList<NxAnnotation> annotations)
        {
            if (map == null)
                return;

            for (int i = 0; i < annotations.Count; i++)
            {
                switch (annotations[i])
                {
                    case SymbolAnnotation symbolAnnotation:
                        {
                            if (symbolManager == null)
                            {
                                symbolManager = new SymbolManager(fragment.MapView, map, mapStyle);

                                // TODO Provide values from Forms
                                symbolManager.IconAllowOverlap = Boolean.True;
                                symbolManager.TextAllowOverlap = Boolean.True;
                                symbolManager.AddClickListener(this);
                            }

                            if (symbolAnnotation.IconImage?.Source != null)
                            {
                                AddStyleImage(symbolAnnotation.IconImage.Source);
                            }

                            var symbolOptions = symbolAnnotation.ToSymbolOptions();
                            var symbol = Android.Runtime.Extensions.JavaCast<Symbol>(symbolManager.Create(symbolOptions));
                            symbolAnnotation.Id = symbol.Id.ToString();
                        }
                        break;
                    case CircleAnnotation circleAnnotation:
                        {
                            // TODO Handle other type of annotation
                        }
                        break;
                }
            }
        }

        IList<Marker> AddMakers(IList<SymbolAnnotation> annotations)
        {
            if (annotations == null || annotations.Count == 0)
                return null;

            var iconSource = new Dictionary<string, Icon>();
            var markerOptions = new List<MarkerOptions>();
            for (int i = 0; i < annotations.Count; i++)
            {
                var annotation = annotations[i];
                var marker = new MarkerOptions();
                marker.SetTitle(annotation.Title);
                marker.SetSnippet(annotation.SubTitle);
                marker.SetPosition(annotation.Coordinates.ToLatLng());
                if (annotation.IconImage?.Source != null)
                {
                    switch (annotation.IconImage.Source)
                    {
                        case FileImageSource fileImageSource:
                            var bitmap = Context.Resources.GetBitmap(fileImageSource.File);
                            if (bitmap == null) continue;
                            var icon = IconFactory.GetInstance(Context).FromBitmap(bitmap);
                            marker.SetIcon(icon);
                            break;
                        default:
                            continue;
                    }
                }

                markerOptions.Add(marker);
            }
            var options = map.AddMarkers(markerOptions.Cast<BaseMarkerOptions>().ToList());
            for (int i = 0; i < options.Count; i++)
            {
                annotations[i].Id = options[i].Id.ToString();
            }
            return options;
        }

        //IList<Polyline> AddPolylines(IList<FPolyline> polylines)
        //{
        //    if (polylines == null || polylines.Count == 0)
        //        return null;
        //    var polylineOptions = new List<MPolyline>();
        //    for (int i = 0; i < polylines.Count; i++)
        //    {
        //        var polyline = polylines[i];
        //        if (polyline.Coordinates == null || polyline.Coordinates.Count() == 0)
        //        {
        //            continue;
        //        }
        //        var notifyCollection = polyline.Coordinates as INotifyCollectionChanged;

        //        var coords = new ArrayList(polyline.Coordinates.Select(d => d.ToLatLng()).ToArray());
        //        var polylineOpt = new PolylineOptions();
        //        var width = polyline.Width == 0 ? 1 : polyline.Width;
        //        polylineOpt.Polyline.Width = Context.ToPixels(width);
        //        var color = string.IsNullOrEmpty(polyline.HexColor) ? Android.Graphics.Color.Red : Android.Graphics.Color.ParseColor(polyline.HexColor);
        //        polylineOpt.Polyline.Color = color;
        //        polylineOpt.AddAll(coords);
        //        polylineOptions.Add(polylineOpt);

        //        if (notifyCollection != null)
        //        {
        //            notifyCollection.CollectionChanged += (s, e) =>
        //            {
        //                switch (e.Action)
        //                {
        //                    case NotifyCollectionChangedAction.Remove:
        //                        if (_annotationDictionaries.ContainsKey(polyline.Id))
        //                        {
        //                            var poly = _annotationDictionaries[polyline.Id] as Polyline;
        //                            poly.Points.Remove(polyline.Coordinates.ElementAt(e.OldStartingIndex).ToLatLng());
        //                        }
        //                        break;
        //                    case NotifyCollectionChangedAction.Add:
        //                        if (_annotationDictionaries.ContainsKey(polyline.Id))
        //                        {
        //                            var poly = _annotationDictionaries[polyline.Id] as Polyline;
        //                            poly.AddPoint(polyline.Coordinates.ElementAt(e.NewStartingIndex).ToLatLng());
        //                        }
        //                        break;
        //                    case NotifyCollectionChangedAction.Reset:
        //                        if (_annotationDictionaries.ContainsKey(polyline.Id))
        //                        {
        //                            var poly = _annotationDictionaries[polyline.Id] as Polyline;
        //                            poly.Points = polyline.Coordinates.Select(d => d.ToLatLng()).ToList();
        //                        }
        //                        break;
        //                }
        //            };

        //        }
        //    }
        //    var options = map.AddPolylines(polylineOptions.ToList());
        //    for (int i = 0; i < options.Count; i++)
        //    {
        //        polylines[i].Id = options[i].Id.ToString();
        //    }
        //    return options;
        //}

        void RemoveAllAnnotations()
        {
            symbolManager?.DeleteAll();
        }

        public void OnAnnotationClick(Symbol symbol)
        {
            if (symbol == null) return;

            if (Element.DidTapOnMarkerCommand?.CanExecute(symbol.Id.ToString()) == true)
            {
                Element.DidTapOnMarkerCommand.Execute(symbol.Id.ToString());
            }
        }
    }

    public partial class MapViewRenderer : IMapFunctions
    {
        public void AddStyleImage(IconImageSource iconImageSource)
        {
            if (iconImageSource?.Source == null) return;

            switch (iconImageSource.Source)
            {
                // TODO: Handle other type of ImageSoure
                case FileImageSource fileImageSource:
                    var cachedImage = mapStyle.GetImage(fileImageSource.File);
                    if (cachedImage != null) break;

                    var id = fileImageSource.File.Split('.').First();
                    var drawable = Context.Resources.GetDrawable(
                        Context.Resources.GetIdentifier(id, "drawable", Context.PackageName),
                        Context.Theme);
                    var bitmap = BitmapUtils.GetBitmapFromDrawable(
                        drawable
                        );

                    if (bitmap == null) break;

                    mapStyle.AddImage(fileImageSource.File, bitmap);
                    iconImageSource.Id = fileImageSource.File;
                    break;
            }
        }
    }
}