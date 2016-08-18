// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MemoryTest
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example(null, "Memory test")]
    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void OpenWindow_Click(object sender, RoutedEventArgs e)
        {
            var type = ((Button)sender).Tag as Type;
            var win = Activator.CreateInstance(type) as Window;
            win.Show();
        }
    }
}