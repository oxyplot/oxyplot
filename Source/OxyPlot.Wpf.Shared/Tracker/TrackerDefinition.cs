// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerDefinition.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a tracker definition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using DependencyPropertyGenerator;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a tracker definition.
    /// </summary>
    /// <remarks>The tracker definitions make it possible to show different trackers for different series.
    /// The <see cref="OxyPlot.Series.Series.TrackerKey" /> property is matched with the <see cref="TrackerDefinition.TrackerKey" />
    /// in the TrackerDefinitions collection in the <see cref="PlotViewBase" /> control.</remarks>
    [DependencyProperty<string>("TrackerKey",
        Description = "Gets or sets the tracker key. The Plot will use this property to find the TrackerDefinition that matches the TrackerKey of the current series.")]
    [DependencyProperty<ControlTemplate>("TrackerTemplate",
        Description = "Gets or sets the tracker template. The tracker control will be added/removed from the Tracker overlay as necessary. The DataContext of the tracker will be set to a TrackerHitResult with the current tracker data.")]
    public partial class TrackerDefinition : DependencyObject
    {
    }
}
