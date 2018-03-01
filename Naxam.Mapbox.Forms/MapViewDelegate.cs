using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Naxam.Controls.Mapbox.Forms
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

        /// <summary>
        /// Did tap on map
        /// </summary>
        /// <returns>((Position) Tapped location,(Point) Tapped point)</returns>
        public ICommand DidTapOnMapCommand {
            get {
                return (ICommand)GetValue (DidTapOnMapCommandProperty);
            }
            set {
                SetValue (DidTapOnMapCommandProperty, (ICommand)value);
            }
        }

        /// <summary>
        /// MGLMapViewDelegate -mapView:tapOnCalloutForAnnotation:
        /// </summary>
        /// <returns>Annotation's id</returns>
        public static BindableProperty DidTapOnCalloutViewCommandProperty = BindableProperty.Create(
            propertyName: nameof(DidTapOnCalloutViewCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(MapView),
            defaultValue: default(ICommand),
            defaultBindingMode: BindingMode.OneWay
        );

        public ICommand DidTapOnCalloutViewCommand
        {
            get { return (ICommand)GetValue(DidTapOnCalloutViewCommandProperty); }
            set { SetValue(DidTapOnCalloutViewCommandProperty, value); }
        }

        /// <summary>
        /// MGLMapViewDelegate -mapView:imageForAnnotation:
        /// </summary>
        /// <returns>(Reusable key, image file name)</returns>
        public static BindableProperty GetImageForAnnotationFuncProperty = BindableProperty.Create(
            propertyName: nameof(GetImageForAnnotationFunc),
            returnType: typeof(Func<string, Tuple<string,string>>),
            declaringType: typeof(MapView),
            defaultValue: default(Func<string, Tuple<string, string>>),
            defaultBindingMode: BindingMode.OneWay
        );

        public Func<string, Tuple<string, string>> GetImageForAnnotationFunc
        {
            get { return (Func<string, Tuple<string, string>>)GetValue(GetImageForAnnotationFuncProperty); }
            set { SetValue(GetImageForAnnotationFuncProperty, value); }
        }
    }
}