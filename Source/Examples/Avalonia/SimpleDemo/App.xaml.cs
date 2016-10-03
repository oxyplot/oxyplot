using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OxyPlot.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDemo
{
    class App : Application
    {
        public App()
        {
            RegisterServices();
        }

        public override void Initialize()
        {
            base.Initialize();
            AvaloniaXamlLoader.Load(this);
        }

        public static void Main(string[] args)
        {
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .BeforeStarting(_ => OxyPlotModule.Initialize())
                .Start<MainWindow>();
        }
    }
}
