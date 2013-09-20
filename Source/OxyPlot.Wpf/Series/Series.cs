// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Series.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Abstract base class for series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Abstract base class for series.
    /// </summary>
    public abstract class Series : ItemsControl
    {
        /// <summary>
        /// The color property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color?), typeof(Series), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The title property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(Series), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The tracker format string property.
        /// </summary>
        public static readonly DependencyProperty TrackerFormatStringProperty =
            DependencyProperty.Register(
                "TrackerFormatString", 
                typeof(string), 
                typeof(Series), 
                new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The tracker key property.
        /// </summary>
        public static readonly DependencyProperty TrackerKeyProperty = DependencyProperty.Register(
            "TrackerKey", typeof(string), typeof(Series), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Initializes static members of the <see cref="Series"/> class.
        /// </summary>
        static Series()
        {
            VisibilityProperty.OverrideMetadata(typeof(Series), new PropertyMetadata(Visibility.Visible, AppearanceChanged));
        }

        /// <summary>
        /// Gets or sets Color.
        /// </summary>
        public Color? Color
        {
            get
            {
                return (Color?)this.GetValue(ColorProperty);
            }

            set
            {
                this.SetValue(ColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the internal series.
        /// </summary>
        public OxyPlot.Series.Series InternalSeries { get; protected set; }

        /// <summary>
        /// Gets or sets Title.
        /// </summary>
        public string Title
        {
            get
            {
                return (string)this.GetValue(TitleProperty);
            }

            set
            {
                this.SetValue(TitleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TrackerFormatString.
        /// </summary>
        public string TrackerFormatString
        {
            get
            {
                return (string)this.GetValue(TrackerFormatStringProperty);
            }

            set
            {
                this.SetValue(TrackerFormatStringProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TrackerKey.
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
        /// Creates the model.
        /// </summary>
        /// <returns>
        /// A series.
        /// </returns>
        public abstract OxyPlot.Series.Series CreateModel();

        /// <summary>
        /// The appearance changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Series)d).OnVisualChanged();
        }

        /// <summary>
        /// The data changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Series)d).OnDataChanged();
        }

        /// <summary>
        /// The on data changed.
        /// </summary>
        protected void OnDataChanged()
        {
            var pc = this.Parent as IPlotControl;
            if (pc != null)
            {
                // pc.UpdateModel();
            }

            this.OnVisualChanged();
        }

        /// <summary>
        /// The on items source changed.
        /// </summary>
        /// <param name="oldValue">
        /// The old value.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            this.OnDataChanged();
        }

        /// <summary>
        /// The on visual changed.
        /// </summary>
        protected void OnVisualChanged()
        {
            var pc = this.Parent as IPlotControl;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="s">
        /// The series.
        /// </param>
        protected virtual void SynchronizeProperties(OxyPlot.Series.Series s)
        {
            s.Background = this.Background.ToOxyColor();
            s.Title = this.Title;
            s.TrackerFormatString = this.TrackerFormatString;
            s.TrackerKey = this.TrackerKey;
            s.TrackerFormatString = this.TrackerFormatString;
            s.IsVisible = this.Visibility == Visibility.Visible;
            s.Font = this.FontFamily.ToString();
            s.TextColor = this.Foreground.ToOxyColor();
        }
    }
}