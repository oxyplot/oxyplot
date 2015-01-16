// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleListActivity.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace ExampleBrowser
{
    [Activity(Label = "Example list", Icon = "@drawable/icon")]
    public class ExampleListActivity : ListActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var category = Intent.GetStringExtra("category");
            this.Title = category;

            var examples = ExampleLibrary.Examples.GetList();
            var plots = examples.Where(e => e.Category == category).Select(e => e.Title).OrderBy(s => s).ToList();
            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.ListItem, plots);
            ListView.TextFilterEnabled = true;
            ListView.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args)
            {
                // When clicked, show a toast with the TextView text
                Toast.MakeText(Application, ((TextView)args.View).Text, ToastLength.Short).Show();
                var second = new Intent(this, typeof(PlotActivity));

                second.PutExtra("category", category);
                second.PutExtra("plot", plots[args.Position]);
                StartActivity(second);
            };
        }
    }
}