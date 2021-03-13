// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolygonItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item in a PolygonSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an item in a <see cref="PolygonSeries" />.
    /// </summary>
    public class PolygonItem : ICodeGenerating
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonItem" /> class.
        /// </summary>
        /// <param name="outline">The outline of the polygons.</param>
        /// <param name="value">The value of the data polygon.</param>
        public PolygonItem(IEnumerable<DataPoint> outline, double value)
            : this(new[] { outline }, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonItem" /> class.
        /// </summary>
        /// <param name="outlines">The outlines of the sub-polygons.</param>
        /// <param name="value">The value of the data polygon.</param>
        public PolygonItem(IEnumerable<IEnumerable<DataPoint>> outlines, double value)
        {
            if (outlines == null)
            {
                throw new ArgumentNullException(nameof(outlines));
            }

            var outlineArrs = outlines.Select(outline => outline.ToArray().AsReadOnlyList()).ToArray().AsReadOnlyList();

            foreach (var outline in outlineArrs)
            {
                foreach (var p in outline)
                {
                    if (!p.IsDefined() || (double.IsInfinity(p.X) || double.IsInfinity(p.Y)))
                    {
                        throw new ArgumentException("Outline contains non-finite elements.");
                    }
                }
            }

            if (outlineArrs.Count < 1)
            {
                throw new ArgumentException($"Outline does not contain any sub-polygons.");
            }

            this.Outlines = outlineArrs;
            this.Value = value;
            this.Bounds = outlineArrs.Select(outline => ComputeBounds(outline)).ToArray().AsReadOnlyList();
        }

        /// <summary>
        /// Gets the outlines of each sub-polygon in the item.
        /// </summary>
        /// <value>The outlines of each sub-polygon in the item..</value>
        public IReadOnlyList<IReadOnlyList<DataPoint>> Outlines { get; }

        /// <summary>
        /// Gets the value of the item.
        /// </summary>
        /// <value>The value can be used to color-code the Polygon.</value>
        public double Value { get; }

        /// <summary>
        /// Gets the bounds of the item.
        /// </summary>
        public IReadOnlyList<OxyRect> Bounds { get; }

        ///// <summary>
        ///// Determines whether the specified point lies within the boundary of the Polygon.
        ///// </summary>
        ///// <returns><c>true</c> if the value of the <param name="p"/> parameter is inside the bounds of this instance.</returns>
        //public bool Contains(DataPoint p)
        //{
        //    if (!this.Bounds.Contains(p.X, p.Y))
        //        return false;

        //    int intersectionCount = 0;

        //    DataPoint cur = outline[0];
        //    DataPoint prev;
        //    for (int i = 1; i < outline.Length; i++)
        //    {
        //        prev = cur;
        //        cur = outline[i];

        //        if (cur.Equals(prev))
        //        {
        //            continue;
        //        }

        //        var y0 = Math.Min(cur.Y, prev.Y);
        //        var y1 = Math.Max(cur.Y, prev.Y);

        //        if (y0 > p.Y || y1 <= p.Y)
        //            continue;

        //        var c = (p.Y - prev.Y) / (cur.Y - prev.Y);
        //        var x = prev.Y + c * (cur.X - prev.Y);

        //        if (x <= p.X)
        //            intersectionCount++;
        //    }

        //    return intersectionCount % 2 == 0;
        //}

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>The to code.</returns>
        public string ToCode()
        {
            // TODO: teach FormatConstructor to translate arrays
            var outlineCode = string.Format("new DataPoint[][] {0}", string.Join(",", this.Outlines.Select(outline => string.Format("new DataPoint[] {0}", outline.Select(p => p.ToCode())))));
            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{2}", outlineCode, this.Value);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var outlineString = string.Join("; ", this.Outlines.Select(outline => string.Join(", ", outline)));
            return $"{outlineString} {this.Value}";
        }

        /// <summary>
        /// Computes the bounds of the polygon in data space.
        /// </summary>
        /// <returns></returns>
        private static OxyRect ComputeBounds(IReadOnlyList<DataPoint> outline)
        {
            var minx = outline[0].X;
            var miny = outline[0].Y;
            var maxx = outline[0].X;
            var maxy = outline[0].Y;

            for (int i = 1; i < outline.Count; i++)
            {
                minx = Math.Min(minx, outline[i].X);
                miny = Math.Min(miny, outline[i].Y);
                maxx = Math.Max(maxx, outline[i].X);
                maxy = Math.Max(maxy, outline[i].Y);
            }

            return new OxyRect(minx, miny, maxx - minx, maxy - miny);
        }
    }
}
