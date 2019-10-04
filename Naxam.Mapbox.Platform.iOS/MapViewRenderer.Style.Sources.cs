using System;
using System.Linq;
using GeoJSON.Net;
using Mapbox;
using Naxam.Mapbox;
using Naxam.Mapbox.Platform.iOS.Extensions;
using Naxam.Mapbox.Sources;
using Newtonsoft.Json;
using UIKit;
using Xamarin.Forms;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer : IMapFunctions
    {
        public void AddStyleImage(IconImageSource iconImageSource)
        {
            if (iconImageSource.Source == null) return;
            
            var cachedImage = mapStyle.ImageForName(iconImageSource.Id);
            if (cachedImage != null) return;
            
            var image = iconImageSource.Source.GetImage();

            if (image == null)
            {
                return;
            }

            if (iconImageSource.IsTemplate)
            {
                image = image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            }

            mapStyle.SetImage(image, iconImageSource.Id);
        }

        public bool AddSource(params Source[] sources)
        {
            for (int i = 0; i < sources.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(sources[i].Id)) continue;

                map.Style.AddSource(sources[i].ToSource());
            }

            return true;
        }

        public bool UpdateSource(string sourceId, IGeoJSONObject featureCollection)
        {
            var source = mapStyle.SourceWithIdentifier(sourceId) as MGLShapeSource;

            if (source == null) return false;

            source.Shape = featureCollection.ToShape();

            return true;
        }

        public bool UpdateSource(string sourceId, ImageSource imageSource)
        {
            var source = mapStyle.SourceWithIdentifier(sourceId) as MGLImageSource;

            if (source == null) return false;

            source.Image = imageSource.GetImage();
            
            return true;
        }

        public void RemoveSource(params string[] sourceIds)
        {
            for (int i = 0; i < sourceIds.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(sourceIds[i])) continue;

                var source = map.Style.SourceWithIdentifier(sourceIds[i]) as MGLSource;

                if (source == null) continue;

                map.Style.RemoveSource(source);
            }
        }
    }
}
