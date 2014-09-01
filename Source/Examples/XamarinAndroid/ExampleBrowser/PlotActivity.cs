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

    using OxyPlot.XamarinAndroid;

    [Activity(Label = "Plot")]
    public class PlotActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			this.RequestWindowFeature (WindowFeatures.NoTitle);

            var category = Intent.GetStringExtra("category");
            var plot = Intent.GetStringExtra("plot");

            var exampleInfo = ExampleLibrary.Examples.GetList().FirstOrDefault(ei => ei.Category == category && ei.Title == plot);
            var model = exampleInfo.PlotModel;
            this.Title = exampleInfo.Title;

            this.SetContentView(Resource.Layout.PlotActivity);
            var plotView = FindViewById<PlotView>(Resource.Id.plotview);
            plotView.Model = model;
        }
    }
}