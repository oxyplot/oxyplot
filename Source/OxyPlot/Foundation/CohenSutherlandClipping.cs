// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CohenSutherlandClipping.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Line clipping algorithm.
    /// </summary>
    public class CohenSutherlandClipping
    {
        // http://en.wikipedia.org/wiki/Cohen%E2%80%93Sutherland
        #region Constants and Fields

        /// <summary>
        ///   The bottom.
        /// </summary>
        private const int BOTTOM = 4; // 0100

        /// <summary>
        ///   The inside.
        /// </summary>
        private const int INSIDE = 0; // 0000

        /// <summary>
        ///   The left.
        /// </summary>
        private const int LEFT = 1; // 0001

        /// <summary>
        ///   The right.
        /// </summary>
        private const int RIGHT = 2; // 0010

        /// <summary>
        ///   The top.
        /// </summary>
        private const int TOP = 8; // 1000

        /// <summary>
        ///   The xmax.
        /// </summary>
        private readonly double xmax;

        /// <summary>
        ///   The xmin.
        /// </summary>
        private readonly double xmin;

        /// <summary>
        ///   The ymax.
        /// </summary>
        private readonly double ymax;

        /// <summary>
        ///   The ymin.
        /// </summary>
        private readonly double ymin;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CohenSutherlandClipping"/> class.
        /// </summary>
        /// <param name="rect">
        /// The rect.
        /// </param>
        public CohenSutherlandClipping(OxyRect rect)
        {
            this.xmin = rect.Left;
            this.xmax = rect.Right;
            this.ymin = rect.Top;
            this.ymax = rect.Bottom;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CohenSutherlandClipping"/> class.
        /// </summary>
        /// <param name="xmin">
        /// The xmin.
        /// </param>
        /// <param name="xmax">
        /// The xmax.
        /// </param>
        /// <param name="ymin">
        /// The ymin.
        /// </param>
        /// <param name="ymax">
        /// The ymax.
        /// </param>
        public CohenSutherlandClipping(double xmin, double xmax, double ymin, double ymax)
        {
            this.xmin = xmin;
            this.ymin = ymin;
            this.xmax = xmax;
            this.ymax = ymax;
        }

        #endregion

        // Compute the bit code for a point (x, y) using the clip rectangle
        // bounded diagonally by (xmin, ymin), and (xmax, ymax)
        #region Public Methods

        /// <summary>
        /// Cohen–Sutherland clipping algorithm clips a line from
        ///   P0 = (x0, y0) to P1 = (x1, y1) against a rectangle with 
        ///   diagonal from (xmin, ymin) to (xmax, ymax).
        /// </summary>
        /// <param name="x0">
        /// </param>
        /// <param name="y0">
        /// </param>
        /// <param name="x1">
        /// </param>
        /// <param name="y1">
        /// </param>
        /// <returns>
        /// true if the line is inside
        /// </returns>
        public bool ClipLine(ref double x0, ref double y0, ref double x1, ref double y1)
        {
            // compute outcodes for P0, P1, and whatever point lies outside the clip rectangle
            int outcode0 = this.ComputeOutCode(x0, y0);
            int outcode1 = this.ComputeOutCode(x1, y1);
            bool accept = false;

            while (true)
            {
                if ((outcode0 | outcode1) == 0)
                {
                    // logical or is 0. Trivially accept and get out of loop
                    accept = true;
                    break;
                }

                if ((outcode0 & outcode1) != 0)
                {
                    // logical and is not 0. Trivially reject and get out of loop
                    break;
                }

                // failed both tests, so calculate the line segment to clip
                // from an outside point to an intersection with clip edge
                double x = 0, y = 0;

                // At least one endpoint is outside the clip rectangle; pick it.
                int outcodeOut = outcode0 != 0 ? outcode0 : outcode1;

                // Now find the intersection point;
                // use formulas y = y0 + slope * (x - x0), x = x0 + (1 / slope) * (y - y0)
                if ((outcodeOut & TOP) != 0)
                {
                    // point is above the clip rectangle
                    x = x0 + (x1 - x0) * (this.ymax - y0) / (y1 - y0);
                    y = this.ymax;
                }
                else if ((outcodeOut & BOTTOM) != 0)
                {
                    // point is below the clip rectangle
                    x = x0 + (x1 - x0) * (this.ymin - y0) / (y1 - y0);
                    y = this.ymin;
                }
                else if ((outcodeOut & RIGHT) != 0)
                {
                    // point is to the right of clip rectangle
                    y = y0 + (y1 - y0) * (this.xmax - x0) / (x1 - x0);
                    x = this.xmax;
                }
                else if ((outcodeOut & LEFT) != 0)
                {
                    // point is to the left of clip rectangle
                    y = y0 + (y1 - y0) * (this.xmin - x0) / (x1 - x0);
                    x = this.xmin;
                }

                // Now we move outside point to intersection point to clip
                // and get ready for next pass.
                if (outcodeOut == outcode0)
                {
                    x0 = x;
                    y0 = y;
                    outcode0 = this.ComputeOutCode(x0, y0);
                }
                else
                {
                    x1 = x;
                    y1 = y;
                    outcode1 = this.ComputeOutCode(x1, y1);
                }
            }

            return accept;
        }

        /// <summary>
        /// Cohen–Sutherland clipping algorithm clips a line from
        ///   P0 = (x0, y0) to P1 = (x1, y1) against a rectangle with 
        ///   diagonal from (xmin, ymin) to (xmax, ymax).
        /// </summary>
        /// <param name="s0">
        /// The s 0.
        /// </param>
        /// <param name="s1">
        /// The s 1.
        /// </param>
        /// <returns>
        /// true if the line is inside
        /// </returns>
        public bool ClipLine(ref ScreenPoint s0, ref ScreenPoint s1)
        {
            return this.ClipLine(ref s0.x, ref s0.y, ref s1.x, ref s1.y);
        }

        /// <summary>
        /// The is inside.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The is inside.
        /// </returns>
        public bool IsInside(double x, double y)
        {
            return this.ComputeOutCode(x, y) == INSIDE;
        }

        /// <summary>
        /// The is inside.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The is inside.
        /// </returns>
        public bool IsInside(ScreenPoint s)
        {
            return this.ComputeOutCode(s.X, s.Y) == INSIDE;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The compute out code.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The compute out code.
        /// </returns>
        private int ComputeOutCode(double x, double y)
        {
            int code = INSIDE; // initialized as being inside of clip window

            if (x < this.xmin)
            {
                // to the left of clip window
                code |= LEFT;
            }
            else if (x > this.xmax)
            {
                // to the right of clip window
                code |= RIGHT;
            }

            if (y < this.ymin)
            {
                // below the clip window
                code |= BOTTOM;
            }
            else if (y > this.ymax)
            {
                // above the clip window
                code |= TOP;
            }

            return code;
        }

        #endregion
    }
}