using System;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class NativeViewExtensions
    {
        public static UIView FormsToNative(this Xamarin.Forms.View formView)
        {
            var xxx = formView.Width;
            if (formView == null) return null;
            var renderer = RendererFactory.GetRenderer(formView);
            var frame = new CGRect
            {
                X = 0,
                Y = 0,
                Width = renderer.NativeView.IntrinsicContentSize.Width,
                Height = renderer.NativeView.IntrinsicContentSize.Height
            };
            renderer.NativeView.Frame = frame;
            renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
            renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;
            renderer.Element.Layout(frame.ToRectangle());
            var nativeView = renderer.NativeView;
            nativeView.SetNeedsLayout();
            return nativeView;
        }

        public static UIView DataTemplateToNativeView(this Xamarin.Forms.DataTemplate dt)
        {
            if (dt == null) return null;
            var content = dt.CreateContent();
            if (content is ViewCell vc)
            {
                var view = vc.View;
                return view.FormsToNative();
            }
            return null;
        }
    }
}
