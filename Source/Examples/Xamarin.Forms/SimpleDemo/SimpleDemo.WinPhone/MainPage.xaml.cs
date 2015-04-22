// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SimpleDemo.WinPhone
{
    using Microsoft.Phone.Controls;

    using OxyPlot.Xamarin.Forms.Platform.WP8;

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

            Forms.Init();
            Xamarin.Forms.Forms.Init();
            this.LoadApplication(new SimpleDemo.App());
        }
    }
}
