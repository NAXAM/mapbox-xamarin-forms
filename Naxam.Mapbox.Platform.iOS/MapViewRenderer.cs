using System;
using CoreGraphics;
using Foundation;
using Mapbox;
using Naxam.Mapbox.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsMap = Naxam.Mapbox.Forms.MapView;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Naxam.Mapbox.Forms.MapView), typeof(Naxam.Mapbox.Platform.iOS.MapViewRenderer))]
namespace Naxam.Mapbox.Platform.iOS
{
	public class MapViewRenderer : ViewRenderer<Naxam.Mapbox.Forms.MapView, MGLMapView>, IMGLMapViewDelegate
	{
		MGLMapView MapView { get; set; }

		protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement != null || Element == null)
			{
				return;
			}

			try
			{
				if (Control == null)
				{
					SetupUserInterface();
					SetupEventHandlers();
					SetNativeControl(MapView);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(@"          ERROR: ", ex.Message);
			}
		}

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

			if (MapView == null)
			{
				return;
			}
			if (e.PropertyName == FormsMap.CenterProperty.PropertyName) {
                UpdateCenter();
            }
			else if (e.PropertyName == FormsMap.ZoomLevelProperty.PropertyName && MapView.ZoomLevel != Element.ZoomLevel) {
				MapView.ZoomLevel = Element.ZoomLevel;
			}
			else if (e.PropertyName == FormsMap.PitchEnabledProperty.PropertyName && MapView.PitchEnabled != Element.PitchEnabled) {
				MapView.PitchEnabled = Element.PitchEnabled;
			}
			else if (e.PropertyName == FormsMap.RotateEnabledProperty.PropertyName && MapView.RotateEnabled != Element.RotateEnabled) {
				MapView.RotateEnabled = Element.RotateEnabled;
			}
			else if (e.PropertyName == FormsMap.StyleUrlProperty.PropertyName 
			         && !string.IsNullOrEmpty(Element.StyleUrl)
			           && (MapView.StyleURL == null 
			               || MapView.StyleURL.AbsoluteString != Element.StyleUrl))
			{
				MapView.StyleURL = new NSUrl(Element.StyleUrl);
			}
        }

		void SetupUserInterface()
		{
			try
			{
				MapView = new MGLMapView(Bounds)
				{
					ShowsUserLocation = true,
					Delegate = this
				};

                MapView.ZoomLevel = Element.ZoomLevel;

                UpdateCenter();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(@"          ERROR: ", ex.Message);
			}
		}

        void UpdateCenter()
        {
			if (Element.Center != null 
			    && (!Element.Center.Lat.Equals(MapView.CenterCoordinate.Latitude) 
			        || !Element.Center.Long.Equals(MapView.CenterCoordinate.Longitude)))
			{
				MapView.SetCenterCoordinate(new CoreLocation.CLLocationCoordinate2D(Element.Center.Lat, Element.Center.Long), true);
			}
        }

        void SetupEventHandlers()
		{
			//Element.CurrentMapStyleChanged += (sender, e) =>
			//{
			//  MapStyle style = (e as MapStyleChangedEventArgs).Style;
			//  if (style != null)
			//  {
			//      if (MapView.Annotations != null)
			//      {
			//          MapView.RemoveAnnotations(MapView.Annotations);
			//      }
			//      MapView.StyleURL = new NSUrl(style.UrlString);
			//      MapView.ReloadStyle(this);
			//      if (style.Center != null && style.Center.Length == 2)
			//      {
			//          MapView.SetCenterCoordinate(new CLLocationCoordinate2D(style.Center[1], style.Center[0]), true);
			//      }
			//  }
			//};


		}

		[Export("mapViewDidFinishRenderingMap:fullyRendered:"),]
		void DidFinishRenderingMap(MGLMapView mapView, bool fullyRendered)
		{
			//Element.DidFinishRenderingCommand?.Execute(fullyRendered);
		}

		[Export("mapView:didUpdateUserLocation:"),]
		void DidUpdateUserLocation(MGLMapView mapView, MGLUserLocation userLocation)
		{
			if (userLocation != null)
			{
				Element.UserLocation = new Position(
					userLocation.Coordinate.Latitude,
					userLocation.Coordinate.Longitude
				);
			}
		}

		[Export("mapView:didFinishLoadingStyle:"),]
		void DidFinishLoadingStyle(MGLMapView mapView, MGLStyle style)
		{
			//var s = new MapStyle()
			//{
			//  Name = style.Name,
			//  Layers = style.Layers.Select(layer => new MapLayer()
			//  {
			//      Identifier = layer.Identifier
			//  }).ToArray()
			//};
			//Element.DidFinishLoadingStyleCommand?.Execute(s);
		}

		[Export("mapViewRegionIsChanging:"),]
		void RegionIsChanging(MGLMapView mapView)
		{
			Element.Center = new Position(mapView.CenterCoordinate.Latitude, mapView.CenterCoordinate.Longitude);
		}

		[Export("mapView:regionDidChangeAnimated:"),]
		void RegionDidChangeAnimated(MGLMapView mapView, bool animated)
		{
			Element.ZoomLevel = mapView.ZoomLevel;
		}
	}
}
