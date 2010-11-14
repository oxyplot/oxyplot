using System.Windows;

namespace OxyPlot.Wpf
{
    public abstract class Axis : DependencyObject
    {
        public OxyPlot.IAxis ModelAxis { get; protected set; }

        public abstract void UpdateModelProperties();
    }
}