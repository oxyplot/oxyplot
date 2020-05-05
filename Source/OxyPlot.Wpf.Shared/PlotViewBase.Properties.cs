// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewBase.Properties.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Base class for WPF PlotView implementations.
    /// </summary>
    public abstract partial class PlotViewBase
    {
        /// <summary>
        /// Identifies the <see cref="Controller"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register(nameof(Controller), typeof(IPlotController), typeof(PlotViewBase));

        /// <summary>
        /// Identifies the <see cref="DefaultTrackerTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultTrackerTemplateProperty =
            DependencyProperty.Register(
                nameof(DefaultTrackerTemplate), typeof(ControlTemplate), typeof(PlotViewBase), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="IsMouseWheelEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsMouseWheelEnabledProperty =
            DependencyProperty.Register(nameof(IsMouseWheelEnabled), typeof(bool), typeof(PlotViewBase), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="Model"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register(nameof(Model), typeof(PlotModel), typeof(PlotViewBase), new PropertyMetadata(null, ModelChanged));

        /// <summary>
        /// Identifies the <see cref="PanCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PanCursorProperty = DependencyProperty.Register(
            nameof(PanCursor), typeof(Cursor), typeof(PlotViewBase), new PropertyMetadata(Cursors.Hand));

        /// <summary>
        /// Identifies the <see cref="ZoomHorizontalCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomHorizontalCursorProperty =
            DependencyProperty.Register(
                nameof(ZoomHorizontalCursor), typeof(Cursor), typeof(PlotViewBase), new PropertyMetadata(Cursors.SizeWE));

        /// <summary>
        /// Identifies the <see cref="ZoomRectangleCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomRectangleCursorProperty =
            DependencyProperty.Register(
                nameof(ZoomRectangleCursor), typeof(Cursor), typeof(PlotViewBase), new PropertyMetadata(Cursors.SizeNWSE));

        /// <summary>
        /// Identifies the <see cref="ZoomRectangleTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomRectangleTemplateProperty =
            DependencyProperty.Register(
                nameof(ZoomRectangleTemplate), typeof(ControlTemplate), typeof(PlotViewBase), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ZoomVerticalCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomVerticalCursorProperty =
            DependencyProperty.Register(
                nameof(ZoomVerticalCursor), typeof(Cursor), typeof(PlotViewBase), new PropertyMetadata(Cursors.SizeNS));

        /// <summary>
        /// Gets or sets the Plot controller.
        /// </summary>
        /// <value>The Plot controller.</value>
        public IPlotController Controller
        {
            get => (IPlotController)this.GetValue(ControllerProperty);
            set => this.SetValue(ControllerProperty, value);
        }

        /// <summary>
        /// Gets or sets the default tracker template.
        /// </summary>
        public ControlTemplate DefaultTrackerTemplate
        {
            get => (ControlTemplate)this.GetValue(DefaultTrackerTemplateProperty);
            set => this.SetValue(DefaultTrackerTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsMouseWheelEnabled.
        /// </summary>
        public bool IsMouseWheelEnabled
        {
            get => (bool)this.GetValue(IsMouseWheelEnabledProperty);
            set => this.SetValue(IsMouseWheelEnabledProperty, value);
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public PlotModel Model
        {
            get => (PlotModel)this.GetValue(ModelProperty);
            set => this.SetValue(ModelProperty, value);
        }

        /// <summary>
        /// Gets or sets the pan cursor.
        /// </summary>
        /// <value>The pan cursor.</value>
        public Cursor PanCursor
        {
            get => (Cursor)this.GetValue(PanCursorProperty);
            set => this.SetValue(PanCursorProperty, value);
        }

        /// <summary>
        /// Gets or sets the horizontal zoom cursor.
        /// </summary>
        /// <value>The zoom horizontal cursor.</value>
        public Cursor ZoomHorizontalCursor
        {
            get => (Cursor)this.GetValue(ZoomHorizontalCursorProperty);
            set => this.SetValue(ZoomHorizontalCursorProperty, value);
        }

        /// <summary>
        /// Gets or sets the rectangle zoom cursor.
        /// </summary>
        /// <value>The zoom rectangle cursor.</value>
        public Cursor ZoomRectangleCursor
        {
            get => (Cursor)this.GetValue(ZoomRectangleCursorProperty);
            set => this.SetValue(ZoomRectangleCursorProperty, value);
        }

        /// <summary>
        /// Gets or sets the zoom rectangle template.
        /// </summary>
        /// <value>The zoom rectangle template.</value>
        public ControlTemplate ZoomRectangleTemplate
        {
            get => (ControlTemplate)this.GetValue(ZoomRectangleTemplateProperty);
            set => this.SetValue(ZoomRectangleTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the vertical zoom cursor.
        /// </summary>
        /// <value>The zoom vertical cursor.</value>
        public Cursor ZoomVerticalCursor
        {
            get => (Cursor)this.GetValue(ZoomVerticalCursorProperty);
            set => this.SetValue(ZoomVerticalCursorProperty, value);
        }
    }
}
