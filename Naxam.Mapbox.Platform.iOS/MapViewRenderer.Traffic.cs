using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Naxam.Mapbox;
using GeoJSON.Net.Feature;
using System.Linq;
using Naxam.Mapbox.Platform.iOS.Extensions;
using Mapbox;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using Foundation;
using Xamarin.Forms.Platform.iOS;
using Naxam.Mapbox.Expressions;
using System.Collections.Generic;
using Naxam.Controls.Forms;
using Naxam.Mapbox.Traffic;

namespace Naxam.Controls.Mapbox.Platform.iOS
{
    public partial class MapViewRenderer : IMapFunctions
    {
        TrafficPlugin trafficPlugin;

        public void ShowTraffic(bool visible)
        {
            if (map == null || mapStyle == null) return;

            if (trafficPlugin == null) {
                trafficPlugin = new TrafficPlugin(map, mapStyle);
            }

            trafficPlugin.SetVisibility(visible);

            // TODO Clean up trafficPlugin
        }
    }
}


namespace Naxam.Mapbox.Traffic
{
    public sealed class TrafficPlugin
    {
        public bool Visible { get; private set; }

        private MGLMapView mapboxMap;
        private MGLStyle style;
        private readonly List<string> layerIds = new List<string>();
        private readonly string belowLayer;

        /**
         * Create a traffic plugin.
         *
         * @param mapView   the MapView to apply the traffic plugin to
         * @param mapboxMap the MapboxMap to apply traffic plugin with
         */
        public TrafficPlugin(MGLMapView mapboxMap, MGLStyle style)
        : this(mapboxMap, style, null)
        {
        }

        /**
         * Create a traffic plugin.
         *
         * @param mapView    the MapView to apply the traffic plugin to
         * @param mapboxMap  the MapboxMap to apply traffic plugin with
         * @param belowLayer the layer id where you'd like the traffic to display below
         */
        public TrafficPlugin(MGLMapView mapboxMap, MGLStyle style,
                             string belowLayer)
        {
            this.mapboxMap = mapboxMap;
            this.style = style;
            this.belowLayer = belowLayer;
        }

        /**
         * Toggles the visibility of the traffic layers.
         *
         * @param visible true for visible, false for none
         */
        public void SetVisibility(bool visible)
        {
            this.Visible = visible;

            var source = style.SourceWithIdentifier(TrafficData.SOURCE_ID);
            if (source == null) {
                Initialise();
            }

            var layers = style.Layers;
            foreach (var layer in layers) {
                if (layerIds.Contains(layer.Identifier)) {
                    layer.Visible = visible;
                }
            }
        }

        /**
         * Initialise the traffic source and layers.
         */
        private void Initialise()
        {
            //try {
            AddTrafficSource();
            AddTrafficLayers();
            //}
            //catch (Exception exception) {
            //    Timber.e(exception, "Unable to attach Traffic to current style: ");
            //}
            //catch (UnsatisfiedLinkError error) {
            //    Timber.e(error, "Unable to load native libraries: ");
            //}
        }

        /**
         * Adds traffic source to the map.
         */
        private void AddTrafficSource()
        {
            var trafficSource = new MGLVectorTileSource(TrafficData.SOURCE_ID, NSUrl.FromString(TrafficData.SOURCE_URL));
            style.AddSource(trafficSource);
        }

        /**
         * Adds traffic layers to the map.
         */
        private void AddTrafficLayers()
        {
            AddLocalLayer();
            AddSecondaryLayer();
            AddPrimaryLayer();
            AddTrunkLayer();
            AddMotorwayLayer();
        }

        /**
         * Add local layer to the map.
         */
        private void AddLocalLayer()
        {
            var local = TrafficLayer.GetLineLayer(
                style.SourceWithIdentifier(TrafficData.SOURCE_ID),
              Local.BASE_LAYER_ID,
              Local.ZOOM_LEVEL,
              Local.FILTER,
              Local.FUNCTION_LINE_COLOR,
              Local.FUNCTION_LINE_WIDTH,
              Local.FUNCTION_LINE_OFFSET
            );

            var localCase = TrafficLayer.GetLineLayer(
                style.SourceWithIdentifier(TrafficData.SOURCE_ID),
              Local.CASE_LAYER_ID,
              Local.ZOOM_LEVEL,
              Local.FILTER,
              Local.FUNCTION_LINE_COLOR_CASE,
              Local.FUNCTION_LINE_WIDTH_CASE,
              Local.FUNCTION_LINE_OFFSET,
              Local.FUNCTION_LINE_OPACITY_CASE
            );

            AddTrafficLayersToMap(localCase, local, PlaceLayerBelow());
        }

