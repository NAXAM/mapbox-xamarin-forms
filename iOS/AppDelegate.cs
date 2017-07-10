using Foundation;
using UIKit;

namespace MapBoxQs.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
            //Mapbox.MGLAccountManager.AccessToken = "sk.eyJ1IjoiamVzcGVyZGF4IiwiYSI6ImNpemo2ajloNTAwMmwyd3I0NWoxNHZoNTYifQ.TnTUuIPwpZzGfS47cr0YMw";
            Mapbox.MGLAccountManager.AccessToken = "pk.eyJ1IjoiaG9uZ2hhIiwiYSI6ImNpemR6dzZ3ZDB4czQzMm9lbTExejA5d3EifQ.FhhVGMXscRld1KGaquiIGw";
            //Mapbox.MGLAccountManager.SetAccessToken(new NSString("sk.eyJ1IjoiamVzcGVyZGF4IiwiYSI6ImNpemo2ajloNTAwMmwyd3I0NWoxNHZoNTYifQ.TnTUuIPwpZzGfS47cr0YMw"));
            new Naxam.Controls.Platform.iOS.MapViewRenderer();

            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
