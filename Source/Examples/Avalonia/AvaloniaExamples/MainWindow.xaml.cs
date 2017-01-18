// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia.Input;
using Avalonia.Media.Imaging;

namespace AvaloniaExamples
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using Avalonia.Diagnostics;
    using Avalonia.Interactivity;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ListBox ListBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ListBox.Items = this.Examples = this.GetExamples(this.GetType().Assembly).OrderBy(e => e.Title).ToArray();
            this.DataContext = this;
            DevTools.Attach(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            ListBox = this.Find<ListBox>(nameof(ListBox));
            ListBox.DoubleTapped += ListBoxMouseDoubleClick;
        }

        /// <summary>
        /// Gets the examples.
        /// </summary>
        /// <value>The examples.</value>
        public IList<Example> Examples { get; private set; }

        /// <summary>
        /// Creates a thumbnail of the specified window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="width">The width of the thumbnail.</param>
        /// <param name="path">The output path.</param>
        private static void CreateThumbnail(Avalonia.Controls.Window window, int width, string path)
        {
            var bitmap = ScreenCapture.Capture(
                (int)window.Bounds.TopLeft.X,
                (int)window.Bounds.TopLeft.Y,
                (int)window.Bounds.Width,
                (int)window.Bounds.Height);
            var newHeight = width * bitmap.Height / bitmap.Width;
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the ListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs" /> instance containing the event data.</param>
        private void ListBoxMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var lb = (ListBox)sender;
            var example = lb.SelectedItem as Example;
            if (example != null)
            {
                var window = example.Create();
                window.Icon = this.Icon;
                window.Show();
                
            }
        }

        /// <summary>
        /// Gets the examples in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        /// <returns>A sequence of <see cref="Example" /> objects.</returns>
        private IEnumerable<Example> GetExamples(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var ea = type.GetCustomAttributes(typeof(ExampleAttribute), false).FirstOrDefault() as ExampleAttribute;
                if (ea != null)
                {
                    yield return new Example(type, ea.Title, ea.Description);
                }
            }
        }
    }
}