using Naxam.Controls.Forms;
using Naxam.Mapbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfflineSimpleOfflineMapPage : ContentPage
    {
        private IOfflineStorageService offlineService;

        public OfflineSimpleOfflineMapPage()
        {
            InitializeComponent();

            map.MapStyle = MapStyle.OUTDOORS;
            map.Center = new LatLng(37.73359, -119.58410);
            map.ZoomLevel = 10;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);


            offlineService = DependencyService.Get<IOfflineStorageService>();
        }

        async void HandleStyleLoaded(MapStyle obj)
        {
            
        }

        private void OfflineService_OfflinePackProgressChanged(object sender, OSSEventArgs e)
        {
            var progress = e.OfflinePack.Progress;
            float percentage = 0;
            if (progress.CountOfResourcesExpected > 0)
            {
                percentage = (float)progress.CountOfResourcesCompleted / progress.CountOfResourcesExpected;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                txtProgress.Text = $"({(percentage).ToString("P")}) >> {progress.CountOfResourcesCompleted} / {progress.CountOfResourcesExpected}";
            });

            System.Diagnostics.Debug.WriteLine($"Downloaded resources: {progress.CountOfResourcesCompleted} ({percentage * 100} %)");
            System.Diagnostics.Debug.WriteLine($"Downloaded tiles: {progress.CountOfTilesCompleted}");
            if (progress.CountOfResourcesExpected == progress.CountOfResourcesCompleted)
            {
                System.Diagnostics.Debug.WriteLine("Download completed");
            }
        }

        async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var bounds = new LatLngBounds(
                new LatLng(37.7897, -119.5073),
                new LatLng(37.6744, -119.6815)
            );

            var offlineResion = new OfflinePackRegion
            {
                Bounds = bounds,
                MinimumZoomLevel = 10,
                MaximumZoomLevel = 20,
                StyleURL = map.MapStyle.UrlString
            };

            offlineService.OfflinePackProgressChanged += OfflineService_OfflinePackProgressChanged;
            var offlinePack = await offlineService.DownloadMap(offlineResion, new Dictionary<string, string> {
                {"regionName", "SImple Offline Region" }
            });
            if (offlinePack != null)
            {
                offlineService.RequestPackProgress(offlinePack);
            }
        }
    }
}