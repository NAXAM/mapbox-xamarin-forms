using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
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
    public partial class StylesRotatingTextAnchorPosition : ContentPage
    {
        private static readonly string[] anchorOptions = new String[]
        {
            LayerProperty.TEXT_ANCHOR_CENTER, // The center of the text is placed closest to the anchor.
            LayerProperty.TEXT_ANCHOR_LEFT, // The left side of the text is placed closest to the anchor.
            LayerProperty.TEXT_ANCHOR_RIGHT, // The right side of the text is placed closest to the anchor.
            LayerProperty.TEXT_ANCHOR_TOP, // The top of the text is placed closest to TapGestureRecognizer_OnTappedProperty.TEXT_ANCHOR_BOTTOM, // The bottom of the text is placed closest to the anchor.
            LayerProperty.TEXT_ANCHOR_TOP_LEFT, // The top left corner of the text is placed closest to the anchor.
            LayerProperty.TEXT_ANCHOR_TOP_RIGHT, // The top right corner of the text is placed closest to the anchor.
            LayerProperty
                .TEXT_ANCHOR_BOTTOM_LEFT, // The bottom left corner of the text is placed closest to the anchor.
            LayerProperty
                .TEXT_ANCHOR_BOTTOM_RIGHT, // The bottom right corner of the text is placed closest to the anchor.
        };

        private static readonly string[] anchorDescriptions = new String[]
        {
            "The center of the text is placed closest to the anchor.",
            "The left side of the text is placed closest to the anchor.",
            "The right side of the text is placed closest to the anchor.",
            "The top of the text is placed closest to the anchor.",
            "The bottom of the text is placed closest to the anchor.",
            "The top left corner of the text is placed closest to the anchor.",
            "The top right corner of the text is placed closest to the anchor.",
            "The bottom left corner of the text is placed closest to the anchor.",
            "The bottom right corner of the text is placed closest to the anchor."
        };

        private int index = 0;

        public StylesRotatingTextAnchorPosition()
        {
            InitializeComponent();

            map.Center = new Naxam.Mapbox.LatLng(32.562100, -101.264445);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 3.4;
            map.ZoomMaxLevel = 5.38;
            map.ZoomMinLevel = 3.4;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        
        SymbolLayer stateLayer = new SymbolLayer("state-label", "built-in")
        {
            TextIgnorePlacement = true,
            TextAllowOverlap = true
        };
        
        private void HandleStyleLoaded(MapStyle obj)
        {   
            ChangeAnchor(index % anchorDescriptions.Length);
        }

        void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            ChangeAnchor(index % anchorDescriptions.Length);
        }

        void ChangeAnchor(int anchorIndex)
        {
            stateLayer.TextAnchor = anchorOptions[anchorIndex];
            map.Functions.UpdateLayer(stateLayer);

            txtState.Text = $"State-label layer's anchor position: {anchorOptions[anchorIndex]} {anchorDescriptions[anchorIndex]} \n (Tap ME to change)";
            index++;
        }
    }
}