        /**
         * Attempts to find the layer which the traffic should be placed below. Depending on the style, this might not always
         * be accurate.
         */
        private string PlaceLayerBelow()
        {
            if (string.IsNullOrWhiteSpace(belowLayer)) {
                var styleLayers = style.Layers;
                for (int i = styleLayers.Length - 1; i >= 0; i--) {
                    if (!(styleLayers[i] is MGLSymbolStyleLayer)) {
                        return styleLayers[i].Identifier;
                    }
                }
            }
            return belowLayer;
        }

        /**
         * Add secondary layer to the map.
         */
        private void AddSecondaryLayer()
        {
            var secondary = TrafficLayer.GetLineLayer(
                style.SourceWithIdentifier(TrafficData.SOURCE_ID),
              Secondary.BASE_LAYER_ID,
              Secondary.ZOOM_LEVEL,
              Secondary.FILTER,
              Secondary.FUNCTION_LINE_COLOR,
              Secondary.FUNCTION_LINE_WIDTH,
              Secondary.FUNCTION_LINE_OFFSET
            );

            var secondaryCase = TrafficLayer.GetLineLayer(
                style.SourceWithIdentifier(TrafficData.SOURCE_ID),
              Secondary.CASE_LAYER_ID,
              Secondary.ZOOM_LEVEL,
              Secondary.FILTER,
              Secondary.FUNCTION_LINE_COLOR_CASE,
              Secondary.FUNCTION_LINE_WIDTH_CASE,
              Secondary.FUNCTION_LINE_OFFSET,
              Secondary.FUNCTION_LINE_OPACITY_CASE
            );

            AddTrafficLayersToMap(secondaryCase, secondary, GetLastAddedLayerId());
        }

        /**
         * Add primary layer to the map.
         */
        private void AddPrimaryLayer()
        {
            var primary = TrafficLayer.GetLineLayer(
                style.SourceWithIdentifier(TrafficData.SOURCE_ID),
              Primary.BASE_LAYER_ID,
              Primary.ZOOM_LEVEL,
              Primary.FILTER,
              Primary.FUNCTION_LINE_COLOR,
              Primary.FUNCTION_LINE_WIDTH,
              Primary.FUNCTION_LINE_OFFSET
            );

            var primaryCase = TrafficLayer.GetLineLayer(
                style.SourceWithIdentifier(TrafficData.SOURCE_ID),
              Primary.CASE_LAYER_ID,
              Primary.ZOOM_LEVEL,
              Primary.FILTER,
              Primary.FUNCTION_LINE_COLOR_CASE,
              Primary.FUNCTION_LINE_WIDTH_CASE,
              Primary.FUNCTION_LINE_OFFSET,
              Primary.FUNCTION_LINE_OPACITY_CASE
            );

            AddTrafficLayersToMap(primaryCase, primary, GetLastAddedLayerId());
        }

        /**
         * Add trunk layer to the map.
         */
        private void AddTrunkLayer()
        {
            var trunk = TrafficLayer.GetLineLayer(
                style.SourceWithIdentifier(TrafficData.SOURCE_ID),
              Trunk.BASE_LAYER_ID,
              Trunk.ZOOM_LEVEL,
              Trunk.FILTER,
              Trunk.FUNCTION_LINE_COLOR,
              Trunk.FUNCTION_LINE_WIDTH,
              Trunk.FUNCTION_LINE_OFFSET
            );

            var trunkCase = TrafficLayer.GetLineLayer(
                style.SourceWithIdentifier(TrafficData.SOURCE_ID),
              Trunk.CASE_LAYER_ID,
              Trunk.ZOOM_LEVEL,
              Trunk.FILTER,
              Trunk.FUNCTION_LINE_COLOR_CASE,
              Trunk.FUNCTION_LINE_WIDTH_CASE,
              Trunk.FUNCTION_LINE_OFFSET
            );

            AddTrafficLayersToMap(trunkCase, trunk, GetLastAddedLayerId());
        }

