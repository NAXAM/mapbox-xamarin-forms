using System;
using Xamarin.Forms;

namespace Naxam.Controls.Mapbox.Forms
{
    public partial class MapView
    {
        static Func<string, bool> DefaultCanShowCalloutChecker = x => true;
        public static readonly BindableProperty CanShowCalloutCheckerProperty = BindableProperty.Create(
            nameof(CanShowCalloutChecker),
            typeof(Func<string, bool>),
            typeof(MapView),
            default(Func<string, bool>),
            BindingMode.OneWay);

        public Func<string, bool> CanShowCalloutChecker
        {
            get
            {
                return (Func<string, bool>)GetValue(CanShowCalloutCheckerProperty) ?? DefaultCanShowCalloutChecker;
            }
            set
            {
                SetValue(CanShowCalloutCheckerProperty, value);
            }
        }

        public static readonly BindableProperty TakeSnapshotFuncProperty = BindableProperty.Create(
            nameof(TakeSnapshotFunc),
            typeof(Func<byte[]>),
            typeof(MapView),
            default(Func<byte[]>),
            defaultBindingMode: BindingMode.OneWayToSource);

        public Func<byte[]> TakeSnapshotFunc
        {
            get
            {
                return (Func<byte[]>)GetValue(TakeSnapshotFuncProperty);
            }
            set
            {
                SetValue(TakeSnapshotFuncProperty, value);
            }
        }

        public byte[] TakSnapshot() {
            return TakeSnapshotFunc?.Invoke();
        }

        public static readonly BindableProperty GetFeaturesAroundPointFuncProperty = BindableProperty.Create(
            nameof(GetFeaturesAroundPointFunc),
            typeof(Func<Point, double, string[], IFeature[]>),
            typeof(MapView),
            default(Func<Point, double, string[], IFeature[]>),
            BindingMode.OneWayToSource);

        public Func<Point, double, string[], IFeature[]> GetFeaturesAroundPointFunc
        {
            get
            {
                return ((Func<Point, double, string[], IFeature[]>)GetValue(GetFeaturesAroundPointFuncProperty));
            }
            set
            {
                SetValue(GetFeaturesAroundPointFuncProperty, value);
            }
        }

        public static readonly BindableProperty ResetPositionActionProperty = BindableProperty.Create(
            nameof(ResetPositionAction),
            typeof(Action),
            typeof(MapView),
            default(Action),
            BindingMode.OneWayToSource);

        public Action ResetPositionAction
        {
            get
            {
                return (Action)GetValue(ResetPositionActionProperty);
            }
            set
            {
                SetValue(ResetPositionActionProperty, value);
            }
        }

        public void ResetPosition() {
            ResetPositionAction?.Invoke();
        }

        public static readonly BindableProperty ReloadStyleActionProperty = BindableProperty.Create(
            nameof(ReloadStyleAction),
            typeof(Action),
            typeof(MapView),
            default(Action),
            BindingMode.OneWayToSource);

        public Action ReloadStyleAction
        {
            get
            {
                return (Action)GetValue(ReloadStyleActionProperty);
            }
            set
            {
                SetValue(ReloadStyleActionProperty, value);
            }
        }

        public void ReloadStyle() {
            ReloadStyleAction?.Invoke();
        }


        public static readonly BindableProperty UpdateShapeOfSourceFuncProperty = BindableProperty.Create(
            nameof(UpdateShapeOfSourceFunc),
            typeof(Func<Annotation, string, bool>),
            typeof(MapView),
            default(Func<Annotation, string, bool>),
             BindingMode.OneWayToSource);

        public Func<Annotation, string, bool> UpdateShapeOfSourceFunc
        {
            get
            {
                return ((Func<Annotation, string, bool>)GetValue(UpdateShapeOfSourceFuncProperty));
            }
            set
            {
                SetValue(UpdateShapeOfSourceFuncProperty, value);
            }
        }

        public bool UpdateShapeOfSource(Annotation shape, string sourceId) {
            return UpdateShapeOfSourceFunc?.Invoke(shape, sourceId) ?? false;
        }

        /// <summary>
        /// The update layer func property.
        /// (layer Id, is visible, is custom layer)
        /// </summary>
        public static readonly BindableProperty UpdateLayerFuncProperty = BindableProperty.Create(
            nameof(UpdateLayerFunc),
            typeof(Func<string, bool, bool, bool>),
            typeof(MapView),
            default(Func<string, bool, bool, bool>),
             BindingMode.OneWayToSource);

        public Func<string, bool, bool, bool> UpdateLayerFunc
        {
            get
            {
                return ((Func<string, bool, bool, bool>)GetValue(UpdateLayerFuncProperty));
            }
            set
            {
                SetValue(UpdateLayerFuncProperty, value);
            }
        }

        public bool UpdateLayer(string layerId, bool isVisible, bool isCustomLayer) {
            return UpdateLayerFunc?.Invoke(layerId, isVisible, isCustomLayer) ?? false;
        }

        /// <summary>
        /// Update view port (camera)
        /// Params: Center location, zoom level, bearing, animated, completion handler
        /// </summary>
        public static readonly BindableProperty UpdateViewPortActionProperty = BindableProperty.Create(
            nameof(UpdateViewPortAction),
            typeof(Action<Position, double?, double?, bool, Action>),
            typeof(MapView),
            default(Action<Position, double?, double?, bool, Action>),
             BindingMode.OneWayToSource);

