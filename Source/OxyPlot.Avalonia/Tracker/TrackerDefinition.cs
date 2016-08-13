// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerDefinition.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a tracker definition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;
using Avalonia.Markup.Xaml.Templates;

namespace OxyPlot.Avalonia
{

    /// <summary>
    /// Represents a tracker definition.
    /// </summary>
    /// <remarks>The tracker definitions make it possible to show different trackers for different series.
    /// The <see cref="OxyPlot.Series.Series.TrackerKey" /> property is matched with the <see cref="TrackerDefinition.TrackerKey" />
    /// in the TrackerDefinitions collection in the <see cref="PlotView" /> control.</remarks>
    public class TrackerDefinition : AvaloniaObject
    {
        /// <summary>
        /// Identifies the <see cref="TrackerKey"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> TrackerKeyProperty = AvaloniaProperty.Register<TrackerDefinition, string>(nameof(TrackerKey));

        /// <summary>
        /// Identifies the <see cref="TrackerTemplate"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<ControlTemplate> TrackerTemplateProperty = AvaloniaProperty.Register<TrackerDefinition, ControlTemplate>(nameof(TrackerTemplate));

        /// <summary>
        /// Gets or sets the tracker key.
        /// </summary>
        /// <remarks>The Plot will use this property to find the TrackerDefinition that matches the TrackerKey of the current series.</remarks>
        public string TrackerKey
        {
            get
            {
                return GetValue(TrackerKeyProperty);
            }

            set
            {
                SetValue(TrackerKeyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the tracker template.
        /// </summary>
        /// <remarks>The tracker control will be added/removed from the Tracker overlay as necessary.
        /// The DataContext of the tracker will be set to a TrackerHitResult with the current tracker data.</remarks>
        public ControlTemplate TrackerTemplate
        {
            get
            {
                return GetValue(TrackerTemplateProperty);
            }

            set
            {
                SetValue(TrackerTemplateProperty, value);
            }
        }
    }
}