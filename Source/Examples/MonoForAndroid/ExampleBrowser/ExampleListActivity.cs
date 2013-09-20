// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleListActivity.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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