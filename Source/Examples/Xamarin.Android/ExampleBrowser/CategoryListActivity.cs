// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryListActivity.cs" company="OxyPlot">
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
    [Activity(Label = "OxyPlot Example Browser", MainLauncher = true, Icon = "@drawable/icon")]
    public class CategoryListActivity : ListActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.Title = "OxyPlot Example Browser";

            var examples=ExampleLibrary.Examples.GetList();
            var categories = examples.Select(e => e.Category).Distinct().OrderBy(s => s).ToList();
            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.ListItem, categories);
            ListView.TextFilterEnabled = true;
            ListView.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args)
            {
                var category = categories[args.Position];
                var second = new Intent(this, typeof(ExampleListActivity));
                second.PutExtra("category", category);
                StartActivity(second);
            };
        }
    }
}