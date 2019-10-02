using System;
using CoreGraphics;
using Mapbox;
using Naxam.Controls.Forms;
using Naxam.Controls.Mapbox.Platform.iOS;
using Xamarin.Forms;

namespace Naxam.Mapbox.Platform.iOS.Extensions
{
    public static class MapboxOptionsExtensions
    {
        public static MGLMapCamera ToNative(this CameraPosition cameraPosition)
        {
            var camera = new MGLMapCamera();

            if (cameraPosition.Bearing.HasValue)
            {
                camera.Heading = cameraPosition.Bearing.Value;
            }

            if (cameraPosition.Target.HasValue)
            {
                camera.CenterCoordinate = cameraPosition.Target.Value.ToCLCoordinate();
            }

            if (cameraPosition.Tilt.HasValue)
            {
                camera.Pitch = (nfloat)cameraPosition.Tilt.Value;
            }

            // TODO iOS Convert Zoom to Altitude
//            if (cameraPosition.Zoom.HasValue)
//            {
//                camera.Altitude = cameraPosition.Zoom.Value;
//            }
            
            return camera;
        }
        
        public static CGPoint ToPoint(this Thickness thickness)
        {
            // TODO iOS - Attribution/Compass margin only has x,y
            return new CGPoint(thickness.Left, thickness.Top);
        }
        
        public static MGLOrnamentPosition ToOrnamentPosition(this Gravity gravity)
        {
            switch (gravity)
            {
                case Gravity.Bottom | Gravity.Left:
                case Gravity.Bottom | Gravity.Start:
                    return MGLOrnamentPosition.BottomLeft;
                case Gravity.Bottom | Gravity.Right:
                case Gravity.Bottom | Gravity.End:
                    return MGLOrnamentPosition.BottomRight;
                case Gravity.Top | Gravity.Left:
                case Gravity.Top | Gravity.Start:
                    return MGLOrnamentPosition.TopLeft;
                case Gravity.Top | Gravity.Right:
                case Gravity.Top | Gravity.End:
                    return MGLOrnamentPosition.TopRight;
            }

            return default(MGLOrnamentPosition);
        }
    }
}