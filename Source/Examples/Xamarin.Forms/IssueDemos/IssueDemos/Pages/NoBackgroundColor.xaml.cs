namespace IssueDemos
{
    using System;

    using OxyPlot;
    using OxyPlot.Series;

    [DemoPage("Background color not defined", "Background color should be the platform default")]
    public partial class NoBackgroundColor
    {
        public NoBackgroundColor()
        {
            this.InitializeComponent();
            this.Model = new PlotModel { Background = OxyColors.Undefined, Title = "PlotModel.Background=Undefined" };
            this.Model.Series.Add(new FunctionSeries(Math.Sin, 0, 20, 500, "sin(x)"));
            this.BindingContext = this;
        }

        public PlotModel Model { get; set; }
    }
}
