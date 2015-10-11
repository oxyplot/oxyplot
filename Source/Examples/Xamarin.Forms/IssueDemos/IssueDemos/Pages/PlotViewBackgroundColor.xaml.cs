namespace IssueDemos
{
    using System;

    using OxyPlot;
    using OxyPlot.Series;

    [DemoPage("PlotView with BackgroundColor", "The background color is set on the PlotView")]
    public partial class PlotViewBackgroundColor
    {
        public PlotViewBackgroundColor()
        {
            this.InitializeComponent();
            this.Model = new PlotModel { Background = OxyColors.Undefined, Title = "PlotView.BackgroundColor=Blue" };
            this.Model.Series.Add(new FunctionSeries(Math.Sin, 0, 20, 500, "sin(x)"));
            this.BindingContext = this;
        }

        public PlotModel Model { get; set; }
    }
}
