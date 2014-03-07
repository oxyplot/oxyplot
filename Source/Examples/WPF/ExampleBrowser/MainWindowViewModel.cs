// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="OxyPlot">
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

namespace ExampleBrowser
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Media;

    using ExampleLibrary;

    using OxyPlot;
    using OxyPlot.Wpf;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IPlotController defaultController;

        private IEnumerable<ExampleInfo> examples;

        private double frameRate;

        private ExampleInfo selectedExample;

        public MainWindowViewModel()
        {
            this.defaultController = new PlotController();
            this.Examples = ExampleLibrary.Examples.GetList();
            this.ExamplesView = CollectionViewSource.GetDefaultView(Examples.OrderBy(e => e.Category));
            this.ExamplesView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
        }

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
                this.RaisePropertyChanged("Model");
                this.RaisePropertyChanged("Controller");
                this.RaisePropertyChanged("SelectedExample");
                this.RaisePropertyChanged("PlotBackground");
            }
        }

        public PlotModel Model
        {
            get
            {
                return this.SelectedExample != null ? this.SelectedExample.PlotModel : null;
            }
        }

        public IPlotController Controller
        {
            get
            {
                return this.SelectedExample != null && this.SelectedExample.PlotController != null ? this.SelectedExample.PlotController : this.defaultController;
            }
        }

        public Brush PlotBackground
        {
            get
            {
                return selectedExample != null && selectedExample.PlotModel.Background.IsVisible() ? selectedExample.PlotModel.Background.ToBrush() : Brushes.Transparent;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}