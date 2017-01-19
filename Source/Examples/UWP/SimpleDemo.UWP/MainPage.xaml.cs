using System;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SimpleDemo.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void OnPlotViewManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            WriteLog("Manipulation started");
        }

        private void OnPlotViewManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            WriteLog("Manipulation ended");
        }

        private void WriteLog(string text)
        {
            Debug.WriteLine(text);

            outputLogging.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                outputLogging.Text += $"{text}{Environment.NewLine}";
            });
        }
    }
}
