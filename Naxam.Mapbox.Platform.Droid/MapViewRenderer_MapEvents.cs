﻿using Com.Mapbox.Mapboxsdk.Maps;
using System;
using System.Linq;
using MapView = Com.Mapbox.Mapboxsdk.Maps.MapView;
using Naxam.Controls.Forms;

namespace Naxam.Controls.Platform.Droid
{
    
    public partial class MapViewRenderer : MapView.IOnMapChangedListener
    {
        void AddMapEvents ()
        {
            map.CameraChange += CameraChanged;
            map.MarkerClick += MarkerClicked;
            map.MapClick += MapClicked;
            map.MyLocationChange += MyLocationChanged;

            fragment.OnMapChangedListener = (this);
        }

        void RemoveMapEvents ()
        {
            map.CameraChange -= CameraChanged;
            map.MarkerClick -= MarkerClicked;
            map.MapClick -= MapClicked;
            map.MyLocationChange -= MyLocationChanged;

            fragment.OnMapChangedListener = null;
        }

        void CameraChanged (object o, MapboxMap.CameraChangeEventArgs args)
        {
            currentCamera.Lat = args.P0.Target.Latitude;
            currentCamera.Long = args.P0.Target.Longitude;
            Element.Center = currentCamera;
        }

        void MyLocationChanged (object o, MapboxMap.MyLocationChangeEventArgs args)
        {
            if (Element.UserLocation == null)
                Element.UserLocation = new Position ();

            Element.UserLocation.Lat = args.P0.Latitude;
            Element.UserLocation.Long = args.P0.Longitude;
        }

        void MapClicked (object o, MapboxMap.MapClickEventArgs args)
        {
            Element.IsTouchInMap = false;

            var point = map.Projection.ToScreenLocation (args.P0);
            var xfPoint = new Xamarin.Forms.Point (point.X, point.Y);
            var xfPosition = new Position (args.P0.Latitude, args.P0.Longitude);

            Element.DidTapOnMapCommand?.Execute (new Tuple<Position, Xamarin.Forms.Point> (xfPosition, xfPoint));
        }

        void MarkerClicked (object o, MapboxMap.MarkerClickEventArgs args)
        {
            Element.Center.Lat = args.P0.Position.Latitude;
            Element.Center.Long = args.P0.Position.Longitude;
            Element.IsMarkerClicked = true;

            var annotationKey = _annotationDictionaries.FirstOrDefault (x => x.Value == args.P0).Key;

            if (Element.CanShowCalloutChecker?.Invoke (annotationKey) == true) {
                args.P0.ShowInfoWindow (map, fragment.View as MapView);
            }
        }

        public void OnMapChanged (int p0)
        {
            switch (p0) {
            case MapView.DidFinishLoadingStyle:
                var mapStyle = Element.MapStyle;
                if (mapStyle == null
                    || (!string.IsNullOrEmpty (map.StyleUrl) && mapStyle.UrlString != map.StyleUrl)) {
                    mapStyle = new MapStyle (map.StyleUrl);
                    Element.MapStyle = mapStyle;
                }
                    Element.MapStyle.OriginalLayers = map.Layers.Select((arg) =>
                                                                        new Layer(arg.Id)
                                                                       ).ToArray();
                Element.DidFinishLoadingStyleCommand?.Execute (mapStyle);
                break;
            case MapView.DidFinishRenderingMap:
                Element.DidFinishRenderingCommand?.Execute (false);
                break;
            case MapView.DidFinishRenderingMapFullyRendered:
                Element.DidFinishRenderingCommand?.Execute (true);
                break;
            case MapView.RegionDidChange:
                Element.RegionDidChangeCommand?.Execute (false);
                break;
            case MapView.RegionDidChangeAnimated:
                Element.RegionDidChangeCommand?.Execute (true);
                break;
            default:
                break;
            }
        }
    }
}