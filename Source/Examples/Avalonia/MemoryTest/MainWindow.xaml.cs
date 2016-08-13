using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MemoryTest
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            App.AttachDevTools(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.Get<Button>(nameof(Window1)).Click += OpenWindow1;
            this.Get<Button>(nameof(Window2)).Click += OpenWindow2;
        }

        private void OpenWindow2(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new Window2().Show();
        }

        private void OpenWindow1(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new Window1().Show();
        }
    }
}
