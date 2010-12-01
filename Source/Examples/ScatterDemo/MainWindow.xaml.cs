﻿using System;
using System.Windows;
using OxyPlot;

namespace ScatterDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel ScatterModel { get; set; }
        
        // todo: performance issues here

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            var tmp = new PlotModel("Scatter plot");
            var s1 = new LineSeries { StrokeThickness = 0, MarkerSize = 3, MarkerFill = OxyColors.Blue, MarkerType = MarkerType.Circle };
            var r = new Random();
            for (int i = 0; i < 1000; i++)
                s1.Points.Add(new DataPoint(r.NextDouble(), r.NextDouble()));
            tmp.Series.Add(s1);
            ScatterModel = tmp;
        }
    }
}