using System.ComponentModel;
using System.Windows.Forms;
using OxyPlot;

namespace Oxyplot.WindowsForms
{
    public partial class PlotControl : Control
    {
        public PlotControl()
        {
        //    InitializeComponent();
            DoubleBuffered = true;
            Model = new PlotModel();
        }

        private OxyPlot.PlotModel model;

        [Browsable(false), DefaultValue(null)]
        public OxyPlot.PlotModel Model
        {
            get { return model; }
            set
            {
                model = value;
                Refresh();
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            if (model != null)
                model.UpdateData();
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var rc = new GraphicsRenderContext(this, e.Graphics, e.ClipRectangle);
            if (model != null)
                model.Render(rc);
        }
    }
}