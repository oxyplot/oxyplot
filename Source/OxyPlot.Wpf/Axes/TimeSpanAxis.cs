namespace OxyPlot.Wpf
{
    public class TimeSpanAxis : AxisBase
    {
        #region Constructors and Destructors

        public TimeSpanAxis()
        {
            this.Axis = new OxyPlot.TimeSpanAxis();
        }

        #endregion

        #region Public Methods

        public override OxyPlot.IAxis CreateModel()
        {
            this.SynchronizeProperties();
            return this.Axis;
        }

        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = this.Axis as OxyPlot.TimeSpanAxis;
        }

        #endregion
    }
}