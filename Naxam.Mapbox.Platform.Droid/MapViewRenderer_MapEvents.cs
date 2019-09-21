using System;
using System.Linq;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Maps;
using Naxam.Controls.Forms;
using NxLatLng = Naxam.Mapbox.LatLng;

namespace Naxam.Controls.Mapbox.Platform.Droid
{

    public partial class MapViewRenderer : MapboxMap.IOnMapClickListener
    {
        bool cameraBusy;
        protected virtual void AddMapEvents()
        {
            //map.MarkerClick += MarkerClicked;
            //map.InfoWindowClick += InfoWindowClick;
            //map.MapClick += MapClicked;
            map.AddOnMapClickListener(this);
            map.CameraIdle += OnCameraIdle;
            map.CameraMoveStarted += Map_CameraMoveStarted;
            map.CameraMoveCancel += Map_CameraMoveCancel;
            map.CameraMove += Map_CameraMove;
        }

        protected virtual void RemoveMapEvents()
        {
            if (map != null)
            {
                map.MarkerClick -= MarkerClicked;
                map.InfoWindowClick -= InfoWindowClick;
                map.RemoveOnMapClickListener(this);
                map.CameraIdle -= OnCameraIdle;
                map.CameraMoveStarted -= Map_CameraMoveStarted;
                map.CameraMoveCancel -= Map_CameraMoveCancel;
                map.CameraMove -= Map_CameraMove;
            }
        }

        private void Map_CameraMove(object sender, EventArgs e)
        {
            cameraBusy = true;
        }

        private void Map_CameraMoveCancel(object sender, EventArgs e)
        {
            cameraBusy = false;
        }

        private void Map_CameraMoveStarted(object sender, MapboxMap.CameraMoveStartedEventArgs e)
        {
            cameraBusy = true;
        }

        private void CameraChange()
        {
            if (map?.SelectedMarkers.Count > 0)
                map.DeselectMarkers();
        }
        private void OnCameraIdle(object sender, EventArgs e)
        {
            cameraBusy = false;
            currentCamera.Lat = map.CameraPosition.Target.Latitude;
            currentCamera.Long = map.CameraPosition.Target.Longitude;
            Element.ZoomLevel = map.CameraPosition.Zoom;
            Element.Center = currentCamera;
        }

        void MarkerClicked(object o, MapboxMap.MarkerClickEventArgs args)
        {
            fragment?.ToggleInfoWindow(map, args.P0);

            if (Element?.Annotations?.Count() > 0)
            {
                var fm = Element.Annotations.FirstOrDefault(d => d.Id == args.P0.Id.ToString());
                if (fm == null)
                    return;
                Element.DidTapOnMarkerCommand?.Execute(fm);
            }
        }

        void InfoWindowClick(object s, MapboxMap.InfoWindowClickEventArgs e)
        {
            if (e.P0 != null)
            {
                Element.DidTapOnCalloutViewCommand?.Execute(e.P0.Id.ToString());
            }
        }

        public bool OnMapClick(LatLng p0)
        {
            var point = map.Projection.ToScreenLocation(p0);
            var xfPoint = new Xamarin.Forms.Point(point.X, point.Y);
            var xfPosition = new NxLatLng(p0.Latitude, p0.Longitude);
            (NxLatLng, Xamarin.Forms.Point) commandParamters = (xfPosition, xfPoint);

            if (Element.DidTapOnMapCommand?.CanExecute(commandParamters) != false)
            {
                Element.DidTapOnMapCommand?.Execute(commandParamters);
            }

            // TODO should return true
            return false;
        }

        //public virtual void OnMapChanged(int p0)
        //{
        //    switch (p0)
        //    {
        //        case MapView.DidFinishLoadingStyle:
        //            var mapStyle = Element.MapStyle;
        //            if (mapStyle == null
        //                || (!string.IsNullOrEmpty(map.StyleUrl) && mapStyle.UrlString != map.StyleUrl))
        //            {
        //                mapStyle = new MapStyle(map.StyleUrl);

        //            }
        //            if (mapStyle.CustomSources != null)
        //            {
        //                var notifiyCollection = Element.MapStyle.CustomSources as INotifyCollectionChanged;
        //                if (notifiyCollection != null)
        //                {
        //                    notifiyCollection.CollectionChanged += OnShapeSourcesCollectionChanged;
        //                }

        //                AddSources(Element.MapStyle.CustomSources.ToList());
        //            }
        //            if (mapStyle.CustomLayers != null)
        //            {
        //                if (Element.MapStyle.CustomLayers is INotifyCollectionChanged notifiyCollection)
        //                {
        //                    notifiyCollection.CollectionChanged += OnLayersCollectionChanged;
        //                }

        //                AddLayers(Element.MapStyle.CustomLayers.ToList());
        //            }
        //            mapStyle.OriginalLayers = map.Layers.Select((arg) =>
        //                                                                new Layer(arg.Id)
        //                                                               ).ToArray();
        //            Element.MapStyle = mapStyle;
        //            Element.DidFinishLoadingStyleCommand?.Execute(mapStyle);
        //            break;
        //        case MapView.DidFinishRenderingMap:
        //            Element.Center = new Position(map.CameraPosition.Target.Latitude, map.CameraPosition.Target.Longitude);
        //            Element.DidFinishRenderingCommand?.Execute(false);
        //            break;
        //        case MapView.DidFinishRenderingMapFullyRendered:
        //            Element.DidFinishRenderingCommand?.Execute(true);
        //            break;
        //        case MapView.RegionDidChange:
        //            Element.RegionDidChangeCommand?.Execute(false);
        //            break;
        //        case MapView.RegionDidChangeAnimated:
        //            Element.RegionDidChangeCommand?.Execute(true);
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}