namespace OxyPlot.Wpf
{
    public class LinearAxis : AxisBase
    {
        #region Constructors and Destructors

        public LinearAxis()
        {
            this.Axis = new OxyPlot.LinearAxis();
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
            var a = this.Axis as OxyPlot.LinearAxis;
        }

        #endregion
    }
}