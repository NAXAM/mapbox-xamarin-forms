using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using Mapbox;
using Naxam.Mapbox.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreLocation;
using FormsMap = Naxam.Mapbox.Forms.MapView;
using FormsMB = Naxam.Mapbox.Forms;
using System.Collections.Specialized;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Naxam.Mapbox.Forms.MapView), typeof(Naxam.Mapbox.Platform.iOS.MapViewRenderer))]
namespace Naxam.Mapbox.Platform.iOS
{
	public class MapViewRenderer : ViewRenderer<Naxam.Mapbox.Forms.MapView, MGLMapView>, IMGLMapViewDelegate
	{
		//private Func<byte[]> captureMapview = () =>
		//{
		//	var image = MapView.Capture(true);
		//	var imageData = image.AsJPEG();
		//	Byte[] imgByteArray = new Byte[imageData.Length];
		//	System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes,
		//												imgByteArray,
		//												0,
		//	                                            Convert.ToInt32(imageData.Length));
		//	return imgByteArray;
		//};

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

					Element.TakeSnapshot = () => {
						var image = MapView.Capture(true);
						var imageData = image.AsJPEG();
						Byte[] imgByteArray = new Byte[imageData.Length];
						System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes,
																	imgByteArray,
																	0,
																	Convert.ToInt32(imageData.Length));
						return imgByteArray;
					};
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
			if (e.PropertyName == FormsMap.CenterProperty.PropertyName)
			{
				UpdateCenter();
			}
			else if (e.PropertyName == FormsMap.ZoomLevelProperty.PropertyName && MapView.ZoomLevel != Element.ZoomLevel)
			{
				//MapView.SetZoomLevel(Element.ZoomLevel, true);
				MapView.ZoomLevel = Element.ZoomLevel;
			}
			else if (e.PropertyName == FormsMap.PitchEnabledProperty.PropertyName && MapView.PitchEnabled != Element.PitchEnabled)
			{
				MapView.PitchEnabled = Element.PitchEnabled;
			}
			else if (e.PropertyName == FormsMap.RotateEnabledProperty.PropertyName && MapView.RotateEnabled != Element.RotateEnabled)
			{
				MapView.RotateEnabled = Element.RotateEnabled;
			}
			else if (e.PropertyName == FormsMap.AnnotationsProperty.PropertyName)
			{
				RemoveAllAnnotations();
				if (Element.Annotations != null)
				{
					AddAnnotations(Element.Annotations.ToArray());
					var notifyCollection = Element.Annotations as INotifyCollectionChanged;
					if (notifyCollection != null)
					{
						notifyCollection.CollectionChanged += OnAnnotationsCollectionChanged;
					}
				}
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
				if (!string.IsNullOrEmpty(Element.StyleUrl))
				{
					MapView.StyleURL = new NSUrl(Element.StyleUrl);
				}
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

		void AddAnnotation(Annotation annotation)
		{
			var shape = ShapeFromAnnotation(annotation);
			if (shape != null)
			{
				MapView.AddAnnotation(shape);
			}
		}

		void AddAnnotations(Annotation[] annotations)
		{
			
			var annots = new List<IMGLAnnotation>();
			foreach (Annotation at in annotations)
			{
				var shape = ShapeFromAnnotation(at);
				if (shape == null)
				{
					continue;
				}
				annots.Add(shape);
			}
			MapView.AddAnnotations(annots.ToArray());
		}

		void RemoveAnnotations(Annotation[] annotations)
		{
			var currentAnnotations = MapView.Annotations;
			if (currentAnnotations == null)
			{
				return;
			}
			var annots = new List<MGLShape>();
			foreach (Annotation at in annotations)
			{
				foreach (NSObject curAnnot in currentAnnotations)
				{
					if (curAnnot is MGLShape)
					{
						var shape = curAnnot as MGLShape;
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
			MapView.RemoveAnnotations(annots.ToArray());
		}

		void RemoveAllAnnotations()
		{
			if (MapView.Annotations != null)
			{
				MapView.RemoveAnnotations(MapView.Annotations);
			}
		}

		private void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				var annots = new List<MGLShape>();
				foreach (Annotation annot in e.NewItems)
				{
					var shape = ShapeFromAnnotation(annot);
					if (shape != null)
					{
						annots.Add(shape);
					}
				}
				MapView.AddAnnotations(annots.ToArray());
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
			else if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				//TODO Update pins
			}
		}

		MGLShape ShapeFromAnnotation(FormsMB.Annotation annotation)
		{
			MGLShape shape = null;
			if (annotation is PointAnnotation)
			{
				shape = new MGLPointAnnotation()
				{
					Coordinate = new CLLocationCoordinate2D(((PointAnnotation)annotation).Coordinate.Lat,
																 ((PointAnnotation)annotation).Coordinate.Long),
				};
			}
			else if (annotation is PolylineAnnotation)
			{
				var polyline = annotation as PolylineAnnotation;
				if (polyline.Coordinates == null || polyline.Coordinates.Length == 0)
				{
					return null;
				}
				var coords = new CLLocationCoordinate2D[polyline.Coordinates.Length];
				for (var i = 0; i < polyline.Coordinates.Length; i++)
				{
					coords[i] = new CLLocationCoordinate2D(polyline.Coordinates[i].Lat, polyline.Coordinates[i].Long);
				}
				var first = coords[0];
				shape = MGLPolyline.PolylineWithCoordinates(ref first, (uint)coords.Length);
			}
			else if (annotation is MultiPolylineAnnotation)
			{
				var polyline = annotation as MultiPolylineAnnotation;
				if (polyline.Coordinates == null || polyline.Coordinates.Length == 0)
				{
					return null;
				}
				var lines = new MGLPolyline[polyline.Coordinates.Length];
				for (var i = 0; i < polyline.Coordinates.Length; i++)
				{
					if (polyline.Coordinates[i].Length == 0)
					{
						continue;
					}
					var coords = new CLLocationCoordinate2D[polyline.Coordinates[i].Length];
					for (var j = 0; j < polyline.Coordinates[i].Length; j++)
					{
						coords[i] = new CLLocationCoordinate2D(polyline.Coordinates[i][j].Lat, polyline.Coordinates[i][j].Long);
					}
					var first = coords[0];
					lines[i] = MGLPolyline.PolylineWithCoordinates(ref first, (uint)polyline.Coordinates[i].Length);
				}
				shape = MGLMultiPolyline.MultiPolylineWithPolylines(lines);
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
			}

			return shape;
		}

		#region MGLMapViewDelegate
		[Export("mapViewDidFinishRenderingMap:fullyRendered:"),]
		void DidFinishRenderingMap(MGLMapView mapView, bool fullyRendered)
		{
			Element.Delegate?.DidFinishRenderingCommand?.Execute(
				new Tuple<FormsMap, bool>(Element, fullyRendered));
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

		[Export("mapView:annotationCanShowCallout:"),]
		bool AnnotationCanShowCallout(MGLMapView mapView, NSObject annotation)
		{
			if (annotation is MGLShape)
			{
				return Element.CanShowCalloutChecker.Invoke(((MGLShape)annotation).Id());
			}
			return true;
		}
		#endregion
	} 
}
