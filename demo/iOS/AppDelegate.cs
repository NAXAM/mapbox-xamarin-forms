using System;
using Foundation;
using Shiny.Net.Http;
using Shiny.Jobs;
using UIKit;
using MapBoxQs.Views;

namespace MapBoxQs.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
            Mapbox.MGLAccountManager.AccessToken = MapBoxQs.Services.MapBoxService.AccessToken;
            new Naxam.Controls.Mapbox.Platform.iOS.MapViewRenderer();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            global::Xamarin.Forms.Forms.Init();

            Shiny.iOSShinyHost.Init(new MyStartup());

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
            => JobManager.OnBackgroundFetch(completionHandler);


        public override void HandleEventsForBackgroundUrl(UIApplication application, string sessionIdentifier, Action completionHandler)
            => HttpTransferManager.SetCompletionHandler(sessionIdentifier, completionHandler);
    }
}
