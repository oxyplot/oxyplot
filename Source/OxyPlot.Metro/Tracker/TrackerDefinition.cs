// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerDefinition.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Metro
{

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// The tracker definition.
    /// </summary>
    public class TrackerDefinition : DependencyObject
    {
        #region Constants and Fields

        /// <summary>
        /// The tracker key property.
        /// </summary>
        public static readonly DependencyProperty TrackerKeyProperty = DependencyProperty2.Register(
            "TrackerKey", typeof(string), typeof(TrackerDefinition), new PropertyMetadata(null));

        /// <summary>
        /// The tracker template property.
        /// </summary>
        public static readonly DependencyProperty TrackerTemplateProperty =
            DependencyProperty2.Register(
                "TrackerTemplate", typeof(ControlTemplate), typeof(TrackerDefinition), new PropertyMetadata(null));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the tracker key.
        /// The Plot will use this property to find the TrackerDefinition that matches the TrackerKey of the current series.
        /// </summary>
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
        /// Gets or sets the tracker template.
        /// The tracker control will be added/removed from the Tracker overlay as neccessary.
        /// The DataContext of the tracker will be set to a TrackerHitResult with the current tracker data.
        /// </summary>
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