OxyPlot is a cross-platform plotting library for .NET

- [Web page](http://oxyplot.org)  
- [Documentation](http://docs.oxyplot.org/)
- [Announcements](http://oxyplot.org/announcements) / [atom](http://oxyplot.org/atom.xml)
- [Discussion forum](http://discussion.oxyplot.org)
- [Source repository](http://github.com/oxyplot/oxyplot)
- [Issue tracker](http://github.com/oxyplot/oxyplot/issues)
- [NuGet packages](http://www.nuget.org/packages?q=oxyplot)
- [Stack Overflow](http://stackoverflow.com/questions/tagged/oxyplot)
- [Twitter](https://twitter.com/hashtag/oxyplot)
- [Gitter](https://gitter.im/oxyplot/oxyplot) (chat)

![License](https://img.shields.io/badge/license-MIT-red.svg)
[![Build status](https://img.shields.io/appveyor/ci/objorke/oxyplot/develop.svg)](https://ci.appveyor.com/project/objorke/oxyplot)

![Plot](http://oxyplot.org/public/images/normal-distributions.png)

#### Getting started

1. Use the NuGet package manager to add a reference to OxyPlot (see details below if you want to use pre-release packages)
2. Add a `PlotView` to your user interface
3. Create a `PlotModel` in your code
4. Bind the `PlotModel` to the `Model` property of your `PlotView`

#### Examples

You can find examples in the `/Source/Examples` folder in the code repository.

#### NuGet packages

The latest pre-release packages are pushed by AppVeyor CI to [myget.org](https://www.myget.org/)
To install these packages, set the myget.org package source `https://www.myget.org/F/oxyplot` and remember the "-pre" flag. 

The stable release packages will be pushed to [nuget.org](https://www.nuget.org/packages?q=oxyplot).
Note that we have currently have a lot of old (v2015.*) and pre-release packages on this feed, this will be cleaned up as soon as we release [v1.0](https://github.com/oxyplot/oxyplot/milestones/v1.0).

Package | Targets | Dependencies
--------|---------|-------------
OxyPlot.Core | .NET Standard 1.0 |
OxyPlot.Wpf | .NET 4.5.2 |
OxyPlot.WindowsForms | .NET 4.5.2 |
OxyPlot.Windows | Universal Windows 10.0 |
OxyPlot.GtkSharp | .NET 4.5.2 | GTK# 2
OxyPlot.GtkSharp3 | .NET 4.5.2 | GTK# 3
OxyPlot.Xamarin.Android | MonoAndroid |
OxyPlot.Xamarin.iOS | MonoTouch and iOS10 |
OxyPlot.Xamarin.Mac | Mac20 |
OxyPlot.Xamarin.Forms | MonoTouch, iOS10, MonoAndroid, WP8 |
OxyPlot.Xwt | .NET 4.5.2 |
OxyPlot.SharpDX.Wpf | .NET 4.5.2 |
OxyPlot.Avalonia | .NET 4.5.2 |
OxyPlot.OpenXML | .NET 4.5.2 |
OxyPlot.Pdf | .NET 4.5.2 | PdfSharp
OxyPlot.Contrib | .NET Standard 1.0 | 
OxyPlot.ExampleLibrary | .NET Standard 1.0 |

#### Contribute

See [Contributing](.github/CONTRIBUTING.md) for information about how to contribute!
