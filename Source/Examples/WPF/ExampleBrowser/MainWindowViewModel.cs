// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;

    using ExampleLibrary;

    using OxyPlot;

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
            this.ExamplesView = CollectionViewSource.GetDefaultView(this.Examples.OrderBy(e => e.Category));
            this.ExamplesView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
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
                this.RaisePropertyChanged("Model");
                this.RaisePropertyChanged("Controller");
                this.RaisePropertyChanged("SelectedExample");
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