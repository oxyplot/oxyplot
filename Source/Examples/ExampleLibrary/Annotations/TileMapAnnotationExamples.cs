// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TileMapAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;

    [Examples("TileMapAnnotation"), Tags("Annotations")]
    public static class TileMapAnnotationExamples
    {
        [Example("TileMapAnnotation (openstreetmap.org)")]
        public static PlotModel TileMapAnnotation2()
        {
            var model = new PlotModel { Title = "TileMapAnnotation" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 10.4, Maximum = 10.6, Title = "Longitude" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 59.88, Maximum = 59.96, Title = "Latitude" });

            // Add the tile map annotation
            model.Annotations.Add(
                new TileMapAnnotation
                    {
                        Url = "http://tile.openstreetmap.org/{Z}/{X}/{Y}.png",
                        CopyrightNotice = "OpenStreetMap"
                    });

            return model;
        }

        [Example("TileMapAnnotation (statkart.no)")]
        public static PlotModel TileMapAnnotation()
        {
            var model = new PlotModel { Title = "TileMapAnnotation" };

            // TODO: scale ratio between the two axes should be fixed (or depending on latitude...)
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 10.4, Maximum = 10.6, Title = "Longitude" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 59.88, Maximum = 59.96, Title = "Latitude" });

            // Add the tile map annotation
            model.Annotations.Add(
                new TileMapAnnotation
                    {
                        Url = "http://opencache.statkart.no/gatekeeper/gk/gk.open_gmaps?layers=toporaster3&zoom={Z}&x={X}&y={Y}",
                        CopyrightNotice = "Kartgrunnlag: Statens kartverk, Geovekst og kommuner.",
                        MinZoomLevel = 5,
                        MaxZoomLevel = 19
                    });

            model.Annotations.Add(new ArrowAnnotation
                                      {
                                          EndPoint = new DataPoint(10.563, 59.888),
                                          ArrowDirection = new ScreenVector(-40, -60),
                                          StrokeThickness = 3,
                                          FontSize = 20,
                                          FontWeight = FontWeights.Bold,
                                          TextColor = OxyColor.FromAColor(160, OxyColors.Magenta),
                                          Color = OxyColor.FromAColor(100, OxyColors.Magenta)
                                      });

            return model;
        }
    }
}