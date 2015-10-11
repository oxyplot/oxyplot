namespace IssueDemos
{
    using System;

    using OxyPlot;
    using OxyPlot.Series;

    [DemoPage("Page with BackgroundColor", "The background color is set on the Page")]
    public partial class PageBackgroundColor
    {
        public PageBackgroundColor()
        {
            this.InitializeComponent();
            this.Model = new PlotModel { Background = OxyColors.Undefined, Title = "Page.BackgroundColor=Yellow" };
            this.Model.Series.Add(new FunctionSeries(Math.Sin, 0, 20, 500, "sin(x)"));
            this.BindingContext = this;
        }

        public PlotModel Model { get; set; }
    }
}
