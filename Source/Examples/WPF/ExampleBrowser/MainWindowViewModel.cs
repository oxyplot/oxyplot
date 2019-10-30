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

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IEnumerable<ExampleInfo> examples;

        private double frameRate;

        private ExampleInfo selectedExample;

        public MainWindowViewModel()
        {
            this.Examples = ExampleLibrary.Examples.GetList();
            this.ExamplesView = CollectionViewSource.GetDefaultView(this.Examples.OrderBy(e => e.Category));
            this.ExamplesView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool MeasureFrameRate { get; set; }

        public double FrameRate
        {
            get => this.frameRate;
            set
            {
                this.frameRate = value;
                this.RaisePropertyChanged(nameof(this.FrameRate));
            }
        }

        public IEnumerable<ExampleInfo> Examples
        {
            get => this.examples;
            set
            {
                this.examples = value;
                this.RaisePropertyChanged(nameof(this.Examples));
            }
        }

        public ICollectionView ExamplesView { get; set; }

        public ExampleInfo SelectedExample
        {
            get => this.selectedExample;
            set
            {
                this.selectedExample = value;
                this.RaisePropertyChanged(nameof(this.SelectedExample));
            }
        }

        protected void RaisePropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
