// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    public partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }
}