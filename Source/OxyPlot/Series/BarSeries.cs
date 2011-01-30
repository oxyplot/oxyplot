using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace OxyPlot
{
    /// <summary>
    /// Bar series.
    /// </summary>
    public class BarSeries : ISeries
    {
        internal IList<double> InternalPoints;

        public BarSeries()
        {
            InternalPoints = new Collection<double>();
        }

        /// <summary>
        ///   Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        ///   Gets or sets the value field.
        /// </summary>
        public string ValueField { get; set; }

        /// <summary>
        ///   Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        [Browsable(false)]
        public IList<double> Values
        {
            get { return InternalPoints; }
            set { InternalPoints = value; }
        }

        /// <summary>
        ///   Gets or sets the category axis.
        /// </summary>
        public IAxis CategoryAxis { get; set; }

        /// <summary>
        ///   Gets or sets the value axis.
        /// </summary>
        public IAxis ValueAxis { get; set; }

        /// <summary>
        ///   Gets or sets the category axis key.
        /// </summary>
        public string CategoryAxisKey { get; set; }

        /// <summary>
        ///   Gets or sets the value axis key.
        /// </summary>
        public string ValueAxisKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this bar series is stacked.
        /// </summary>
        public bool IsStacked { get; set; }

        /// <summary>
        ///   Gets or sets the min X of the dataset.
        /// </summary>
        /// <value>The min X.</value>
        public double MinX { get; protected set; }

        /// <summary>
        ///   Gets or sets the max X of the dataset.
        /// </summary>
        /// <value>The max X.</value>
        public double MaxX { get; protected set; }

        /// <summary>
        ///   Gets or sets the min Y of the dataset.
        /// </summary>
        /// <value>The min Y.</value>
        public double MinY { get; protected set; }

        /// <summary>
        ///   Gets or sets the max Y of the dataset.
        /// </summary>
        /// <value>The max Y.</value>
        public double MaxY { get; protected set; }

        #region ISeries Members

        /// <summary>
        ///   Gets the title of the Series.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        ///   Renders the Series on the specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "model">The model.</param>
        public virtual void Render(IRenderContext rc, PlotModel model)
        {
        }

        /// <summary>
        ///   Renders the legend symbol on the specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "legendBox">The rect.</param>
        public virtual void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
        }

        #endregion

        public virtual void UpdateData()
        {
            if (ItemsSource == null)
            {
                return;
            }

            InternalPoints.Clear();


            // Using reflection on DataFieldX and DataFieldY
            PropertyInfo piy = null;
            Type t = null;

            foreach (var o in ItemsSource)
            {
                if (piy == null || o.GetType() != t)
                {
                    t = o.GetType();
                    piy = t.GetProperty(ValueField);
                    if (piy == null)
                    {
                        throw new InvalidOperationException(string.Format("Could not find data field {0} on type {1}",
                                                                          ValueField, t));
                    }
                }

                var y = Convert.ToDouble(piy.GetValue(o, null));
                InternalPoints.Add(y);
            }
        }

        public bool AreAxesRequired()
        {
            return true;
        }

        public void EnsureAxes(Collection<IAxis> axes, IAxis defaultXAxis, IAxis defaultYAxis)
        {
            if (CategoryAxisKey != null)
            {
                CategoryAxis = axes.FirstOrDefault(a => a.Key == CategoryAxisKey);
            }
            if (ValueAxisKey != null)
            {
                ValueAxis = axes.FirstOrDefault(a => a.Key == ValueAxisKey);
            }

            // If axes are not found, use the default axes
            if (CategoryAxis == null)
            {
                CategoryAxis = defaultXAxis;
            }
            if (ValueAxis == null)
            {
                ValueAxis = defaultYAxis;
            }
        }

        /// <summary>
        ///   Updates the max/min from the datapoints.
        /// </summary>
        public virtual void UpdateMaxMin()
        {
            MinX = MinY = MaxX = MaxY = double.NaN;

            if (InternalPoints == null || InternalPoints.Count == 0)
            {
                return;
            }

            MinY = MaxY = InternalPoints[0];
            foreach (var v in InternalPoints)
            {
                MinY = Math.Min(MinY, v);
                MaxY = Math.Max(MaxY, v);
            }

            ValueAxis.Include(MinY);
            ValueAxis.Include(MaxY);
        }

        /// <summary>
        ///   Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        /// <param name = "point">The point.</param>
        /// <param name="dpn">The nearest point (data coordinates).</param>
        /// <param name="spn">The nearest point (screen coordinates).</param>
        /// <returns></returns>
        public bool GetNearestPoint(ScreenPoint point, out DataPoint dpn, out ScreenPoint spn)
        {
            dpn = DataPoint.Undefined;
            spn = ScreenPoint.Undefined;
            return false;
        }

        /// <summary>
        ///   Gets the point on the curve that is nearest the specified point.
        /// </summary>
        /// <param name = "point">The point.</param>
        /// <param name="dpn">The nearest point (data coordinates).</param>
        /// <param name="spn">The nearest point (screen coordinates).</param>
        /// <returns></returns>
        public bool GetNearestInterpolatedPoint(ScreenPoint point, out DataPoint dpn, out ScreenPoint spn)
        {
            dpn = DataPoint.Undefined;
            spn = ScreenPoint.Undefined;
            return false;
        }

        /// <summary>
        ///   Gets the value from the specified X.
        /// </summary>
        /// <param name = "x">The x.</param>
        /// <returns></returns>
        public double? GetValueFromX(double x)
        {
            return null;
        }
    }
}