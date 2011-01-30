using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OxyPlot
{
    public class CategoryAxis : LinearAxis
    {
        /// <summary>
        ///   Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        ///   Gets or sets the data field X.
        /// </summary>
        /// <value>The data field X.</value>
        public string LabelField { get; set; }

        public StringCollection Labels { get; set; }

        internal int AttachedSeriesCount { get; set; }

        internal double[] BaseValue { get; set; }

        public CategoryAxis()
        {
            TickStyle = TickStyle.Outside;
            Position = AxisPosition.Bottom;
        }

        public virtual void UpdateLabels()
        {
            if (ItemsSource == null)
            {
                return;
            }

            if (Labels == null)
                Labels = new StringCollection();

            Labels.Clear();

            PropertyInfo pil = null;
            Type t = null;

            foreach (var o in ItemsSource)
            {
                if (pil == null || o.GetType() != t)
                {
                    t = o.GetType();
                    pil = t.GetProperty(LabelField);
                    if (pil == null)
                    {
                        throw new InvalidOperationException(string.Format("Could not find label field {0} on type {1}",
                                                                          LabelField, t));
                    }

                }

                var label = Convert.ToString(pil.GetValue(o, null));
                Labels.Add(label);
            }

            AttachedSeriesCount = 0;
            if (Labels.Count>0)
                BaseValue=new double[Labels.Count];
        }

        public override string FormatValue(double x)
        {
            int index = (int)x;
            if (Labels != null && index >= 0 && index < Labels.Count)
                return Labels[index];
            return null;
        }

        public override void UpdateActualMaxMin()
        {
            base.UpdateActualMaxMin();
            ActualMinimum = -0.5;

            if (Labels != null && Labels.Count > 0)
                ActualMaximum = (Labels.Count - 1) + 0.5;
            else
                ActualMaximum = 0.5;

            MajorStep = 1;
            MinorStep = 1;
        }

    }
}
