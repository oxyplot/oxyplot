// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotActivity.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using System.Linq;

    using Android.App;
    using Android.OS;
    using Android.Views;

    using OxyPlot.Xamarin.Android;

    [Activity(Label = "Plot")]
    public class PlotActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.RequestWindowFeature(WindowFeatures.NoTitle);

            var category = this.Intent.GetStringExtra("category");
            var plot = this.Intent.GetStringExtra("plot");

            var exampleInfo = ExampleLibrary.Examples.GetList().FirstOrDefault(ei => ei.Category == category && ei.Title == plot);
            var model = exampleInfo.PlotModel;
            this.Title = exampleInfo.Title;

            this.SetContentView(Resource.Layout.PlotActivity);
            var plotView = this.FindViewById<PlotView>(Resource.Id.plotview);
            plotView.Model = model;
        }
    }
}