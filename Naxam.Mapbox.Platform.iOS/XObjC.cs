using System;
using System.Runtime.InteropServices;
using CoreGraphics;
using Foundation;
using Mapbox;
using ObjCRuntime;

namespace Naxam.Mapbox.Platform.iOS
{
    internal static class MapboxIndependentFunction
    {
        const string DllName = "__Internal";
        const string LIBOBJC_DYLIB = "/usr/lib/libobjc.dylib";

        //FOUNDATION_EXTERN MGL_EXPORT CLLocationDistance MGLAltitudeForZoomLevel(double zoomLevel, CGFloat pitch, CLLocationDegrees latitude, CGSize size);
        [DllImport(DllName, EntryPoint = "MGLAltitudeForZoomLevel")]
        internal static extern double MGLAltitudeForZoomLevel(double zoomLevel, nfloat pitch, double lat, CGSize size);
        
        [DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
        internal extern static IntPtr IntPtr_objc_msgSend_IntPtr (IntPtr receiever, IntPtr selector, IntPtr arg1);
    }

    /// <summary>
    /// Workaround for https://github.com/xamarin/xamarin-macios/issues/7174
    /// </summary>
    internal static class NSExpressionAdditions
    {
        [BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
        static readonly IntPtr class_ptr = Class.GetHandle ("NSExpression");
        
        [Export ("expressionForConstantValue:")]
        [BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
        public static NSExpression FromConstant (NSObject value)
        {
            var intPtr = MapboxIndependentFunction.IntPtr_objc_msgSend_IntPtr(
                class_ptr,
                Selector.GetHandle("expressionForConstantValue:"), 
                value?.Handle ?? IntPtr.Zero);
            return Runtime.GetNSObject<NSExpression> (intPtr);
        }
    }
}