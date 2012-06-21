using MonoTouch.UIKit;
using System.Drawing;

using OxyPlot;
using OxyPlot.MonoTouch;
using ExampleLibrary;

namespace MonoTouch.Demo
{
	public class GraphView : UIView
	{	
		private ExampleInfo exampleInfo;
		
		public GraphView (ExampleInfo exampleInfo)
		{
			this.exampleInfo = exampleInfo;
		}
		
		public override void Draw (System.Drawing.RectangleF rect)
		{
			var plot = exampleInfo.PlotModel;
			
			plot.PlotMargins = new OxyThickness(5);
			plot.Background = OxyColors.LightGray;
			plot.Update(true);
			
			var context = UIGraphics.GetCurrentContext();
			
			var renderer = new MonoTouchRenderContext(context, rect);
			plot.Render(renderer);
		}
	}
}