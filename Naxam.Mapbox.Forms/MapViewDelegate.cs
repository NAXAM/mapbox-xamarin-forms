using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
    public partial class MapView
    {
        public static readonly BindableProperty StyleImageMissingCommandProperty = BindableProperty.Create(
            nameof(StyleImageMissingCommand),
            typeof(ICommand),
            typeof(MapView),
            default(ICommand),
            BindingMode.OneWay
        );

        public ICommand StyleImageMissingCommand
        {
            get => (ICommand) GetValue(StyleImageMissingCommandProperty);
            set => SetValue(StyleImageMissingCommandProperty, value);
        }
        
        public static readonly BindableProperty DidFinishLoadingStyleCommandProperty = BindableProperty.Create(
                    nameof(DidFinishLoadingStyleCommand),
                    typeof(ICommand),
                    typeof(MapView),
                    default(ICommand),
                    BindingMode.OneWay);
        public ICommand DidFinishLoadingStyleCommand
        {
            get => (ICommand)GetValue(DidFinishLoadingStyleCommandProperty);
            set => SetValue(DidFinishLoadingStyleCommandProperty, value);
        }

        public static readonly BindableProperty DidFinishRenderingCommandProperty = BindableProperty.Create(
                    nameof(DidFinishRenderingCommand),
                    typeof(ICommand),
                    typeof(MapView),
                    default(ICommand),
                    BindingMode.TwoWay);
        public ICommand DidFinishRenderingCommand
        {
            get => (ICommand)GetValue(DidFinishRenderingCommandProperty);
            set => SetValue(DidFinishRenderingCommandProperty, value);
        }

        public static readonly BindableProperty RegionDidChangeCommandProperty = BindableProperty.Create(
                    nameof(RegionDidChangeCommand),
                    typeof(ICommand),
                    typeof(MapView),
                    default(ICommand),
                    BindingMode.TwoWay);
        public ICommand RegionDidChangeCommand
        {
            get => (ICommand)GetValue(RegionDidChangeCommandProperty);
            set => SetValue(RegionDidChangeCommandProperty, value);
        }

        public static readonly BindableProperty DidTapOnMapCommandProperty = BindableProperty.Create(
            nameof(DidTapOnMapCommand),
            typeof(ICommand),
            typeof(MapView),
            default(ICommand),
            BindingMode.OneWay);

        /// <summary>
        /// Did tap on map
        /// </summary>
        /// <returns>((Position) Tapped location,(Point) Tapped point)</returns>
        public ICommand DidTapOnMapCommand
        {
            get => (ICommand)GetValue(DidTapOnMapCommandProperty);
            set => SetValue(DidTapOnMapCommandProperty, value);
        }

        /// <summary>
        /// MGLMapViewDelegate -mapView:tapOnCalloutForAnnotation:
        /// </summary>
        /// <returns>Annotation's id</returns>
        public static BindableProperty DidTapOnCalloutViewCommandProperty = BindableProperty.Create(
            nameof(DidTapOnCalloutViewCommand),
            typeof(ICommand),
            typeof(MapView),
            default(ICommand),
            BindingMode.OneWay
        );

        public ICommand DidTapOnCalloutViewCommand
        {
            get { return (ICommand)GetValue(DidTapOnCalloutViewCommandProperty); }
            set { SetValue(DidTapOnCalloutViewCommandProperty, value); }
        }


        public static BindableProperty DidTapOnMarkerCommandProperty = BindableProperty.Create(
           nameof(DidTapOnMarkerCommand),
           typeof(ICommand),
           typeof(MapView),
           default(ICommand),
           BindingMode.TwoWay
           );
        public ICommand DidTapOnMarkerCommand
        {
            get => (ICommand)GetValue(DidTapOnMarkerCommandProperty);
            set => SetValue(DidTapOnMarkerCommandProperty, value);
        }
    }
}