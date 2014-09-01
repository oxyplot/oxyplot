// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window2.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for Window2.xaml
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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public IList<PlotModel> Plots { get; set; }

        public Window2()
        {
            InitializeComponent();
            Plots = new List<PlotModel>();
            for (int i = 0; i < 10; i++)
            {
                var p = new PlotModel { Title = "Plot " + i };
                p.Series.Add(new FunctionSeries(x => Math.Cos(x * i), 0, 10, 0.01));
                Plots.Add(p);
            }
            DataContext = this;
        }
    }
}