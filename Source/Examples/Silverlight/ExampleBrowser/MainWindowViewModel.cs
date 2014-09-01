// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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