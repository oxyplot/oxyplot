// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RangeAxis.cs" company="OxyPlot">
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
using System.Windows;

namespace OxyPlot.Wpf
{
    public class RangeAxis : Axis
    {
        public double StartPosition
        {
            get { return (double)GetValue(StartPositionProperty); }
            set { SetValue(StartPositionProperty, value); }
        }

        public static readonly DependencyProperty StartPositionProperty =
            DependencyProperty.Register("StartPosition", typeof(double), typeof(RangeAxis), new UIPropertyMetadata(0.0));

        public double EndPosition
        {
            get { return (double)GetValue(EndPositionProperty); }
            set { SetValue(EndPositionProperty, value); }
        }

        public static readonly DependencyProperty EndPositionProperty =
            DependencyProperty.Register("EndPosition", typeof(double), typeof(RangeAxis), new UIPropertyMetadata(1.0));

        public bool PositionAtZeroCrossing
        {
            get { return (bool)GetValue(PositionAtZeroCrossingProperty); }
            set { SetValue(PositionAtZeroCrossingProperty, value); }
        }

        public static readonly DependencyProperty PositionAtZeroCrossingProperty =
            DependencyProperty.Register("PositionAtZeroCrossing", typeof(bool), typeof(RangeAxis), new UIPropertyMetadata(false));

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(RangeAxis), new UIPropertyMetadata(0.0));

        public TickStyle TickStyle
        {
            get { return (TickStyle)GetValue(TickStyleProperty); }
            set { SetValue(TickStyleProperty, value); }
        }

        public static readonly DependencyProperty TickStyleProperty =
            DependencyProperty.Register("TickStyle", typeof(TickStyle), typeof(RangeAxis), new UIPropertyMetadata(TickStyle.Inside));

        public LineStyle MajorGridlineStyle
        {
            get { return (LineStyle)GetValue(MajorGridlineStyleProperty); }
            set { SetValue(MajorGridlineStyleProperty, value); }
        }

        public static readonly DependencyProperty MajorGridlineStyleProperty =
            DependencyProperty.Register("MajorGridlineStyle", typeof(LineStyle), typeof(RangeAxis), new UIPropertyMetadata(LineStyle.Solid));

        public double MajorGridlineThickness
        {
            get { return (double)GetValue(MajorGridlineThicknessProperty); }
            set { SetValue(MajorGridlineThicknessProperty, value); }
        }

        public static readonly DependencyProperty MajorGridlineThicknessProperty =
            DependencyProperty.Register("MajorGridlineThickness", typeof(double), typeof(RangeAxis), new UIPropertyMetadata(1.0));

        public LineStyle MinorGridlineStyle
        {
            get { return (LineStyle)GetValue(MinorGridlineStyleProperty); }
            set { SetValue(MinorGridlineStyleProperty, value); }
        }

        public static readonly DependencyProperty MinorGridlineStyleProperty =
            DependencyProperty.Register("MinorGridlineStyle", typeof(LineStyle), typeof(RangeAxis), new UIPropertyMetadata(LineStyle.Solid));

        public double MinorGridlineThickness
        {
            get { return (double)GetValue(MinorGridlineThicknessProperty); }
            set { SetValue(MinorGridlineThicknessProperty, value); }
        }

        public static readonly DependencyProperty MinorGridlineThicknessProperty =
            DependencyProperty.Register("MinorGridlineThickness", typeof(double), typeof(RangeAxis), new UIPropertyMetadata(1.0));

        public AxisPosition Position
        {
            get { return (AxisPosition)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(AxisPosition), typeof(RangeAxis), new UIPropertyMetadata(AxisPosition.Left));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(Axis), new UIPropertyMetadata(double.NaN));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(Axis), new UIPropertyMetadata(double.NaN));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Axis), new UIPropertyMetadata(null));

        public RangeAxis()
        {
            ModelAxis = new OxyPlot.Axis();
        }

        public override void UpdateModelProperties()
        {
            var a = ModelAxis as OxyPlot.Axis;
            a.Minimum = Minimum;
            a.Maximum = Maximum;
            a.Title = Title;
            a.Position = Position;
            a.PositionAtZeroCrossing = PositionAtZeroCrossing;
            a.TickStyle = TickStyle;
            a.MajorGridlineStyle = MajorGridlineStyle;
            a.MinorGridlineStyle = MinorGridlineStyle;
            //TicklineColor = TicklineColor;
            //MajorGridlineColor = MajorGridlineColor;
            //TicklineColor = TicklineColor;
            //MinorGridlineColor = MinorGridlineColor;
            a.MajorGridlineThickness = MajorGridlineThickness;
            a.MinorGridlineThickness = MinorGridlineThickness;

            //a.ExtraGridlineStyle = ExtraGridlineStyle;
            //a.ExtraGridlineColor = ExtraGridlineColor;
            //a.ExtraGridlineThickness = ExtraGridlineThickness;

            //a.ShowMinorTicks = ShowMinorTicks;

            //a.FontFamily = FontFamily;
            //a.FontSize = FontSize;

            //a.MinorTickSize = MinorTickSize;
            //a.MajorTickSize = MajorTickSize;

            a.StartPosition = StartPosition;
            a.EndPosition = EndPosition;

            a.Angle = Angle;
        }
    }
}