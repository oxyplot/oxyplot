// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Series.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Abstract base class for series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System;
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
        /// Identifies the <see cref="RenderInLegend"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RenderInLegendProperty = DependencyProperty.Register(
            "RenderInLegend", typeof(bool), typeof(Series), new PropertyMetadata(true, AppearanceChanged));

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
        /// The event listener used to subscribe to ItemSource.CollectionChanged events
        /// </summary>
        private readonly EventListener eventListener;

        /// <summary>
        /// Initializes static members of the <see cref="Series" /> class.
        /// </summary>
        static Series()
        {
            VisibilityProperty.OverrideMetadata(typeof(Series), new PropertyMetadata(Visibility.Visible, DataChanged));
            BackgroundProperty.OverrideMetadata(typeof(Series), new FrameworkPropertyMetadata(null, AppearanceChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Series" /> class.
        /// </summary>
        protected Series()
        {
            this.eventListener = new EventListener(this.OnCollectionChanged);
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
        /// Gets or sets a value indicating whether the series should be rendered in the legend.
        /// </summary>
        public bool RenderInLegend 
        {
            get
            {
                return (bool)this.GetValue(RenderInLegendProperty);
            }

            set
            {
                this.SetValue(RenderInLegendProperty, value);
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
            s.RenderInLegend = this.RenderInLegend;
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
                CollectionChangedEventManager.RemoveListener(collection, this.eventListener);
            }

            collection = newValue as INotifyCollectionChanged;
            if (collection != null)
            {
                CollectionChangedEventManager.AddListener(collection, this.eventListener);
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

        /// <summary>
        /// Listens to and forwards any collection changed events
        /// </summary>
        private class EventListener : IWeakEventListener
        {
            /// <summary>
            /// The delegate to forward to
            /// </summary>
            private readonly EventHandler<NotifyCollectionChangedEventArgs> onCollectionChanged;

            /// <summary>
            /// Initializes a new instance of the <see cref="EventListener" /> class
            /// </summary>
            /// <param name="onCollectionChanged">The handler</param>
            public EventListener(EventHandler<NotifyCollectionChangedEventArgs> onCollectionChanged)
            {
                this.onCollectionChanged = onCollectionChanged;
            }

            /// <summary>
            /// Receive a weak event
            /// </summary>
            /// <param name="managerType">The manager type</param>
            /// <param name="sender">The sender</param>
            /// <param name="e">The arguments</param>
            /// <returns>Whether the event was handled or not</returns>
            public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
            {
                if (managerType == typeof(CollectionChangedEventManager))
                {
                    this.onCollectionChanged(sender, (NotifyCollectionChangedEventArgs)e);
                    return true;
                }

                return false;
            }
        }
    }
}
