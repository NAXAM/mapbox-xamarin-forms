
using Xamarin.Forms;

namespace Naxam.Mapbox.Forms
{
    public class Position
    {
        public double Lat { get; set; }

        public double Long { get; set; }

		public Position()
		{
			
		}

		public Position(double lat, double lon)
		{
			Lat = lat;
			Long = lon;
		}
    }

    public class MapView : View
    {
        public static BindableProperty CenterProperty = BindableProperty.Create(
            nameof(Center),
            typeof(Position),
            typeof(MapView),
            default(Position),
            defaultBindingMode: BindingMode.OneWay
        );
        public Position Center
        {
            get
            {
                return (Position)GetValue(CenterProperty);
            }
            set { SetValue(CenterProperty, value); }
        }

		public static readonly BindableProperty UserLocationProperty = BindableProperty.Create(
			nameof(UserLocation), 
			typeof(Position), 
			typeof(MapView), 
			default(Position), 
			BindingMode.OneWayToSource);

		public Position UserLocation
		{
			get
			{
				return (Position)GetValue(UserLocationProperty);
			}
			set
			{
				SetValue(UserLocationProperty, (Position)value);
			}
		}

		public double ZoomLevel { get; set; }
    }
}
