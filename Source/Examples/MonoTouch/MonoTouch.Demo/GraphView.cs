using MonoTouch.UIKit;
using OxyPlot;
using OxyPlot.MonoTouch;
using ExampleLibrary;

namespace MonoTouch.Demo
{
	public class GraphView : UIView
	{
		private readonly ExampleInfo exampleInfo;
		public GraphView (ExampleInfo exampleInfo)
		{
			this.exampleInfo = exampleInfo;
		}
		
		public override void Draw (System.Drawing.RectangleF rect)
		{
	        base.Draw (rect);
			
			var plot = exampleInfo.PlotModel;
			
			plot.PlotMargins = new OxyThickness(20);
			plot.Background = OxyColors.LightGray;
			
			//Set compatible IOS font
			PlotModel.DefaultFont = "Helvetica";
			
			plot.Update(true);
			
			var renderer = new MonoTouchRenderContext(UIGraphics.GetCurrentContext(), rect);
			plot.Render(renderer);
		}
	}
}