// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotBase.Properties.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;
using Avalonia.Input;
using Avalonia.Markup.Xaml.Templates;

namespace OxyPlot.Avalonia
{

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    public partial class PlotBase
    {
        /// <summary>
        /// Identifies the <see cref="DefaultTrackerTemplate"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<ControlTemplate> DefaultTrackerTemplateProperty = AvaloniaProperty.Register<PlotBase, ControlTemplate>(nameof(DefaultTrackerTemplate));

        /// <summary>
        /// Identifies the <see cref="IsMouseWheelEnabled"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> IsMouseWheelEnabledProperty = AvaloniaProperty.Register<PlotBase, bool>(nameof(IsMouseWheelEnabled), true);

        /// <summary>
        /// Identifies the <see cref="PanCursor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Cursor> PanCursorProperty = AvaloniaProperty.Register<PlotBase, Cursor>(nameof(PanCursor), new Cursor(StandardCursorType.Hand));

        /// <summary>
        /// Identifies the <see cref="ZoomHorizontalCursor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Cursor> ZoomHorizontalCursorProperty = AvaloniaProperty.Register<PlotBase, Cursor>(nameof(ZoomHorizontalCursor), new Cursor(StandardCursorType.SizeWestEast));

        /// <summary>
        /// Identifies the <see cref="ZoomRectangleCursor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Cursor> ZoomRectangleCursorProperty = AvaloniaProperty.Register<PlotBase, Cursor>(nameof(ZoomRectangleCursor), new Cursor(StandardCursorType.SizeAll));

        /// <summary>
        /// Identifies the <see cref="ZoomRectangleTemplate"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<ControlTemplate> ZoomRectangleTemplateProperty = AvaloniaProperty.Register<PlotBase, ControlTemplate>(nameof(ZoomRectangleTemplate));

        /// <summary>
        /// Identifies the <see cref="ZoomVerticalCursor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Cursor> ZoomVerticalCursorProperty = AvaloniaProperty.Register<PlotBase, Cursor>(nameof(ZoomVerticalCursor), new Cursor(StandardCursorType.SizeNorthSouth));

        static PlotBase()
        {
        }

        /// <summary>
        /// Gets or sets the default tracker template.
        /// </summary>
        public ControlTemplate DefaultTrackerTemplate
        {
            get
            {
                return GetValue(DefaultTrackerTemplateProperty);
            }

            set
            {
                SetValue(DefaultTrackerTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsMouseWheelEnabled.
        /// </summary>
        public bool IsMouseWheelEnabled
        {
            get
            {
                return GetValue(IsMouseWheelEnabledProperty);
            }

            set
            {
                SetValue(IsMouseWheelEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the pan cursor.
        /// </summary>
        /// <value>The pan cursor.</value>
        public global::Avalonia.Input.Cursor PanCursor
        {
            get
            {
                return GetValue(PanCursorProperty);
            }

            set
            {
                SetValue(PanCursorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the horizontal zoom cursor.
        /// </summary>
        /// <value>The zoom horizontal cursor.</value>
        public Cursor ZoomHorizontalCursor
        {
            get
            {
                return GetValue(ZoomHorizontalCursorProperty);
            }

            set
            {
                SetValue(ZoomHorizontalCursorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the rectangle zoom cursor.
        /// </summary>
        /// <value>The zoom rectangle cursor.</value>
        public Cursor ZoomRectangleCursor
        {
            get
            {
                return GetValue(ZoomRectangleCursorProperty);
            }

            set
            {
                SetValue(ZoomRectangleCursorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the zoom rectangle template.
        /// </summary>
        /// <value>The zoom rectangle template.</value>
        public ControlTemplate ZoomRectangleTemplate
        {
            get
            {
                return GetValue(ZoomRectangleTemplateProperty);
            }

            set
            {
                SetValue(ZoomRectangleTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the vertical zoom cursor.
        /// </summary>
        /// <value>The zoom vertical cursor.</value>
        public Cursor ZoomVerticalCursor
        {
            get
            {
                return GetValue(ZoomVerticalCursorProperty);
            }

            set
            {
                SetValue(ZoomVerticalCursorProperty, value);
            }
        }
    }
}