        public Action<Position, double?, double?, bool, Action> UpdateViewPortAction
        {
            get
            {
                return ((Action<Position, double?, double?, bool, Action>)GetValue(UpdateViewPortActionProperty));
            }
            set
            {
                SetValue(UpdateViewPortActionProperty, value);
            }
        }

        public void UpdateViewPort(Position center, double? zoomLevel = null, double? bearing = null, bool animated = false, Action completionHandler = null) {
            UpdateViewPortAction?.Invoke(center, zoomLevel, bearing, animated, completionHandler);
        }

        //public static readonly BindableProperty GetMapScaleReciprocalFuncProperty = BindableProperty.Create(
        //    nameof(GetMapScaleReciprocalFunc),
        //    typeof(Func<double>),
        //    typeof(MapView),
        //    default(Func<double>),
        //    BindingMode.OneWayToSource);

        //public Func<double> GetMapScaleReciprocalFunc
        //{
        //    get
        //    {
        //        return ((Func<double>)GetValue(GetMapScaleReciprocalFuncProperty));
        //    }
        //    set
        //    {
        //        SetValue(GetMapScaleReciprocalFuncProperty, value);
        //    }
        //}

        /// <summary>
        /// Show/hide scale bar
        /// Input: Is scale bar visible
        /// Output: Is scale bar existing
        /// </summary>
        public static readonly BindableProperty ToggleScaleBarFuncProperty = BindableProperty.Create(
            nameof(ToggleScaleBarFunc),
            typeof(Func<bool, bool>),
            typeof(MapView),
            default(Func<bool, bool>),
            BindingMode.OneWayToSource);

        public Func<bool, bool> ToggleScaleBarFunc
        {
            get
            {
                return ((Func<bool, bool>)GetValue(ToggleScaleBarFuncProperty));
            }
            set
            {
                SetValue(ToggleScaleBarFuncProperty, value);
            }
        }

        public bool ToggleScaleBar(bool isVisible) {
            return ToggleScaleBarFunc?.Invoke(isVisible) ?? false;
        }

        public static readonly BindableProperty GetStyleImageFuncProperty = BindableProperty.Create(
            nameof(GetStyleImageFunc),
            typeof(Func<string, Byte[]>),
            typeof(MapView),
            default(Func<string, Byte[]>),
            BindingMode.OneWayToSource);

        public Func<string, Byte[]> GetStyleImageFunc
        {
            get
            {
                return ((Func<string, Byte[]>)GetValue(GetStyleImageFuncProperty));
            }
            set
            {
                SetValue(GetStyleImageFuncProperty, value);
            }
        }

        public Byte[] GetStyleImage(string imageName) {
            return GetStyleImageFunc?.Invoke(imageName);
        }

        /// <summary>
        /// Get StyleLayer
        /// Params: (layer Id, is custom layer)
        /// </summary>
        public static BindableProperty GetStyleLayerFuncProperty = BindableProperty.Create(
            propertyName: nameof(GetStyleLayerFunc),
            returnType: typeof(Func<string, bool, StyleLayer>),
            declaringType: typeof(MapView),
            defaultValue: default(Func<string, bool, StyleLayer>),
            defaultBindingMode: BindingMode.OneWayToSource
        );


        public Func<string, bool, StyleLayer> GetStyleLayerFunc
        {
            get { return (Func<string, bool, StyleLayer>)GetValue(GetStyleLayerFuncProperty); }
            set { SetValue(GetStyleLayerFuncProperty, value); }
        }

        public StyleLayer GetStyleLayer(string layerId, bool isCustomLayer) {
            return GetStyleLayerFunc?.Invoke(layerId, isCustomLayer);
        }

        public static BindableProperty InsertLayerAboveLayerFuncProperty = BindableProperty.Create(
            propertyName: nameof(InsertLayerAboveLayerFunc),
            returnType: typeof(Func<StyleLayer, string, bool>),
            declaringType: typeof(MapView),
            defaultValue: default(Func<StyleLayer, string, bool>),
            defaultBindingMode: BindingMode.OneWayToSource
        );

        public Func<StyleLayer, string, bool> InsertLayerAboveLayerFunc
        {
            get { return (Func<StyleLayer, string, bool>)GetValue(InsertLayerAboveLayerFuncProperty); }
            set { SetValue(InsertLayerAboveLayerFuncProperty, value); }
        }

        public bool InsertLayerAboveLayer(StyleLayer layerToInsert, string siblingLayerId) {
            return InsertLayerAboveLayerFunc?.Invoke(layerToInsert, siblingLayerId) ?? false;
        }

        public static BindableProperty InsertLayerBelowLayerFuncProperty = BindableProperty.Create(             propertyName: nameof(InsertLayerBelowLayerFunc),             returnType: typeof(Func<StyleLayer, string, bool>),             declaringType: typeof(MapView),             defaultValue: default(Func<StyleLayer, string, bool>),             defaultBindingMode: BindingMode.OneWayToSource         );          public Func<StyleLayer, string, bool> InsertLayerBelowLayerFunc         {             get { return (Func<StyleLayer, string, bool>)GetValue(InsertLayerBelowLayerFuncProperty); }             set { SetValue(InsertLayerBelowLayerFuncProperty, value); }         }          public bool InsertLayerBelowLayer(StyleLayer layerToInsert, string siblingLayerId)
        {             return InsertLayerBelowLayerFunc?.Invoke(layerToInsert, siblingLayerId) ?? false;         } 
    }
}
