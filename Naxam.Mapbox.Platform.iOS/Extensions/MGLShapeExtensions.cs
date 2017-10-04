using System;
using System.Runtime.InteropServices;
using Foundation;
using Mapbox;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
	enum AssociationPolicy
	{
		Assign = 0,
		RetainNonAtomic = 1,
		CopyNonAtomic = 3,
		Retain = 01401,
		Copy = 01403,
	}

	public static class MGLShapeExtensions
	{
		#region Extension properties
		[DllImport("/usr/lib/libobjc.dylib")]
		static extern void objc_setAssociatedObject(
		IntPtr pointer, IntPtr key,
		IntPtr value, AssociationPolicy policy);

		[DllImport("/usr/lib/libobjc.dylib")]
		static extern IntPtr objc_getAssociatedObject(
			IntPtr pointer, IntPtr key);

		private static T GetProperty<T>(
			this MGLShape controller,
			NSString propertyKey) where T : NSObject
		{
			var pointer = objc_getAssociatedObject(
				controller.Handle,
				propertyKey.Handle
			);

			return ObjCRuntime.Runtime.GetNSObject<T>(pointer);
		}

		private static void SetProperty<T>(
			this MGLShape controller,
			NSString propertyKey,
			T value,
			AssociationPolicy policy) where T : NSObject
		{
			objc_setAssociatedObject(
				controller.Handle,
				propertyKey.Handle,
				value.Handle,
				policy
			);
		}
		#endregion

		static readonly NSString kId = (NSString)"kId";
		public static string Id(this MGLShape shape)
		{
			var prop = shape.GetProperty<NSString>(kId);
			return prop;
		}
		public static void SetId(this MGLShape shape, string id)
		{
			shape.SetProperty(kId, (NSString)id, AssociationPolicy.RetainNonAtomic);
		}
		//public static bool IsShowingLoadingIndicator(this UIViewController viewController)
		//{
		//	var prop = viewController.GetProperty<NSNumber>(kIsShowingLoadingIndicator);
		//	if (prop != null)
		//	{
		//		return prop.BoolValue;
		//	}
		//	return false;
		//}

		//public static void SetIsShowingLoadingIndicator(this UIViewController viewController, bool isShowing)
		//{
		//	viewController.SetProperty(kIsShowingLoadingIndicator, NSNumber.FromBoolean(isShowing), AssociationPolicy.RetainNonAtomic);
		//}
	}
}
