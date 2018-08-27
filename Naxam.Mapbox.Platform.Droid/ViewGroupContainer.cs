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
using Com.Mapbox.Mapboxsdk.Annotations;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Naxam.Mapbox.Platform.Droid
{
    using Platform = Xamarin.Forms.Platform.Android.Platform;

    public class ViewGroupContainer : BubbleLayout
    {
        IVisualElementRenderer _renderer;
        ViewCell _viewCell;
        public ViewGroupContainer(Context context,
                                 ViewCell viewCell) : base(context)
        {
            _viewCell = viewCell;
            _renderer = Platform.CreateRendererWithContext(_viewCell.View, context);
            Platform.SetRenderer(_viewCell.View, _renderer);
            var view = _renderer.View;
            LayoutParameters = new LayoutParams((int)Math.Round(context.ToPixels(_viewCell.View.WidthRequest)), (int)Math.Round(context.ToPixels(_viewCell.View.HeightRequest)));
            AddView(view, LayoutParameters);
        }

        public ViewGroupContainer(Context context, Xamarin.Forms.View fview) : base(context)
        {
            _renderer = Platform.CreateRendererWithContext(fview, context);
            Platform.SetRenderer(fview, _renderer);
            var view = _renderer.View;
            LayoutParameters = new LayoutParams((int)Math.Round(context.ToPixels(fview.WidthRequest)), (int)Math.Round(context.ToPixels(fview.HeightRequest)));
            AddView(view, LayoutParameters);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            double width = Context.FromPixels(r - l);
            double height = Context.FromPixels(b - t);
            Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_renderer.Element, new Rectangle(0, 0, width, height));
            _renderer.UpdateLayout();
            
            base.OnLayout(changed, l, t, r, b);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = MeasureSpec.GetSize(widthMeasureSpec);
            int height = MeasureSpec.GetSize(heightMeasureSpec);
            int widthSpec = MeasureSpec.MakeMeasureSpec(width, MeasureSpec.GetMode(widthMeasureSpec));
            int heightSpec = MeasureSpec.MakeMeasureSpec(height, MeasureSpec.GetMode(heightMeasureSpec));
         //   base.SetMeasuredDimension(width, height);
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec); 
        }

    }
}