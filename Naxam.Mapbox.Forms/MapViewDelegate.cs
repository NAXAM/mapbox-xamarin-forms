using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
    public partial class MapView
    {
        public static readonly BindableProperty DidFinishLoadingStyleCommandProperty = BindableProperty.Create (
                    nameof (DidFinishLoadingStyleCommand),
                    typeof (ICommand),
                    typeof (MapView),
                    default (ICommand),
                    BindingMode.OneWay);

        /*
         * Output: (MapStyle) style
         */
        public ICommand DidFinishLoadingStyleCommand {
            get {
                return (ICommand)GetValue (DidFinishLoadingStyleCommandProperty);
            }
            set {
                SetValue (DidFinishLoadingStyleCommandProperty, (ICommand)value);
            }
        }

        public static readonly BindableProperty DidFinishRenderingCommandProperty = BindableProperty.Create (
                    nameof (DidFinishRenderingCommand),
                    typeof (ICommand),
                    typeof (MapView),
                    default (ICommand),
                    BindingMode.TwoWay);
        /*
         * Output: None
         */
        public ICommand DidFinishRenderingCommand {
            get {
                return (ICommand)GetValue (DidFinishRenderingCommandProperty);
            }
            set {
                SetValue (DidFinishRenderingCommandProperty, (ICommand)value);
            }
        }

        public static readonly BindableProperty RegionDidChangeCommandProperty = BindableProperty.Create (
                    nameof (RegionDidChangeCommand),
                    typeof (ICommand),
                    typeof (MapView),
                    default (ICommand),
                    BindingMode.TwoWay);

        /*
         * Output: (bool) animated
         */
        public ICommand RegionDidChangeCommand {
            get {
                return (ICommand)GetValue (RegionDidChangeCommandProperty);
            }
            set {
                SetValue (RegionDidChangeCommandProperty, (ICommand)value);
            }
        }

        public static readonly BindableProperty DidTapOnMapCommandProperty = BindableProperty.Create (
            nameof (DidTapOnMapCommand),
            typeof (ICommand),
            typeof (MapView),
            default (ICommand),
            BindingMode.OneWay);

        /*
         * Output:
         *   - (Position) Tapped location
         *   - (Point) Tapped point
         */
        public ICommand DidTapOnMapCommand {
            get {
                return (ICommand)GetValue (DidTapOnMapCommandProperty);
            }
            set {
                SetValue (DidTapOnMapCommandProperty, (ICommand)value);
            }
        }

    }

}