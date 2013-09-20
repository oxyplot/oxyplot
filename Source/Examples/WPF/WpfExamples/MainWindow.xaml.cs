// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
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
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WpfExamples
{
    using System.Collections.Generic;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Examples = new List<Example>
                {
                    new Example(typeof(AnnotationDemo.MainWindow), null, "Shows different types of annotations."),
                    new Example(typeof(AreaDemo.MainWindow), null, "Plotting with AreaSeries."),
                    new Example(typeof(AxesDemo.MainWindow), null, "Demonstrates functionality of the axes."),
                    new Example(typeof(BarSeriesDemo.MainWindow), null, "Shows bar series."),
                    new Example(typeof(BindingDemo.MainWindow), null, "Demonstrates data binding."),
                    new Example(typeof(ColorBindingDemo.MainWindow), null, "Binds fore and background colors."),
                    new Example(typeof(ColumnSeriesDemo.MainWindow), null, "Shows column series."),
                    new Example(typeof(ContourDemo.MainWindow), null, "Plotting with contour series."),
                    new Example(typeof(CoupledAxesDemo.MainWindow), null, "Shows how to keep two axes in sync."),
                    new Example(typeof(CsvDemo.MainWindow), null, "Plotting data from CSV files."),
                    new Example(typeof(CustomTrackerDemo.MainWindow), null, "Demonstrates a custom tracker."),
                    new Example(typeof(DateTimeDemo.MainWindow), null, "Plotting with DateTime axes."),
                    new Example(typeof(HistogramDemo.MainWindow), null, "Plots a histogram."),
                    new Example(typeof(LegendsDemo.MainWindow), null, "Demonstrates legend box capabilities."),
                    new Example(typeof(PieDemo.MainWindow), null, "Shows a pie chart."),
                    new Example(typeof(PolarDemo.MainWindow), null, "Creates a polar plot."),
                    new Example(typeof(RealtimeDemo.MainWindow), null, "Plotting a curve that updates automatically."),
                    new Example(typeof(RefreshDemo.MainWindow), null, "Demonstrates invalidating/refreshing the plot."),
                    new Example(typeof(ScatterDemo.MainWindow), null, "Plotting a barnsley fern with a scatter series."),
                    new Example(typeof(TaskDemo.MainWindow), null, "Updating a LineSeries from a Task running on the UI thread synchronization context."),
                    new Example(typeof(UserControlDemo.MainWindow), null, "Demonstrates a Plot in a UserControl."),
                    new Example(typeof(UserControlDemo.MainWindow2), null, "Demonstrates a Plot in a UserControl in a DataTemplate."),
                    new Example(typeof(UserControlDemo.MainWindow3), null, "Demonstrates a Plot in a DataTemplate.")
                };
        }

        /// <summary>
        /// Gets the examples.
        /// </summary>
        /// <value>The examples.</value>
        public IList<Example> Examples { get; private set; }

        /// <summary>
        /// Handles the MouseDoubleClick event of the ListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void ListBoxMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var lb = (ListBox)sender;
            var example = lb.SelectedItem as Example;
            if (example != null)
            {
                var window = example.Create();
                window.Show();
            }
        }
    }
}