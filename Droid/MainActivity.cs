
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MapBoxQs.Droid
{
    [Activity(Label = "MapBoxQs.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Mapbox.Sdk.MapboxAccountManager.Start(this, "sk.eyJ1IjoiamVzcGVyZGF4IiwiYSI6ImNpemo2ajloNTAwMmwyd3I0NWoxNHZoNTYifQ.TnTUuIPwpZzGfS47cr0YMw");

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }
    }


}
