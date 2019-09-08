using System;
using System.Collections.Specialized;
using System.Linq;
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
                    renderer.Element.Functions = renderer;
                    renderer.Element.DidFinishLoadingStyleCommand?.Execute(renderer.Element.MapStyle);

                    renderer.AddAnnotations(renderer.Element.Annotations?.ToArray());
                    if (renderer.Element.Annotations is INotifyCollectionChanged notifyCollection)
                    {
                        notifyCollection.CollectionChanged -= renderer.OnAnnotationsCollectionChanged;
                        notifyCollection.CollectionChanged += renderer.OnAnnotationsCollectionChanged;
                    }
                    renderer.Element.AnnotationsChanged -= renderer.Element_AnnotationsChanged;
                    renderer.Element.AnnotationsChanged += renderer.Element_AnnotationsChanged;
                }
            }
        }
    }
}