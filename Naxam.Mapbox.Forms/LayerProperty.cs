namespace Naxam.Controls.Forms
{
    public static class LayerProperty
    {
        // VISIBILITY: Whether this layer is displayed.

        /**
         * The layer is shown.
         */
        public const string VISIBLE = "visible";

        /**
         * The layer is hidden.
         */
        public const string NONE = "none";

        // LINE_CAP: The display of line endings.

        /**
         * A cap with a squared-off end which is drawn to the exact endpoint of the line.
         */
        public const string LINE_CAP_BUTT = "butt";

        /**
         * A cap with a rounded end which is drawn beyond the endpoint of the line at a radius of one-half of the line's width and centered on the endpoint of the line.
         */
        public const string LINE_CAP_ROUND = "round";

        /**
         * A cap with a squared-off end which is drawn beyond the endpoint of the line at a distance of one-half of the line's width.
         */
        public const string LINE_CAP_SQUARE = "square";

        // LINE_JOIN: The display of lines when joining.

        /**
         * A join with a squared-off end which is drawn beyond the endpoint of the line at a distance of one-half of the line's width.
         */
        public const string LINE_JOIN_BEVEL = "bevel";

        /**
         * A join with a rounded end which is drawn beyond the endpoint of the line at a radius of one-half of the line's width and centered on the endpoint of the line.
         */
        public const string LINE_JOIN_ROUND = "round";

        /**
         * A join with a sharp, angled corner which is drawn with the outer sides beyond the endpoint of the path until they meet.
         */
        public const string LINE_JOIN_MITER = "miter";

        // SYMBOL_PLACEMENT: Label placement relative to its geometry.

        /**
         * The label is placed at the point where the geometry is located.
         */
        public const string SYMBOL_PLACEMENT_POINT = "point";

        /**
         * The label is placed along the line of the geometry. Can only be used on LineString and Polygon geometries.
         */
        public const string SYMBOL_PLACEMENT_LINE = "line";

        /**
         * The label is placed at the center of the line of the geometry. Can only be used on LineString and Polygon geometries. Note that a single feature in a vector tile may contain multiple line geometries.
         */
        public const string SYMBOL_PLACEMENT_LINE_CENTER = "line-center";

        // SYMBOL_Z_ORDER: Controls the order in which overlapping symbols in the same layer are rendered

        /**
         * If symbol sort key is set, sort based on that. Otherwise sort symbols by their y-position relative to the viewport.
         */
        public const string SYMBOL_Z_ORDER_AUTO = "auto";

        /**
         * Symbols will be sorted by their y-position relative to the viewport.
         */
        public const string SYMBOL_Z_ORDER_VIEWPORT_Y = "viewport-y";

        /**
         * Symbols will be rendered in the same order as the source data with no sorting applied.
         */
        public const string SYMBOL_Z_ORDER_SOURCE = "source";

        // ICON_ROTATION_ALIGNMENT: In combination with `symbol-placement`, determines the rotation behavior of icons.

        /**
         * When {@link SYMBOL_PLACEMENT} is set to {@link Property#SYMBOL_PLACEMENT_POINT}, aligns icons east-west. When {@link SYMBOL_PLACEMENT} is set to {@link Property#SYMBOL_PLACEMENT_LINE} or {@link Property#SYMBOL_PLACEMENT_LINE_CENTER}, aligns icon x-axes with the line.
         */
        public const string ICON_ROTATION_ALIGNMENT_MAP = "map";

        /**
         * Produces icons whose x-axes are aligned with the x-axis of the viewport, regardless of the value of {@link SYMBOL_PLACEMENT}.
         */
        public const string ICON_ROTATION_ALIGNMENT_VIEWPORT = "viewport";

        /**
         * When {@link SYMBOL_PLACEMENT} is set to {@link Property#SYMBOL_PLACEMENT_POINT}, this is equivalent to {@link Property#ICON_ROTATION_ALIGNMENT_VIEWPORT}. When {@link SYMBOL_PLACEMENT} is set to {@link Property#SYMBOL_PLACEMENT_LINE} or {@link Property#SYMBOL_PLACEMENT_LINE_CENTER}, this is equivalent to {@link Property#ICON_ROTATION_ALIGNMENT_MAP}.
         */
        public const string ICON_ROTATION_ALIGNMENT_AUTO = "auto";

        // ICON_TEXT_FIT: Scales the icon to fit around the associated text.

        /**
         * The icon is displayed at its intrinsic aspect ratio.
         */
        public const string ICON_TEXT_FIT_NONE = "none";

        /**
         * The icon is scaled in the x-dimension to fit the width of the text.
         */
        public const string ICON_TEXT_FIT_WIDTH = "width";

        /**
         * The icon is scaled in the y-dimension to fit the height of the text.
         */
        public const string ICON_TEXT_FIT_HEIGHT = "height";

        /**
         * The icon is scaled in both x- and y-dimensions.
         */
        public const string ICON_TEXT_FIT_BOTH = "both";

        // ICON_ANCHOR: Part of the icon placed closest to the anchor.

        /**
         * The center of the icon is placed closest to the anchor.
         */
        public const string ICON_ANCHOR_CENTER = "center";

        /**
         * The left side of the icon is placed closest to the anchor.
         */
        public const string ICON_ANCHOR_LEFT = "left";

        /**
         * The right side of the icon is placed closest to the anchor.
         */
        public const string ICON_ANCHOR_RIGHT = "right";

        /**
         * The top of the icon is placed closest to the anchor.
         */
        public const string ICON_ANCHOR_TOP = "top";

        /**
         * The bottom of the icon is placed closest to the anchor.
         */
        public const string ICON_ANCHOR_BOTTOM = "bottom";

        /**
         * The top left corner of the icon is placed closest to the anchor.
         */
        public const string ICON_ANCHOR_TOP_LEFT = "top-left";

        /**
         * The top right corner of the icon is placed closest to the anchor.
         */
        public const string ICON_ANCHOR_TOP_RIGHT = "top-right";

        /**
         * The bottom left corner of the icon is placed closest to the anchor.
         */
        public const string ICON_ANCHOR_BOTTOM_LEFT = "bottom-left";

        /**
         * The bottom right corner of the icon is placed closest to the anchor.
         */
        public const string ICON_ANCHOR_BOTTOM_RIGHT = "bottom-right";

        // ICON_PITCH_ALIGNMENT: Orientation of icon when map is pitched.

        /**
         * The icon is aligned to the plane of the map.
         */
        public const string ICON_PITCH_ALIGNMENT_MAP = "map";

        /**
         * The icon is aligned to the plane of the viewport.
         */
        public const string ICON_PITCH_ALIGNMENT_VIEWPORT = "viewport";

        /**
         * Automatically matches the value of {@link ICON_ROTATION_ALIGNMENT}.
         */
        public const string ICON_PITCH_ALIGNMENT_AUTO = "auto";

        // TEXT_PITCH_ALIGNMENT: Orientation of text when map is pitched.

        /**
         * The text is aligned to the plane of the map.
         */
        public const string TEXT_PITCH_ALIGNMENT_MAP = "map";

        /**
         * The text is aligned to the plane of the viewport.
         */
        public const string TEXT_PITCH_ALIGNMENT_VIEWPORT = "viewport";

        /**
         * Automatically matches the value of {@link TEXT_ROTATION_ALIGNMENT}.
         */
        public const string TEXT_PITCH_ALIGNMENT_AUTO = "auto";

        // TEXT_ROTATION_ALIGNMENT: In combination with `symbol-placement`, determines the rotation behavior of the individual glyphs forming the text.

        /**
         * When {@link SYMBOL_PLACEMENT} is set to {@link Property#SYMBOL_PLACEMENT_POINT}, aligns text east-west. When {@link SYMBOL_PLACEMENT} is set to {@link Property#SYMBOL_PLACEMENT_LINE} or {@link Property#SYMBOL_PLACEMENT_LINE_CENTER}, aligns text x-axes with the line.
         */
        public const string TEXT_ROTATION_ALIGNMENT_MAP = "map";

        /**
         * Produces glyphs whose x-axes are aligned with the x-axis of the viewport, regardless of the value of {@link SYMBOL_PLACEMENT}.
         */
        public const string TEXT_ROTATION_ALIGNMENT_VIEWPORT = "viewport";

        /**
         * When {@link SYMBOL_PLACEMENT} is set to {@link Property#SYMBOL_PLACEMENT_POINT}, this is equivalent to {@link Property#TEXT_ROTATION_ALIGNMENT_VIEWPORT}. When {@link SYMBOL_PLACEMENT} is set to {@link Property#SYMBOL_PLACEMENT_LINE} or {@link Property#SYMBOL_PLACEMENT_LINE_CENTER}, this is equivalent to {@link Property#TEXT_ROTATION_ALIGNMENT_MAP}.
         */
        public const string TEXT_ROTATION_ALIGNMENT_AUTO = "auto";

        // TEXT_JUSTIFY: Text justification options.

        /**
         * The text is aligned towards the anchor position.
         */
        public const string TEXT_JUSTIFY_AUTO = "auto";

        /**
         * The text is aligned to the left.
         */
        public const string TEXT_JUSTIFY_LEFT = "left";

        /**
         * The text is centered.
         */
        public const string TEXT_JUSTIFY_CENTER = "center";

        /**
         * The text is aligned to the right.
         */
        public const string TEXT_JUSTIFY_RIGHT = "right";

        // TEXT_ANCHOR: Part of the text placed closest to the anchor.

        /**
         * The center of the text is placed closest to the anchor.
         */
        public const string TEXT_ANCHOR_CENTER = "center";

        /**
         * The left side of the text is placed closest to the anchor.
         */
        public const string TEXT_ANCHOR_LEFT = "left";

        /**
         * The right side of the text is placed closest to the anchor.
         */
        public const string TEXT_ANCHOR_RIGHT = "right";

        /**
         * The top of the text is placed closest to the anchor.
         */
        public const string TEXT_ANCHOR_TOP = "top";

        /**
         * The bottom of the text is placed closest to the anchor.
         */
        public const string TEXT_ANCHOR_BOTTOM = "bottom";

        /**
         * The top left corner of the text is placed closest to the anchor.
         */
        public const string TEXT_ANCHOR_TOP_LEFT = "top-left";

        /**
         * The top right corner of the text is placed closest to the anchor.
         */
        public const string TEXT_ANCHOR_TOP_RIGHT = "top-right";

        /**
         * The bottom left corner of the text is placed closest to the anchor.
         */
        public const string TEXT_ANCHOR_BOTTOM_LEFT = "bottom-left";

        /**
         * The bottom right corner of the text is placed closest to the anchor.
         */
        public const string TEXT_ANCHOR_BOTTOM_RIGHT = "bottom-right";

        // TEXT_TRANSFORM: Specifies how to capitalize text, similar to the CSS `text-transform` property.

        /**
         * The text is not altered.
         */
        public const string TEXT_TRANSFORM_NONE = "none";

        /**
         * Forces all letters to be displayed in uppercase.
         */
        public const string TEXT_TRANSFORM_UPPERCASE = "uppercase";

        /**
         * Forces all letters to be displayed in lowercase.
         */
        public const string TEXT_TRANSFORM_LOWERCASE = "lowercase";

        // FILL_TRANSLATE_ANCHOR: Controls the frame of reference for `fill-translate`.

        /**
         * The fill is translated relative to the map.
         */
        public const string FILL_TRANSLATE_ANCHOR_MAP = "map";

        /**
         * The fill is translated relative to the viewport.
         */
        public const string FILL_TRANSLATE_ANCHOR_VIEWPORT = "viewport";

        // LINE_TRANSLATE_ANCHOR: Controls the frame of reference for `line-translate`.

        /**
         * The line is translated relative to the map.
         */
        public const string LINE_TRANSLATE_ANCHOR_MAP = "map";

        /**
         * The line is translated relative to the viewport.
         */
        public const string LINE_TRANSLATE_ANCHOR_VIEWPORT = "viewport";

        // ICON_TRANSLATE_ANCHOR: Controls the frame of reference for `icon-translate`.

        /**
         * Icons are translated relative to the map.
         */
        public const string ICON_TRANSLATE_ANCHOR_MAP = "map";

        /**
         * Icons are translated relative to the viewport.
         */
        public const string ICON_TRANSLATE_ANCHOR_VIEWPORT = "viewport";

        // TEXT_TRANSLATE_ANCHOR: Controls the frame of reference for `text-translate`.

        /**
         * The text is translated relative to the map.
         */
        public const string TEXT_TRANSLATE_ANCHOR_MAP = "map";

        /**
         * The text is translated relative to the viewport.
         */
        public const string TEXT_TRANSLATE_ANCHOR_VIEWPORT = "viewport";

        // CIRCLE_TRANSLATE_ANCHOR: Controls the frame of reference for `circle-translate`.

        /**
         * The circle is translated relative to the map.
         */
        public const string CIRCLE_TRANSLATE_ANCHOR_MAP = "map";

        /**
         * The circle is translated relative to the viewport.
         */
        public const string CIRCLE_TRANSLATE_ANCHOR_VIEWPORT = "viewport";

        // CIRCLE_PITCH_SCALE: Controls the scaling behavior of the circle when the map is pitched.

        /**
         * Circles are scaled according to their apparent distance to the camera.
         */
        public const string CIRCLE_PITCH_SCALE_MAP = "map";

        /**
         * Circles are not scaled.
         */
        public const string CIRCLE_PITCH_SCALE_VIEWPORT = "viewport";

        // CIRCLE_PITCH_ALIGNMENT: Orientation of circle when map is pitched.

        /**
         * The circle is aligned to the plane of the map.
         */
        public const string CIRCLE_PITCH_ALIGNMENT_MAP = "map";

        /**
         * The circle is aligned to the plane of the viewport.
         */
        public const string CIRCLE_PITCH_ALIGNMENT_VIEWPORT = "viewport";

        // FILL_EXTRUSION_TRANSLATE_ANCHOR: Controls the frame of reference for `fill-extrusion-translate`.

        /**
         * The fill extrusion is translated relative to the map.
         */
        public const string FILL_EXTRUSION_TRANSLATE_ANCHOR_MAP = "map";

        /**
         * The fill extrusion is translated relative to the viewport.
         */
        public const string FILL_EXTRUSION_TRANSLATE_ANCHOR_VIEWPORT = "viewport";

        // RASTER_RESAMPLING: The resampling/interpolation method to use for overscaling, also known as texture magnification filter

        /**
         * (Bi)linear filtering interpolates pixel values using the weighted average of the four closest original source pixels creating a smooth but blurry look when overscaled
         */
        public const string RASTER_RESAMPLING_LINEAR = "linear";

        /**
         * Nearest neighbor filtering interpolates pixel values using the nearest original source pixel creating a sharp but pixelated look when overscaled
         */
        public const string RASTER_RESAMPLING_NEAREST = "nearest";

        // HILLSHADE_ILLUMINATION_ANCHOR: Direction of light source when map is rotated.

        /**
         * The hillshade illumination is relative to the north direction.
         */
        public const string HILLSHADE_ILLUMINATION_ANCHOR_MAP = "map";

        /**
         * The hillshade illumination is relative to the top of the viewport.
         */
        public const string HILLSHADE_ILLUMINATION_ANCHOR_VIEWPORT = "viewport";

        // ANCHOR: Whether extruded geometries are lit relative to the map or viewport.

        /**
         * The position of the light source is aligned to the rotation of the map.
         */
        public const string ANCHOR_MAP = "map";

        /**
         * The position of the light source is aligned to the rotation of the viewport.
         */
        public const string ANCHOR_VIEWPORT = "viewport";

        // TEXT_WRITING_MODE: The property allows control over a symbol's orientation. Note that the property values act as a hint, so that a symbol whose language doesn’t support the provided orientation will be laid out in its natural orientation. Example: English point symbol will be rendered horizontally even if array value contains single 'vertical' enum value. The order of elements in an array define priority order for the placement of an orientation variant.

        /**
         * If a text's language supports horizontal writing mode, symbols with point placement would be laid out horizontally.
         */
        public const string TEXT_WRITING_MODE_HORIZONTAL = "horizontal";

        /**
         * If a text's language supports vertical writing mode, symbols with point placement would be laid out vertically.
         */
        public const string TEXT_WRITING_MODE_VERTICAL = "vertical";
    }
}