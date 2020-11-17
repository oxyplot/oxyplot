// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransposableAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    /// <summary>
    /// Provides an abstract base class for transposable annotations.
    /// </summary>
    public abstract class TransposableAnnotation : Annotation, ITransposablePlotElement
    {
        /// <inheritdoc/>
        public override OxyRect GetClippingRect()
        {
            var rect = this.PlotModel.PlotArea;
            var axisRect = PlotElementUtilities.GetClippingRect(this);

            var minX = 0d;
            var maxX = double.PositiveInfinity;
            var minY = 0d;
            var maxY = double.PositiveInfinity;

            if (this.ClipByXAxis)
            {
                minX = this.Orientate(axisRect.TopLeft).X;
                maxX = this.Orientate(axisRect.BottomRight).X;
            }

            if (this.ClipByYAxis)
            {
                minY = this.Orientate(axisRect.TopLeft).Y;
                maxY = this.Orientate(axisRect.BottomRight).Y;
            }

            var minPoint = this.Orientate(new ScreenPoint(minX, minY));
            var maxPoint = this.Orientate(new ScreenPoint(maxX, maxY));

            var axisClipRect = new OxyRect(minPoint, maxPoint);
            return rect.Clip(axisClipRect);
        }

        /// <inheritdoc/>
        public override ScreenPoint Transform(DataPoint p)
        {
            return PlotElementUtilities.TransformOrientated(this, p);
        }

        /// <inheritdoc/>
        public override DataPoint InverseTransform(ScreenPoint p)
        {
            return PlotElementUtilities.InverseTransformOrientated(this, p);
        }
    }
}
