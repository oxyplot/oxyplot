
#if OXYPLOT_COREDRAWING
namespace OxyPlot.Core.Drawing
#else
namespace OxyPlot.WindowsForms
#endif
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Describes a GDI+ Pen.
    /// </summary>
    public class GraphicsPenDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPenDescription" /> class.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        public GraphicsPenDescription(OxyColor color, double thickness, double[] dashArray = null, OxyPlot.LineJoin lineJoin = OxyPlot.LineJoin.Miter)
        {
            Color = color;
            Thickness = thickness;
            DashArray = dashArray;
            LineJoin = lineJoin;

            cachedHashCode = ComputeHashCode();
        }
        
        /// <summary>
        /// Gets the color of the pen.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color { get; }
        
        /// <summary>
        /// Gets the line thickness.
        /// </summary>
        /// <value>The line thickness.</value>
        public double Thickness { get; }
        
        /// <summary>
        /// Gets the dash array.
        /// </summary>
        /// <value>The dash array.</value>
        public double[] DashArray { get; }
        
        /// <summary>
        /// Gets the line join type.
        /// </summary>
        /// <value>The line join type.</value>
        public LineJoin LineJoin { get; }

        /// <summary>
        /// The HashCode of the <see cref="GraphicsPenDescription" /> instance, as computed in the constructor.
        /// </summary>
        private readonly int cachedHashCode;
        
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c> .</returns>
        public override bool Equals(object obj)
        {
            var description = obj as GraphicsPenDescription;

            return description != null &&
                   Color.Equals(description.Color) &&
                   Thickness == description.Thickness &&
                   DashArraysEquals(DashArray, description.DashArray) &&
                   LineJoin == description.LineJoin;
        }
        
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return cachedHashCode;
        }

        /// <summary>
        /// Computes the HashCode for the instance.
        /// </summary>
        /// <returns>The HashCode for the instance.</returns>
        private int ComputeHashCode()
        {
            var hashCode = 754997215;
            
            unchecked
            {
                hashCode = hashCode * -1521134295 + Color.GetHashCode();
                hashCode = hashCode * -1521134295 + Thickness.GetHashCode();
                hashCode = hashCode * -1521134295 + ComputeDashArrayHashCode(DashArray);
                hashCode = hashCode * -1521134295 + LineJoin.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Computes a HashCode for the given array based on every element in the array.
        /// </summary>
        /// <param name="array">The array</param>
        /// <returns>A HashCode</returns>
        private static int ComputeDashArrayHashCode(double[] array)
        {
            if (array == null)
            {
                return -1;
            }

            int hashCode = array.Length;

            for (int i = 0; i < array.Length; i++)
            {
                unchecked
                {
                    hashCode = hashCode * 31 + array[i].GetHashCode();
                }
            }

            return hashCode;
        }

        /// <summary>
        /// Determines whether two arrays are equivalent.
        /// </summary>
        /// <param name="l">The left array.</param>
        /// <param name="r">The right array.</param>
        /// <returns><c>true</c> if the arrays are the same array, are both null, or have the same elements; otherwise <c>false</c></returns>
        private static bool DashArraysEquals(double[] l, double[] r)
        {
            if (l == r)
            {
                return true;
            }

            if (l == null || r == null)
            {
                return false;
            }

            if (l.Length != r.Length)
            {
                return false;
            }

            for (int i = 0; i < l.Length; i++)
            {
                if (l[i] != r[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
