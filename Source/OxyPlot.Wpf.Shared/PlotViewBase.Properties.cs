// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewBase.Properties.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using DependencyPropertyGenerator;
    using System.Windows.Controls;
    using System.Windows.Input;

    [DependencyProperty<IPlotController>("Controller",
        Description = "Gets or sets the Plot controller.")]
    [DependencyProperty<ControlTemplate>("DefaultTrackerTemplate",
        Description = "Gets or sets the default tracker template.")]
    [DependencyProperty<bool>("IsMouseWheelEnabled", DefaultValue = true,
        Description = "Gets or sets a value indicating whether IsMouseWheelEnabled.")]
    [DependencyProperty<PlotModel>("Model",
        Description = "Gets or sets the model.")]
    [DependencyProperty<Cursor>("PanCursor", DefaultValueExpression = "global::System.Windows.Input.Cursors.Hand",
        Description = "Gets or sets the pan cursor.")]
    [DependencyProperty<Cursor>("ZoomHorizontalCursor", DefaultValueExpression = "global::System.Windows.Input.Cursors.SizeWE",
        Description = "Gets or sets the horizontal zoom cursor.")]
    [DependencyProperty<Cursor>("ZoomRectangleCursor", DefaultValueExpression = "global::System.Windows.Input.Cursors.SizeNWSE",
        Description = "Gets or sets the rectangle zoom cursor.")]
    [DependencyProperty<ControlTemplate>("ZoomRectangleTemplate",
        Description = "Gets or sets the zoom rectangle template.")]
    [DependencyProperty<Cursor>("ZoomVerticalCursor", DefaultValueExpression = "global::System.Windows.Input.Cursors.SizeNS",
        Description = "Gets or sets the vertical zoom cursor.")]
    public abstract partial class PlotViewBase
    {
    }
}
