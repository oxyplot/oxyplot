namespace IssueDemos
{
    using System;

    using Xamarin.Forms;

    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Xamarin.Forms;

    [DemoPage("Add/Remove plot views", "Create and remove PlotViews")]
    public partial class AddPlotViews
    {
        private static OxyColor[] backgroundColors = { OxyColors.LightGreen, OxyColors.White, OxyColors.LightCyan, OxyColors.LightGreen, OxyColors.LightPink };

        public AddPlotViews()
        {
            this.InitializeComponent();
        }

        public void AddButtonClicked(object sender, EventArgs e)
        {
            int n = this.layout1.Children.Count + 1;
            var pm = new PlotModel { Title = "Plot " + n, Background = backgroundColors[n % backgroundColors.Length] };
            pm.Series.Add(new FunctionSeries(x => Math.Sin(n * x), 0, 20, 500, "sin(x)"));
            var pv = new PlotView();
            pv.Model = pm;
            pv.HorizontalOptions = LayoutOptions.FillAndExpand;
            pv.HeightRequest = 200;
            this.layout1.Children.Add(pv);
        }
        public void RemoveButtonClicked(object sender, EventArgs e)
        {
            if (this.layout1.Children.Count > 0) this.layout1.Children.RemoveAt(this.layout1.Children.Count - 1);
        }
    }
}
