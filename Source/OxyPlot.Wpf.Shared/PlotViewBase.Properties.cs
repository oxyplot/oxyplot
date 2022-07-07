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

    [DependencyProperty<IPlotController>("Controller")]
    [DependencyProperty<ControlTemplate>("DefaultTrackerTemplate")]
    [DependencyProperty<bool>("IsMouseWheelEnabled", DefaultValue = true)]
    [DependencyProperty<PlotModel>("Model")]
    [DependencyProperty<Cursor>("PanCursor", DefaultValueExpression = "global::System.Windows.Input.Cursors.Hand")]
    [DependencyProperty<Cursor>("ZoomHorizontalCursor", DefaultValueExpression = "global::System.Windows.Input.Cursors.SizeWE")]
    [DependencyProperty<Cursor>("ZoomRectangleCursor", DefaultValueExpression = "global::System.Windows.Input.Cursors.SizeNWSE")]
    [DependencyProperty<ControlTemplate>("ZoomRectangleTemplate")]
    [DependencyProperty<Cursor>("ZoomVerticalCursor", DefaultValueExpression = "global::System.Windows.Input.Cursors.SizeNS")]
    public abstract partial class PlotViewBase
    {
    }
}
