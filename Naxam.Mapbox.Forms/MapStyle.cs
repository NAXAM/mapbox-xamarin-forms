using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Naxam.Controls.Forms
{
    public abstract class NotifyableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        protected void SetProperty<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(currentValue, newValue)) return;

            PropertyChanging?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            currentValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MapStyle : NotifyableObject
    {
        public static readonly MapStyle STREETS = new MapStyle("mapbox://styles/mapbox/streets-v11")
        {
            Name = "Streets"
        };
        public static readonly MapStyle OUTDOORS = new MapStyle("mapbox://styles/mapbox/outdoors-v11")
        {
            Name = "Outdoors"
        };
        public static readonly MapStyle LIGHT = new MapStyle("mapbox://styles/mapbox/light-v10")
        {
            Name = "Light"
        };
        public static readonly MapStyle DARK = new MapStyle("mapbox://styles/mapbox/dark-v10")
        {
            Name = "Dark"
        };
        public static readonly MapStyle SATELITE = new MapStyle("mapbox://styles/mapbox/satellite-v9")
        {
            Name = "Satellite"
        };
        public static readonly MapStyle SATELITE_STREETS = new MapStyle("mapbox://styles/mapbox/satellite-streets-v11")
        {
            Name = "Satellite Streets"
        };
        public static readonly MapStyle NAVIGATION_PREVIEW_DAY = new MapStyle("mapbox://styles/mapbox/navigation-preview-day-v4")
        {
            Name = "Navigation Preview Day"
        };
        public static readonly MapStyle NAVIGATION_PREVIEW_NIGHT = new MapStyle("mapbox://styles/mapbox/navigation-preview-night-v4")
        {
            Name = "Navigation Preview Night"
        };
        public static readonly MapStyle NAVIGATION_GUIDANCE_DAY = new MapStyle("mapbox://styles/mapbox/navigation-guidance-day-v4")
        {
            Name = "Navigation Guidance Day"
        };
        public static readonly MapStyle NAVIGATION_GUIDANCE_NIGHT = new MapStyle("mapbox://styles/mapbox/navigation-guidance-night-v4")
        {
            Name = "Navigation Guidance Night"
        };

        public string Id { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public double[] Center { get; set; }

        public string UrlString
        {
            get
            {
                if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Owner))
                {

                    return "mapbox://styles/" + Owner + "/" + Id;
                }
                return null;
            }
        }

        public MapStyle()
        {
        }

        public MapStyle(string id, string name, double[] center = null, string owner = null)
        {
            Id = id;
            Name = name;
            Center = center;
            Owner = owner;
        }

        public MapStyle(string urlString)
        {
            UpdateIdAndOwner(urlString);
        }

        public void SetUrl(string urlString)
        {
            UpdateIdAndOwner(urlString);
        }

        void UpdateIdAndOwner(string urlString)
        {
            if (!string.IsNullOrEmpty(urlString))
            {
                var segments = (new Uri(urlString)).Segments;
                if (string.IsNullOrEmpty(Id) && segments.Length != 0)
                {
                    Id = segments[segments.Length - 1].Trim('/');
                }
                if (string.IsNullOrEmpty(Owner) && segments.Length > 1)
                {
                    Owner = segments[segments.Length - 2].Trim('/');
                }
            }
        }

        //IEnumerable<Source> _CustomSources;
        //public IEnumerable<Source> CustomSources
        //{
        //    get => _CustomSources;
        //    set => SetProperty(ref _CustomSources, value);
        //}

        //IEnumerable<Layer> _CustomLayers;
        //public IEnumerable<Layer> CustomLayers
        //{
        //    get => _CustomLayers;
        //    set => SetProperty(ref _CustomLayers, value);
        //}

        //IEnumerable<Layer> _OriginalLayers;
        //public IEnumerable<Layer> OriginalLayers
        //{
        //    get => _OriginalLayers;
        //    set => SetProperty(ref _OriginalLayers, value);
        //}
    }
}
