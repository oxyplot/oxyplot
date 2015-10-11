namespace IssueDemos
{
    using System;

    using OxyPlot;
    using OxyPlot.Series;

    [DemoPage("Change PlotView visibility", "Use the switch to toggle the visibility")]
    public partial class ChangeVisibility
    {
        public ChangeVisibility()
        {
            this.InitializeComponent();
            this.Model = new PlotModel { Background = OxyColors.White };
            this.Model.Series.Add(new FunctionSeries(Math.Sin, 0, 20, 500, "sin(x)"));
            this.BindingContext = this;
        }

        public PlotModel Model { get; set; }
    }
}
