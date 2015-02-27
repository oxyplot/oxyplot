﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Series.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Abstract base class for series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Abstract base class for series.
    /// </summary>
    public abstract class Series : ItemsControl
    {
        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(Series), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(Series), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TrackerFormatString"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackerFormatStringProperty =
            DependencyProperty.Register(
                "TrackerFormatString",
                typeof(string),
                typeof(Series),
                new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TrackerKey"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackerKeyProperty = DependencyProperty.Register(
            "TrackerKey", typeof(string), typeof(Series), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Initializes static members of the <see cref="Series" /> class.
        /// </summary>
        static Series()
        {
            VisibilityProperty.OverrideMetadata(typeof(Series), new PropertyMetadata(Visibility.Visible, AppearanceChanged));
            BackgroundProperty.OverrideMetadata(typeof(Series), new FrameworkPropertyMetadata(null, AppearanceChanged));
        }

        /// <summary>
        /// Gets or sets Color.
        /// </summary>
        public Color Color
        {
            get
            {
                return (Color)this.GetValue(ColorProperty);
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
        /// <returns>A series.</returns>
        public abstract OxyPlot.Series.Series CreateModel();

        /// <summary>
        /// The appearance changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        protected static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Series)d).OnVisualChanged();
        }

        /// <summary>
        /// The data changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        protected static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Series)d).OnDataChanged();
        }

        /// <summary>
        /// The on data changed.
        /// </summary>
        protected void OnDataChanged()
        {
            var pc = this.Parent as IPlotView;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }

        /// <summary>
        /// The on items source changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            this.SubscribeToCollectionChanged(oldValue, newValue);
            this.OnDataChanged();
        }

        /// <summary>
        /// The on visual changed.
        /// </summary>
        protected void OnVisualChanged()
        {
            var pc = this.Parent as IPlotView;
            if (pc != null)
            {
                pc.InvalidatePlot(false);
            }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="s">The series.</param>
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

        /// <summary>
        /// If the ItemsSource implements INotifyCollectionChanged update the visual when the collection changes.
        /// </summary>
        /// <param name="oldValue">The old ItemsSource</param>
        /// <param name="newValue">The new ItemsSource</param>
        private void SubscribeToCollectionChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            var collection = oldValue as INotifyCollectionChanged;
            if (collection != null)
            {
                var oldCollection = collection;
                oldCollection.CollectionChanged -= this.OnCollectionChanged;
            }

            collection = newValue as INotifyCollectionChanged;
            if (collection != null)
            {
                var oldCollection = collection;
                oldCollection.CollectionChanged += this.OnCollectionChanged;
            }
        }

        /// <summary>
        /// Invalidate the view when the collection changes
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="notifyCollectionChangedEventArgs">The collection changed args</param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            this.OnDataChanged();
        }
    }
}