using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public partial class DdsExpressionIntegration : ContentPage
    {
        private const string GEOJSON_SRC_ID = "extremes_source_id";
        private const string MIN_TEMP_LAYER_ID = "min_temp_layer_id";
        private const string MAX_TEMP_LAYER_ID = "max_temp_layer_id";
        private const string RED_PIN_IMAGE_ID = "red_pin_id";
        private const string BLUE_PIN_IMAGE_ID = "blue_pin_id";
        private const string DEGREES_C = "℃"; //"\u2103";
        private const string DEGREES_F = "℉"; //"\u2109";
        private readonly List<State> states = new List<State>();
        private bool isImperial = true;

        /**
       * weather_data_per_state_before2006.geojson file (found in assets)
       * contains various weather related records per state/territory.
       * Once that file is parsed we will keep a list of states (with name and bounds info).
       * We need bounds of for data shown on map per state.
       * We first find out all latLng points per state to be shown, than we create a LatLngBounds
       * using those points.
       */
        private class State
        {
            List<LatLng> latLongs;
            public LatLngBounds bounds;
            public string name;

            public State(string name, LatLng latLng)
            {
                this.name = name;
                latLongs = new List<LatLng>();
                latLongs.Add(latLng);
                bounds = new LatLngBounds(latLng, latLng);
            }

            public void Add(LatLng latLng)
            {
                latLongs.Add(latLng);
                bounds = LatLngBounds.FromLatLngs(latLongs);
            }
        }

        public DdsExpressionIntegration()
        {
            InitializeComponent();

            map.Center = new LatLng(40, -100);
            map.MapStyle = MapStyle.STREETS;
            map.ZoomLevel = 6;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            SetUpMapImagePins();

            using (var stream = GetType().Assembly.GetManifestResourceStream("MapBoxQs.geojson.weather_data_per_state_before2006.geojson")) {
                using (var textReader = new StreamReader(stream)) {
                    using (var jsonReader = new JsonTextReader(textReader)) {
                        var serializer = new JsonSerializer();
                        var featureCollection = serializer.Deserialize<FeatureCollection>(jsonReader);

                        // Find out the states represented in the FeatureCollection
                        // and bounds of the extreme conditions
                        foreach (Feature feature in featureCollection.Features) {
                            var stateName = feature.Properties["state"].ToString();
                            var lat = feature.Properties["latitude"].ToString();
                            var lon = feature.Properties["longitude"].ToString();

                            var latLng = new LatLng(double.Parse(lat), double.Parse(lon));

                            State state = null;
                            foreach (State curState in states) {
                                if (curState.name.Equals(stateName)) {
                                    state = curState;
                                    break;
                                }
                            }
                            if (state == null) {
                                states.Add(CreateState(stateName, latLng));
                            }
                            else {
                                state.Add(latLng);
                            }
                        }

                        AddDataToMap(featureCollection);
                        PopulateMenu();
                    }
                }
            }
        }


        private void AddDataToMap(FeatureCollection featureCollection)
        {
            // Retrieves GeoJSON from local file and adds it to the map
            var geoJsonSource = new GeoJsonSource(GEOJSON_SRC_ID, featureCollection);
            map.Functions.AddSource(geoJsonSource);

            InitTemperatureLayers();

            PopulateMenu();

            // show Connecticut by default
            int indexOfState = IndexOfState("Connecticut");
            SelectState(states[indexOfState].name, indexOfState);
        }


        private State CreateState(string stateName, LatLng latLng)
        {
            return new State(stateName, latLng);
        }

        /**
         * Adds the marker image to the map for use as a SymbolLayer icon
         */
        private void SetUpMapImagePins()
        {
            map.Functions.AddStyleImage(new IconImageSource
            {
                Id = RED_PIN_IMAGE_ID,
                Source = "red_marker.png",
            });

            map.Functions.AddStyleImage(new IconImageSource
            {
                Id = BLUE_PIN_IMAGE_ID,
                Source = "blue_marker.png",
            });
        }

        private void InitTemperatureLayers()
        {
            // Adds a SymbolLayer to display maximum temperature in state
            var maxTempLayer = new SymbolLayer(MAX_TEMP_LAYER_ID, GEOJSON_SRC_ID)
            {
                IconImage = (RED_PIN_IMAGE_ID),
                TextField = (GetTemperatureValue()),
                TextSize = (17f),
                TextOffset = (new float[] { 0f, -1.75f }),
                TextColor = (Color.Red),
                TextAllowOverlap = (true),
                TextIgnorePlacement = (true),
                IconAllowOverlap = (true),
                IconIgnorePlacement = (true),
                Filter = (Expression.Eq(Expression.Get("element"), Expression.Literal("All-Time Maximum Temperature")))
            };
            map.Functions.AddLayer(maxTempLayer);

            // Adds a SymbolLayer to display minimum temperature in state
            var minTempLayer = new SymbolLayer(MIN_TEMP_LAYER_ID, GEOJSON_SRC_ID)
            {
                IconImage = BLUE_PIN_IMAGE_ID,
                TextField = GetTemperatureValue(),
                TextSize = 17f,
                TextOffset = (new float[] { 0f, -2.5f }),
                TextColor = Color.Blue,
                TextAllowOverlap = true,
                TextIgnorePlacement = true,
                IconAllowOverlap = true,
                IconIgnorePlacement = true,
                Filter = Expression.Eq(Expression.Get("element"), Expression.Literal("All-Time Minimum Temperature"))
            };
            map.Functions.AddLayer(minTempLayer);

            unitsText.Text = (isImperial ? DEGREES_C : DEGREES_F);
        }

        void HandleTap(object sender, EventArgs e)
        {
            ChangeTemperatureUnits(!isImperial);
        }


        private void ChangeTemperatureUnits(bool v)
        {
            if (this.isImperial != v) {
                this.isImperial = v;

                // Apply new units to the data displayed in text fields of SymbolLayers
                var maxTempLayer = new SymbolLayer(MAX_TEMP_LAYER_ID, GEOJSON_SRC_ID)
                {
                    TextField = (GetTemperatureValue())
                };
                map.Functions.UpdateLayer(maxTempLayer);

                var minTempLayer = new SymbolLayer(MIN_TEMP_LAYER_ID, GEOJSON_SRC_ID)
                {
                    TextField = (GetTemperatureValue())
                };
                map.Functions.UpdateLayer(minTempLayer);

                unitsText.Text = (v ? DEGREES_C : DEGREES_F);
            }
        }

        private Expression GetTemperatureValue()
        {
            if (isImperial) {
                return Expression.Concat(Expression.Get("value"), Expression.Literal(DEGREES_F)); // For imperial we just need to add "F"
            }

            Expression value = Expression.ToNumber(Expression.Get("value"));  // value --> Number
            value = Expression.Subtract(value, Expression.ToNumber(32.0)); // value - 32
            value = Expression.Product(value, Expression.ToNumber(5.0)); // value * 5
            value = Expression.Division(value, Expression.ToNumber(9.0)); // value / 9
            value = Expression.Round(value); // round to nearest int
            return Expression.Concat(Expression.ToString(value), DEGREES_C); // add C at the end
        }


        private void PopulateMenu()
        {
            var items = states.Select(x => x.name).ToList();
            statesPicker.ItemsSource = items;
            statesPicker.SelectedItem = items[0];
        }

        void HandleStateSelected(object sender, EventArgs e)
        {
            if (statesPicker.SelectedIndex < 0) return;

            var state = states[statesPicker.SelectedIndex];
            var index = statesPicker.SelectedIndex;
            SelectState(state.name, index);
        }

        private void SelectState(string stateName, int stateIndex)
        {
            if (IndexOfState(stateName) == stateIndex) {
                // Adds a SymbolLayer to display maximum temperature in state
                SymbolLayer maxTempLayer = new SymbolLayer(MAX_TEMP_LAYER_ID, GEOJSON_SRC_ID)
                {
                    Filter = Expression.All(
                      Expression.Eq(Expression.Get("element"), ("All-Time Maximum Temperature")),
                      Expression.Eq(Expression.Get("state"), (stateName))
                    )
                };
                map.Functions.UpdateLayer(maxTempLayer);

                // Adds a SymbolLayer to display minimum temperature in state
                SymbolLayer minTempLayer = new SymbolLayer(MIN_TEMP_LAYER_ID, GEOJSON_SRC_ID)
                {
                    Filter = (Expression.All(
                      Expression.Eq(Expression.Get("element"), ("All-Time Minimum Temperature")),
                      Expression.Eq(Expression.Get("state"), (stateName))))
                };
                map.Functions.UpdateLayer(minTempLayer);

                var cameara = new CameraBounds(states[stateIndex].bounds, 100);
                map.Functions.AnimateCamera(cameara, 100);
            }
        }

        private int IndexOfState(string name)
        {
            if (name != null) {
                for (int i = 0; i < states.Count; i++) {
                    if (name == (states[i].name)) {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}