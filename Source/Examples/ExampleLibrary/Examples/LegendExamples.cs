using System;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("Legends")]
    public static class LegendExamples
    {
        private static PlotModel CreateModel()
        {
            var model = new PlotModel("LineSeries");
            for (int i=1;i<=20;i++)
            {
                var s = new LineSeries("Series " + i);
                model.Series.Add(s);
                for (double x = 0; x < 2 * Math.PI; x += 0.1)
                    s.Points.Add(new DataPoint(x, Math.Sin(x*i)/i+i));
            }
            return model;

        }


        [Example("Legend at top")]
        public static PlotModel LegendTop()
        {
            var model = CreateModel();
            model.LegendPosition = LegendPosition.Top;
            model.LegendLayout = LegendLayout.Horizontal;
            model.PlotMargins=new OxyThickness(60,100,50,50);
            return model;
        }
        
        [Example("Legend at bottom")]
        public static PlotModel LegendBottom()
        {
            var model = CreateModel();
            model.LegendPosition = LegendPosition.Bottom;
            model.LegendLayout = LegendLayout.Horizontal;
            return model;
        }

        [Example("Legend at top right")]
        public static PlotModel LegendTopRight()
        {
            var model = CreateModel();
            model.LegendPosition = LegendPosition.TopRight;
            return model;
        }

        [Example("Legend at top right outside")]
        public static PlotModel LegendTopRightOutside()
        {
            var model = CreateModel();
            model.LegendPosition = LegendPosition.TopRight;
            model.IsLegendOutsidePlotArea = true;
            return model;
        }
    }
}
