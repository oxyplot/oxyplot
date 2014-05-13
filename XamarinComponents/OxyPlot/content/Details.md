OxyPlot is an open source plotting library that is licensed under the MIT license, 
which permits use in proprietary software.

The main goals for the library are:

- it should be easy to use
- it should be open for extensions
- it should have high performance

See [oxyplot.org](http://oxyplot.org/) for examples and documentation (under construction).

Note that this is an open source project, please collaborate. 
Please use the [Codeplex](http://oxyplot.codeplex.com/) site to discuss, report bugs and request new features.

# Main features

- Linear axes
- Logarithmic axes
- Line series
- Scatter series
- Area series
- Bar and column series
- Heat maps
- Annotations
- Export plots to pdf, svg and png

# Code example

To define a plot, start with creating a `PlotModel`:
```csharp
var plotModel = new PlotModel { Title = "OxyPlot Demo" };

plotModel.Axes.Add (new LinearAxis { Position = AxisPosition.Bottom });
plotModel.Axes.Add (new LinearAxis { Position = AxisPosition.Left, Maximum = 10, Minimum = 0 });

var series1 = new LineSeries {
	MarkerType = OxyPlot.MarkerType.Circle,
	MarkerSize = 4,
	MarkerStroke = OxyPlot.OxyColors.White
};

series1.Points.Add (new DataPoint (0.0, 6.0));
series1.Points.Add (new DataPoint (1.4, 2.1));
series1.Points.Add (new DataPoint (2.0, 4.2));
series1.Points.Add (new DataPoint (3.3, 2.3));
series1.Points.Add (new DataPoint (4.7, 7.4));
series1.Points.Add (new DataPoint (6.0, 6.2));
series1.Points.Add (new DataPoint (8.9, 8.9));

plotModel.Series.Add (series1);
```

Then add a `PlotView` in your iOS or Android app, and assign the `Model` property of the view 
to the `PlotModel` you just created. See the getting started section for more information.
The Xamarin Component includes sample solutions for both Android and iOS. 

# Compatibility

OxyPlot is tested on
- iPhone Retina iOS 7.1
- iPad iOS 7.1
- iPad Retina iOS 7.1
- iPad Retina (64-bit) iOS 7.1
- Android