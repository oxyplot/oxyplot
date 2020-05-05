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
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a tracker definition.
    /// </summary>
    /// <remarks>The tracker definitions make it possible to show different trackers for different series.
    /// The <see cref="OxyPlot.Series.Series.TrackerKey" /> property is matched with the <see cref="TrackerDefinition.TrackerKey" />
    /// in the TrackerDefinitions collection in the <see cref="PlotView" /> control.</remarks>
    public class TrackerDefinition : DependencyObject
    {
        /// <summary>
        /// Identifies the <see cref="TrackerKey"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackerKeyProperty = DependencyProperty.Register(
            nameof(TrackerKey), typeof(string), typeof(TrackerDefinition), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="TrackerTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackerTemplateProperty =
            DependencyProperty.Register(
                nameof(TrackerTemplate), typeof(ControlTemplate), typeof(TrackerDefinition), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the tracker key.
        /// </summary>
        /// <remarks>The Plot will use this property to find the TrackerDefinition that matches the TrackerKey of the current series.</remarks>
        public string TrackerKey
        {
            get => (string)this.GetValue(TrackerKeyProperty);
            set => this.SetValue(TrackerKeyProperty, value);
        }

        /// <summary>
        /// Gets or sets the tracker template.
        /// </summary>
        /// <remarks>The tracker control will be added/removed from the Tracker overlay as necessary.
        /// The DataContext of the tracker will be set to a TrackerHitResult with the current tracker data.</remarks>
        public ControlTemplate TrackerTemplate
        {
            get => (ControlTemplate)this.GetValue(TrackerTemplateProperty);
            set => this.SetValue(TrackerTemplateProperty, value);
        }
    }
}
