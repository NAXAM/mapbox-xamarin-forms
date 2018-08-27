using System;
using CoreGraphics;
using Mapbox;
using UIKit;

namespace Naxam.Mapbox.Platform.iOS
{
	public class DraggableAnnotationView : MGLAnnotationView
	{
		public event EventHandler DragFinished;
        public DraggableAnnotationView(string reuseIdentifier, float size) : base(reuseIdentifier)
        {
            Draggable = true;

            ScalesWithViewingDistance = false;
            Frame = new CGRect(0, 0, size, size);
            BackgroundColor = UIColor.Blue;
            Layer.CornerRadius = size / 2;
            Layer.BorderWidth = 1;
            Layer.BorderColor = UIColor.White.CGColor;
            Layer.ShadowColor = UIColor.Black.CGColor;
            Layer.ShadowOpacity = 0.1f;
        }

		public override void SetDragState(MGLAnnotationViewDragState dragState, bool animated)
		{
			base.SetDragState(dragState, animated);
			switch (dragState)
			{
				case MGLAnnotationViewDragState.Starting:
					StartDragging();
					break;
				case MGLAnnotationViewDragState.Dragging:
					break;
				case MGLAnnotationViewDragState.Ending: case MGLAnnotationViewDragState.Canceling:
					EndDragging();
					break;
				default:
					break;
			}
		}

		private void EndDragging()
		{
			Transform = CGAffineTransform.MakeIdentity();
            Transform.Scale((nfloat)3, (nfloat)3);
			UIView.Animate(duration: 0.3, animation: () =>
            {
                Layer.Opacity = 0.8f;
                Transform.Scale((nfloat)1, (nfloat)1);
            }, completion: null);
            DragFinished.Invoke(this, null);
		}

		private void StartDragging()
		{
			UIView.Animate(duration: 0.3, animation: () =>
			{
				Layer.Opacity = 0.8f;
				Transform = CGAffineTransform.MakeIdentity();
				Transform.Scale((nfloat)3, (nfloat)3);
			}, completion: null);
		}
	}
}
