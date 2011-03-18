using System;

namespace OxyPlot.Reporting
{
    public abstract class Figure : ReportItem
    {
        public string FigureText { get; set; }
        public int FigureNumber { get; set; }

        public string GetFullCaption(ReportStyle style)
        {
            return String.Format(style.FigureTextFormatString, FigureNumber, FigureText); 
        }
    }
}