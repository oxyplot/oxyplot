<Query Kind="Program">
  <Namespace>OxyPlot</Namespace>
</Query>

void Main()
{
	var pm = new PlotModel("Simple Line Plot");
								
	var lineSeries1 = new LineSeries("LineSeries1");

	lineSeries1.Points.Add( new DataPoint (1,1));
	lineSeries1.Points.Add( new DataPoint (2,2));
	lineSeries1.Points.Add( new DataPoint (3,1.5));

	pm.Series.Add(lineSeries1);
	
	pm.Show();
}