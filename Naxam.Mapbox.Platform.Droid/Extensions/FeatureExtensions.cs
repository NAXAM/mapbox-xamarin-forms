using System;
using Com.Mapbox.Geojson;
using Point = Com.Mapbox.Geojson.Point;
using Naxam.Controls.Forms;
using System.Linq;
using Android.Graphics;
using System.Collections.Generic;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public static class FeatureExtensions
    {
        public static IFeature ToFeature(this Feature feature)
        {
            if (feature == null)
            {
                return null;
            }

            IFeature forms = null;

            if (feature.Geometry() is Point point)
            {
                var cords = point.Coordinates();
                forms = new PointFeature(new PointAnnotation
                {
                    Id = feature.Id(),
                    Coordinate = new Position
                    {
                        Lat = cords[0].DoubleValue(),
                        Long = cords[1].DoubleValue()
                    }
                });
            }
            else if (feature.Geometry() is LineString line)
            {
                var coords = line.Coordinates()
                                  .Select(ToForms)
                                  .ToArray();

                forms = new PolylineFeature(new PolylineAnnotation
                {
                    Id = feature.Id(),
                    Coordinates = coords
                });
            }
            else if (feature.Geometry() is MultiLineString mline)
            {
                var coords = mline.Coordinates()
                                  .Select(x => x.Select(ToForms).ToArray())
                                  .ToArray();

                forms = new MultiPolylineFeature(new MultiPolylineAnnotation
                {
                    Id = feature.Id(),
                    Coordinates = coords
                });
            }

            if (forms != null)
            {
                var json = feature.Properties().ToString();

                forms.Attributes = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }

            return forms;
        }

        public static Position ToForms(this Point position)
        {
            return new Position
            {
                Lat = position.Latitude(),
                Long = position.Longitude()
            };
        }

        public static RectF ToRect(this Xamarin.Forms.Point point, double radius)
        {
            return new RectF(
                            (float)(point.X - radius),
                            (float)(point.Y - radius),
                            (float)(point.X + radius),
                            (float)(point.Y + radius));
        }

        public static IEnumerable<T> Cast<T>(this Android.Runtime.JavaList list)
        {
            return list.ToArray().Cast<T>();
        }
    }
}
