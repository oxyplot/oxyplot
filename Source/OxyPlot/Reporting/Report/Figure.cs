namespace OxyPlot.Reporting
{
    public abstract class Figure : ReportItem
    {
        public string FigureText { get; set; }

        public Figure()
        {
            Class = "figure";
        }
    }
}