using System.Collections.Generic;
using System.Timers;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Point = Xamarin.Forms.Point;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesMissingIcon : ContentPage
    {
        const string ICON_SOURCE_ID = "ICON_SOURCE_ID";
        const string ICON_LAYER_ID = "ICON_LAYER_ID";
        const string PROFILE_NAME = "PROFILE_NAME";
        const string CARLOS = "Carlos";
        const string ANTONY = "Antony";
        const string MARIA = "Maria";
        const string LUCIANA = "Luciana";

        public StylesMissingIcon()
        {
            InitializeComponent();

            map.Center = new LatLng(39.6226149, -8.4155273);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 6;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
            
            //TODO  No equivalent event for iOS
            map.StyleImageMissingCommand = new Command<string>(HandleMissingStyleImage);
        }

        private void HandleMissingStyleImage(string obj)
        {
            map.Functions.AddStyleImage(new IconImageSource
            {
                Id = obj,
                Source = obj.ToLower() + ".png"
            });
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            // Add Features which represent the location of each profile photo SymbolLayer icon
            Feature carlosFeature = new Feature(new GeoJSON.Net.Geometry.Point(new Position(41.2778064, -7.9760742)));
            carlosFeature.Properties.Add(PROFILE_NAME, CARLOS);

            Feature antonyFeature = new Feature(new GeoJSON.Net.Geometry.Point(new Position(37.5445773, -8.0639648
            )));
            antonyFeature.Properties.Add(PROFILE_NAME, ANTONY);

            Feature mariaFeature = new Feature(new GeoJSON.Net.Geometry.Point(new Position(38.9764924, -9.1845703
            )));
            mariaFeature.Properties.Add(PROFILE_NAME, MARIA);

            Feature lucianaFeature = new Feature(new GeoJSON.Net.Geometry.Point(new Position(40.2459915, -7.5146484)));
            lucianaFeature.Properties.Add(PROFILE_NAME, LUCIANA);

            var source = new GeoJsonSource(ICON_SOURCE_ID,
                new FeatureCollection(new List<Feature>
                {
                    carlosFeature,
                    antonyFeature,
                    mariaFeature,
                    lucianaFeature
                }));
            map.Functions.AddSource(source);

            map.Functions.AddLayer(new SymbolLayer(ICON_LAYER_ID, ICON_SOURCE_ID)
            {
                IconImage = Expression.Get(PROFILE_NAME),
                IconIgnorePlacement = true,
                IconAllowOverlap = true,
                TextField = Expression.Get(PROFILE_NAME),
                TextIgnorePlacement = true,
                TextAllowOverlap = true,
                TextOffset = new[] {0f, 2f}
            });
        }
    }
}