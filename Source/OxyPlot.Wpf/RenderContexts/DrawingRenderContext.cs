// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Implementation of IRenderContext to a DrawingContext
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// Implementation of IRenderContext to a DrawingContext
    /// </summary>
    public class DrawingRenderContext : IRenderContext
    {
        private readonly DrawingContext dc;

        public DrawingRenderContext(DrawingContext dc, double width, double height)
        {
            this.dc = dc;
            Width = width;
            Height = height;
        }

        public double Width { get; private set; }
        public double Height { get; private set; }

        public void DrawLineSegments(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray,
                                     LineJoin lineJoin, bool aliased)
        {
            for (int i = 0; i + 1 < points.Count; i += 2)
                DrawLine(new[] {points[i], points[i + 1]}, stroke, thickness, dashArray, lineJoin, aliased);
        }

        public void DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray,
                             LineJoin lineJoin, bool aliased)
        {
            var pen = CreatePen(stroke, thickness, dashArray, lineJoin);

            // todo: alias line
            //            if (aliased)
            //              .SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            var lp = new Point();
            int i = 0;
            foreach (var point in points)
            {
                var p = point.ToPoint();
                if (i > 0)
                {
                    dc.DrawLine(pen, lp, p);
                }
                i++;
                lp = p;
            }
        }

        private static Pen CreatePen(OxyColor stroke, double thickness, double[] dashArray,
                                     LineJoin lineJoin = LineJoin.Miter)
        {
            if (stroke == null)
                return null;
            var pen = new Pen(stroke.ToBrush(), thickness);
            if (dashArray != null)
            {
                pen.DashStyle = new DashStyle(dashArray, 0);
            }
            switch (lineJoin)
            {
                case LineJoin.Round:
                    pen.LineJoin = PenLineJoin.Round;
                    break;
                case LineJoin.Bevel:
                    pen.LineJoin = PenLineJoin.Bevel;
                    break;
                    //  The default LineJoin is Miter
            }
            return pen;
        }

        public Geometry CreateGeometry(IList<ScreenPoint> points, bool isClosed, bool isFilled = true)
        {
            var geometry = new PathGeometry();
            var figure = new PathFigure();
            figure.IsClosed = isClosed;
            figure.IsFilled = isFilled;
            bool first = true;
            foreach (var pt in points)
            {
                if (first)
                {
                    figure.StartPoint = pt.ToPoint();
                    first = false;
                }
                else
                {
                    figure.Segments.Add(new LineSegment(pt.ToPoint(), true));
                }
            }
            geometry.Figures.Add(figure);
            return geometry;
        }

        public void DrawPolygon(IList<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness,
                                double[] dashArray, LineJoin lineJoin, bool aliased)
        {
            Brush brush = null;
            if (fill != null)
                brush = fill.ToBrush();

            var pen = CreatePen(stroke, thickness, dashArray, lineJoin);
            var g = CreateGeometry(points, true);

            dc.DrawGeometry(brush, pen, g);
        }

        public void DrawPolygons(IList<IList<ScreenPoint>> polygons, OxyColor fill, OxyColor stroke, double thickness, double[] dashArray, LineJoin lineJoin, bool aliased)
        {
            foreach (var polygon in polygons)
                DrawPolygon(polygon, fill, stroke, thickness, dashArray, lineJoin, aliased);
        }

        public void DrawRectangle(double x, double y, double width, double height, OxyColor fill, OxyColor stroke,
                                  double thickness)
        {
            Brush brush = null;
            if (fill != null)
                brush = fill.ToBrush();
            var pen = CreatePen(stroke, thickness, null);
            dc.DrawRectangle(brush, pen, new Rect(x, y, width, height));
        }

        public void DrawEllipse(double x, double y, double width, double height, OxyColor fill, OxyColor stroke,
                                double thickness)
        {
            Brush brush = null;
            if (fill != null)
                brush = fill.ToBrush();

            var pen = CreatePen(stroke, thickness, null);
            var center = new Point(x + width/2, y + height/2);
            dc.DrawEllipse(brush, pen, center, width/2, height/2);
        }

        public void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize,
                             double fontWeight, double rotate, HorizontalAlignment halign, VerticalTextAlign valign)
        {
            if (text == null)
                return;

            // http://msdn.microsoft.com/en-us/library/bb613560.aspx

            // todo: create a glyphRun...
            // using FormattedText now

            var ft = CreateFormattedText(text, fontFamily, fontWeight, fontSize, fill);

            double w = ft.Width;
            double h = ft.Height;

            double dx = 0;
            if (halign == HorizontalAlignment.Center)
                dx = -w/2;
            if (halign == HorizontalAlignment.Right)
                dx = -w;

            double dy = 0;
            if (valign == VerticalTextAlign.Middle)
                dy = -h/2;
            if (valign == VerticalTextAlign.Bottom)
                dy = -h;

            var t = new TransformGroup();
            t.Children.Add(new TranslateTransform(dx, dy));
            if (rotate != 0)
                t.Children.Add(new RotateTransform(rotate));
            t.Children.Add(new TranslateTransform(p.X, p.Y));
            dc.PushTransform(t);

            // dc.DrawGlyphRun(fill.ToBrush(),glyphRun);

            dc.DrawText(ft, new Point(0, 0));

            dc.Pop();
        }

        private FormattedText CreateFormattedText(string text, string fontFamily, double fontWeight, double fontSize,
                                                  OxyColor fill)
        {
            var fw = FontWeights.Normal;
            if (fontWeight >= 1 && fontWeight <= 999)
                fw = FontWeight.FromOpenTypeWeight((int) fontWeight);

            var typeface = new Typeface(
                new FontFamily(fontFamily),
                FontStyles.Normal,
                fw,
                FontStretches.Normal);

            return new FormattedText(text, CultureInfo.CurrentCulture,
                                     FlowDirection.LeftToRight,
                                     typeface, fontSize, fill.ToBrush());
        }

        public OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (String.IsNullOrEmpty(text))
                return OxySize.Empty;

            var ft = CreateFormattedText(text, fontFamily, fontWeight, fontSize, OxyColors.Black);

            return new OxySize(ft.Width, ft.Height);
        }
    }
}