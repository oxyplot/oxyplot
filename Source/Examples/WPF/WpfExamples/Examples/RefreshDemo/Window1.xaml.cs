using System;

namespace RefreshDemo
{
    using System.ComponentModel;
    using System.Windows.Media;

    using OxyPlot;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : INotifyPropertyChanged
    {
        #region PropertyChanged Block
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion

        private PlotModel plotModel;

        public PlotModel PlotModel
        {
            get
            {
                return this.plotModel;
            }
            set
            {
                this.plotModel = value;
                this.RaisePropertyChanged("PlotModel");
            }
        }

        public Window1()
        {
            InitializeComponent();
            this.DataContext = this;
            CompositionTarget.Rendering += this.CompositionTarget_Rendering;
        }

        private double lastUpdateTime;

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            double seconds = ((RenderingEventArgs)e).RenderingTime.TotalSeconds;
            if (seconds > this.lastUpdateTime + 0.2)
            {
                this.PlotModel = this.CreatePlot();
                this.lastUpdateTime = seconds;
            }
        }

        private double x;

        private PlotModel CreatePlot()
        {
            var pm = new PlotModel("Plot updated: " + DateTime.Now);
            pm.Series.Add(new FunctionSeries(Math.Sin, x, x + 4, 0.01));
            x += 0.1;
            return pm;
        }
    }
}
