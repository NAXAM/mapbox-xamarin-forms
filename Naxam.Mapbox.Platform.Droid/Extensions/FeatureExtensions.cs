using System;
using Mapbox.Services.Commons.Geojson;
using Point = Mapbox.Services.Commons.Geojson.Point;
using Position = Mapbox.Services.Commons.Models.Position;
using Naxam.Mapbox.Forms;
using System.Linq;
using Android.Graphics;
using System.Collections.Generic;

namespace Naxam.Controls.Platform.Droid
{
    public static class FeatureExtensions
    {
        public static IFeature ToFeature (this Feature feature)
        {
            if (feature == null) {
                return null;
            }

            IFeature forms = null;

            if (feature.Geometry is Point) {
                var point = (Point)feature.Geometry;
                var position = (Position)point.Coordinates;

                forms = new PointFeature (new PointAnnotation {
                    Id = feature.Id,
                    Coordinate = position.ToForms ()
                });
            } else if (feature.Geometry is LineString) {
                var line = (LineString)feature.Geometry;
                var positions = (Android.Runtime.JavaList)line.Coordinates;
                var coords = positions.Cast<Position>()
                                      .Select (ToForms)
                                      .ToArray ();

                forms = new PolylineFeature (new PolylineAnnotation {
                    Id = feature.Id,
                    Coordinates = coords
                });
            } else if (feature.Geometry is MultiLineString) {
                var line = (MultiLineString)feature.Geometry;
                var positions = (Android.Runtime.JavaList)line.Coordinates;
                var coords = positions.Cast<Android.Runtime.JavaList>()
                                      .Select (x => x.Cast<Position>().Select(ToForms).ToArray())
                                      .ToArray ();

                forms = new MultiPolylineFeature (new MultiPolylineAnnotation {
                    Id = feature.Id,
                    Coordinates = coords
                });
            }

            if (forms != null) {
                var json = feature.Properties.ToString();

                forms.Attributes = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }

            return forms;
        }

        public static Mapbox.Forms.Position ToForms (this Position position)
        {
            return new Mapbox.Forms.Position {
                Lat = position.Latitude,
                Long = position.Longitude
            };
        }

        public static RectF ToRect (this Xamarin.Forms.Point point, double radius)
        {
            return new RectF (
                            (float)(point.X - radius),
                            (float)(point.Y - radius),
                            (float)(point.X + radius),
                            (float)(point.Y + radius));
        }

        public static IEnumerable<T> Cast<T> (this Android.Runtime.JavaList list) {
            return list.ToArray ().Cast<T> ();
        }
    }
}
