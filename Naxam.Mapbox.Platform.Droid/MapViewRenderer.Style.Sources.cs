using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Naxam.Controls.Forms;
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
                    AddSources(e.NewItems.Cast<MapSource>().ToList());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveSources(e.OldItems.Cast<MapSource>().ToList());
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
                    RemoveSources(e.OldItems.Cast<MapSource>().ToList());
                    AddSources(e.NewItems.Cast<MapSource>().ToList());
                    break;
            }
        }

        void AddSources(List<MapSource> sources)
        {
            if (sources == null || map == null)
            {
                return;
            }

            foreach (MapSource mapSource in sources)
            {
                if (mapSource.Id != null)
                {
                    if (mapSource is ShapeSource shapeSource && shapeSource.Shape != null)
                    {
                        var shape = shapeSource.Shape.ToFeatureCollection();

                        var source = map.Style.GetSource(shapeSource.Id.Prefix()) as Sdk.Style.Sources.GeoJsonSource;

                        if (source == null)
                        {
                            source = new Sdk.Style.Sources.GeoJsonSource(shapeSource.Id.Prefix(), shape);
                            map.Style.AddSource(source);
                        }
                        else
                        {
                            source.SetGeoJson(shape);
                        }
                    }
                    else if (mapSource is RasterSource rs)
                    {
                        var source = map.Style.GetSource(rs.Id);
                        if (source == null)
                        {
                            Sdk.Style.Sources.RasterSource rasterSource = new Sdk.Style.Sources.RasterSource(rs.Id, rs.ConfigurationURL, (int)rs.TileSize);
                            map.Style.AddSource(rasterSource);
                        }
                    }
                }
            }
        }

        void RemoveSources(List<MapSource> sources)
        {
            if (sources == null)
            {
                return;
            }
            foreach (MapSource source in sources)
            {
                if (source.Id != null)
                {
                    map.Style.RemoveSource(source.Id.Prefix());
                }
            }
        }

    }
}