// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SimpleDemo.WinPhone
{
    using Microsoft.Phone.Controls;

    using Xamarin.Forms.Platform.WinPhone;

    /// <summary>
    /// The main page.
    /// </summary>
    public partial class MainPage : FormsApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            Xamarin.Forms.Forms.Init();
            OxyPlot.Xamarin.Forms.Platform.WP8.PlotViewRenderer.Init();
            this.LoadApplication(new SimpleDemo.App());
        }
    }
}
