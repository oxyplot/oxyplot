// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerDefinition.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if WPF

namespace OxyPlot.Wpf
#endif
#if SILVERLIGHT
namespace OxyPlot.Silverlight
#endif
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The tracker definition.
    /// </summary>
    public class TrackerDefinition : DependencyObject
    {
        #region Constants and Fields

        /// <summary>
        ///   The tracker key property.
        /// </summary>
        public static readonly DependencyProperty TrackerKeyProperty = DependencyProperty.Register(
            "TrackerKey", typeof(string), typeof(TrackerDefinition), new PropertyMetadata(null));

        /// <summary>
        ///   The tracker template property.
        /// </summary>
        public static readonly DependencyProperty TrackerTemplateProperty =
            DependencyProperty.Register(
                "TrackerTemplate", typeof(ControlTemplate), typeof(TrackerDefinition), new PropertyMetadata(null));

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the tracker key.
        /// </summary>
        /// <remarks>
        ///   The Plot will use this property to find the TrackerDefinition that matches the TrackerKey of the current series.
        /// </remarks>
        public string TrackerKey
        {
            get
            {
                return (string)this.GetValue(TrackerKeyProperty);
            }

            set
            {
                this.SetValue(TrackerKeyProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the tracker template.
        /// </summary>
        /// <remarks>
        ///   The tracker control will be added/removed from the Tracker overlay as neccessary.
        ///   The DataContext of the tracker will be set to a TrackerHitResult with the current tracker data.
        /// </remarks>
        public ControlTemplate TrackerTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(TrackerTemplateProperty);
            }

            set
            {
                this.SetValue(TrackerTemplateProperty, value);
            }
        }

        #endregion
    }
}