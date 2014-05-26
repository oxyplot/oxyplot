// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WpfExamples
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Examples = this.GetExamples(this.GetType().Assembly).OrderBy(e => e.Title).ToArray();
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
        private static void CreateThumbnail(Window window, int width, string path)
        {
            var bitmap = ScreenCapture.Capture(
                (int)window.Left,
                (int)window.Top,
                (int)window.ActualWidth,
                (int)window.ActualHeight);
            var newHeight = width * bitmap.Height / bitmap.Width;
            var resizedBitmap = BitmapTools.Resize(bitmap, width, newHeight);
            resizedBitmap.Save(path);
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the ListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs" /> instance containing the event data.</param>
        private void ListBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var lb = (ListBox)sender;
            var example = lb.SelectedItem as Example;
            if (example != null)
            {
                var window = example.Create();
                window.Icon = this.Icon;
                window.Show();

                window.KeyDown += (s, args) =>
                {
                    if (args.Key == Key.F12)
                    {
                        CreateThumbnail(window, 120, Path.Combine(@"..\..\Images\", example.ThumbnailFileName));
                        MessageBox.Show(window, "Demo image updated.");
                        e.Handled = true;
                    }
                };
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