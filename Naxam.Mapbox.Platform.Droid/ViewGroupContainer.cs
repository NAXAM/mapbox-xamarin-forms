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
    public class ViewGroupContainer : FormsViewGroup
    {
        IVisualElementRenderer _renderer;
        private Context _context;
        private VisualElement _visualElement;
        private Android.Views.View _parent;
        Android.Views.View _view;
        public Element Element
        {
            get { return _visualElement; }
        }


        public ViewGroupContainer(Context context, Android.Views.View parent, VisualElement visualElement) : base(context)
        {
            _context = context;
            _visualElement = visualElement;
            _parent = parent;
            _renderer = Xamarin.Forms.Platform.Android.Platform.CreateRendererWithContext(_visualElement, context);
            Xamarin.Forms.Platform.Android.Platform.SetRenderer(_visualElement, _renderer);
            _view = _renderer.View;
            AddView(_view,new LayoutParams(-2,-2));
            //LayoutParameters = new LayoutParams(100, 100);

        }
        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            double width = Context.FromPixels(right - left);
            double height = Context.FromPixels(bottom - top);
            Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_renderer.Element, new Rectangle(0, 0, width, height));
            _renderer.UpdateLayout();
            System.Diagnostics.Debug.WriteLine($"{nameof(OnLayout)}: {right - left}x{bottom - top}");

        }
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = MeasureSpec.GetSize(widthMeasureSpec);
            int height = MeasureSpec.GetSize(heightMeasureSpec);
            var x = _renderer.Element;
            SetMeasuredDimension(width, height);
            System.Diagnostics.Debug.WriteLine($"{nameof(OnMeasure)}: {width}x{height}");
        }
    }
}