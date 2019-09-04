using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NxSource = Naxam.Mapbox.Sources.Source;
using NxRasterSource = Naxam.Mapbox.Sources.RasterSource;
using Sdk = Com.Mapbox.Mapboxsdk;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer
    {

        void OnShapeSourcesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddSources(e.NewItems.Cast<NxSource>().ToList());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveSources(e.OldItems.Cast<NxSource>().ToList());
                    break;
                case NotifyCollectionChangedAction.Reset:
                    var sources = map.Style.Sources;
                    foreach (var source in sources)
                    {
                        if (source.Id.HasPrefix())
                        {
                            map.Style.RemoveSource(source);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveSources(e.OldItems.Cast<NxSource>().ToList());
                    AddSources(e.NewItems.Cast<NxSource>().ToList());
                    break;
            }
        }

        void AddSources(List<NxSource> sources)
        {
            if (sources == null || map == null)
            {
                return;
            }

            foreach (NxSource source in sources)
            {
                if (string.IsNullOrWhiteSpace(source.Id))
                {
                    continue;
                }

                //switch(source)
                //{
                //    case NxShapeSource shapeSrc:
                //        if (shapeSrc.Shape == null) continue;

                //        var shape = shapeSource.Shape.ToFeatureCollection();

                //        var source = map.Style.GetSource(shapeSource.Id.Prefix()) as Sdk.Style.Sources.GeoJsonSource;

                //        if (source == null)
                //        {
                //            source = new Sdk.Style.Sources.GeoJsonSource(shapeSource.Id.Prefix(), shape);
                //            map.Style.AddSource(source);
                //        }
                //        else
                //        {
                //            source.SetGeoJson(shape);
                //        }
                //        break;
                //}

                //if (source is RasterSource rs)
                //    {
                //        var source = map.Style.GetSource(rs.Id);
                //        if (source == null)
                //        {
                //            Sdk.Style.Sources.RasterSource rasterSource = new Sdk.Style.Sources.RasterSource(rs.Id, rs.ConfigurationURL, (int)rs.TileSize);
                //            map.Style.AddSource(rasterSource);
                //        }
                //    }
            }
        }

        void RemoveSources(List<NxSource> sources)
        {
            if (sources == null)
            {
                return;
            }
            foreach (NxSource source in sources)
            {
                if (source.Id != null)
                {
                    map.Style.RemoveSource(source.Id.Prefix());
                }
            }
        }

    }
}