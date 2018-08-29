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
            object content = (dt is DataTemplateSelector dts)
                ? dts.SelectTemplate(bindingContext, rootView)
                : dt.CreateContent();

            View view = null;

            switch (content)
            {
                case ViewCell viewCell:
                    viewCell.Parent = rootView;
                    viewCell.BindingContext = bindingContext;
                    view = viewCell.View;
                    break;
                case View view1:
                    view1.Parent = rootView;
                    view1.BindingContext = bindingContext;
                    view = view1;
                    break;
                default:
                    return null;
            }

            return view.FormsToNative();
        }
    }
}
