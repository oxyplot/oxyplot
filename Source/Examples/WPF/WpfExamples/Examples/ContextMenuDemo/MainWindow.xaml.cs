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

namespace ContextMenuDemo
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using OxyPlot;
    using OxyPlot.Series;

    using WpfExamples;

    using DelegateCommand = PropertyTools.Wpf.DelegateCommand;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Integrate with a ContextMenu")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            var model = new PlotModel { Title = "ContextMenu" };
            model.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 200));
            this.Model = model;

            this.ResetCommand = new DelegateCommand(() =>
            {
                this.Model.ResetAllAxes();
                this.Model.InvalidatePlot(false);
            });

            this.DataContext = this;
        }

        public ICommand ResetCommand { get; set; }

        public PlotModel Model { get; set; }
    }
}