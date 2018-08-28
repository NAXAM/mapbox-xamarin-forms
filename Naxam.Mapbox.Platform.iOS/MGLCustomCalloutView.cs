using System;
using CoreGraphics;
using Foundation;
using Mapbox;
using UIKit;
using Xamarin.Forms;

namespace Naxam.Mapbox.Platform.iOS
{
    public class MGLCustomCalloutView : UIView, IMGLCalloutView, IMGLCalloutViewDelegate
    {
        UIView customCallout;
        nfloat tipWidth = 0f;
        nfloat tipHeight = 0f;
        public MGLCustomCalloutView()
        {
            
        }

        public MGLAnnotation RepresentedObject { get; set;}
        public UIView LeftAccessoryView { get; set;}
        public UIView RightAccessoryView { get; set; }
        public NSObject WeakDelegate { get; set;}
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
        public void DismissCalloutAnimated(bool animated)
        {
            if(Superview != null)
            {
                if (animated)
                {
                    UIView.Animate(0.2, () => { this.Alpha = 0; }, () => { this.RemoveFromSuperview(); });
                }
                else RemoveFromSuperview();
            }
        }

        public void PresentCalloutFromRect(CGRect rect, UIView view, CGRect constrainedRect, bool animated)
        {
            view.AddSubview(this);
            nfloat frameWidht = customCallout.Frame.Width;
            nfloat frameHeight = customCallout.Frame.Height;
            nfloat frameOriginx = rect.X + (rect.Size.Width / 2) - (frameWidht / 2);
            nfloat frameOrifinY = rect.Y - frameHeight;
            CGRect frame = new CGRect(frameOriginx, frameOrifinY, frameWidht, frameHeight);
            customCallout.Frame = frame;
        }

        public MGLCustomCalloutView(MGLAnnotation representedObject, UIView customView) :base(frame:CGRect.Empty)
        {
            this.RepresentedObject = representedObject;
            this.customCallout = customView;
            this.tipWidth = customView.Frame.Width;
            this.tipHeight = customView.Frame.Height;
            this.AddSubview(customView);
        }

        public MGLCustomCalloutView(NSCoder decoder)
        {
            System.Diagnostics.Debug.WriteLine("MGLCustomCallout has not been implement");
        }
    }
}
