// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryListActivity.cs" company="OxyPlot">
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