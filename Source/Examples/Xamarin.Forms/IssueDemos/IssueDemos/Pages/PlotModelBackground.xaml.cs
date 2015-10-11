namespace IssueDemos
{
    using System;

    using OxyPlot;
    using OxyPlot.Series;

    [DemoPage("PlotModel with Background", "The background color is set on the PlotModel")]
    public partial class PlotModelBackground
    {
        public PlotModelBackground()
        {
            this.InitializeComponent();
            this.Model = new PlotModel { Background = OxyColors.Red, Title = "PlotModel.Background=Red" };
            this.Model.Series.Add(new FunctionSeries(Math.Sin, 0, 20, 500, "sin(x)"));
            this.BindingContext = this;
        }

        public PlotModel Model { get; set; }
    }
}
