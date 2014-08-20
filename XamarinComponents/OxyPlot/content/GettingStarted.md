To show a plot, you must define a `PlotModel` that includes all the data that should be displayed.
The following example shows how to add axes and a simple line series.
See the online ['Example browser'](http://resources.oxyplot.org/examplebrowser/) on [oxyplot.org](http://oxyplot.org/) for more example models.
Note that the code defining the plots is portable, the example library is a portable class library (PCL).

```csharp
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

...

private PlotModel CreatePlotModel() {

	var plotModel = new PlotModel { Title = "OxyPlot Demo" };

	plotModel.Axes.Add (new LinearAxis { Position = AxisPosition.Bottom });
	plotModel.Axes.Add (new LinearAxis { Position = AxisPosition.Left, Maximum = 10, Minimum = 0 });

	var series1 = new LineSeries {
		MarkerType = MarkerType.Circle,
		MarkerSize = 4,
		MarkerStroke = OxyColors.White
	};

	series1.Points.Add (new DataPoint (0.0, 6.0));
	series1.Points.Add (new DataPoint (1.4, 2.1));
	series1.Points.Add (new DataPoint (2.0, 4.2));
	series1.Points.Add (new DataPoint (3.3, 2.3));
	series1.Points.Add (new DataPoint (4.7, 7.4));
	series1.Points.Add (new DataPoint (6.0, 6.2));
	series1.Points.Add (new DataPoint (8.9, 8.9));

	plotModel.Series.Add (series1);

	return plotModel;
}
```

## iOS

To show the plot in your iOS app, add a `PlotView` to the view controller class:

```csharp
using OxyPlot.XamarinIOS;
...

public override void ViewDidLoad ()
{
	...
	var plotView = new PlotView {
		Model = CreatePlotModel();
		Frame = this.View.Frame 
	};
	this.View.AddSubview (plotView);
}    

// Invalidate the plot view when the orientation of the device changes
public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
{
	base.DidRotate (fromInterfaceOrientation);
	this.plotView.InvalidatePlot (false);
}
```

## Android

To show the plot in your Android app, add a `PlotView` to the activity class:

```csharp
using OxyPlot.XamarinAndroid;
...

protected override void OnCreate (Bundle bundle)
{
	base.OnCreate (bundle);

	var plotView = new PlotView (this);
	plotView.Model = CreatePlotModel();
    
	this.AddContentView (plotView,
		new ViewGroup.LayoutParams (ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent));
}
```

## Other Resources

* [oxyplot.org](http://oxyplot.org/)
* [Documentation](http://oxyplot.org/documentation/)
* [Source](http://github.com/oxyplot/) `git clone https://github.com/oxyplot/oxyplot.git`
* [Support forum](http://discussion.oxyplot.org/)
* [Issue tracker](https://github.com/oxyplot/oxyplot/issues)
