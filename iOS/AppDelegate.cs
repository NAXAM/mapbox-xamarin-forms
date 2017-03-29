using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Mapbox;
using Naxam.Mapbox.Platform.iOS;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace MapBoxQs.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Mapbox.MGLAccountManager.SetAccessToken(new NSString("sk.eyJ1IjoiamVzcGVyZGF4IiwiYSI6ImNpemo2ajloNTAwMmwyd3I0NWoxNHZoNTYifQ.TnTUuIPwpZzGfS47cr0YMw"));

            new MapViewRenderer();

            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
