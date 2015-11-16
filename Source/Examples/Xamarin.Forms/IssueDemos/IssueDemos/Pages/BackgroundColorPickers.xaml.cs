namespace IssueDemos
{
    using System;
    using Xamarin.Forms;
    using OxyPlot.Xamarin.Forms;

    using OxyPlot;
    using OxyPlot.Series;

    [DemoPage("Select background color by picker", "Select background color of page, view and model by picker")]
    public partial class BackgroundColorPickers
    {
        public BackgroundColorPickers()
        {
            this.InitializeComponent();
            var colors = new[] { OxyColors.Undefined, OxyColors.Black, OxyColors.White, OxyColors.Fuchsia, OxyColors.Lime, OxyColors.Teal };
            foreach (var color in colors)
            {
                var name = color.GetColorName();
                pageBackgroundPicker.Items.Add(name);
                viewBackgroundPicker.Items.Add(name);
                modelBackgroundPicker.Items.Add(name);
            }
            var model = new PlotModel { Title = "Background color by picker" };
            model.Series.Add(new FunctionSeries(Math.Sin, 0, 20, 500, "sin(x)"));
            Func<Picker, OxyColor> getSelectedColor = s => colors[((Picker)s).SelectedIndex];
            pageBackgroundPicker.SelectedIndexChanged += (s, e) => { this.BackgroundColor = getSelectedColor((Picker)s).ToXamarinForms(); };
            viewBackgroundPicker.SelectedIndexChanged += (s, e) => { this.plotView1.BackgroundColor = this.plotViewLabel.BackgroundColor = getSelectedColor((Picker)s).ToXamarinForms(); };
            modelBackgroundPicker.SelectedIndexChanged += (s, e) => { model.Background = getSelectedColor((Picker)s); model.InvalidatePlot(false); };
            this.plotView1.Model = model;
        }
    }
}
