// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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

namespace ExampleBrowser
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using ExampleLibrary;

    using OxyPlot;
    using OxyPlot.Metro;

    using Windows.UI;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private double frameRate;

        private IEnumerable<ExampleInfo> examples;

        private ExampleInfo selectedExample;

        public MainPageViewModel()
        {
            this.Examples = ExampleLibrary.Examples.GetList();
            var groups = this.Examples.GroupBy(example => example.Category).OrderBy(g => g.Key);
            var ex = new CollectionViewSource { Source = groups, IsSourceGrouped = true };
            this.ExamplesView = ex.View;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool MeasureFrameRate { get; set; }

        public double FrameRate
        {
            get
            {
                return this.frameRate;
            }

            set
            {
                this.frameRate = value;
                this.RaisePropertyChanged("FrameRate");
            }
        }

        public IEnumerable<ExampleInfo> Examples
        {
            get
            {
                return this.examples;
            }

            set
            {
                this.examples = value;
                this.RaisePropertyChanged("Examples");
            }
        }

        public ICollectionView ExamplesView { get; set; }

        public ExampleInfo SelectedExample
        {
            get
            {
                return this.selectedExample;
            }

            set
            {
                this.selectedExample = value;
                this.RaisePropertyChanged("SelectedExample");
                this.RaisePropertyChanged("PlotModel");
                this.RaisePropertyChanged("PlotController");
                this.RaisePropertyChanged("PlotBackground");
            }
        }

        public PlotModel PlotModel
        {
            get
            {
                return this.SelectedExample != null ? this.SelectedExample.PlotModel : null;
            }
        }

        public IPlotController PlotController
        {
            get
            {
                return this.SelectedExample != null ? this.SelectedExample.PlotController : null;
            }
        }

        public Brush PlotBackground
        {
            get
            {
                return this.selectedExample != null && this.selectedExample.PlotModel.Background.IsVisible() ? this.selectedExample.PlotModel.Background.ToBrush() : new SolidColorBrush(Colors.White);
            }
        }

        public string Version
        {
            get
            {
                return typeof(PlotModel).GetTypeInfo().Assembly.FullName.Split(',')[1];
            }
        }

        protected void RaisePropertyChanged(string property)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}