// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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

namespace OverlayDemo
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using OxyPlot;
    using OxyPlot.Axes;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("WPF overlays.")]
    public partial class MainWindow : Window
    {
        private LinearAxis horizontalAxis;

        private LinearAxis verticalAxis;

        private PlotModel model;
        
        private Rectangle rect;

        public MainWindow()
        {
            this.InitializeComponent();
            this.model = new PlotModel();
            this.horizontalAxis = new LinearAxis { Position = AxisPosition.Bottom };
            this.verticalAxis = new LinearAxis { Position = AxisPosition.Left };
            this.model.Axes.Add(this.horizontalAxis);
            this.model.Axes.Add(this.verticalAxis);
            plot1.Model = this.model;
            this.DataContext = this;

            // Subscribe to transform changes on both axes
            this.horizontalAxis.TransformChanged += this.HandleTransformChanged;
            this.verticalAxis.TransformChanged += this.HandleTransformChanged;

            this.rect = new Rectangle { Fill = new RadialGradientBrush(Colors.Black, Colors.White) };
            canvas1.Children.Add(this.rect);
        }

        void HandleTransformChanged(object sender, EventArgs e)
        {
            // Transform to screen coordinates
            var p1 = this.horizontalAxis.Transform(0, 0, this.verticalAxis);
            var p2 = this.horizontalAxis.Transform(100, 100, this.verticalAxis);

            // Change the position and size of the WPF shape
            this.rect.Width = Math.Abs(p2.X - p1.X);
            this.rect.Height = Math.Abs(p2.Y - p1.Y);
            Canvas.SetLeft(this.rect, Math.Min(p1.X, p2.X));
            Canvas.SetTop(this.rect, Math.Min(p1.Y, p2.Y));

            // Update the clipping rectangle for the canvas
            var plotArea = new Rect(this.model.PlotArea.Left, this.model.PlotArea.Top, this.model.PlotArea.Width, this.model.PlotArea.Height);
            canvas1.Clip = new RectangleGeometry(plotArea);
        }
    }
}