        /**
         * Add motorway layer to the map.
         */
        private void AddMotorwayLayer()
        {
            var motorWay = TrafficLayer.GetLineLayer(
                style.SourceWithIdentifier(TrafficData.SOURCE_ID),
              MotorWay.BASE_LAYER_ID,
              MotorWay.ZOOM_LEVEL,
              MotorWay.FILTER,
              MotorWay.FUNCTION_LINE_COLOR,
              MotorWay.FUNCTION_LINE_WIDTH,
              MotorWay.FUNCTION_LINE_OFFSET
            );

            var motorwayCase = TrafficLayer.GetLineLayer(
                style.SourceWithIdentifier(TrafficData.SOURCE_ID),
              MotorWay.CASE_LAYER_ID,
              MotorWay.ZOOM_LEVEL,
              MotorWay.FILTER,
              MotorWay.FUNCTION_LINE_COLOR_CASE,
              MotorWay.FUNCTION_LINE_WIDTH_CASE,
              MotorWay.FUNCTION_LINE_OFFSET
            );

            AddTrafficLayersToMap(motorwayCase, motorWay, GetLastAddedLayerId());
        }

        /**
         * Returns the last added layer id.
         *
         * @return the id of the last added layer
         */
        private string GetLastAddedLayerId()
        {
            return layerIds[layerIds.Count - 1];
        }

        /**
         * Add Layer to the map and track the id.
         *
         * @param layer        the layer to be added to the map
         * @param idAboveLayer the id of the layer above
         */
        private void AddTrafficLayersToMap(MGLStyleLayer layerCase, MGLStyleLayer layer, string idAboveLayer)
        {
            style.InsertLayerBelow(layerCase, style.LayerWithIdentifier(idAboveLayer));
            style.InsertLayerAbove(layer, layerCase);
            layerIds.Add(layerCase.Identifier);
            layerIds.Add(layer.Identifier);
        }

        private static class TrafficLayer
        {

            public static MGLLineStyleLayer GetLineLayer(MGLSource source, String lineLayerId, float minZoom, Expression filter,
                                          Expression lineColor, Expression lineWidth, Expression lineOffset)
            {
                return GetLineLayer(source, lineLayerId, minZoom, filter, lineColor, lineWidth, lineOffset, null);
            }

            public static MGLLineStyleLayer GetLineLayer(MGLSource source, string lineLayerId, float minZoom, Expression filter,
                                          Expression lineColorExpression, Expression lineWidthExpression,
                                          Expression lineOffsetExpression, Expression lineOpacityExpression)
            {
                var lineLayer = new MGLLineStyleLayer(lineLayerId, source);
                lineLayer.SourceLayerIdentifier = (TrafficData.SOURCE_LAYER);
                lineLayer.LineCap = Expression.Literal(LayerProperty.LINE_CAP_ROUND).ToNative();
                lineLayer.LineJoin = Expression.Literal(LayerProperty.LINE_JOIN_ROUND).ToNative();
                lineLayer.LineColor = lineColorExpression.ToNative();
                lineLayer.LineWidth = lineWidthExpression.ToNative();
                lineLayer.LineOffset = lineOffsetExpression.ToNative();

                if (lineOpacityExpression != null) {
                    lineLayer.LineOpacity = lineOpacityExpression.ToNative();
                }

                lineLayer.Predicate = filter.ToPredicate();
                lineLayer.MinimumZoomLevel = minZoom;
                return lineLayer;
            }
        }

        private static class TrafficFunction
        {
            public static Expression GetLineColorFunction(Color low, Color moderate, Color heavy,
                                                   Color severe)
            {
                return Expression.Match(
                    Expression.Get("congestion"),
                    Expression.Color(Color.Transparent),
                    Expression.CreateStop("low", Expression.Color(low)),
                    Expression.CreateStop("moderate", Expression.Color(moderate)),
                    Expression.CreateStop("heavy", Expression.Color(heavy)),
                    Expression.CreateStop("severe", Expression.Color(severe)));
            }
        }

