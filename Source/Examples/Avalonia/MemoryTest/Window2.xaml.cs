using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;

namespace MemoryTest
{
    public class Window2 : Window
    {
        public IList<PlotModel> Plots { get; private set; }

        public Window2()
        {
            Plots = new List<PlotModel>();
            for (int i = 0; i < 10; i++)
            {
                var p = new PlotModel { Title = "Plot " + i };
                p.Series.Add(new FunctionSeries(x => Math.Cos(x * i), 0, 10, 0.01));
                Plots.Add(p);
            }

            InitializeComponent();
            App.AttachDevTools(this);

            DataContext = Plots;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
