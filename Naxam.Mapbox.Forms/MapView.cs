
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Xamarin.Forms;

namespace Naxam.Mapbox.Forms
{
    public class Position
    {
        public double Lat { get; set; }

        public double Long { get; set; }
    }

    public class PositionChangeEventArgs : EventArgs
    {
        private Position _newPosition;

        public PositionChangeEventArgs(Position newPosition)
        {
            NewPosition = newPosition;
        }

        public Position NewPosition
        {
            [CompilerGenerated]
            get;
            [CompilerGenerated]
            private set;
        }
    }

    
    public class MapView : View
    {

        public static readonly BindableProperty FocusPositionProperty = BindableProperty.Create(
           nameof(IsTouchInMap),
           typeof(bool),
           typeof(MapView),
           default(bool),
        BindingMode.TwoWay
       //          null,
       //          ((bindable, newValue, oldValue) =>
       //          {
       //              ((MapView)bindable).OnMapCenterChange(bindable,(Position)newValue, (Position)oldValue);
       //          })

       );


        public bool IsTouchInMap
        {
            get
            {
                return (bool)GetValue(FocusPositionProperty);
            }
            set { SetValue(FocusPositionProperty, value); }
        }


        public static BindableProperty CenterProperty = BindableProperty.Create(
            nameof(Center),
            typeof(Position),
            typeof(MapView),
            default(Position),
         BindingMode.TwoWay
//          null,
//          ((bindable, newValue, oldValue) =>
//          {
//              ((MapView)bindable).OnMapCenterChange(bindable,(Position)newValue, (Position)oldValue);
//          })

        );


        public Position Center
        {
            get
            {
                return (Position)GetValue(CenterProperty);
            }
            set { SetValue(CenterProperty, value); }
        }
      public  void OnMapCenterChange(BindableObject bindable, Position nePosition,Position olPosition)
        {
            if (ReferenceEquals(nePosition, olPosition))
            {
                MapCenterHandler.Invoke(this,new PositionChangeEventArgs(nePosition));
            }
        }

        public EventHandler MapCenterHandler { get; set; }




        public static BindableProperty UserLocationProperty = BindableProperty.Create(
            nameof(UserLocation),
            typeof(Position),
            typeof(MapView),
            default(Position),
            defaultBindingMode: BindingMode.TwoWay
        );
        public Position UserLocation
        {
            get
            {
                return (Position)GetValue(UserLocationProperty);
            }
            set { SetValue(UserLocationProperty, value); }
        }



       /* public static BindableProperty FocusLocationProperty = BindableProperty.Create(
            nameof(FocusLocationCommand),
            typeof(ICommand),
            typeof(MapView),
            default(ICommand)
            );

        public ICommand FocusLocationCommand
        {
            get { return (ICommand)GetValue(FocusLocationProperty); }
            set
            {
                SetValue(FocusLocationProperty,(ICommand)value);
            }
        }*/
        public double ZoomLevel { get; set; }
    }
}
