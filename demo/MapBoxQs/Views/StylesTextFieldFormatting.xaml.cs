using System;
using System.Timers;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesTextFieldFormatting : ContentPage
    {
        static readonly string[] textFonts = new string[] {"Roboto Black", "Arial Unicode MS Regular"};
        static readonly float[] textSizes = new float[] {25f, 13f};
        static readonly Color[] colors = new Color[] { Color.FromHex("#00FF08"), Color.FromHex("#ffd43a")};
        static readonly string WATER_RELATED_LAYER = "water-";
        static readonly string WATER_SHADOW_LAYER_ID = "water-shadow";

        private int fontIndex, colorIndex, sizeIndex;

        public StylesTextFieldFormatting()
        {
            InitializeComponent();

            map.Center = new LatLng(19.948045, -84.654463);
            map.MapStyle = MapStyle.DARK;
            map.ZoomLevel = 3.371717;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            
        }
        
        private void AdjustText(Action<SymbolLayer> action)
        {
            if (map.Functions == null) return;
            
            var layers = map.Functions.GetLayers();

            foreach (var singleMapLayer in layers)
            {
                if (singleMapLayer is SymbolLayer symbolLayer 
                    && singleMapLayer.Id.Contains(WATER_RELATED_LAYER)
                    && !singleMapLayer.Id.Equals(WATER_SHADOW_LAYER_ID))
                {
                    action(symbolLayer);
                    map.Functions.UpdateLayer(symbolLayer);
                }
            }
        }

        private void ChangeFont(object sender, EventArgs e)
        {
            fontIndex++;
            AdjustText(symbolLayer => symbolLayer.TextFont = new [] {textFonts[fontIndex % 2]});
        }
        private void ChangeSize(object sender, EventArgs e)
        {
            sizeIndex++;
            AdjustText(symbolLayer => symbolLayer.TextSize = textSizes[sizeIndex % 2]);
        }
        
        private void ChangeColor(object sender, EventArgs e)
        {
            colorIndex++;
            AdjustText(symbolLayer => symbolLayer.TextColor = colors[colorIndex % 2]);   
        }
    }
}