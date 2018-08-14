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

    public class ViewGroupContainer : ViewGroup, INativeElementView
    {
        readonly Android.Views.View _parent;
        IVisualElementRenderer _renderer;
        ViewCell _viewCell;
        public ViewGroupContainer(Context context,
                                 Android.Views.View parent,
                                 ViewCell viewCell) : base(context)
        {
            _viewCell = viewCell;
            _parent = parent;
            _renderer = Platform.CreateRendererWithContext(_viewCell.View, context);
            Platform.SetRenderer(_viewCell.View, _renderer);
            //var width = (int)Context.ToPixels(_renderer.Element.WidthRequest);
            //if (width <= 0)
            //    width = (int)Context.ToPixels(20);
            //var height = (int)Context.ToPixels(_renderer.Element.HeightRequest);
            //if (height <= 0)
            //    height = (int)Context.ToPixels(20);
            var view = _renderer.View;
            //view.Measure((int)MeasureSpecMode.AtMost, (int)MeasureSpecMode.AtMost);
            LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
            AddView(view);
            UpdateIsEnabled();
        }

        public void Update(object data)
        {
            _viewCell.BindingContext = data;
        }

        public Element Element
        {
            get { return _viewCell; }
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            if (!Enabled)
                return true;
            return base.OnInterceptTouchEvent(ev);
        }

        public void UpdateIsEnabled()
        {
            Enabled = _viewCell.IsEnabled;
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            double width = Context.FromPixels(r - l);
            double height = Context.FromPixels(b - t);

            _renderer.Element.Layout(new Rectangle(0, 0, width, height));

            _renderer.UpdateLayout();

            System.Diagnostics.Debug.WriteLine($"{nameof(OnLayout)}: {r - l}x{b - t}");
        }

        //protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        //{
        //    int width = MeasureSpec.GetSize(widthMeasureSpec);
        //    int height = MeasureSpec.GetSize(heightMeasureSpec);
        //    var x = _renderer.Element;

        //    SetMeasuredDimension(width, height);

        //    System.Diagnostics.Debug.WriteLine($"{nameof(OnMeasure)}: {width}x{height}");
        //}
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = MeasureSpec.GetSize(widthMeasureSpec);
            int height = MeasureSpec.GetSize(heightMeasureSpec);
            int widthSpec = MeasureSpec.MakeMeasureSpec(width, MeasureSpec.GetMode(widthMeasureSpec));
            int heightSpec = MeasureSpec.MakeMeasureSpec(height, MeasureSpec.GetMode(heightMeasureSpec));
            base.SetMeasuredDimension(width, height);
        }
 
    }
}