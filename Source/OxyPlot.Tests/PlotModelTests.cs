using System;
using NUnit.Framework;

namespace OxyPlot.Tests
{
    [TestFixture]
    public class PlotModelTests
    {
        [Test]
        public void Render_SimplePlot_SvgFile()
        {
            var plot = new PlotModel();
            plot.Title = "Test1";
            plot.Subtitle = "subtitle";
            var ls = new LineSeries();
            for (double i = 0; i < 30; i += 0.1)
                ls.Points.Add(new DataPoint(i, Math.Sin(i) * 20));
            plot.Series.Add(ls);
            plot.Axes.Add(new LinearAxis { Title = "Y-axis1", Position = AxisPosition.Left });
            plot.Axes.Add(new LinearAxis { Title = "X-axis1", Position = AxisPosition.Bottom });
            plot.Axes.Add(new LinearAxis { Title = "Y-axis2", Position = AxisPosition.Right });
            plot.Axes.Add(new LinearAxis { Title = "X-axis2", Position = AxisPosition.Top });

            using (var svgrc = new SvgRenderContext("test.svg", 1200, 800))
            {
                plot.UpdateData();
                plot.Render(svgrc);
            }
        }
       
    }
}
