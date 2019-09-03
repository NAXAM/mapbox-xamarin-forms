using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Naxam.Controls.Mapbox.Forms;
using Sdk = Com.Mapbox.Mapboxsdk;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer
    {
        void OnLayersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddLayers(e.NewItems.Cast<Layer>().ToList());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveLayers(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    var layers = map.Style.Layers;
                    foreach (var layer in layers)
                    {
                        if (layer.Id.HasPrefix())
                        {
                            map.Style.RemoveLayer(layer);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveLayers(e.OldItems);
                    AddLayers(e.NewItems.Cast<Layer>().ToList());
                    break;
            }
        }

        void RemoveLayers(System.Collections.IList layers)
        {
            if (layers == null)
            {
                return;
            }
            foreach (Layer layer in layers)
            {
                var native = map.Style.GetLayer(layer.Id.Prefix());

                if (native != null)
                {
                    map.Style.RemoveLayer(native);
                }
            }
        }

        void AddLayers(List<Naxam.Controls.Mapbox.Forms.Layer> layers)
        {
            if (layers == null)
            {
                return;
            }
            foreach (Layer layer in layers)
            {
                if (string.IsNullOrEmpty(layer.Id))
                {
                    continue;
                }

                map.Style.RemoveLayer(layer.Id.Prefix());

                if (layer is CircleLayer)
                {
                    var cross = (CircleLayer)layer;

                    var source = map.Style.GetSource(cross.SourceId.Prefix());
                    if (source == null)
                    {
                        continue;
                    }

                    map.Style.AddLayer(cross.ToNative());
                }
                else if (layer is LineLayer)
                {
                    var cross = (LineLayer)layer;

                    var source = map.Style.GetSource(cross.SourceId.Prefix());
                    if (source == null)
                    {
                        continue;
                    }

                    map.Style.AddLayer(cross.ToNative());
                }
                else if (layer is RasterLayer cross)
                {
                    var source = map.Style.GetSource(cross.SourceId);
                    if (source == null)
                    {
                        continue;
                    }

                    map.Style.AddLayer(cross.ToNative());
                }
            }
        }
    }
}