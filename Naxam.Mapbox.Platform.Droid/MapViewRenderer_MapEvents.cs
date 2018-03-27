using System;
using System.Collections.Specialized;
using System.Linq;
using Com.Mapbox.Mapboxsdk.Maps;
using Naxam.Controls.Mapbox.Forms;
using MapView = Com.Mapbox.Mapboxsdk.Maps.MapView;

namespace Naxam.Controls.Mapbox.Platform.Droid
{

    public partial class MapViewRenderer : MapView.IOnMapChangedListener
    {
        void AddMapEvents ()
        {
            map.MarkerClick += MarkerClicked;
            map.MapClick += MapClicked;
            map.MyLocationChange += MyLocationChanged;
            map.CameraIdle += OnCameraIdle;

            fragment.OnMapChangedListener = (this);
        }

 
        void RemoveMapEvents ()
        {
            map.MarkerClick -= MarkerClicked;
            map.MapClick -= MapClicked;
            map.MyLocationChange -= MyLocationChanged;
            map.CameraIdle -= OnCameraIdle;

            fragment.OnMapChangedListener = null;
        }

		private void OnCameraIdle(object sender, EventArgs e)
		{
            currentCamera.Lat = map.CameraPosition.Target.Latitude;
            currentCamera.Long = map.CameraPosition.Target.Longitude;
			Element.ZoomLevel = map.CameraPosition.Zoom;
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
            Element.FocusPosition = false;

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
                   
                }
                    if (mapStyle.CustomSources != null)
					{
						var notifiyCollection = Element.MapStyle.CustomSources as INotifyCollectionChanged;
						if (notifiyCollection != null)
						{
							notifiyCollection.CollectionChanged += OnShapeSourcesCollectionChanged;
						}

						AddSources(Element.MapStyle.CustomSources.ToList());
					}
                    if (mapStyle.CustomLayers != null)
					{
						if (Element.MapStyle.CustomLayers is INotifyCollectionChanged notifiyCollection)
						{
							notifiyCollection.CollectionChanged += OnLayersCollectionChanged;
						}

						AddLayers(Element.MapStyle.CustomLayers.ToList());
					}
                    mapStyle.OriginalLayers = map.Layers.Select((arg) =>
                                                                        new Layer(arg.Id) 
                                                                       ).ToArray();
					Element.MapStyle = mapStyle;
                    Element.DidFinishLoadingStyleCommand?.Execute(mapStyle);
                break;
            case MapView.DidFinishRenderingMap:
					Element.Center = new Position(map.CameraPosition.Target.Latitude, map.CameraPosition.Target.Longitude);
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