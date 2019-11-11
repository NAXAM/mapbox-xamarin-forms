using System.Collections.Generic;
using System.IO;
using System.Timers;
using GeoJSON.Net.Feature;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DdsChoroplethJsonVectorMix : ContentPage
    {
        private const string STATE_UNEMPLOYMENT_INFO_JSON_FILE = "MapBoxQs.geojson.state_unemployment_info.json";
        private const string VECTOR_SOURCE_NAME = "states";
        private const string DATA_MATCH_PROP = "STATE_ID";
        private const string DATA_STYLE_UNEMPLOYMENT_PROP = "unemployment";


        public DdsChoroplethJsonVectorMix()
        {
            InitializeComponent();

            map.Center = new LatLng(38.00312, -98.398423);
            map.MapStyle = MapStyle.LIGHT;
            map.ZoomLevel = 1;
            
            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            map.Functions.AddSource(new VectorSource(VECTOR_SOURCE_NAME, "mapbox://mapbox.us_census_states_2015"));

            using (var stream = GetType().Assembly.GetManifestResourceStream(STATE_UNEMPLOYMENT_INFO_JSON_FILE)) {
                using(var textReader = new StreamReader(stream)) {
                    using(var jsonReader = new JsonTextReader(textReader)) {
                        var serializer = new JsonSerializer();
                        var data = serializer.Deserialize<Dictionary<string, string>[]>(jsonReader);

                        // Create stops array
                        var stops = new Stop[data.Length];

                        for (int x = 0; x < data.Length; x++) {
                            double green = double.Parse(data[x][DATA_STYLE_UNEMPLOYMENT_PROP]) / 14 * 255;

                            // Add new stop to array of stops
                            stops[x] = Expression.CreateStop(
                              double.Parse(data[x][DATA_MATCH_PROP]),
                              Expression.Rgba(0, green, 0, 1)
                            );
                        }

                        AddDataToMap(stops);
                    }
                }
            }

        }

        /**
        * Create layer from the vector tile source with data-driven style
        *
        * @param stops the array of stops to use in the FillLayer
        */
        private void AddDataToMap(Stop[] stops) {

            FillLayer statesJoinLayer = new FillLayer("states-join", VECTOR_SOURCE_NAME);
            statesJoinLayer.SourceLayer = (VECTOR_SOURCE_NAME);
            statesJoinLayer.FillColor = (Expression.Match(Expression.ToNumber(Expression.Get(DATA_MATCH_PROP)),
                Expression.Rgba(0, 0, 0, 1), stops));

            // Add layer to map below the "waterway-label" layer
            map.Functions.AddLayerAbove(statesJoinLayer, "waterway-label");
        }
    }
}