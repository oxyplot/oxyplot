namespace WindowsFormsApplication1
{
    using System;
    using System.Windows.Forms;

    using OxyPlot;
    using OxyPlot.Series;

    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
            var myModel = new PlotModel("Example 1");
            myModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            this.plot1.Model = myModel;
        }
    }
}
