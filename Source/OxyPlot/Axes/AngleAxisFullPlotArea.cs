using System;
using System.Collections.Generic;
using System.Text;

namespace OxyPlot.Axes
{
    public class AngleAxisFullPlotArea : AngleAxis
    {
        /// <summary>
        /// Renders the axis on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="pass">The pass.</param>
        public override void Render(IRenderContext rc, int pass)
        {
            var r = new AngleAxisFullPlotAreaRenderer(rc, this.PlotModel);
            r.Render(this, pass);
        }
    }
}
