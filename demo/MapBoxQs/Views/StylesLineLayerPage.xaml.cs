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
    public partial class StylesLineLayerPage : ContentPage
    {
        public StylesLineLayerPage()
        {
            InitializeComponent();

            map.Center = new Naxam.Mapbox.LatLng(33.39243835, -118.38265415000001);
            map.MapStyle = MapStyle.OUTDOORS;
            map.ZoomLevel = 13.448986053466797;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            var routeCoordinates = GetCoords();

            var source = new GeoJsonSource
            {
                Id = "line.src",
                Data = new FeatureCollection(
                    new List<Feature> {
                        new Feature(new LineString(routeCoordinates.Select(x => new Position(x.Lat, x.Long))))
                    })
            };

            map.Functions.AddSource(source);

            var layer = new LineLayer("line.layer", source.Id)
            {
                LineDasharray = Expression.Literal(new[] { 0.01f, 2f }),
                LineCap = Expression.Literal("round"),
                LineJoin = Expression.Literal("round"),
                LineWidth = Expression.Literal(5f),
                LineColor = Expression.Color(Color.FromHex("#e55e5e"))
            };
            map.Functions.AddLayer(layer);
        }

        List<LatLng> GetCoords()
        {
            var routeCoordinates = new List<LatLng>();
            routeCoordinates.Add(LatLng.FromLngLat(-118.39439114221236, 33.397676454651766));
            routeCoordinates.Add(LatLng.FromLngLat(-118.39421054012902, 33.39769799454838));
            routeCoordinates.Add(LatLng.FromLngLat(-118.39408583869053, 33.39761901490136));
            routeCoordinates.Add(LatLng.FromLngLat(-118.39388373635917, 33.397328225582285));
            routeCoordinates.Add(LatLng.FromLngLat(-118.39372033447427, 33.39728514560042));
            routeCoordinates.Add(LatLng.FromLngLat(-118.3930882271826, 33.39756875508861));
            routeCoordinates.Add(LatLng.FromLngLat(-118.3928216241072, 33.39759029501192));
            routeCoordinates.Add(LatLng.FromLngLat(-118.39227981785722, 33.397234885594564));
            routeCoordinates.Add(LatLng.FromLngLat(-118.392021814881, 33.397005125197666));
            routeCoordinates.Add(LatLng.FromLngLat(-118.39090810203379, 33.396814854409186));
            routeCoordinates.Add(LatLng.FromLngLat(-118.39040499623022, 33.39696563506828));
            routeCoordinates.Add(LatLng.FromLngLat(-118.39005669221234, 33.39703025527067));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38953208616074, 33.39691896489222));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38906338075398, 33.39695127501678));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38891287901787, 33.39686511465794));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38898167981154, 33.39671074380141));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38984598978178, 33.396064537239404));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38983738968255, 33.39582400356976));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38955358640874, 33.3955978295119));
            routeCoordinates.Add(LatLng.FromLngLat(-118.389041880506, 33.39578092284221));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38872797688494, 33.3957916930261));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38817327048618, 33.39561218978703));
            routeCoordinates.Add(LatLng.FromLngLat(-118.3872530598711, 33.3956265500598));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38653065153775, 33.39592811523983));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38638444985126, 33.39590657490452));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38638874990086, 33.395737842093304));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38723155962309, 33.395027006653244));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38734766096238, 33.394441819579285));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38785936686516, 33.39403972556368));
            routeCoordinates.Add(LatLng.FromLngLat(-118.3880743693453, 33.393616088784825));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38791956755958, 33.39331092541894));
            routeCoordinates.Add(LatLng.FromLngLat(-118.3874852625497, 33.39333964672257));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38686605540683, 33.39387816940854));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38607484627983, 33.39396792286514));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38519763616081, 33.39346171215717));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38523203655761, 33.393196040109466));
            routeCoordinates.Add(LatLng.FromLngLat(-118.3849955338295, 33.393023711860515));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38355931726203, 33.39339708930139));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38323251349217, 33.39305243325907));
            routeCoordinates.Add(LatLng.FromLngLat(-118.3832583137898, 33.39244928189641));
            routeCoordinates.Add(LatLng.FromLngLat(-118.3848751324406, 33.39108499551671));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38522773650804, 33.38926830725471));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38508153482152, 33.38916777794189));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38390332123025, 33.39012280171983));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38318091289693, 33.38941192035707));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38271650753981, 33.3896129783018));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38275090793661, 33.38902416443619));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38226930238106, 33.3889451769069));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38258750605169, 33.388420985121336));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38177049662707, 33.388083490107284));
            routeCoordinates.Add(LatLng.FromLngLat(-118.38080728551597, 33.38836353925403));
            routeCoordinates.Add(LatLng.FromLngLat(-118.37928506795642, 33.38717870977523));
            routeCoordinates.Add(LatLng.FromLngLat(-118.37898406448423, 33.3873079646849));
            routeCoordinates.Add(LatLng.FromLngLat(-118.37935386875012, 33.38816247841951));
            routeCoordinates.Add(LatLng.FromLngLat(-118.37794345248027, 33.387810620840135));
            routeCoordinates.Add(LatLng.FromLngLat(-118.37546662390886, 33.38847843095069));
            routeCoordinates.Add(LatLng.FromLngLat(-118.37091717142867, 33.39114243958559));
            return routeCoordinates;
        }
    }
}