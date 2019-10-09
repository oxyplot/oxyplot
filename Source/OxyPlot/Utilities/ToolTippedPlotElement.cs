// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionPath.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to reflect a path of properties.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    public class ToolTippedPlotElement
    {
        public PlotElement PlotElement { get; private set; } = null;

        public bool IsPlotTitle { get; private set; } = false;

        public bool Exists
        {
            get
            {
                return PlotElement != null || IsPlotTitle;
            }
        }

        public ToolTippedPlotElement(PlotElement el)
        {
            PlotElement = el;
        }

        public ToolTippedPlotElement(bool isPlotTitle = false)
        {
            IsPlotTitle = isPlotTitle;
        }

        public static bool operator ==(ToolTippedPlotElement t1, ToolTippedPlotElement t2)
        {
            return t1.Exists && t2.Exists &&
                (t1.PlotElement == t2.PlotElement ||
                (t1.IsPlotTitle == true && t2.IsPlotTitle == true));
        }
        public static bool operator !=(ToolTippedPlotElement t1, ToolTippedPlotElement t2)
        {
            return !(t1 == t2);
        }

        public static bool operator ==(PlotElement t1, ToolTippedPlotElement t2)
        {
            return t1 == t2.PlotElement;
        }
        public static bool operator !=(PlotElement t1, ToolTippedPlotElement t2)
        {
            return !(t1 == t2);
        }

        public static bool operator ==(ToolTippedPlotElement t1, PlotElement t2)
        {
            return t1.Exists && t1.PlotElement == t2;
        }
        public static bool operator !=(ToolTippedPlotElement t1, PlotElement t2)
        {
            return !(t1 == t2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            if (PlotElement != null)
            {
                return $"PlotElement";
            }

            if (IsPlotTitle)
            {
                return "PlotTitle";
            }

            return "None";
        }
    }
}
