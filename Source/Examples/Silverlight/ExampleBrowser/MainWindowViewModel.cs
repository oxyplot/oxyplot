// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="OxyPlot">
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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Controls;

    using ExampleLibrary;

    using OxyPlot;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IEnumerable<ExampleInfo> examples;
        private object selectedItem;
        private ExampleInfo selectedExample;
        private double frameRate;

        public MainWindowViewModel()
        {
            this.Examples = ExampleLibrary.Examples.GetList();
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
                this.frameRate = value; this.RaisePropertyChanged("FrameRate");
            }
        }

        public IEnumerable<ExampleInfo> Examples
        {
            get { return this.examples; }
            set { this.examples = value; this.RaisePropertyChanged("Examples"); }
        }

        public object SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                this.selectedItem = value;
                var cc = value as ContentControl;
                this.SelectedExample = cc.Content as ExampleInfo;
            }
        }

        public ExampleInfo SelectedExample
        {
            get { return selectedExample; }
            set
            {
                this.selectedExample = value;
                this.RaisePropertyChanged("SelectedExample");
            }
        }

        public string Version
        {
            get
            {
                return typeof(PlotModel).Assembly.FullName.Split(',')[1];
            }
        }

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    // Simple IGroupingItemsControlConverterSelector implementation for grouping by an Animal's species
    public class ExampleInfoGroupSelector : IGroupingItemsControlConverterSelector
    {
        public Func<object, IComparable> GetGroupSelector()
        {
            return (o) => ((ExampleInfo)o).Category;
        }
    }
}