using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Plugins.Annotation;
using Com.Mapbox.Mapboxsdk.Utils;
using Java.Interop;
using Java.Lang;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Annotations;
using Naxam.Mapbox.Platform.Droid.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NxAnnotation = Naxam.Mapbox.Annotations.Annotation;

namespace Naxam.Controls.Mapbox.Platform.Droid
{
    public partial class MapViewRenderer : IOnSymbolClickListener
    {
        SymbolManager symbolManager;
        CircleManager circleManager;

        private void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddAnnotations(e.NewItems.Cast<NxAnnotation>().ToArray());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveAnnotations(e.OldItems.Cast<NxAnnotation>().ToArray());
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveAllAnnotations();
                    AddAnnotations(Element.Annotations.ToList());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    var itemsToRemove = new List<NxAnnotation>();
                    foreach (NxAnnotation annotation in e.OldItems)
                    {
                        itemsToRemove.Add(annotation);
                    }
                    RemoveAnnotations(itemsToRemove.ToArray());

                    var itemsToAdd = new List<NxAnnotation>();
                    foreach (NxAnnotation annotation in e.NewItems)
                    {
                        itemsToRemove.Add(annotation);
                    }
                    AddAnnotations(itemsToAdd.ToArray());
                    break;
            }
        }

        void Element_AnnotationsChanged(object sender, AnnotationChangedEventArgs e)
        {
            if (mapReady)
            {
                RemoveAllAnnotations();
                AddAnnotations(Element?.Annotations?.ToArray());
            }

            if (e.OldAnnotations is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= OnAnnotationsCollectionChanged;
            }

            if (e.NewAnnotations is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += OnAnnotationsCollectionChanged;
            }
        }

        void RemoveAnnotations(IList<NxAnnotation> annotations)
        {
            if (map == null)
                return;

            for (int i = 0; i < annotations.Count; i++)
            {
                switch (annotations[i])
                {
                    case SymbolAnnotation symbolAnnotation:
                        {
                            if (symbolManager == null) continue;
                            Java.Lang.Object symbol = null;
                            try
                            {
                                symbol = new Java.Lang.Object(
                                    symbolAnnotation.NativeHandle,
                                    Android.Runtime.JniHandleOwnership.DoNotTransfer
                                    );
                                symbolManager.Delete(symbol);
                            }
                            finally
                            {
                                symbol?.Dispose();
                            }
                        }
                        break;
                    case CircleAnnotation circleAnnotation:
                        {
                            // TODO Android - Map CircleAnnotation
                        }
                        break;
                }
            }
        }

        void AddAnnotations(IList<NxAnnotation> annotations)
        {
            if (map == null || annotations == null || annotations.Count == 0)
                return;

            for (int i = 0; i < annotations.Count; i++)
            {
                switch (annotations[i])
                {
                    case SymbolAnnotation symbolAnnotation:
                        {
                            if (symbolManager == null)
                            {
                                symbolManager = new SymbolManager(fragment.MapView, map, mapStyle);

                                // TODO Provide values from Forms
                                symbolManager.IconAllowOverlap = Boolean.True;
                                symbolManager.TextAllowOverlap = Boolean.True;
                                symbolManager.AddClickListener(this);
                            }

                            if (symbolAnnotation.IconImage?.Source != null)
                            {
                                AddStyleImage(symbolAnnotation.IconImage);
                            }

                            var symbolOptions = symbolAnnotation.ToSymbolOptions();
                            var symbol = Android.Runtime.Extensions.JavaCast<Symbol>(symbolManager.Create(symbolOptions));
                            symbolAnnotation.Id = symbol.Id.ToString();
                        }
                        break;
                    case CircleAnnotation circleAnnotation:
                        {
                            // TODO Handle other type of annotation
                        }
                        break;
                }
            }
        }

        void RemoveAllAnnotations()
        {
            symbolManager?.DeleteAll();
        }

        public void OnAnnotationClick(Symbol symbol)
        {
            if (symbol == null) return;

            if (Element.DidTapOnMarkerCommand?.CanExecute(symbol.Id.ToString()) == true)
            {
                Element.DidTapOnMarkerCommand.Execute(symbol.Id.ToString());
            }
        }
    }

    public partial class MapViewRenderer : IMapFunctions
    {
        public void AddStyleImage(IconImageSource iconImageSource)
        {
            if (iconImageSource?.Source == null) return;

            switch (iconImageSource.Source)
            {
                // TODO: Android = Handle other type of ImageSoure
                case FileImageSource fileImageSource:
                    var cachedImage = mapStyle.GetImage(iconImageSource.Id);
                    if (cachedImage != null) break;

                    var resId = fileImageSource.GetResId();
                    var drawable = Context.Resources.GetDrawable(resId, Context.Theme);
                    var bitmap = BitmapUtils.GetBitmapFromDrawable(drawable);

                    if (bitmap == null)
                    {
                        drawable?.Dispose();
                        break;
                    }

                    mapStyle.AddImage(iconImageSource.Id, bitmap, iconImageSource.IsTemplate);

                    break;
            }
        }
    }
}