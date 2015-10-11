namespace IssueDemos
{
    using System;

    using OxyPlot;
    using OxyPlot.Series;

    [DemoPage("Background color set on Page, PlotView and PlotModel", "The background color of the plot should be violet")]
    public partial class AllBackgroundColorsSet
    {
        public AllBackgroundColorsSet()
        {
            this.InitializeComponent();
            this.Model = new PlotModel { Background = OxyColors.Violet, Title = "PlotModel.Background = Violet" };
            this.Model.Series.Add(new FunctionSeries(Math.Sin, 0, 20, 500, "sin(x)"));
            this.BindingContext = this;
        }

        public PlotModel Model { get; set; }
    }
}
