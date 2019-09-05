using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Plugins.Annotation;
using Naxam.Controls.Forms;
using Naxam.Mapbox.Annotations;
using NxAnnotation = Naxam.Mapbox.Annotations.Annotation;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer
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
                            IntPtr handleId = IntPtr.Zero;
                            try
                            {
                                handleId = Marshal.StringToHGlobalUni(symbolAnnotation.NativeHandle);
                                var symbol = new Java.Lang.Object(
                                    handleId,
                                    Android.Runtime.JniHandleOwnership.DoNotTransfer
                                    );
                                symbolManager.Delete(symbol);
                            }
                            finally
                            {
                                Marshal.FreeHGlobal(handleId);
                            }
                        }
                        break;
                    case CircleAnnotation circleAnnotation:
                        {
                            if (circleManager == null) continue;
                            IntPtr handleId = IntPtr.Zero;
                            try
                            {
                                handleId = Marshal.StringToHGlobalUni(circleAnnotation.NativeHandle);
                                var symbol = new Java.Lang.Object(
                                    handleId,
                                    Android.Runtime.JniHandleOwnership.DoNotTransfer
                                    );
                                circleManager.Delete(symbol);
                            }
                            finally
                            {
                                Marshal.FreeHGlobal(handleId);
                            }
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
                                symbolManager.IconAllowOverlap = Java.Lang.Boolean.True;
                                symbolManager.TextAllowOverlap = Java.Lang.Boolean.True;
                            }
                            var symbolOptions = symbolAnnotation.ToSymbolOptions();
                            var symbol = Android.Runtime.Extensions.JavaCast<Symbol>(symbolManager.Create(symbolOptions));
                            symbolAnnotation.NativeHandle = symbol.Handle;
                            symbolAnnotation.Id = symbol.Id.ToString();
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

        //IList<Marker> AddMakers(IList<FMarker> markers)
        //{
        //    if (markers == null || markers.Count == 0)
        //        return null;
        //    var iconSource = new Dictionary<string, Icon>();
        //    var markerOptions = new List<MMarker>();
        //    for (int i = 0; i < markers.Count; i++)
        //    {
        //        var annotation = markers[i];
        //        var marker = new MarkerOptions();
        //        marker.SetTitle(annotation.Title);
        //        marker.SetSnippet(annotation.SubTitle);
        //        marker.SetPosition(annotation.Coordinate.ToLatLng());
        //        if (string.IsNullOrEmpty(annotation.Icon) == false)
        //        {
        //            if (iconSource.ContainsKey(annotation.Icon))
        //            {
        //                marker.SetIcon(iconSource[annotation.Icon]);
        //            }
        //            else
        //            {
        //                var icon = Context.GetIconFromResource(annotation.Icon);
        //                if (icon != null)
        //                {
        //                    marker.SetIcon(icon);
        //                    iconSource.Add(annotation.Icon, icon);
        //                }
        //            }
        //        }

        //        markerOptions.Add(marker);
        //    }
        //    var options = map.AddMarkers(markerOptions.Cast<BaseMarkerOptions>().ToList());
        //    for (int i = 0; i < options.Count; i++)
        //    {
        //        markers[i].Id = options[i].Id.ToString();
        //    }
        //    return options;
        //}

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

    }
}