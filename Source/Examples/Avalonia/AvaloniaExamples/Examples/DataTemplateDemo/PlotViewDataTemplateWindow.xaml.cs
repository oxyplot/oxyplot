// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewDataTemplateWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for PlotViewDataTemplateWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.DataTemplateDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Series;

    using AvaloniaExamples;
    using Avalonia.Controls.Templates;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml.Data;

    /// <summary>
    /// Interaction logic for PlotViewDataTemplateWindow.xaml
    /// </summary>
    [Example("Demonstrates a PlotView in a DataTemplate.")]
    public partial class PlotViewDataTemplateWindow : Avalonia.Controls.Window
    {
        public PlotViewDataTemplateWindow()
        {
            this.InitializeComponent();
            this.DataContext = new { Models = CreateModels().ToArray() };
            this.DataTemplates.Add(new FuncDataTemplate<Model>(model => new OxyPlot.Avalonia.PlotView
            {
                [!!OxyPlot.Avalonia.PlotView.ModelProperty] = new Binding(nameof(Model.PlotModel)),
                [!!OxyPlot.Avalonia.PlotView.ControllerProperty] = new Binding(nameof(Model.PlotController)) 
            }));

        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
            App.AttachDevTools(this);
        }

        private static Random r = new Random(13);

        private static IEnumerable<Model> CreateModels()
        {
            for (var i = 0; i < 3; i++)
            {
                var pm = new PlotModel { Title = string.Format("Plot {0}", i + 1) };

                var series = new LineSeries();
                for (var j = 0; j < 10; j++)
                {
                    series.Points.Add(new DataPoint(j, r.NextDouble()));
                }

                pm.Series.Add(series);

                var pc = new PlotController();
                pc.UnbindAll();
                pc.BindKeyDown(OxyKey.Left, PlotCommands.PanRight);
                pc.BindKeyDown(OxyKey.Right, PlotCommands.PanLeft);

                yield return new Model { PlotModel = pm, PlotController = pc };
            }
        }
    }

    public class Model
    {
        public PlotModel PlotModel { get; set; }
        public IPlotController PlotController { get; set; }
    }
}