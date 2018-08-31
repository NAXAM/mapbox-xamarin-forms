using System;
using CoreGraphics;
using Foundation;
using Mapbox;
using UIKit;
using Xamarin.Forms;

namespace Naxam.Mapbox.Platform.iOS
{
    public class MGLCustomCalloutView : UIView, IMGLCalloutView
    {
        UIView customCallout;

        public MGLAnnotation RepresentedObject { get; set; }
        public UIView LeftAccessoryView { get; set; }
        public UIView RightAccessoryView { get; set; }
        public NSObject WeakDelegate { get; set; }

        public override CGPoint Center
        {
            get => base.Center;
            set
            {
                var newCenter = value;
                newCenter.Y = newCenter.Y - Bounds.Y;
                base.Center = newCenter;
            }
        }

        public MGLCustomCalloutView(MGLAnnotation representedObject, UIView customView) : base(frame: CGRect.Empty)
        {
            this.RepresentedObject = representedObject;
            this.customCallout = customView;
            this.AddSubview(customView);
        }

        public MGLCustomCalloutView(NSCoder decoder)
        {
            throw new NotImplementedException();
        }

        public void DismissCalloutAnimated(bool animated)
        {
            if (Superview == null) return;

            if (animated)
            {
                Animate(
                    0.2, () => { this.Alpha = 0; },
                    () => { this.RemoveFromSuperview(); }
                );
            }
            else
            {
                RemoveFromSuperview();
            }
        }

        public void PresentCalloutFromRect(CGRect rect, UIView view, CGRect constrainedRect, bool animated)
        {
            view.AddSubview(this);
            var size = customCallout.SizeThatFits(constrainedRect.Size);

            nfloat frameWidth = size.Width;
            nfloat frameHeight = size.Height;
            nfloat frameOriginx = rect.X + (rect.Width / 2) - (frameWidth / 2);
            nfloat frameOrifinY = rect.Y - frameHeight;
            CGRect frame = new CGRect(frameOriginx, frameOrifinY, frameWidth, frameHeight);
            Frame = frame;
            customCallout.Frame = Bounds;
        }
    }
}
