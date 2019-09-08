using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Foundation;
using Mapbox;
using Naxam.Controls.Forms;
using Naxam.Controls.Mapbox.Platform.iOS;
using Naxam.Mapbox.Layers;
using UIKit;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]
namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer : ViewRenderer<MapView, MGLMapView>, IMGLMapViewDelegate, IUIGestureRecognizerDelegate
    {
        protected virtual void UpdateMapStyle()
        {
            if (Element.MapStyle != null && !string.IsNullOrEmpty(Element.MapStyle.UrlString))
            {
                map.StyleURL = new NSUrl(Element.MapStyle.UrlString);
            }
        }

        MGLStyle mapStyle;

        [Export("mapView:didFinishLoadingStyle:"),]
        public void DidFinishLoadingStyle(MGLMapView mapView, MGLStyle style)
        {
            MapStyle newStyle;
            if (Element.MapStyle == null)
            {
                newStyle = new MapStyle(mapView.StyleURL.AbsoluteString);
                newStyle.Name = style.Name;
                Element.MapStyle = newStyle;
            }
            else
            {
                if (Element.MapStyle.UrlString == null
                || Element.MapStyle.UrlString != mapView.StyleURL.AbsoluteString)
                {
                    Element.MapStyle.SetUrl(mapView.StyleURL.AbsoluteString);
                    Element.MapStyle.Name = style.Name;
                }
                newStyle = Element.MapStyle;
            }
            
            newStyle.Name = style.Name;
            Element.Functions = this;
            Element.DidFinishLoadingStyleCommand?.Execute(newStyle);
        }
    }

}
