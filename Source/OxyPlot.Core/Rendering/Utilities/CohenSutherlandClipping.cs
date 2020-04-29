// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CohenSutherlandClipping.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a line clipping algorithm.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides a line clipping algorithm.
    /// </summary>
    /// <remarks>See http://en.wikipedia.org/wiki/Cohen%E2%80%93Sutherland</remarks>
    public class CohenSutherlandClipping
    {
        /// <summary>
        /// The bottom code.
        /// </summary>
        private const int Bottom = 4; // 0100

        /// <summary>
        /// The inside code.
        /// </summary>
        private const int Inside = 0; // 0000

        /// <summary>
        /// The left code.
        /// </summary>
        private const int Left = 1; // 0001

        /// <summary>
        /// The right code.
        /// </summary>
        private const int Right = 2; // 0010

        /// <summary>
        /// The top code.
        /// </summary>
        private const int Top = 8; // 1000

        /// <summary>
        /// The x maximum.
        /// </summary>
        private readonly double xmax;

        /// <summary>
        /// The x minimum.
        /// </summary>
        private readonly double xmin;

        /// <summary>
        /// The y maximum.
        /// </summary>
        private readonly double ymax;

        /// <summary>
        /// The y minimum.
        /// </summary>
        private readonly double ymin;

        /// <summary>
        /// Initializes a new instance of the <see cref="CohenSutherlandClipping" /> class.
        /// </summary>
        /// <param name="rect">The clipping rectangle.</param>
        public CohenSutherlandClipping(OxyRect rect)
        {
            this.xmin = rect.Left;
            this.xmax = rect.Right;
            this.ymin = rect.Top;
            this.ymax = rect.Bottom;
        }

        /// <summary>
        /// Cohen–Sutherland clipping algorithm clips a line from
        /// P0 = (x0, y0) to P1 = (x1, y1) against a rectangle with
        /// diagonal from <code>(xmin, ymin)</code> to <code>(xmax, ymax)</code>.
        /// </summary>
        /// <param name="p0">The point P0.</param>
        /// <param name="p1">The point P1.</param>
        /// <returns><c>true</c> if the line is inside</returns>
        public bool ClipLine(ref ScreenPoint p0, ref ScreenPoint p1)
        {
            // compute out codes for P0, P1, and whatever point lies outside the clip rectangle

            // the following method is inlined manually
            // int outcode0 = this.ComputeOutCode(p0.x, p0.y);           
            int outcode0 = Inside; // initialized as being inside of clip window

            if (p0.x < this.xmin)
            {
                // to the left of clip window
                outcode0 |= Left;
            }
            else if (p0.x > this.xmax)
            {
                // to the right of clip window
                outcode0 |= Right;
            }

            if (p0.y < this.ymin)
            {
                // below the clip window
                outcode0 |= Bottom;
            }
            else if (p0.y > this.ymax)
            {
                // above the clip window
                outcode0 |= Top;
            }

            // the following method is inlined manually
            // int outcode1 = this.ComputeOutCode(p1.x, p1.y);
            int outcode1 = Inside; // initialized as being inside of clip window

            if (p1.x < this.xmin)
            {
                // to the left of clip window
                outcode1 |= Left;
            }
            else if (p1.x > this.xmax)
            {
                // to the right of clip window
                outcode1 |= Right;
            }

            if (p1.y < this.ymin)
            {
                // below the clip window
                outcode1 |= Bottom;
            }
            else if (p1.y > this.ymax)
            {
                // above the clip window
                outcode1 |= Top;
            }

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
                if ((outcodeOut & Top) != 0)
                {
                    // point is above the clip rectangle
                    x = p0.x + ((p1.x - p0.x) * (this.ymax - p0.y) / (p1.y - p0.y));
                    y = this.ymax;
                }
                else if ((outcodeOut & Bottom) != 0)
                {
                    // point is below the clip rectangle
                    x = p0.x + ((p1.x - p0.x) * (this.ymin - p0.y) / (p1.y - p0.y));
                    y = this.ymin;
                }
                else if ((outcodeOut & Right) != 0)
                {
                    // point is to the right of clip rectangle
                    y = p0.y + ((p1.y - p0.y) * (this.xmax - p0.x) / (p1.x - p0.x));
                    x = this.xmax;
                }
                else if ((outcodeOut & Left) != 0)
                {
                    // point is to the left of clip rectangle
                    y = p0.y + ((p1.y - p0.y) * (this.xmin - p0.x) / (p1.x - p0.x));
                    x = this.xmin;
                }

                // Now we move outside point to intersection point to clip
                // and get ready for next pass.
                if (outcodeOut == outcode0)
                {
                    p0.x = x;
                    p0.y = y;

                    // the following code is inlined
                    // outcode0 = this.ComputeOutCode(p0.x, p0.y);
                    outcode0 = Inside; // initialized as being inside of clip window

                    if (p0.x < this.xmin)
                    {
                        // to the left of clip window
                        outcode0 |= Left;
                    }
                    else if (p0.x > this.xmax)
                    {
                        // to the right of clip window
                        outcode0 |= Right;
                    }

                    if (p0.y < this.ymin)
                    {
                        // below the clip window
                        outcode0 |= Bottom;
                    }
                    else if (p0.y > this.ymax)
                    {
                        // above the clip window
                        outcode0 |= Top;
                    }
                }
                else
                {
                    p1.x = x;
                    p1.y = y;

                    // the following method is inlined manually
                    // outcode1 = this.ComputeOutCode(p1.x, p1.y);
                    outcode1 = Inside; // initialized as being inside of clip window

                    if (p1.x < this.xmin)
                    {
                        // to the left of clip window
                        outcode1 |= Left;
                    }
                    else if (p1.x > this.xmax)
                    {
                        // to the right of clip window
                        outcode1 |= Right;
                    }

                    if (p1.y < this.ymin)
                    {
                        // below the clip window
                        outcode1 |= Bottom;
                    }
                    else if (p1.y > this.ymax)
                    {
                        // above the clip window
                        outcode1 |= Top;
                    }
                }
            }

            return accept;
        }

        /// <summary>
        /// Determines whether the specified point is inside the rectangle.
        /// </summary>
        /// <param name="s">The point.</param>
        /// <returns><c>true</c> if the specified point is inside; otherwise, <c>false</c>.</returns>
        public bool IsInside(ScreenPoint s)
        {
            if (s.x < this.xmin)
            {
                return false;
            }

            if (s.x > this.xmax)
            {
                return false;
            }

            if (s.y < this.ymin)
            {
                return false;
            }

            if (s.y > this.ymax)
            {
                return false;
            }

            return true;
        }
    }
}