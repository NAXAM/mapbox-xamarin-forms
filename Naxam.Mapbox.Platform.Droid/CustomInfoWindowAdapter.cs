using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Maps;
using Naxam.Mapbox.Platform.Droid.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Com.Mapbox.Mapboxsdk.Maps.MapboxMap;

namespace Naxam.Mapbox.Platform.Droid
{
    public class CustomInfoWindowAdapter : Java.Lang.Object, IInfoWindowAdapter
    {
        Context _context;
        Naxam.Controls.Mapbox.Forms.MapView _map;
        readonly MapboxMap mapBox;
        private readonly MapView mv;

        public CustomInfoWindowAdapter(Context context, Naxam.Controls.Mapbox.Forms.MapView map, MapboxMap mapBox, Com.Mapbox.Mapboxsdk.Maps.MapView mv)
        {
            _context = context;
            _map = map;
            this.mapBox = mapBox;
            this.mv = mv;
        }
        public Android.Views.View GetInfoWindow(Marker marker)
        {

            if (marker?.InfoWindow?.View != null)
            {
                return marker.InfoWindow.View;
            }

            var bindingContext = _map.Annotations.FirstOrDefault(d => d.Id == marker.Id.ToString());

            var dt = _map.InfoWindowTemplate;
            object content;
            if (_map.InfoWindowTemplate is DataTemplateSelector ds)
            {
                content = ds.SelectTemplate(bindingContext, _map).CreateContent();
            }
            else
            {
                content = dt.CreateContent();
            }

            if (content is Xamarin.Forms.View fView)
            {
                fView.BindingContext = bindingContext;
                fView.Parent = _map;
                var output = new ViewGroupContainer(_context, fView);
                return output;
            }

            if (content is ViewCell cell)
            {
                cell.BindingContext = bindingContext;
                cell.View.Parent = _map;
                var output = new ViewGroupContainer(_context, cell);
                return output;
            }
            return null;
        }
    }
}