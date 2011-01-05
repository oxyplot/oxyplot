﻿using System;

namespace OxyPlot.Reporting
{
    public abstract class Figure : ReportItem
    {
        public string FigureText { get; set; }
        public int FigureNumber { get; set; }
        private const string CAPTION_FORMAT_STRING = "Figure {0}. {1}";

        protected Figure()
        {
            Class = "figure";
        }

        public string FullCaption
        {
            get { return String.Format(CAPTION_FORMAT_STRING, FigureNumber, FigureText); }
        }
    }
}