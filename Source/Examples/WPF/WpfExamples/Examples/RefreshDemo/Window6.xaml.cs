// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window6.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for Window6.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RefreshDemo
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for Window6.xaml
    /// </summary>
    public partial class Window6 : Window
    {
        public Window6()
        {
            this.InitializeComponent();
            var vm = new Window6ViewModel();
            this.Closed += (s, e) => vm.Close();
            this.DataContext = vm;
        }
    }
}