        static class TrafficData
        {
            public const string SOURCE_ID = "traffic";
            public const string SOURCE_LAYER = "traffic";
            public const string SOURCE_URL = "mapbox://mapbox.mapbox-traffic-v1";
        }

        class TrafficType
        {
            public static readonly Expression FUNCTION_LINE_COLOR = TrafficFunction.GetLineColorFunction(TrafficColor.BASE_GREEN,
              TrafficColor.BASE_YELLOW, TrafficColor.BASE_ORANGE, TrafficColor.BASE_RED);
            public static readonly Expression FUNCTION_LINE_COLOR_CASE = TrafficFunction.GetLineColorFunction(
              TrafficColor.CASE_GREEN, TrafficColor.CASE_YELLOW, TrafficColor.CASE_ORANGE, TrafficColor.CASE_RED);
        }

        class MotorWay : TrafficType
        {
            public const string BASE_LAYER_ID = "traffic-motorway";
            public const string CASE_LAYER_ID = "traffic-motorway-bg";
            public const float ZOOM_LEVEL = 6.0f;
            public static readonly Expression FILTER = Expression.Match(
              Expression.Get("class"), Expression.Literal(false),
          Expression.CreateStop("motorway", true)
        );

            public static readonly Expression FUNCTION_LINE_WIDTH = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(6, 0.5f),
              Expression.CreateStop(9, 1.5f),
              Expression.CreateStop(18.0f, 14.0f),
              Expression.CreateStop(20.0f, 18.0f)
            );

            public static readonly Expression FUNCTION_LINE_WIDTH_CASE = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(6, 0.5f),
              Expression.CreateStop(9, 3.0f),
              Expression.CreateStop(18.0f, 16.0f),
              Expression.CreateStop(20.0f, 20.0f)
            );

