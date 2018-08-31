using System.Linq;
using Android.Content;
using Com.Mapbox.Mapboxsdk.Annotations;
using Xamarin.Forms;
using static Com.Mapbox.Mapboxsdk.Maps.MapboxMap;
using Android.Widget;

namespace Naxam.Mapbox.Platform.Droid
{
    using Platform = Xamarin.Forms.Platform.Android.Platform;

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
            if (marker.InfoWindow?.View != null)
            {
                return marker.InfoWindow.View;
            }

            if (_dataTemPlate == null)
                return null;

            var bindingContext = _mapView.Annotations?.FirstOrDefault(d => d.Id == marker.Id.ToString());
            var templateContent = (_dataTemPlate is DataTemplateSelector dataTemplateSelector)
                ? dataTemplateSelector.SelectTemplate(bindingContext, _mapView).CreateContent()
                : _dataTemPlate.CreateContent();

            View view = null;

            switch (templateContent)
            {
                case ViewCell viewCell:
                    viewCell.BindingContext = bindingContext;
                    viewCell.Parent = _mapView;
                    view = viewCell.View;
                    break;
                case View view1:
                    view1.BindingContext = bindingContext;
                    view1.Parent = _mapView;
                    view = view1;
                    break;
                default:
                    return null;
            }

            var renderer = Platform.GetRenderer(view) ?? Platform.CreateRendererWithContext(view, _context);
            Platform.SetRenderer(view, renderer);

            var output = new ViewGroupContainer(_context)
            {
                Child = renderer
            };
            return output;
        }
    }
}