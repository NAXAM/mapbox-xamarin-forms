using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Foundation;
using Mapbox;
using Naxam.Controls.Forms;
using Naxam.Controls.Mapbox.Platform.iOS;
using UIKit;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]
namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer : ViewRenderer<MapView, MGLMapView>, IMGLMapViewDelegate, IUIGestureRecognizerDelegate
    {
        protected virtual void UpdateMapStyle()
        {
            if (Element.MapStyle != null && !string.IsNullOrEmpty(Element.MapStyle.UrlString))
            {
                map.StyleURL = new NSUrl(Element.MapStyle.UrlString);

                Element.MapStyle.PropertyChanging -= OnMapStylePropertyChanging;
                Element.MapStyle.PropertyChanged -= OnMapStylePropertyChanged;
                Element.MapStyle.PropertyChanging += OnMapStylePropertyChanging;
                Element.MapStyle.PropertyChanged += OnMapStylePropertyChanged;
            }
        }

        void OnMapStylePropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            //if (e.PropertyName == MapStyle.CustomSourcesProperty.PropertyName
            //    && (sender as MapStyle).CustomSources != null)
            //{
            //    var notifiyCollection = (sender as MapStyle).CustomSources as INotifyCollectionChanged;
            //    if (notifiyCollection != null)
            //    {
            //        notifiyCollection.CollectionChanged -= OnShapeSourcesCollectionChanged;
            //    }
            //    RemoveSources(Element.MapStyle.CustomSources.ToList());
            //}
            //else if (e.PropertyName == MapStyle.CustomLayersProperty.PropertyName
            //           && (sender as MapStyle).CustomLayers != null)
            //{
            //    var notifiyCollection = Element.MapStyle.CustomLayers as INotifyCollectionChanged;
            //    if (notifiyCollection != null)
            //    {
            //        notifiyCollection.CollectionChanged -= OnLayersCollectionChanged;
            //    }
            //    RemoveLayers(Element.MapStyle.CustomLayers.ToList());
            //}
        }

        void OnMapStylePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == MapStyle.CustomSourcesProperty.PropertyName
            //    && (sender as MapStyle).CustomSources != null)
            //{
            //    var notifiyCollection = Element.MapStyle.CustomSources as INotifyCollectionChanged;
            //    if (notifiyCollection != null)
            //    {
            //        notifiyCollection.CollectionChanged += OnShapeSourcesCollectionChanged;
            //    }
            //    AddSources(Element.MapStyle.CustomSources.ToList());
            //}
            //else if (e.PropertyName == MapStyle.CustomLayersProperty.PropertyName && (sender as MapStyle).CustomLayers != null)
            //{
            //    var notifiyCollection = Element.MapStyle.CustomLayers as INotifyCollectionChanged;
            //    if (notifiyCollection != null)
            //    {
            //        notifiyCollection.CollectionChanged += OnLayersCollectionChanged;
            //    }
            //    AddLayers(Element.MapStyle.CustomLayers.ToList());
            //}
        }

        void OnLayersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var currentLayersCount = map.Style.Layers.Length;
                    var index = currentLayersCount + e.NewStartingIndex - Element.MapStyle.CustomLayers.Count() + e.NewItems.Count;
                    if (index < currentLayersCount)
                    {
                        AddLayers(e.NewItems, index);
                    }
                    else
                    {
                        AddLayers(e.NewItems);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveLayers(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    var layersToRemove = new List<MGLStyleLayer>();
                    foreach (MGLStyleLayer layer in map.Style.Layers)
                    {
                        if (layer.Identifier.IsCustomId())
                        {
                            layersToRemove.Add(layer);
                        }
                    }
                    foreach (MGLStyleLayer layer in layersToRemove)
                    {
                        map.Style.RemoveLayer(layer);

                    }
                    layersToRemove.Clear();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveLayers(e.OldItems);
                    AddLayers(e.NewItems);
                    break;
                default:
                    break;
            }
        }

        void AddLayers(System.Collections.IList layers, int startIndex = -1)
        {
            if (layers == null)
            {
                return;
            }
            nuint index = (nuint)Math.Max(0, startIndex);
            foreach (Layer layer in layers)
            {
                if (string.IsNullOrEmpty(layer.Id))
                {
                    continue;
                }
                NSString id = layer.Id.ToCustomId();
                var oldLayer = map.Style.LayerWithIdentifier(id);
                if (oldLayer != null)
                {
                    map.Style.RemoveLayer(oldLayer);
                }
                if (layer is StyleLayer sl)
                {
                    var newLayer = GetStyleLayer(sl, id);
                    if (newLayer != null)
                    {
                        if (startIndex == -1)
                        {
                            map.Style.AddLayer(newLayer);
                        }
                        else
                        {
                            map.Style.InsertLayer(newLayer, index);
                            index += 1;
                        }
                    }
                }
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
                if (string.IsNullOrEmpty(layer.Id))
                {
                    continue;
                }
                NSString id = layer.Id.ToCustomId();
                var oldLayer = map.Style.LayerWithIdentifier(id);
                if (oldLayer != null)
                {
                    map.Style.RemoveLayer(oldLayer);
                }
            }
        }

        void OnShapeSourcesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddSources(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveSources(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    var sourcesToRemove = new List<MGLSource>();
                    foreach (MGLSource source in map.Style.Sources)
                    {
                        if (source.Identifier.IsCustomId())
                        {
                            sourcesToRemove.Add(source);
                        }
                    }
                    foreach (MGLSource source in sourcesToRemove)
                    {
                        map.Style.RemoveSource(source);

                    }
                    sourcesToRemove.Clear();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveSources(e.OldItems);
                    AddSources(e.NewItems);
                    break;
                default: break;
            }
        }

        void AddSources(System.Collections.IList sources)
        {
            if (sources == null || map == null || map.Style == null)
            {
                return;
            }
            //foreach (MapSource source in sources)
            //{
            //    MGLSource mglSource = null;
            //    if (source.Id != null)
            //    {
            //        var sourceId = source.Id.ToCustomId();
            //        if (source is ShapeSource shapeSource)
            //        {
            //            if (shapeSource.Shape == null) continue;
            //            var shape = ShapeFromAnnotation(shapeSource.Shape);
            //            var oldSource = MapView.Style?.SourceWithIdentifier(sourceId);
            //            if (oldSource != null && oldSource is MGLShapeSource)
            //            {
            //                (oldSource as MGLShapeSource).Shape = shape;
            //            }
            //            else
            //            {
            //                mglSource = new MGLShapeSource(sourceId, shape, null);
            //            }
            //        }
            //        else if (source is RasterSource rasterSource)
            //        {
            //            if (string.IsNullOrEmpty(rasterSource.ConfigurationURL) == false
            //                && NSUrl.FromString(rasterSource.ConfigurationURL) is NSUrl url)
            //            {
            //                if (rasterSource.TileSize <= 0)
            //                {
            //                    mglSource = new MGLRasterTileSource(sourceId, url);
            //                }
            //                else
            //                {
            //                    mglSource = new MGLRasterTileSource(sourceId, url, (nfloat)rasterSource.TileSize);
            //                }
            //            }
            //            else if (rasterSource.TileURLTemplates != null)
            //            {
            //                if (rasterSource.Options != null)
            //                {
            //                    var keys = new List<NSString>();
            //                    var values = new List<NSObject>();
            //                    foreach (TileSourceOption key in rasterSource.Options.Keys)
            //                    {
            //                        switch (key)
            //                        {
            //                            case TileSourceOption.AttributionHTMLString:
            //                                keys.Add(MGLTileSourceOptions.AttributionHTMLString);
            //                                values.Add((NSString)(rasterSource.Options[key] as string));
            //                                break;
            //                            case TileSourceOption.AttributionInfos:
            //                                if (rasterSource.Options[key] is AttributionInfo[] infos)
            //                                {
            //                                    var infosList = new NSMutableArray<MGLAttributionInfo>();
            //                                    foreach (AttributionInfo info in infos)
            //                                    {
            //                                        var attr = new MGLAttributionInfo(new NSAttributedString(info.Title), info.Url != null ? NSUrl.FromString(info.Url) : null);
            //                                        infosList.Add(attr);
            //                                    }
            //                                    keys.Add(MGLTileSourceOptions.AttributionInfos);
            //                                    values.Add(infosList);
            //                                }
            //                                break;
            //                            case TileSourceOption.MaximumZoomLevel:
            //                                keys.Add(MGLTileSourceOptions.MaximumZoomLevel);
            //                                values.Add(NSNumber.FromDouble((double)rasterSource.Options[key]));
            //                                break;
            //                            case TileSourceOption.MinimumZoomLevel:
            //                                keys.Add(MGLTileSourceOptions.MinimumZoomLevel);
            //                                values.Add(NSNumber.FromDouble((double)rasterSource.Options[key]));
            //                                break;
            //                            case TileSourceOption.TileCoordinateSystem:
            //                                if (rasterSource.Options[key] is TileCoordinateSystem sys)
            //                                {
            //                                    if (sys == TileCoordinateSystem.TileCoordinateSystemTMS)
            //                                    {
            //                                        keys.Add(MGLTileSourceOptions.TileCoordinateSystem);
            //                                        values.Add(NSNumber.FromUInt64((ulong)MGLTileCoordinateSystem.Tms));
            //                                    }
            //                                    else
            //                                    {
            //                                        keys.Add(MGLTileSourceOptions.TileCoordinateSystem);
            //                                        values.Add(NSNumber.FromUInt64((ulong)MGLTileCoordinateSystem.Xyz));
            //                                    }
            //                                }
            //                                break;
            //                            default: break;
            //                        }
            //                    }
            //                    mglSource = new MGLRasterTileSource(sourceId,
            //                                                    tileURLTemplates: rasterSource.TileURLTemplates,
            //                                                    options: new NSDictionary<NSString, NSObject>(keys.ToArray(), values.ToArray()));
            //                }
            //                else
            //                {
            //                    mglSource = new MGLRasterTileSource(sourceId,
            //                                                    tileURLTemplates: rasterSource.TileURLTemplates,
            //                                                    options: null);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            mglSource = new MGLSource(sourceId);
            //        }
            //    }
            //    if (mglSource != null)
            //    {
            //        MapView.Style.AddSource(mglSource);
            //    }
            //}
        }

        void RemoveSources(System.Collections.IList sources)
        {
            //if (sources == null)
            //{
            //    return;
            //}
            //foreach (MapSource source in sources)
            //{
            //    if (source.Id != null)
            //    {
            //        var oldSource = MapView.Style.SourceWithIdentifier(source.Id.ToCustomId());
            //        if (oldSource != null)
            //        {
            //            MapView.Style.RemoveSource(oldSource);
            //        }
            //    }
            //}
        }

        MGLStyle mapStyle;

        [Export("mapView:didFinishLoadingStyle:"),]
        public void DidFinishLoadingStyle(MGLMapView mapView, MGLStyle style)
        {
            MapStyle newStyle;
            if (Element.MapStyle == null)
            {
                newStyle = new MapStyle(mapView.StyleURL.AbsoluteString);
                newStyle.Name = style.Name;
                Element.MapStyle = newStyle;
            }
            else
            {
                if (Element.MapStyle.UrlString == null
                || Element.MapStyle.UrlString != mapView.StyleURL.AbsoluteString)
                {
                    Element.MapStyle.SetUrl(mapView.StyleURL.AbsoluteString);
                    Element.MapStyle.Name = style.Name;
                }
                newStyle = Element.MapStyle;
            }
            //if (Element.MapStyle.CustomSources != null)
            //{
            //    var notifiyCollection = Element.MapStyle.CustomSources as INotifyCollectionChanged;
            //    if (notifiyCollection != null)
            //    {
            //        notifiyCollection.CollectionChanged += OnShapeSourcesCollectionChanged;
            //    }

            //    AddSources(Element.MapStyle.CustomSources.ToList());
            //}
            if (Element.MapStyle.CustomLayers != null)
            {
                if (Element.MapStyle.CustomLayers is INotifyCollectionChanged notifiyCollection)
                {
                    notifiyCollection.CollectionChanged += OnLayersCollectionChanged;
                }

                AddLayers(Element.MapStyle.CustomLayers.ToList());
            }

            newStyle.OriginalLayers = style.Layers.Select((MGLStyleLayer arg) => new Layer(arg.Identifier)
            {
                IsVisible = arg.Visible
            }).ToArray();
            newStyle.Name = style.Name;
            Element.DidFinishLoadingStyleCommand?.Execute(newStyle);
            Element.Functions = this;
        }
    }

}
