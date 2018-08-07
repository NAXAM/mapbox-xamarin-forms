using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AViews = Android.Views;
using APlatform = Xamarin.Forms.Platform.Android.Platform;
using Xamarin.Forms;

namespace Naxam.Mapbox.Platform.Droid.Extensions
{
    public static class ViewExtensions
    {
        public static AViews.View ToAndroid(this Xamarin.Forms.View view)
        {
            if (APlatform.GetRenderer(view) == null)
                //TODO Replace deprecated CreateRenderer(...)
                APlatform.SetRenderer(view, APlatform.CreateRenderer(view));
            var vRenderer = APlatform.GetRenderer(view);
            var viewGroup = vRenderer.View;
            vRenderer.Tracker.UpdateLayout();
            var layoutParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            viewGroup.LayoutParameters = layoutParams;
            viewGroup.Layout(0, 0, (int)view.WidthRequest, (int)view.HeightRequest);
            return viewGroup;
        }
        public static List<Android.Views.View> GetAllChildView(this ViewGroup v)
        {
            List<Android.Views.View> result = new List<Android.Views.View>();
            ViewGroup viewGroup = (ViewGroup)v;
            for (int i = 0; i < viewGroup.ChildCount; i++)
            {
                Android.Views.View child = viewGroup.GetChildAt(i);
                List<Android.Views.View> viewArrayList = new List<Android.Views.View>();
                viewArrayList.Add(v);
                viewArrayList.AddRange(GetAllChildView((ViewGroup) child));
                result.AddRange(viewArrayList);
            }
            return result;
        }
    }
}