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
        private Context _context;
        private DataTemplate _dataTemPlate;
        private Naxam.Controls.Mapbox.Forms.MapView _map;
        private MapboxMap _mapboxMap;
        public CustomInfoWindowAdapter(Context context, DataTemplate dataTemplate, Naxam.Controls.Mapbox.Forms.MapView map, MapboxMap mapboxMap)
        {
            _context = context;
            _dataTemPlate = dataTemplate;
            this._map = map;
            this._mapboxMap = mapboxMap;
        }
        public Android.Views.View GetInfoWindow(Marker marker)
        {
            Xamarin.Forms.View formsView = null;
            LinearLayout _container = new LinearLayout(_context);
            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            layoutParams.Gravity = GravityFlags.Top;
            _container.LayoutParameters = layoutParams;
            object bindingContext = null;
            _map.ItemsSource = new List<Marker> { marker };
            var source = _map.ItemsSource?.Cast<object>();
            if (source != null)
                bindingContext = source.ElementAt(0);
            var dt = bindingContext as DataTemplate;
            var view = bindingContext as Xamarin.Forms.View;

            if (dt != null)
                formsView = (Xamarin.Forms.View)dt.CreateContent();
            else
            {
                if (view != null)
                {
                    formsView = view;
                }
                else
                {
                    var selector = _map.InfoWindowTemplate as DataTemplateSelector;
                    if (selector != null)
                        formsView = (Xamarin.Forms.View)selector.SelectTemplate(bindingContext, _map).CreateContent();
                    else
                    {
                        var content = _map.InfoWindowTemplate.CreateContent();
                        if (content is ViewCell cell)
                        {
                            cell.BindingContext = bindingContext;
                            var output = new ViewGroupContainer(_container.Context, _container, cell);
                            _container.AddView(output, new ViewGroup.LayoutParams(-2, -2));
                            return _container;
                        }
                        else
                            formsView = content as Xamarin.Forms.View;
                    }
                }
            }
            if (formsView != null)
            {
                formsView.BindingContext = bindingContext;
                _container.AddView(formsView.ToAndroid());
            }

            return _container;
        }
        
    }
}