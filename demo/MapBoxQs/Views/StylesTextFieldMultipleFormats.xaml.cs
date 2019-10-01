using System.Timers;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesTextFieldMultipleFormats : ContentPage
    {
        public StylesTextFieldMultipleFormats()
        {
            InitializeComponent();

            map.Center = new LatLng(48.4, 18.49);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 4.5;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var bigLabel = Expression.CreateFormatEntry(
                Expression.Get("name_en"),
                FormatOption.FormatFontScale(1.5),
                FormatOption.FormatTextColor(Color.Blue),
                FormatOption.FormatTextFont(new [] {"Ubuntu Medium", "Arial Unicode MS Regular"})
            );

            var newLine = Expression.CreateFormatEntry(
                // Add "\n" in order to break the line and have the second label underneath
                "\n",
                FormatOption.FormatFontScale(0.5)
            );

            var smallLabel = Expression.CreateFormatEntry(
                Expression.Get("name"),
                FormatOption.FormatTextColor(Color.FromHex("#d6550a")),
                FormatOption.FormatTextFont(new [] {"Caveat Regular", "Arial Unicode MS Regular"})
            );

            var format = Expression.Format(bigLabel, newLine, smallLabel);

            // Retrieve the country label layers from the style and update them with the formatting expression
            foreach (var mapLabelLayer in map.Functions.GetLayers()) {
                if (mapLabelLayer.Id.Contains("country-label")
                && mapLabelLayer is SymbolLayer symbolLayer) {
                    // Apply formatting expression
                    symbolLayer.TextField = format;
                    map.Functions.UpdateLayer(symbolLayer);
                }
            }
        }
    }
}