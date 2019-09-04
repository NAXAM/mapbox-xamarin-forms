using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.App;
using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Maps;
using Java.Util;
using Naxam.Controls.Mapbox.Forms;
using Naxam.Controls.Mapbox.Platform.Droid;
using Naxam.Mapbox.Platform.Droid;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Com.Mapbox.Mapboxsdk.Maps.MapboxMap;
using Annotation = Naxam.Controls.Mapbox.Forms.Annotation;
using Bitmap = Android.Graphics.Bitmap;
using MapView = Naxam.Controls.Mapbox.Forms.MapView;
using Point = Xamarin.Forms.Point;
using Sdk = Com.Mapbox.Mapboxsdk;
using View = Android.Views.View;
using FAnnotation = Naxam.Controls.Mapbox.Forms.Annotation;

using FMarker = Naxam.Controls.Mapbox.Forms.PointAnnotation;
using MMarker = Com.Mapbox.Mapboxsdk.Annotations.MarkerOptions;

using FPolyline = Naxam.Controls.Mapbox.Forms.PolylineAnnotation;
using MPolyline = Com.Mapbox.Mapboxsdk.Annotations.PolylineOptions;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer
    {

        private void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddAnnotations(e.NewItems.Cast<FAnnotation>().ToArray());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveAnnotations(e.OldItems.Cast<FAnnotation>().ToArray());
                    break;
                case NotifyCollectionChangedAction.Reset:
                    map.RemoveAnnotations();
                    _annotationDictionaries.Clear();
                    AddAnnotations(Element.Annotations.ToList());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    var itemsToRemove = new List<Annotation>();
                    foreach (Annotation annotation in e.OldItems)
                    {
                        itemsToRemove.Add(annotation);
                    }
                    RemoveAnnotations(itemsToRemove.ToArray());

                    var itemsToAdd = new List<Annotation>();
                    foreach (Annotation annotation in e.NewItems)
                    {
                        itemsToRemove.Add(annotation);
                    }
                    AddAnnotations(itemsToAdd.ToArray());
                    break;
            }
        }

        void Element_AnnotationChanged(object sender, AnnotationChangeEventArgs e)
        {
            if (e.OldAnnotation is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= OnAnnotationsCollectionChanged;
            }

            if (e.NewAnnotation is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += OnAnnotationsCollectionChanged;
            }

            if (mapReady)
            {
                RemoveAllAnnotations();
                AddAnnotations(Element?.Annotations?.ToArray());
            }
        }

        void RemoveAnnotations(IList<FAnnotation> annotations)
        {
            var currentAnnotations = map.Annotations.Where(d => annotations.Any(annotation => annotation.Id == d.Id.ToString()));
            if (currentAnnotations == null || currentAnnotations.Count() == 0)
            {
                return;
            }
            for (int i = 0; i < annotations.Count(); i++)
            {
                _annotationDictionaries.Remove(annotations[i].Id);
            }

            map.RemoveAnnotations(currentAnnotations.ToList());
        }

        void AddAnnotations(IList<Annotation> annotations)
        {
            if (map == null)
                return;
            AddMakers(annotations.Where(d => d is FMarker).Cast<FMarker>().ToList());
            AddPolylines(annotations.Where(d => d is FPolyline).Cast<FPolyline>().ToList());
        }

        IList<Marker> AddMakers(IList<FMarker> markers)
        {
            if (markers == null || markers.Count == 0)
                return null;
            var iconSource = new Dictionary<string, Icon>();
            var markerOptions = new List<MMarker>();
            for (int i = 0; i < markers.Count; i++)
            {
                var annotation = markers[i];
                var marker = new MarkerOptions();
                marker.SetTitle(annotation.Title);
                marker.SetSnippet(annotation.SubTitle);
                marker.SetPosition(annotation.Coordinate.ToLatLng());
                if (string.IsNullOrEmpty(annotation.Icon) == false)
                {
                    if (iconSource.ContainsKey(annotation.Icon))
                    {
                        marker.SetIcon(iconSource[annotation.Icon]);
                    }
                    else
                    {
                        var icon = Context.GetIconFromResource(annotation.Icon);
                        if (icon != null)
                        {
                            marker.SetIcon(icon);
                            iconSource.Add(annotation.Icon, icon);
                        }
                    }
                }

                markerOptions.Add(marker);
            }
            var options = map.AddMarkers(markerOptions.Cast<BaseMarkerOptions>().ToList());
            for (int i = 0; i < options.Count; i++)
            {
                markers[i].Id = options[i].Id.ToString();
            }
            return options;
        }

        IList<Polyline> AddPolylines(IList<FPolyline> polylines)
        {
            if (polylines == null || polylines.Count == 0)
                return null;
            var polylineOptions = new List<MPolyline>();
            for (int i = 0; i < polylines.Count; i++)
            {
                var polyline = polylines[i];
                if (polyline.Coordinates == null || polyline.Coordinates.Count() == 0)
                {
                    continue;
                }
                var notifyCollection = polyline.Coordinates as INotifyCollectionChanged;

                var coords = new ArrayList(polyline.Coordinates.Select(d => d.ToLatLng()).ToArray());
                var polylineOpt = new PolylineOptions();
                var width = polyline.Width == 0 ? 1 : polyline.Width;
                polylineOpt.Polyline.Width = Context.ToPixels(width);
                var color = string.IsNullOrEmpty(polyline.HexColor) ? Android.Graphics.Color.Red : Android.Graphics.Color.ParseColor(polyline.HexColor);
                polylineOpt.Polyline.Color = color;
                polylineOpt.AddAll(coords);
                polylineOptions.Add(polylineOpt);

                if (notifyCollection != null)
                {
                    notifyCollection.CollectionChanged += (s, e) =>
                    {
                        switch (e.Action)
                        {
                            case NotifyCollectionChangedAction.Remove:
                                if (_annotationDictionaries.ContainsKey(polyline.Id))
                                {
                                    var poly = _annotationDictionaries[polyline.Id] as Polyline;
                                    poly.Points.Remove(polyline.Coordinates.ElementAt(e.OldStartingIndex).ToLatLng());
                                }
                                break;
                            case NotifyCollectionChangedAction.Add:
                                if (_annotationDictionaries.ContainsKey(polyline.Id))
                                {
                                    var poly = _annotationDictionaries[polyline.Id] as Polyline;
                                    poly.AddPoint(polyline.Coordinates.ElementAt(e.NewStartingIndex).ToLatLng());
                                }
                                break;
                            case NotifyCollectionChangedAction.Reset:
                                if (_annotationDictionaries.ContainsKey(polyline.Id))
                                {
                                    var poly = _annotationDictionaries[polyline.Id] as Polyline;
                                    poly.Points = polyline.Coordinates.Select(d => d.ToLatLng()).ToList();
                                }
                                break;
                        }
                    };

                }
            }
            var options = map.AddPolylines(polylineOptions.ToList());
            for (int i = 0; i < options.Count; i++)
            {
                polylines[i].Id = options[i].Id.ToString();
            }
            return options;
        }

        void RemoveAllAnnotations()
        {
            if (map?.Annotations != null)
            {
                map.RemoveAnnotations(map.Annotations);
            }
        }

    }
}