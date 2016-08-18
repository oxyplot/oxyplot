// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window1.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for Window1.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MemoryTest
{
    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public PlotModel Model { get; set; }

        public Window1()
        {
            InitializeComponent();
            DataContext = this;
            Model = new PlotModel { Title = "Test 1" };
            Model.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.01));
        }
    }
}