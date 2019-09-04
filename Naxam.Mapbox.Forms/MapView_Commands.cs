using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naxam.Controls.Forms
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
            get => (Func<string, bool>)GetValue(CanShowCalloutCheckerProperty) ?? DefaultCanShowCalloutChecker;
            set => SetValue(CanShowCalloutCheckerProperty, value);
        }

        public static readonly BindableProperty TakeSnapshotFuncProperty = BindableProperty.Create(
            nameof(TakeSnapshotFunc),
            typeof(Func<Task<byte[]>>),
            typeof(MapView),
            default(Func<Task<byte[]>>),
            defaultBindingMode: BindingMode.OneWayToSource);

        public Func<Task<byte[]>> TakeSnapshotFunc
        {
            get => (Func<Task<byte[]>>)GetValue(TakeSnapshotFuncProperty);
            set => SetValue(TakeSnapshotFuncProperty, value);
        }

        public Task<byte[]> TakSnapshot()
        {
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
            get => ((Func<Point, double, string[], IFeature[]>)GetValue(GetFeaturesAroundPointFuncProperty));
            set => SetValue(GetFeaturesAroundPointFuncProperty, value);
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

        public void ResetPosition()
        {
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

        public void ReloadStyle()
        {
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
            get => (Func<Annotation, string, bool>)GetValue(UpdateShapeOfSourceFuncProperty);
            set => SetValue(UpdateShapeOfSourceFuncProperty, value);
        }
        public bool UpdateShapeOfSource(Annotation shape, string sourceId)
        {
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
            get => ((Func<string, bool, bool, bool>)GetValue(UpdateLayerFuncProperty));
            set => SetValue(UpdateLayerFuncProperty, value);
        }
        public bool UpdateLayer(string layerId, bool isVisible, bool isCustomLayer)
        {
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
            get => ((Action<Position, double?, double?, bool, Action>)GetValue(UpdateViewPortActionProperty));
            set => SetValue(UpdateViewPortActionProperty, value);
        }

        public void UpdateViewPort(Position center, double? zoomLevel = null, double? bearing = null, bool animated = false, Action completionHandler = null)
        {
            UpdateViewPortAction?.Invoke(center, zoomLevel, bearing, animated, completionHandler);
        }

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
            get => (Func<bool, bool>)GetValue(ToggleScaleBarFuncProperty);
            set => SetValue(ToggleScaleBarFuncProperty, value);
        }

        public bool ToggleScaleBar(bool isVisible)
        {
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
            get => (Func<string, Byte[]>)GetValue(GetStyleImageFuncProperty);
            set => SetValue(GetStyleImageFuncProperty, value);
        }

        public Byte[] GetStyleImage(string imageName)
        {
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

        public StyleLayer GetStyleLayer(string layerId, bool isCustomLayer)
        {
            return GetStyleLayerFunc?.Invoke(layerId, isCustomLayer);
        }

        #region Layers
        public static BindableProperty InsertLayerAboveLayerFuncProperty = BindableProperty.Create(
            nameof(InsertLayerAboveLayerFunc),
            typeof(Func<StyleLayer, string, bool>),
            typeof(MapView),
            default(Func<StyleLayer, string, bool>),
            BindingMode.OneWayToSource
        );

        public Func<StyleLayer, string, bool> InsertLayerAboveLayerFunc
        {
            get { return (Func<StyleLayer, string, bool>)GetValue(InsertLayerAboveLayerFuncProperty); }
            set { SetValue(InsertLayerAboveLayerFuncProperty, value); }
        }

        public bool InsertLayerAboveLayer(StyleLayer layerToInsert, string siblingLayerId)
        {
            return InsertLayerAboveLayerFunc?.Invoke(layerToInsert, siblingLayerId) ?? false;
        }

        public static BindableProperty InsertLayerBelowLayerFuncProperty = BindableProperty.Create(             nameof(InsertLayerBelowLayerFunc),             typeof(Func<StyleLayer, string, bool>),             typeof(MapView),             default(Func<StyleLayer, string, bool>),             BindingMode.OneWayToSource         );          public Func<StyleLayer, string, bool> InsertLayerBelowLayerFunc         {             get { return (Func<StyleLayer, string, bool>)GetValue(InsertLayerBelowLayerFuncProperty); }             set { SetValue(InsertLayerBelowLayerFuncProperty, value); }         }          public bool InsertLayerBelowLayer(StyleLayer layerToInsert, string siblingLayerId)
        {             return InsertLayerBelowLayerFunc?.Invoke(layerToInsert, siblingLayerId) ?? false;         }
        #endregion

        #region Annotations
        /// <summary>
        /// Select an annotation
        /// Params (annotation id, is animated)
        /// </summary>
        public static BindableProperty SelectAnnotationActionProperty = BindableProperty.Create(             nameof(SelectAnnotationAction),             typeof(Action<Tuple<string, bool>>),             typeof(MapView),             default(Action<Tuple<string, bool>>),             BindingMode.OneWayToSource         );          public Action<Tuple<string, bool>> SelectAnnotationAction         {             get { return (Action<Tuple<string, bool>>)GetValue(SelectAnnotationActionProperty); }             set { SetValue(SelectAnnotationActionProperty, value); }         } 
        public static BindableProperty DeselectAnnotationActionProperty = BindableProperty.Create(             nameof(DeselectAnnotationAction),             typeof(Action<Tuple<string, bool>>),             typeof(MapView),             default(Action<Tuple<string, bool>>),             BindingMode.OneWayToSource         );          public Action<Tuple<string, bool>> DeselectAnnotationAction         {             get { return (Action<Tuple<string, bool>>)GetValue(DeselectAnnotationActionProperty); }             set { SetValue(DeselectAnnotationActionProperty, value); }         } 
        #endregion

        public static BindableProperty ApplyOfflinePackFuncProperty = BindableProperty.Create(
            nameof(ApplyOfflinePackFunc),
            typeof(Func<OfflinePack, bool>),
            typeof(MapView),
            default(Func<OfflinePack, bool>),
            BindingMode.OneWayToSource
       );

        public Func<OfflinePack, bool> ApplyOfflinePackFunc
        {
            get { return (Func<OfflinePack, bool>)GetValue(ApplyOfflinePackFuncProperty); }
            set { SetValue(ApplyOfflinePackFuncProperty, value); }
        }

        public bool ApplyOfflinePack(OfflinePack pack)
        {
            return ApplyOfflinePackFunc?.Invoke(pack)?? false;
        }
    }
}
