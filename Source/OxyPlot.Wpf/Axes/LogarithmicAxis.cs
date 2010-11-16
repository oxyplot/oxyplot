namespace OxyPlot.Wpf
{
    public class LogarithmicAxis : Axis
    {
        public LogarithmicAxis()
        {
            ModelAxis = new OxyPlot.LogarithmicAxis();
        }

        public override void UpdateModelProperties()
        {
            var a = ModelAxis as OxyPlot.LogarithmicAxis;
            base.UpdateModelProperties();
        }
    }
}