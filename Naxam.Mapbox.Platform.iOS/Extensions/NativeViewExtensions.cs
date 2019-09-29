using System;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using RectangleF = CoreGraphics.CGRect;
using SizeF = CoreGraphics.CGSize;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    using Platform = Xamarin.Forms.Platform.iOS.Platform;

    public static class NativeViewExtensions
    {
        public static UIView FormsToNative(this View formView)
        {
            if (formView == null) return null;
            var renderer = Platform.GetRenderer(formView) ?? Platform.CreateRenderer(formView);
            Platform.SetRenderer(formView, renderer);

            var frame = new RectangleF
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

            View view;

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

            return new FormsViewContainer
            {
                View = view
            };
        }
    }

    internal class FormsViewContainer : UIView, INativeElementView
    {
        WeakReference<IVisualElementRenderer> _rendererRef;
        View _view;
        bool _disposed;

        Element INativeElementView.Element => View;

        public View View
        {
            get { return _view; }
            set
            {
                if (_view == value)
                    return;
                UpdateView(value);
            }
        }

        public override void LayoutSubviews()
        {
            //This sets the content views frame.
            base.LayoutSubviews();

            var contentFrame = Frame;
            var view = View;

            Layout.LayoutChildIntoBoundingRegion(view, contentFrame.ToRectangle());

            if (_rendererRef == null)
                return;

            IVisualElementRenderer renderer;
            if (_rendererRef.TryGetTarget(out renderer))
                renderer.NativeView.Frame = view.Bounds.ToRectangleF();
        }

        public override SizeF SizeThatFits(SizeF size)
        {
            IVisualElementRenderer renderer;
            if (!_rendererRef.TryGetTarget(out renderer))
                return base.SizeThatFits(size);

            if (renderer.Element == null)
                return SizeF.Empty;

            double width = size.Width;
            var height = size.Height > 0 ? size.Height : double.PositiveInfinity;
            var result = renderer.Element.Measure(width, height, MeasureFlags.IncludeMargins);

            return new SizeF(result.Request.Width, result.Request.Height);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                IVisualElementRenderer renderer;
                if (_rendererRef != null && _rendererRef.TryGetTarget(out renderer) && renderer.Element != null)
                {
                    // TODO DisposeModelAndChildrenRenderers
                    //if (platform != null)
                    //platform.DisposeModelAndChildrenRenderers(renderer.Element);

                    _rendererRef = null;
                }

                _view = null;
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        IVisualElementRenderer GetNewRenderer()
        {
            if (_view == null)
                throw new InvalidOperationException($"ViewCell must have a {nameof(_view)}");

            var newRenderer = Platform.CreateRenderer(_view);
            _rendererRef = new WeakReference<IVisualElementRenderer>(newRenderer);
            AddSubview(newRenderer.NativeView);
            return newRenderer;
        }

        void UpdateView(View view)
        {
            this._view = view;

            IVisualElementRenderer renderer;
            if (_rendererRef == null || !_rendererRef.TryGetTarget(out renderer))
                renderer = GetNewRenderer();
            else
            {
                //if (renderer.Element != null && renderer == Platform.GetRenderer(renderer.Element))
                //    renderer.Element.ClearValue(Platform.RendererProperty);

                //var type = Internals.Registrar.Registered.GetHandlerTypeForObject(this._viewCell.View);
                //var reflectableType = renderer as System.Reflection.IReflectableType;
                //var rendererType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : renderer.GetType();
                //if (rendererType == type || (renderer is Platform.DefaultRenderer && type == null))
                //    renderer.SetElement(this._viewCell.View);
                //else
                //{
                //when cells are getting reused the element could be already set to another cell
                //so we should dispose based on the renderer and not the renderer.Element
                var platform = renderer.Element.Platform as Platform;

                // TODO DisposeRendererAndChildren
                // platform.DisposeRendererAndChildren(renderer);

                renderer = GetNewRenderer();
                //}
            }

            Platform.SetRenderer(this._view, renderer);
        }
    }
}
