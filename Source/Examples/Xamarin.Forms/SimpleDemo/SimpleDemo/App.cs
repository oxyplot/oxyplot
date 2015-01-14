// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SimpleDemo
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Xamarin.Forms;

    using Xamarin.Forms;

    /// <summary>
    /// Represents the simple demo app.
    /// </summary>
    public class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            var plotModel = new PlotModel
            {
                Title = "OxyPlot in Xamarin.Forms",
                Subtitle = string.Format("OS: {0}, Idiom: {1}", Device.OS, Device.Idiom),
                Background = OxyColors.LightYellow,
                PlotAreaBackground = OxyColors.LightGray
            };
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom };
            var valueAxis = new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0 };
            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(valueAxis);
            var series = new ColumnSeries();
            series.Items.Add(new ColumnItem { Value = 3 });
            series.Items.Add(new ColumnItem { Value = 14 });
            series.Items.Add(new ColumnItem { Value = 11 });
            series.Items.Add(new ColumnItem { Value = 12 });
            series.Items.Add(new ColumnItem { Value = 7 });
            plotModel.Series.Add(series);

            this.MainPage = new ContentPage
            {
                Padding = new Thickness(0, 20, 0, 0),
                Content = new PlotView
                {
                    Model = plotModel,
                    VerticalOptions = LayoutOptions.Fill,
                    HorizontalOptions = LayoutOptions.Fill,
                },
            };
        }

        /// <summary>
        /// Handles when your app starts.
        /// </summary>
        protected override void OnStart()
        {
        }

        /// <summary>
        /// Handles when your app sleeps.
        /// </summary>
        protected override void OnSleep()
        {
        }

        /// <summary>
        /// Handles when your app resumes.
        /// </summary>
        protected override void OnResume()
        {
        }
    }
}
