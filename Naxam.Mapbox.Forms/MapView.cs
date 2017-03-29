
using Xamarin.Forms;

namespace Naxam.Mapbox.Forms
{
    public class Position
    {
        public double Lat { get; set; }

        public double Long { get; set; }
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

        public double ZoomLevel { get; set; }
    }
}
