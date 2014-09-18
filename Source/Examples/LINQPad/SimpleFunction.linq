<Query Kind="Program">
  <Namespace>OxyPlot</Namespace>
</Query>

void Main()
{
	var pm = new PlotModel("Simple Function Plots") { PlotType = PlotType.Cartesian };
							
	pm.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)"));
	pm.Series.Add(new FunctionSeries(Math.Cos, -10, 10, 0.1, "cos(x)"));
	pm.Series.Add(new FunctionSeries(t => 5 * Math.Cos(t), t => 5 * Math.Sin(t), 0, 2 * Math.PI, 0.1, "5cos(t),5sin(t)"));

  pm.Show();
}