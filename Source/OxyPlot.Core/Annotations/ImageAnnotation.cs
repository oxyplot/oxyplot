// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an annotation that shows an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    using System;

    /// <summary>
    /// Represents an annotation that shows an image.
    /// </summary>
    public class ImageAnnotation : Annotation
    {
        /// <summary>
        /// The actual bounds of the rendered image.
        /// </summary>
        private OxyRect actualBounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageAnnotation" /> class.
        /// </summary>
        public ImageAnnotation()
        {
            this.X = new PlotLength(0.5, PlotLengthUnit.RelativeToPlotArea);
            this.Y = new PlotLength(0.5, PlotLengthUnit.RelativeToPlotArea);
            this.OffsetX = new PlotLength(0, PlotLengthUnit.ScreenUnits);
            this.OffsetY = new PlotLength(0, PlotLengthUnit.ScreenUnits);
            this.Width = new PlotLength(double.NaN, PlotLengthUnit.ScreenUnits);
            this.Height = new PlotLength(double.NaN, PlotLengthUnit.ScreenUnits);
            this.Opacity = 1.0;
            this.Interpolate = true;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Middle;
        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>The image source.</value>
        public OxyImage ImageSource { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment.
        /// </summary>
        /// <value>The horizontal alignment.</value>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the X position of the image.
        /// </summary>
        /// <value>The X.</value>
        public PlotLength X { get; set; }

        /// <summary>
        /// Gets or sets the Y position of the image.
        /// </summary>
        /// <value>The Y.</value>
        public PlotLength Y { get; set; }

        /// <summary>
        /// Gets or sets the X offset.
        /// </summary>
        /// <value>The offset X.</value>
        public PlotLength OffsetX { get; set; }

        /// <summary>
        /// Gets or sets the Y offset.
        /// </summary>
        /// <value>The offset Y.</value>
        public PlotLength OffsetY { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public PlotLength Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public PlotLength Height { get; set; }

        /// <summary>
        /// Gets or sets the opacity (0-1).
        /// </summary>
        /// <value>The opacity value.</value>
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to apply smooth interpolation to the image.
        /// </summary>
        /// <value><c>true</c> if the image should be interpolated (using a high-quality bi-cubic interpolation); <c>false</c> if the nearest neighbor should be used.</value>
        public bool Interpolate { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment.
        /// </summary>
        /// <value>The vertical alignment.</value>
        public VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// Renders the image annotation.
        /// </summary>
        /// <param name="rc">The render context.</param>
        public override void Render(IRenderContext rc)
        {
            base.Render(rc);

            var p = this.GetPoint(this.X, this.Y, this.PlotModel);
            var o = this.GetVector(this.OffsetX, this.OffsetY, this.PlotModel);
            var position = p + o;

            var clippingRectangle = this.GetClippingRect();

            var s = this.GetVector(this.Width, this.Height, this.PlotModel);

            var width = s.X;
            var height = s.Y;

            if (double.IsNaN(width) && double.IsNaN(height))
            {
                width = this.ImageSource.Width;
                height = this.ImageSource.Height;
            }

            if (double.IsNaN(width))
            {
                width = height / this.ImageSource.Height * this.ImageSource.Width;
            }

            if (double.IsNaN(height))
            {
                height = width / this.ImageSource.Width * this.ImageSource.Height;
            }

            width = Math.Abs(width);
            height = Math.Abs(height);

            var x = position.X;
            var y = position.Y;

            var ha = this.HorizontalAlignment;
            var va = this.VerticalAlignment;

            if (ha == HorizontalAlignment.Center)
            {
                x -= width * 0.5;
            }
            else if (ha == HorizontalAlignment.Right)
            {
                x -= width;
            }

            if (va == VerticalAlignment.Middle)
            {
                y -= height * 0.5;
            }
            else if (va == VerticalAlignment.Bottom)
            {
                y -= height;
            }

            this.actualBounds = new OxyRect(x, y, width, height);

            if (this.X.Unit == PlotLengthUnit.Data || this.Y.Unit == PlotLengthUnit.Data)
            {
                rc.DrawClippedImage(clippingRectangle, this.ImageSource, x, y, width, height, this.Opacity, this.Interpolate);
            }
            else
            {
                rc.DrawImage(this.ImageSource, x, y, width, height, this.Opacity, this.Interpolate);
            }
        }

        /// <summary>
        /// When overridden in a derived class, tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// The result of the hit test.
        /// </returns>
        protected override HitTestResult HitTestOverride(HitTestArguments args)
        {
            if (this.actualBounds.Contains(args.Point))
            {
                return new HitTestResult(this, args.Point);
            }

            return null;
        }

        /// <summary>
        /// Gets the point.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="model">The model.</param>
        /// <returns>The point in screen coordinates.</returns>
        protected ScreenPoint GetPoint(PlotLength x, PlotLength y, PlotModel model)
        {
            var xd = double.NaN;
            var yd = double.NaN;

            if (x.Unit == PlotLengthUnit.Data || y.Unit == PlotLengthUnit.Data)
            {
                var dataX = x.Unit == PlotLengthUnit.Data ? x.Value : double.NaN;
                var dataY = y.Unit == PlotLengthUnit.Data ? y.Value : double.NaN;
                var p = this.Transform(dataX, dataY);
                xd = p.X;
                yd = p.Y;
            }

            switch (x.Unit)
            {
                case PlotLengthUnit.Data:
                    break;
                case PlotLengthUnit.ScreenUnits:
                    xd = x.Value;
                    break;
                case PlotLengthUnit.RelativeToViewport:
                    xd = model.Width * x.Value;
                    break;
                case PlotLengthUnit.RelativeToPlotArea:
                    xd = model.PlotArea.Left + (model.PlotArea.Width * x.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (y.Unit)
            {
                case PlotLengthUnit.Data:
                    break;
                case PlotLengthUnit.ScreenUnits:
                    yd = y.Value;
                    break;
                case PlotLengthUnit.RelativeToViewport:
                    yd = model.Height * y.Value;
                    break;
                case PlotLengthUnit.RelativeToPlotArea:
                    yd = model.PlotArea.Top + (model.PlotArea.Height * y.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new ScreenPoint(xd, yd);
        }

        /// <summary>
        /// Gets the vector.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="model">The model.</param>
        /// <returns>The vector in screen coordinates.</returns>
        protected ScreenVector GetVector(PlotLength x, PlotLength y, PlotModel model)
        {
            var xd = double.NaN;
            var yd = double.NaN;

            if (x.Unit == PlotLengthUnit.Data || y.Unit == PlotLengthUnit.Data)
            {
                var dataX = x.Unit == PlotLengthUnit.Data ? x.Value : double.NaN;
                var dataY = y.Unit == PlotLengthUnit.Data ? y.Value : double.NaN;
                var v = this.Transform(dataX, dataY) - this.Transform(0, 0);
                xd = v.X;
                yd = v.Y;
            }

            switch (x.Unit)
            {
                case PlotLengthUnit.Data:
                    break;
                case PlotLengthUnit.ScreenUnits:
                    xd = x.Value;
                    break;
                case PlotLengthUnit.RelativeToViewport:
                    xd = model.Width * x.Value;
                    break;
                case PlotLengthUnit.RelativeToPlotArea:
                    xd = model.PlotArea.Width * x.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (y.Unit)
            {
                case PlotLengthUnit.Data:
                    break;
                case PlotLengthUnit.ScreenUnits:
                    yd = y.Value;
                    break;
                case PlotLengthUnit.RelativeToViewport:
                    yd = model.Height * y.Value;
                    break;
                case PlotLengthUnit.RelativeToPlotArea:
                    yd = model.PlotArea.Height * y.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new ScreenVector(xd, yd);
        }
    }
}
