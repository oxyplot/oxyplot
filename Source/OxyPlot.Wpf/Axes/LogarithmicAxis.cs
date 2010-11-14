namespace OxyPlot.Wpf
{
    public class LogarithmicAxis : LinearAxis
    {
        public LogarithmicAxis()
        {
            ModelAxis = new OxyPlot.LogarithmicRangeAxis();
        }

        public override void UpdateModelProperties()
        {
            var a = ModelAxis as LogarithmicAxis;
        }
    }
}