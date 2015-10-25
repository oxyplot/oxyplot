// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Linq;

    using ExampleLibrary;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IEnumerable<ExampleInfo> examples;

        private ExampleInfo selectedExample;

        public MainWindowViewModel()
        {
            this.Examples = ExampleLibrary.Examples.GetList().OrderBy(e => e.Category);
            this.SelectedExample = this.Examples.FirstOrDefault(ei => ei.Title == Properties.Settings.Default.SelectedExample);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<ExampleInfo> Examples
        {
            get { return this.examples; }
            set { this.examples = value; this.RaisePropertyChanged("Examples"); }
        }

        public ExampleInfo SelectedExample
        {
            get { return this.selectedExample; }
            set
            {
                this.selectedExample = value; this.RaisePropertyChanged("SelectedExample");
                Properties.Settings.Default.SelectedExample = value != null ? value.Title : null;
                Properties.Settings.Default.Save();
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