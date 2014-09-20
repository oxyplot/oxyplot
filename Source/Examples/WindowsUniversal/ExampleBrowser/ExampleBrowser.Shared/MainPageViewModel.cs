// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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

    using Windows.UI.Xaml.Data;

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