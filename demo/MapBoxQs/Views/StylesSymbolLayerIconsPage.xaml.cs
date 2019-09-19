using Acr.UserDialogs;
using GeoJSON.Net.Feature;
using MapBoxQs.Services;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MapBoxQs.Views
{
    public partial class StylesSymbolLayerIconsPage : ContentPage
    {
        public StylesSymbolLayerIconsPage()
        {
            InitializeComponent();

            Title = "Symbol layer icons";

            map.Center = new LatLng(-33.213144 , - 57.225365);
            map.MapStyle = new MapStyle("mapbox://styles/mapbox/cjf4m44iw0uza2spb3q0a7s41");

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        void HandleStyleLoaded(MapStyle obj)
        {
            IconImageSource iconImageSource = (ImageSource)"red_marker.png";
            map.Functions.AddStyleImage(iconImageSource);

            var symbolLayerIconFeatureList = new List<Feature>();
            symbolLayerIconFeatureList.Add(
                new Feature(new GeoJSON.Net.Geometry.Point(
                    new GeoJSON.Net.Geometry.Position(-33.213144, -57.225365))));
            symbolLayerIconFeatureList.Add(
                new Feature(new GeoJSON.Net.Geometry.Point(
                    new GeoJSON.Net.Geometry.Position(-33.981818, -54.14164))));
            symbolLayerIconFeatureList.Add(
                new Feature(new GeoJSON.Net.Geometry.Point(
                    new GeoJSON.Net.Geometry.Position(-30.583266, -56.990533))));

            var featureCollection = new FeatureCollection(symbolLayerIconFeatureList);

            var source = new GeoJsonSource
            {
                Id = "feature.memory.src",
                Data = featureCollection
            };
            map.Functions.AddSource(source);

            var symbolLayer = new SymbolLayer("feature.symbol.layer", source.Id)
            {
                IconAllowOverlap = Expression.Literal(true),
                IconImage = Expression.Literal(iconImageSource.Id),
                IconOffset = Expression.Literal(new[] { 0.0, -9.0 })
            };
            map.Functions.AddLayer(symbolLayer);
        }

        async void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            var buttons = MapBoxService.DefaultStyles.Select((arg) => arg.Name).ToArray();
            var choice = await UserDialogs.Instance.ActionSheetAsync("Change style", "Cancel", "Reload current style", buttons: buttons);

            if (buttons.Contains(choice))
            {
                map.MapStyle = MapBoxService.DefaultStyles.FirstOrDefault((arg) => arg.Name == choice);
            }
        }
    }
}
