// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WpfRenderContext.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OxyPlot.Wpf
{
    public class WpfRenderContext : IRenderContext
    {
        private readonly Canvas canvas;

        public WpfRenderContext(Canvas canvas)
        {
            this.canvas = canvas;
            this.Width = canvas.ActualWidth;
            this.Height = canvas.ActualHeight;
        }

        public double Width { get; private set; }
        public double Height { get; private set; }

        public void DrawLine(IEnumerable<Point> points, Color stroke, double thickness, double[] dashArray, bool aliased)
        {
            var pl = new Polyline();
            if (stroke != null)
                pl.Stroke = new SolidColorBrush(stroke.ToColor());
            pl.StrokeLineJoin = PenLineJoin.Miter;
            foreach (var p in points)
                pl.Points.Add(ToPoint(p));
            pl.StrokeThickness = thickness;
            pl.Fill = null;
            if (dashArray != null)
                pl.StrokeDashArray = new DoubleCollection(dashArray);

            if (aliased)
                pl.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            canvas.Children.Add(pl);
        }

        public void DrawPolygon(IEnumerable<Point> points, Color fill, Color stroke, double thickness, double[] dashArray, bool aliased)
        {
            var po = new Polygon();
            if (stroke != null)
                po.Stroke = new SolidColorBrush(stroke.ToColor());
            if (fill != null)
                po.Fill = new SolidColorBrush(fill.ToColor());
            po.StrokeLineJoin = PenLineJoin.Miter;
            foreach (var p in points)
                po.Points.Add(ToPoint(p));
            po.StrokeThickness = thickness;
            if (dashArray != null)
                po.StrokeDashArray = new DoubleCollection(dashArray);

            if (aliased)
                po.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            canvas.Children.Add(po);
        }

        private static System.Windows.Point ToPoint(Point point)
        {
            return new System.Windows.Point(point.X, point.Y);
        }

        public void DrawText(Point p, string text, Color fill, string fontFamily, double fontSize, double fontWeight, double rotate, HorizontalTextAlign halign, VerticalTextAlign valign)
        {
            var tb = new TextBlock
                         {
                             Text = text,
                             Foreground = new SolidColorBrush(fill.ToColor())
                         };
            if (fontFamily != null)
                tb.FontFamily = new FontFamily(fontFamily);
            if (fontSize > 0)
                tb.FontSize = fontSize;
            if (fontWeight > 0)
                tb.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight((int)fontWeight);

            tb.Measure(new System.Windows.Size(1000, 1000));

            double dx = 0;
            if (halign == HorizontalTextAlign.Center)
                dx = -tb.DesiredSize.Width / 2;
            if (halign == HorizontalTextAlign.Right)
                dx = -tb.DesiredSize.Width;

            double dy = 0;
            if (valign == VerticalTextAlign.Middle)
                dy = -tb.DesiredSize.Height / 2;
            if (valign == VerticalTextAlign.Bottom)
                dy = -tb.DesiredSize.Height;

            var transform = new TransformGroup();
            transform.Children.Add(new TranslateTransform(dx, dy));
            if (rotate != 0)
                transform.Children.Add(new RotateTransform(rotate));
            transform.Children.Add(new TranslateTransform(p.X, p.Y));
            tb.RenderTransform = transform;

            tb.SetValue(RenderOptions.ClearTypeHintProperty, ClearTypeHint.Enabled);

            canvas.Children.Add(tb);
        }

        public Size MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (String.IsNullOrEmpty(text))
                return Size.Empty;

            var tb = new TextBlock
                         {
                             Text = text
                         };

            if (fontFamily != null)
                tb.FontFamily = new FontFamily(fontFamily);
            if (fontSize > 0)
                tb.FontSize = fontSize;
            if (fontWeight > 0)
                tb.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight((int)fontWeight);

            tb.Measure(new System.Windows.Size(1000, 1000));

            return new Size(tb.DesiredSize.Width, tb.DesiredSize.Height);
        }
    }
}