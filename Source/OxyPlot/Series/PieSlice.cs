namespace OxyPlot
{
    /// <summary>
    ///   A slice of the PieSeries
    /// </summary>
    public class PieSlice
    {
        public PieSlice()
        {
        }

        public PieSlice(string label, double value, OxyColor fill = null)
        {
            Label = label;
            Value = value;
            Fill = fill;
        }

        public string Label { get; set; }
        public double Value { get; set; }
        public OxyColor Fill { get; set; }
        public bool IsExploded { get; set; }
    }
}