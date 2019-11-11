using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Naxam.Controls.Forms
{
    public partial class MapView
    {
        public static readonly BindableProperty DragFinishedCommandProperty = BindableProperty.Create(
            nameof(DragFinishedCommand),
            typeof(ICommand),
            typeof(MapView),
            default(ICommand),
            BindingMode.OneWay);
        public ICommand DragFinishedCommand
        {
            get => (ICommand)GetValue(DragFinishedCommandProperty);
            set => SetValue(DragFinishedCommandProperty, value);
        }

        static Func<string, bool> DefaultCanShowCalloutChecker = x => true;
        public static readonly BindableProperty CanShowCalloutCheckerProperty = BindableProperty.Create(
            nameof(CanShowCalloutChecker),
            typeof(Func<string, bool>),
            typeof(MapView),
            DefaultCanShowCalloutChecker,
            BindingMode.OneWay);

        public Func<string, bool> CanShowCalloutChecker
        {
            get => (Func<string, bool>)GetValue(CanShowCalloutCheckerProperty) ?? DefaultCanShowCalloutChecker;
            set => SetValue(CanShowCalloutCheckerProperty, value);
        }

        public static readonly BindableProperty CameraMovedCommandProperty = BindableProperty.Create(
            nameof(CameraMovedCommand),
            typeof(ICommand),
            typeof(MapView),
            default(ICommand),
            BindingMode.OneWay);
        public ICommand CameraMovedCommand
        {
            get => (ICommand)GetValue(CameraMovedCommandProperty);
            set => SetValue(CameraMovedCommandProperty, value);
        }
    }
}
