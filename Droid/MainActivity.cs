
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MapBoxQs.Droid
{
    [Activity(Label = "MapBoxQs.Droid", Icon = "@mipmap/ic_launcher", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Com.Mapbox.Mapboxsdk.Mapbox.GetInstance(this, MapBoxQs.Services.MapBoxService.AccessToken);
            Acr.UserDialogs.UserDialogs.Init(() => this);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            System.Diagnostics.Debug.WriteLine("Mapbox version: " + Com.Mapbox.Mapboxsdk.BuildConfig.MapboxVersionString);

            LoadApplication(new App());
        }
    }


}
