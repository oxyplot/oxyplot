namespace OxyPlot.Wpf
{
    public class LogarithmicAxis : RangeAxis
    {
        public LogarithmicAxis()
        {
            ModelAxis = new OxyPlot.LogarithmicRangeAxis();
        }

        public override void UpdateModelProperties()
        {
            var a = ModelAxis as LogarithmicAxis;
            base.UpdateModelProperties();
        }
    }
}