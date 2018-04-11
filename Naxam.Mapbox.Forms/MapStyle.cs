using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Naxam.Controls.Mapbox.Forms
{
    public sealed class PreserveAttribute : System.Attribute
    {
        public bool AllMembers;
        public bool Conditional;
    }

    public class MapStyle : BindableObject
    {
        public string Id {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public string Owner {
            get;
            set;
        }

        public double [] Center {
            get;
            set;
        }

        public string UrlString {
            get {
                if (!string.IsNullOrEmpty (Id) && !string.IsNullOrEmpty (Owner)) {

                    return "mapbox://styles/" + Owner + "/" + Id;
                }
                return null;
            }
        }

        private Layer[] layers;

        public MapStyle ()
        {
        }

        public MapStyle (string id, string name, double [] center = null, string owner = null)
        {
            Id = id;
            Name = name;
            Center = center;
            Owner = owner;
        }

        public MapStyle (string urlString)
        {
            UpdateIdAndOwner (urlString);
        }

        public void SetUrl (string urlString)
        {
            UpdateIdAndOwner (urlString);
        }

        void UpdateIdAndOwner (string urlString)
        {
            if (!string.IsNullOrEmpty (urlString)) {
                var segments = (new Uri (urlString)).Segments;
                if (string.IsNullOrEmpty (Id) && segments.Length != 0) {
                    Id = segments [segments.Length - 1].Trim ('/');
                }
                if (string.IsNullOrEmpty (Owner) && segments.Length > 1) {
                    Owner = segments [segments.Length - 2].Trim ('/');
                }
            }
        }

        public static readonly BindableProperty CustomSourcesProperty = BindableProperty.Create (
                   nameof (CustomSources),
            typeof (IEnumerable<MapSource>),
           typeof (MapView),
            default (IEnumerable<MapSource>),
                   BindingMode.TwoWay);

        public IEnumerable<MapSource> CustomSources {
            get {
                return (IEnumerable<MapSource>)GetValue (CustomSourcesProperty);
            }
            set {
                SetValue (CustomSourcesProperty, (IList<MapSource>)value);
            }
        }

        public static readonly BindableProperty CustomLayersProperty = BindableProperty.Create (
                    nameof (CustomLayers),
                    typeof (IEnumerable<Layer>),
                    typeof (MapStyle),
                    default (IEnumerable<Layer>),
                    BindingMode.TwoWay);

        public IEnumerable<Layer> CustomLayers {
            get {
                return (IEnumerable<Layer>)GetValue (CustomLayersProperty);
            }
            set {
                SetValue (CustomLayersProperty, (IEnumerable<Layer>)value);
            }
        }

        public static readonly BindableProperty OriginalLayersProperty = BindableProperty.Create(
					nameof(CustomLayers),
					typeof(Layer[]),
					typeof(MapStyle),
					default(Layer[]),
            BindingMode.OneWayToSource);
        public Layer[] OriginalLayers
		{
			get
			{
				return (Layer[])GetValue(OriginalLayersProperty);
			}
			set
			{
				SetValue(OriginalLayersProperty, (Layer[])value);
			}
		}
    }
}
