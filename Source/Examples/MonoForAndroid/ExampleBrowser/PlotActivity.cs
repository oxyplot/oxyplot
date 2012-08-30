using Android.App;
using Android.OS;

namespace ExampleBrowser
{
    using System.Linq;

    using OxyPlot.MonoForAndroid;

    [Activity(Label = "Plot")]
    public class PlotActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
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

