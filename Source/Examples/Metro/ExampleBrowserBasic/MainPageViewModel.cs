// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ExampleLibrary;
using OxyPlot;

namespace ExampleBrowserBasic
{
    using OxyPlot.Metro;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;

    public class MainPageViewModel : INotifyPropertyChanged
    {
        public bool MeasureFrameRate { get; set; }

        private double frameRate;

        public double FrameRate
        {
            get
            {
                return this.frameRate;
            }
            set
            {
                this.frameRate = value; RaisePropertyChanged("FrameRate");
            }
        }

        private IEnumerable<ExampleInfo> examples;
        public IEnumerable<ExampleInfo> Examples
        {
            get { return examples; }
            set { examples = value; RaisePropertyChanged("Examples"); }
        }

        public ICollectionView ExamplesView { get; set; }

        private ExampleInfo selectedExample;
        public ExampleInfo SelectedExample
        {
            get { return selectedExample; }
            set
            {
                selectedExample = value;
                RaisePropertyChanged("SelectedExample");
                RaisePropertyChanged("PlotBackground");
            }
        }

        public Brush PlotBackground
        {
            get
            {
                return selectedExample != null && selectedExample.PlotModel.Background != null ? selectedExample.PlotModel.Background.ToBrush() : new SolidColorBrush() { Opacity = 0 }; // Brushes.Transparent;
            }
        }

        public MainPageViewModel()
        {
            Examples = ExampleLibrary.Examples.GetList();

            var groups = 
                from example in Examples
                group example by example.Category into g
                orderby g.Key
                select g;                        

            var ex = new CollectionViewSource() { Source = groups, IsSourceGrouped = true };
            ExamplesView = ex.View;
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