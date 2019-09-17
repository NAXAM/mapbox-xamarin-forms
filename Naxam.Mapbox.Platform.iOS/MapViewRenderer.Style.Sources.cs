using System;
using System.Linq;
using Mapbox;
using Naxam.Mapbox;
using Naxam.Mapbox.Platform.iOS.Extensions;
using Naxam.Mapbox.Sources;
using UIKit;
using Xamarin.Forms;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer : IMapFunctions
    {
        public void AddStyleImage(IconImageSource iconImageSource)
        {
            if (iconImageSource.Source == null) return;

            switch (iconImageSource.Source)
            {
                // TODO: iOS - Handle other image sources
                case FileImageSource fileImageSource:
                    var cachedImage = mapStyle.ImageForName(fileImageSource.File);
                    if (cachedImage != null) break;

                    var image = UIImage.FromBundle(fileImageSource.File);

                    if (image == null)
                    {
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("Resource not found: " + fileImageSource.File);
#endif
                        break;
                    }

                    if (iconImageSource.IsTemplate)
                    {
                        image = image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                    }

                    mapStyle.SetImage(image, fileImageSource.File);
                    iconImageSource.Id = fileImageSource.File;
                    break;
            }
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

        public void RemoveSource(params string[] sourceIds)
        {
            for (int i = 0; i < sourceIds.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(sourceIds[i])) continue;

                var source = map.Style.Sources.FirstOrDefault(x => x is MGLSource s && s.Identifier == sourceIds[i]) as MGLSource;

                if (source == null) continue;

                map.Style.RemoveSource(source);
            }
        }
    }
}
