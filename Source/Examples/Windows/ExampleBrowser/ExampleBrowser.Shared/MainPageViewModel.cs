// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using System;
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
        private readonly IList<IGrouping<string, ExampleInfo>> exampleGroups;

        private double frameRate;
        
        private ExampleInfo selectedExample;

        private string filterString;
        
        public MainPageViewModel()
        {
            var examples = Examples.GetList();
            this.exampleGroups = examples.GroupBy(example => example.Category).OrderBy(g => g.Key).ToList();
            var examplesViewSource = new CollectionViewSource { Source = this.exampleGroups, IsSourceGrouped = true };
            this.ExamplesView = examplesViewSource.View;
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
                this.RaisePropertyChanged(nameof(this.FrameRate));
            }
        }

        public IEnumerable<IGrouping<string, ExampleInfo>> ExampleGroups => this.exampleGroups;
        
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

                this.RaisePropertyChanged(nameof(this.SelectedExample));
                this.RaisePropertyChanged(nameof(this.PlotModel));
                this.RaisePropertyChanged(nameof(this.PlotController));
            }
        }

        public string FilterString
        {
            get
            {
                return this.filterString;
            }

            set
            {
                this.filterString = value;
                this.UpdateFilter();

                this.RaisePropertyChanged(nameof(this.FilterString));
                this.RaisePropertyChanged(nameof(this.ExamplesView));
            }
        }

        public PlotModel PlotModel => this.SelectedExample?.PlotModel;

        public IPlotController PlotController => this.SelectedExample?.PlotController;

        public string Version => typeof(PlotModel).GetTypeInfo().Assembly.FullName.Split(',')[1];

        /// <summary>
        /// Case-insensitively filters the groups based on the given filter string and updates the <see cref="ICollectionView"/> containing the examples.
        /// </summary>
        protected void UpdateFilter()
        {
            var result = new List<IGrouping<string, ExampleInfo>>();

            foreach (var group in this.ExampleGroups)
            {
                result.AddRange(
                    group.Where(item => item.Title.IndexOf(this.FilterString, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToLookup(item => group.Key));
            }

            var source = new CollectionViewSource { Source = result, IsSourceGrouped = true };
            this.ExamplesView = source.View;
        }

        protected void RaisePropertyChanged(string property)
        {
            var handler = this.PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}