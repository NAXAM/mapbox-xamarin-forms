//using Naxam.Controls.Forms;

//using System.Collections.Generic;
//using System.Linq;
//using Com.Mapbox.Mapboxsdk.Annotations;
//using FAnnotation = Naxam.Controls.Forms.Annotation;
//using Android.Content;
//using Com.Mapbox.Geojson;

//namespace Naxam.Controls.Mapbox.Platform.Droid
//{
//    public static class AnnotationExtensions
//    {
//        public static Icon GetIconFromResource(this Context context, string name)
//        {
//            var resId = context.Resources.GetIdentifier(name, "drawable", context.PackageName);
//            if (resId == 0)
//                return null;
//            IconFactory iconFactory = IconFactory.GetInstance(context);
//            return iconFactory.FromResource(resId);
//        }

//        public static FeatureCollection ToFeatureCollection(this FAnnotation annotation)
//        {
//            var list = new List<Feature>();

//            if (annotation is PointAnnotation)
//            {
//                var pa = (PointAnnotation)annotation;
//                list.Add(pa.ToFeature());
//            }
//            else if (annotation is PolylineAnnotation)
//            {
//                var pa = (PolylineAnnotation)annotation;
//                list.Add(pa.ToFeature());
//            }
//            else if (annotation is MultiPolylineAnnotation)
//            {
//                var pa = (MultiPolylineAnnotation)annotation;
//                list.Add(pa.ToFeature());
//            }

//            return FeatureCollection.FromFeatures(list.ToArray());
//        }

//        public static Feature ToFeature(this PointAnnotation annotation)
//        {
//            //var coords = new[] { annotation.Coordinate.Long, annotation.Coordinate.Lat };
//            var coords = ToCoords(annotation.Coordinate);
//            var geometry = Com.Mapbox.Geojson.Point.FromLngLat(annotation.Coordinate.Long, annotation.Coordinate.Lat);
//            return Feature.FromGeometry(geometry);
//        }

//        public static Feature ToFeature(this PolylineAnnotation annotation)
//        {
//            var coords = annotation.Coordinates
//                                   .Select(position => ToCoords(position))
//                                  .ToArray();

//            var geometry = LineString.FromLngLats(MultiPoint.FromLngLats(coords));
//            return Feature.FromGeometry(geometry);
//        }

//        public static Feature ToFeature(this MultiPolylineAnnotation annotation)
//        {
//            var coords = annotation.Coordinates
//                                   .Select(pp => pp.Select(position => ToCoords(position)).ToArray())
//                                   .ToArray();

//            var geometry = MultiLineString.FromLngLats(coords);

//            return Feature.FromGeometry(geometry);
//        }

//        public static Point ToCoords(Forms.Point pos)
//        {
//            return Com.Mapbox.Geojson.Point.FromLngLat(pos.Long, pos.Lat);
//        }
//    }

//}
