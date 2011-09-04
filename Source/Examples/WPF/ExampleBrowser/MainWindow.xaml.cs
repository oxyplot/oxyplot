//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExampleBrowser
{
    using System.Diagnostics;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel vm = new MainWindowViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            watch.Start();
        }

        private int frameCount = 0;
        Stopwatch watch = new Stopwatch();

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            frameCount++;
            if (watch.ElapsedMilliseconds > 1000 && frameCount > 1)
            {
                vm.FrameRate = frameCount / (watch.ElapsedMilliseconds * 0.001);
                frameCount = 0;
                watch.Restart();
            }

            if (vm.MeasureFrameRate)
            {
                Plot1.RefreshPlot(true);
            }
        }
    }
}
