using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OxyPlot;
using OxyPlot.Series;
using System;

namespace MemoryTest
{
    public class Window1 : Window
    {
        public PlotModel Model { get; set; }

        public Window1()
        {
            Model = new PlotModel { Title = "Test 1" };
            Model.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.01));
            DataContext = Model;

            InitializeComponent();

            App.AttachDevTools(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
