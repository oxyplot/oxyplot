// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Series.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Abstract base class for series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Controls;
    using global::Avalonia.Media;
    using global::Avalonia.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Specialized;

    /// <summary>
    /// Abstract base class for series.
    /// </summary>
    public abstract class Series : ItemsControl
    {
        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> ColorProperty = AvaloniaProperty.Register<Series, Color>(nameof(Color), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<Series, string>(nameof(Title), null);

         /// <summary>
        /// Identifies the <see cref="RenderInLegend"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> RenderInLegendProperty = AvaloniaProperty.Register<Series, bool>(nameof(RenderInLegend), true);

        /// <summary>
        /// Identifies the <see cref="TrackerFormatString"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> TrackerFormatStringProperty = AvaloniaProperty.Register<Series, string>(nameof(TrackerFormatString), null);

        /// <summary>
        /// Identifies the <see cref="TrackerKey"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> TrackerKeyProperty = AvaloniaProperty.Register<Series, string>(nameof(TrackerKey), null);

        /// <summary>
        /// The event listener used to subscribe to ItemSource.CollectionChanged events
        /// </summary>
        private readonly EventListener eventListener;

        /// <summary>
        /// Initializes static members of the <see cref="Series" /> class.
        /// </summary>
        static Series()
        {
            IsVisibleProperty.Changed.AddClassHandler<Series>(AppearanceChanged);
            BackgroundProperty.Changed.AddClassHandler<Series>(AppearanceChanged);
            ColorProperty.Changed.AddClassHandler<Series>(AppearanceChanged);
            TitleProperty.Changed.AddClassHandler<Series>(AppearanceChanged);
            RenderInLegendProperty.Changed.AddClassHandler<Series>(AppearanceChanged);
            TrackerFormatStringProperty.Changed.AddClassHandler<Series>(AppearanceChanged);
            TrackerKeyProperty.Changed.AddClassHandler<Series>(AppearanceChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Series" /> class.
        /// </summary>
        protected Series()
        {
            eventListener = new EventListener(OnCollectionChanged);
        }

        /// <summary>
        /// Gets or sets Color.
        /// </summary>
        public Color Color
        {
            get
            {
                return GetValue(ColorProperty);
            }

            set
            {
                SetValue(ColorProperty, value);
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
                return GetValue(TitleProperty);
            }

            set
            {
                SetValue(TitleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the series should be rendered in the legend.
        /// </summary>
        public bool RenderInLegend 
        {
            get
            {
                return GetValue(RenderInLegendProperty);
            }

            set
            {
                SetValue(RenderInLegendProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TrackerFormatString.
        /// </summary>
        public string TrackerFormatString
        {
            get
            {
                return GetValue(TrackerFormatStringProperty);
            }

            set
            {
                SetValue(TrackerFormatStringProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TrackerKey.
        /// </summary>
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
        /// Creates the model.
        /// </summary>
        /// <returns>A series.</returns>
        public abstract OxyPlot.Series.Series CreateModel();

        /// <summary>
        /// The appearance changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        protected static void AppearanceChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ((Series)d).OnVisualChanged();
        }

        /// <summary>
        /// The data changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        protected static void DataChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ((Series)d).OnDataChanged();
        }

        /// <summary>
        /// The on data changed.
        /// </summary>
        protected void OnDataChanged()
        {
            var pc = Parent as IPlotView;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }

        /// <summary>
        /// The on items source changed.
        /// </summary>
        /// <param name="e">Event args</param>
        /// 
        protected override void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.ItemsChanged(e);
            SubscribeToCollectionChanged(e.OldValue as IEnumerable, e.NewValue as IEnumerable);
            OnDataChanged();
        }

        /// <summary>
        /// The on visual changed.
        /// </summary>
        protected void OnVisualChanged()
        {
            var pc = Parent as IPlotView;
            if (pc != null)
            {
                pc.InvalidatePlot(false);
            }
        }

        protected override void OnAttachedToLogicalTree(global::Avalonia.LogicalTree.LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);
            //BeginInit();
            //EndInit();
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="s">The series.</param>
        protected virtual void SynchronizeProperties(OxyPlot.Series.Series s)
        {
            s.Background = Background.ToOxyColor();
            s.Title = Title;
            s.RenderInLegend = RenderInLegend;
            s.TrackerFormatString = TrackerFormatString;
            s.TrackerKey = TrackerKey;
            s.TrackerFormatString = TrackerFormatString;
            s.IsVisible = IsVisible;
            s.Font = FontFamily.ToString();
            s.TextColor = Foreground.ToOxyColor();
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
                WeakSubscriptionManager.Unsubscribe(collection, "CollectionChanged", eventListener);
            }

            collection = newValue as INotifyCollectionChanged;
            if (collection != null)
            {
                WeakSubscriptionManager.Subscribe(collection, "CollectionChanged", eventListener);
            }
        }

        /// <summary>
        /// Invalidate the view when the collection changes
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="notifyCollectionChangedEventArgs">The collection changed args</param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            OnDataChanged();
        }

        /// <summary>
        /// Listens to and forwards any collection changed events
        /// </summary>
        private class EventListener : IWeakSubscriber<NotifyCollectionChangedEventArgs>
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

            public void OnEvent(object sender, NotifyCollectionChangedEventArgs e)
            {
                onCollectionChanged(sender, e);
            }
        }
    }
}