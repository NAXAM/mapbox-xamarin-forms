using Naxam.Mapbox.Forms;
using Geojson = Mapbox.Services.Commons.Geojson;

using System.Collections.Generic;
using System.Linq;

namespace Naxam.Controls.Platform.Droid
{
    public static class AnnotationExtensions
    {
        public static Geojson.FeatureCollection ToFeatureCollection(this Annotation annotation)
        {
            var list = new List<Geojson.Feature>();

            if (annotation is PointAnnotation)
            {
                var pa = (PointAnnotation)annotation;
                list.Add(pa.ToFeature());
            }
            else if (annotation is PolylineAnnotation)
            {
                var pa = (PolylineAnnotation)annotation;
                list.Add(pa.ToFeature());
            }
            else if (annotation is MultiPolylineAnnotation)
            {
                var pa = (MultiPolylineAnnotation)annotation;
                list.Add(pa.ToFeature());
            }

            return Geojson.FeatureCollection.FromFeatures(list.ToArray());
        }

        public static Geojson.Feature ToFeature(this PointAnnotation annotation)
        {
            //var coords = new[] { annotation.Coordinate.Long, annotation.Coordinate.Lat };
            var coords = ToCoords(annotation.Coordinate);
            var geometry = Geojson.Point.FromCoordinates(coords);
            return Geojson.Feature.FromGeometry(geometry);
        }

        public static Geojson.Feature ToFeature(this PolylineAnnotation annotation)
        {
            var coords = annotation.Coordinates
                                   .Select(position => ToCoords(position))
                                  .ToArray();

            var geometry = Geojson.LineString.FromCoordinates(coords);
            return Geojson.Feature.FromGeometry(geometry);
        }

        public static Geojson.Feature ToFeature(this MultiPolylineAnnotation annotation)
        {
            var coords = annotation.Coordinates
                                   .Select(pp => pp.Select(position => ToCoords(position)).ToArray())
                                   .ToArray();

            var geometry = Geojson.MultiLineString.FromCoordinates(coords);

            return Geojson.Feature.FromGeometry(geometry);
        }

        public static double[] ToCoords(Position pos)
        {
            return new[] { pos.Long, pos.Lat };
        }
    }

}
