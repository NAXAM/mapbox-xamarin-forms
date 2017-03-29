using System;
using CoreGraphics;
using Foundation;
using Mapbox;
using Naxam.Mapbox.Forms;
using Xamarin.Forms.Platform.iOS;

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

            if (e.PropertyName == Naxam.Mapbox.Forms.MapView.CenterProperty.PropertyName) {
                UpdateCenter();
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
			if (Element.Center != null)
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

			//Element.MapCenterChanged = (sender, e) =>
			//{
			//  MBPosition pos = (e as PositionChangedEventArgs).Position;
			//  MapView.SetCenterCoordinate(new CLLocationCoordinate2D(pos.Latitude, pos.Longitude), true);
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
			//if (userLocation != null)
			//{
			//  Element.UserLocation = new MBPosition()
			//  {
			//      Latitude = userLocation.Coordinate.Latitude,
			//      Longitude = userLocation.Coordinate.Longitude
			//  };
			//}
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
	}
}
