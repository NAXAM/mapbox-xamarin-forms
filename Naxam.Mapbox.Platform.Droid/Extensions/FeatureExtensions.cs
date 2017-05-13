using System;
using Com.Mapbox.Services.Commons.Geojson;
using Point = Com.Mapbox.Services.Commons.Geojson.Point;
using Position = Com.Mapbox.Services.Commons.Models.Position;
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

                forms = new PointFeature (new PointAnnotation {
                    Id = feature.Id,
                    Coordinate = point.Coordinates.ToForms ()
                });
            } else if (feature.Geometry is LineString) {
                var line = (LineString)feature.Geometry;
                var coords = line.Coordinates
                                  .Select (ToForms)
                                  .ToArray ();

                forms = new PolylineFeature (new PolylineAnnotation {
                    Id = feature.Id,
                    Coordinates = coords
                });
            } else if (feature.Geometry is MultiLineString) {
                var line = (MultiLineString)feature.Geometry;
                var coords = line.Coordinates
                                  .Select (x => x.Select(ToForms).ToArray())
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
