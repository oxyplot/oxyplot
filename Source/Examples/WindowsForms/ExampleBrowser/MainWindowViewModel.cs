// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    using ExampleLibrary;
    using OxyPlot;

    using System.ComponentModel;

    using OxyPlot.WindowsForms;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IEnumerable<ExampleInfo> examples;
        public IEnumerable<ExampleInfo> Examples
        {
            get { return examples; }
            set { examples = value; RaisePropertyChanged("Examples"); }
        }

        private ExampleInfo selectedExample;
        public ExampleInfo SelectedExample
        {
            get { return selectedExample; }
            set { selectedExample = value; RaisePropertyChanged("SelectedExample"); }
        }

        public MainWindowViewModel()
        {
            this.Examples = ExampleLibrary.Examples.GetList().OrderBy(e => e.Category);
        }

        public Color PlotBackground
        {
            get
            {
                return SelectedExample != null && SelectedExample.PlotModel.Background != null
                           ? SelectedExample.PlotModel.Background.ToColor()
                           : Color.White;
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