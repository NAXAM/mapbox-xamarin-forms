using System.Linq;
using Android.Content;
using Com.Mapbox.Mapboxsdk.Annotations;
using Xamarin.Forms;
using static Com.Mapbox.Mapboxsdk.Maps.MapboxMap;

namespace Naxam.Mapbox.Platform.Droid
{
    public class CustomInfoWindowAdapter : Java.Lang.Object, IInfoWindowAdapter
    {
        Context _context;
        Naxam.Controls.Mapbox.Forms.MapView _mapView;
        DataTemplate _dataTemPlate;
        public CustomInfoWindowAdapter(Context context, Naxam.Controls.Mapbox.Forms.MapView map)
        {
            _context = context;
            _mapView = map;
            _dataTemPlate = map.InfoWindowTemplate;
        }
        public Android.Views.View GetInfoWindow(Marker marker)
        {
            marker.SetTopOffsetPixels(-marker.Icon.Bitmap.Height / 2 - 24);
            marker.SetTopOffsetPixels(-marker.Icon.Bitmap.Width);
            if (marker.InfoWindow?.View != null)
                return marker.InfoWindow.View;
            if (_dataTemPlate == null)
                return null;
            object content = null;
            var bindingContext = _mapView.Annotations?.FirstOrDefault(d => d.Id == marker.Id.ToString());
            if (_dataTemPlate is DataTemplateSelector dts)
            {
                content = dts.SelectTemplate(bindingContext, _mapView).CreateContent();
            }
            else
            {
                content = _dataTemPlate.CreateContent();
            }

            if (content is ViewCell vc)
            {
                vc.Parent = _mapView;
                vc.BindingContext = bindingContext;
                var output = new ViewGroupContainer(_context, vc);
                return output;
            }

            if (content is View view)
            {
                view.Parent = _mapView;
                view.BindingContext = bindingContext;
                var output = new ViewGroupContainer(_context, view);
                return output;
            }

            return null;
        }
    }
}