// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputBindingX.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Represents an input binding that supports binding to the InputGesture.
    /// </summary>
    public class InputBindingX : InputBinding
    {
        #region Constants and Fields

        /// <summary>
        ///   The gesture property.
        /// </summary>
        public static readonly DependencyProperty GeztureProperty = DependencyProperty.Register(
            "Gezture", typeof(InputGesture), typeof(InputBindingX), new UIPropertyMetadata(null, GeztureChanged));

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the gesture.
        /// </summary>
        /// <value>The gezture.</value>
        public InputGesture Gezture
        {
            get
            {
                return (InputGesture)this.GetValue(GeztureProperty);
            }

            set
            {
                this.SetValue(GeztureProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when the gesture property changed.
        /// </summary>
        protected virtual void OnGeztureChanged()
        {
            this.Gesture = this.Gezture;
        }

        /// <summary>
        /// Called when the gesture property changed.
        /// </summary>
        /// <param name="d">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private static void GeztureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((InputBindingX)d).OnGeztureChanged();
        }

        #endregion
    }
}