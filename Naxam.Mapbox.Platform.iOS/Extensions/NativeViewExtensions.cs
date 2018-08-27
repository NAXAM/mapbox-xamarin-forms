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
                Width = (nfloat)formView.WidthRequest,
                Height = (nfloat)formView.HeightRequest
            };

            renderer.NativeView.Frame = frame;
            renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
            renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;
            renderer.Element.Layout(frame.ToRectangle());
            var nativeView = renderer.NativeView;
            nativeView.SetNeedsLayout();
            return nativeView;
        }

        public static UIView DataTemplateToNativeView(this Xamarin.Forms.DataTemplate dt, object bindingContext, Xamarin.Forms.View rootView)
        {
            if (dt == null) return null;
            object content = null;
            if (dt is DataTemplateSelector dts)
            {
                content = dts.SelectTemplate(bindingContext, rootView);
            }
            else content = dt.CreateContent();
            if (content is ViewCell vc)
            {
                vc.Parent = rootView;
                vc.BindingContext = bindingContext;
                var resultView = vc.View;
                return resultView.FormsToNative();
            }
            if (content is View view)
            {
                view.Parent = rootView;
                view.BindingContext = bindingContext;
                return view.FormsToNative();
            }
            return null;
        }
    }
}