            public static readonly Expression FUNCTION_LINE_OFFSET = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(7, 0.0f),
              Expression.CreateStop(9, 1.2f),
              Expression.CreateStop(11, 1.2f),
              Expression.CreateStop(18, 10.0f),
              Expression.CreateStop(20, 15.5f));
        }

        class Trunk : TrafficType
        {
            public const string BASE_LAYER_ID = "traffic-trunk";
            public const string CASE_LAYER_ID = "traffic-trunk-bg";
            public const float ZOOM_LEVEL = 6.0f;

            public static readonly Expression FILTER = Expression.Match(
              Expression.Get("class"), Expression.Literal(false),
              Expression.CreateStop("trunk", true)
            );

            public static readonly Expression FUNCTION_LINE_WIDTH = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(8, 0.75f),
              Expression.CreateStop(18, 11f),
              Expression.CreateStop(20f, 15.0f)
            );

            public static readonly Expression FUNCTION_LINE_WIDTH_CASE = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(8, 0.5f),
              Expression.CreateStop(9, 2.25f),
              Expression.CreateStop(18.0f, 13.0f),
              Expression.CreateStop(20.0f, 17.5f)
            );

            public static readonly Expression FUNCTION_LINE_OFFSET = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(7, 0.0f),
              Expression.CreateStop(9, 1f),
              Expression.CreateStop(18, 13f),
              Expression.CreateStop(20, 18.0f));
        }

        class Primary : TrafficType
        {
            public const string BASE_LAYER_ID = "traffic-primary";
            public const string CASE_LAYER_ID = "traffic-primary-bg";
            public const float ZOOM_LEVEL = 6.0f;

            public static readonly Expression FILTER = Expression.Match(
              Expression.Get("class"), Expression.Literal(false),
                  Expression.CreateStop("primary", Expression.Literal(true))
                );

            public static readonly Expression FUNCTION_LINE_WIDTH = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(10, 1.0f),
              Expression.CreateStop(15, 4.0f),
              Expression.CreateStop(20, 16f)
            );

            public static readonly Expression FUNCTION_LINE_WIDTH_CASE = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(10, 0.75f),
              Expression.CreateStop(15, 6f),
              Expression.CreateStop(20.0f, 18.0f)
            );

            public static readonly Expression FUNCTION_LINE_OFFSET = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(10, 0.0f),
              Expression.CreateStop(12, 1.5f),
              Expression.CreateStop(18, 13f),
              Expression.CreateStop(20, 16.0f)
            );

            public static readonly Expression FUNCTION_LINE_OPACITY_CASE = Expression.Interpolate(Expression.Exponential(1.0f), Expression.Zoom(),
              Expression.CreateStop(11, 0.0f),
              Expression.CreateStop(12, 1.0f)
            );
        }

        class Secondary : TrafficType
        {
            public const string BASE_LAYER_ID = "traffic-secondary-tertiary";
            public const string CASE_LAYER_ID = "traffic-secondary-tertiary-bg";
            public const float ZOOM_LEVEL = 6.0f;

            public static readonly Expression FILTER = Expression.Match(
              Expression.Get("class"), Expression.Literal(false),
                  Expression.CreateStop("secondary", true),
                  Expression.CreateStop("tertiary", true)
                );

            public static readonly Expression FUNCTION_LINE_WIDTH = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(9, 0.5f),
              Expression.CreateStop(18, 9.0f),
              Expression.CreateStop(20, 14f)
            );

            public static readonly Expression FUNCTION_LINE_WIDTH_CASE = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(9, 1.5f),
              Expression.CreateStop(18, 11f),
              Expression.CreateStop(20.0f, 16.5f)
            );

            public static readonly Expression FUNCTION_LINE_OFFSET = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(10, 0.5f),
              Expression.CreateStop(15, 5f),
              Expression.CreateStop(18, 11f),
              Expression.CreateStop(20, 14.5f)
            );

            public static readonly Expression FUNCTION_LINE_OPACITY_CASE = Expression.Interpolate(Expression.Exponential(1.0f), Expression.Zoom(),
              Expression.CreateStop(13, 0.0f),
              Expression.CreateStop(14, 1.0f)
            );
        }

        class Local : TrafficType
        {
            public const string BASE_LAYER_ID = "traffic-local";
            public const string CASE_LAYER_ID = "traffic-local-case";
            public const float ZOOM_LEVEL = 15.0f;

            public static readonly Expression FILTER = Expression.Match(
              Expression.Get("class"), Expression.Literal(false),
                  Expression.CreateStop("motorway_link", true),
                  Expression.CreateStop("service", true),
                  Expression.CreateStop("street", true)
                );

            public static readonly Expression FUNCTION_LINE_WIDTH = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(14, 1.5f),
              Expression.CreateStop(20, 13.5f)
            );
            public static readonly Expression FUNCTION_LINE_WIDTH_CASE = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(14, 2.5f),
              Expression.CreateStop(20, 15.5f)
            );
            public static readonly Expression FUNCTION_LINE_OFFSET = Expression.Interpolate(Expression.Exponential(1.5f), Expression.Zoom(),
              Expression.CreateStop(14, 2f),
              Expression.CreateStop(20, 18f)
            );

            public static readonly Expression FUNCTION_LINE_OPACITY_CASE = Expression.Interpolate(Expression.Exponential(1.0f), Expression.Zoom(),
              Expression.CreateStop(15, 0.0f),
              Expression.CreateStop(16, 1.0f)
            );
        }

        private static class TrafficColor
        {
            public readonly static Color BASE_GREEN = Color.FromRgb(0x39, 0xc6, 0x6d);
            public readonly static Color CASE_GREEN = Color.FromRgb(0x05, 0x94, 0x41);
            public readonly static Color BASE_YELLOW = Color.FromRgb(0xff, 0x8c, 0x1a);
            public readonly static Color CASE_YELLOW = Color.FromRgb(0xd6, 0x6b, 0x00);
            public readonly static Color BASE_ORANGE = Color.FromRgb(0xff, 0x00, 0x15);
            public readonly static Color CASE_ORANGE = Color.FromRgb(0xbd, 0x00, 0x10);
            public readonly static Color BASE_RED = Color.FromRgb(0x98, 0x1b, 0x25);
            public readonly static Color CASE_RED = Color.FromRgb(0x5f, 0x11, 0x17);
        }
    }
}