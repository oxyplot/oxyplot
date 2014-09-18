<Query Kind="Program">
  <Reference Relative="..\..\..\Output\OxyPlot.dll"></Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationCore.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationFramework.dll</Reference>
  <Reference Relative="..\..\..\Output\OxyPlot.Wpf.dll"></Reference>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
  <Namespace>System.Windows.Media</Namespace>
  <Namespace>OxyPlot</Namespace>
  <Namespace>OxyPlot.Wpf</Namespace>
</Query>

void Main()
{
   var pm = new PlotModel("Sine curve");
   pm.Series.Add(new FunctionSeries(Math.Sin,0,20,200));
   pm.Show();
}

public static class MyExtensions
{
	public static void Show(this PlotModel model, double width = 800, double height = 500) {
		var w = new Window() { Title = "OxyPlot.Wpf.PlotView : " + model.Title, Width = width, Height = height };
		var plot = new PlotView();
		plot.Model = model;
		w.Content = plot;
		w.Show();
	}	
}

// You can also define non-static classes, enums, etc.