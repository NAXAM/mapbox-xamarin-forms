using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Naxam.Controls.Mapbox.Forms;
using Sdk = Com.Mapbox.Mapboxsdk;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer
    {
        Sdk.Maps.Style mapStyle;

        protected virtual void UpdateMapStyle()
        {
            if (Element.MapStyle != null && !string.IsNullOrEmpty(Element.MapStyle.UrlString))
            {
                map.SetStyle(Element.MapStyle.UrlString, new StyleLoadedCallback(new WeakReference<MapViewRenderer>(this)));

                Element.MapStyle.PropertyChanging -= OnMapStylePropertyChanging;
                Element.MapStyle.PropertyChanging += OnMapStylePropertyChanging;

                Element.MapStyle.PropertyChanged -= OnMapStylePropertyChanged;
                Element.MapStyle.PropertyChanged += OnMapStylePropertyChanged;
            }
        }

        void OnMapStylePropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            var style = sender as MapStyle;
            if (style == null) return;

            if (e.PropertyName == MapStyle.CustomSourcesProperty.PropertyName)
            {
                if (style.CustomSources == null)
                {
                    return;
                }

                var notifiyCollection = style.CustomSources as INotifyCollectionChanged;
                if (notifiyCollection != null)
                {
                    notifiyCollection.CollectionChanged -= OnShapeSourcesCollectionChanged;
                }

                RemoveSources(style.CustomSources.ToList());
            }
            else if (e.PropertyName == MapStyle.CustomLayersProperty.PropertyName)
            {
                if (style.CustomLayers == null)
                {
                    return;
                }

                var notifiyCollection = Element.MapStyle.CustomLayers as INotifyCollectionChanged;
                if (notifiyCollection != null)
                {
                    notifiyCollection.CollectionChanged -= OnLayersCollectionChanged;
                }

                RemoveLayers(Element.MapStyle.CustomLayers.ToList());
            }
        }

        void OnMapStylePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var style = sender as MapStyle;
            if (style == null) return;

            if (e.PropertyName == MapStyle.CustomSourcesProperty.PropertyName)
            {
                if (style.CustomSources == null)
                {
                    return;
                }

                var notifiyCollection = style.CustomSources as INotifyCollectionChanged;
                if (notifiyCollection != null)
                {
                    notifiyCollection.CollectionChanged += OnShapeSourcesCollectionChanged;
                }

                AddSources(style.CustomSources.ToList());
            }
            else if (e.PropertyName == MapStyle.CustomLayersProperty.PropertyName)
            {
                if (style.CustomLayers == null)
                {
                    return;
                }

                var notifiyCollection = Element.MapStyle.CustomLayers as INotifyCollectionChanged;
                if (notifiyCollection != null)
                {
                    notifiyCollection.CollectionChanged += OnLayersCollectionChanged;
                }

                AddLayers(Element.MapStyle.CustomLayers.ToList());
            }
        }

        class StyleLoadedCallback : Java.Lang.Object, Sdk.Maps.Style.IOnStyleLoaded
        {
            WeakReference<MapViewRenderer> rendererReference;

            public StyleLoadedCallback(WeakReference<MapViewRenderer> rendererReference)
            {
                this.rendererReference = rendererReference;
            }

            public void OnStyleLoaded(Sdk.Maps.Style p0)
            {
                if (rendererReference.TryGetTarget(out var renderer))
                {
                    renderer.mapStyle = p0;
                    renderer.Element.MapStyle.OriginalLayers = p0.Layers.Select((arg) =>
                                                                                    new Layer(arg.Id)
                                                                                   ).ToArray();
                    renderer.Element.DidFinishLoadingStyleCommand?.Execute(renderer.Element.MapStyle);
                }
            }
        }
    }
}