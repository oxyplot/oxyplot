namespace OxyPlot
{
    public class LineSeries : DataSeries
    {
        public LineSeries()
        {
            MinimumSegmentLength = 2;
            Thickness = 2;

            MarkerSize = 3;
            MarkerStrokeThickness = 1;
        }

        public Color Color { get; set; }


        public double Thickness { get; set; }
        public LineStyle LineStyle { get; set; }
        public double[] Dashes { get; set; }

        public MarkerType MarkerType { get; set; }
        public double MarkerSize { get; set; }
        public Color MarkerStroke { get; set; }
        public double MarkerStrokeThickness { get; set; }
        public Color MarkerFill { get; set; }

        /// <summary>
        /// Minimum length of a segment on the curve
        /// Increasing this number will increase performance, 
        /// but make the curve less accurate
        /// </summary>
        public double MinimumSegmentLength { get; set; }
    }
}