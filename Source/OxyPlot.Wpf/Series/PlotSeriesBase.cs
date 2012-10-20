// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotSeriesBase.cs" company="OxyPlot">
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
//   Abstract base class for series that use X and Y axes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Abstract base class for series that use X and Y axes.
    /// </summary>
    public abstract class PlotSeriesBase : ItemsControl, ISeries
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color?), typeof(PlotSeriesBase), new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(PlotSeriesBase), new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty TrackerFormatStringProperty =
            DependencyProperty.Register(
                "TrackerFormatString",
                typeof(string),
                typeof(PlotSeriesBase),
                new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty TrackerKeyProperty = DependencyProperty.Register(
            "TrackerKey", typeof(string), typeof(PlotSeriesBase), new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty XAxisKeyProperty = DependencyProperty.Register(
            "XAxisKey", typeof(string), typeof(PlotSeriesBase), new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty YAxisKeyProperty = DependencyProperty.Register(
            "YAxisKey", typeof(string), typeof(PlotSeriesBase), new UIPropertyMetadata(null, VisualChanged));

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

        public string XAxisKey
        {
            get
            {
                return (string)this.GetValue(XAxisKeyProperty);
            }
            set
            {
                this.SetValue(XAxisKeyProperty, value);
            }
        }

        public string YAxisKey
        {
            get
            {
                return (string)this.GetValue(YAxisKeyProperty);
            }
            set
            {
                this.SetValue(YAxisKeyProperty, value);
            }
        }

        public abstract OxyPlot.ISeries CreateModel();

        protected virtual void SynchronizeProperties(OxyPlot.ISeries series)
        {
            var s = series as OxyPlot.PlotSeriesBase;
            s.ItemsSource = this.ItemsSource;
            s.Background = this.Background.ToOxyColor();
            s.Title = this.Title;
            s.TrackerFormatString = this.TrackerFormatString;
            s.TrackerKey = this.TrackerKey;
            s.TrackerFormatString = this.TrackerFormatString;
            s.XAxisKey = this.XAxisKey;
            s.YAxisKey = this.YAxisKey;
        }

        protected static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotSeriesBase)d).OnDataChanged();
        }

        protected static void VisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotSeriesBase)d).OnVisualChanged();
        }

        protected void OnDataChanged()
        {
            var pc = this.Parent as IPlotControl;
            if (pc != null)
            {
                // pc.UpdateModel();
            }
            this.OnVisualChanged();
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            this.OnDataChanged();
        }

        protected void OnVisualChanged()
        {
            var pc = this.Parent as IPlotControl;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }

    }
}