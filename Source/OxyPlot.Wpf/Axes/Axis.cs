using System.Windows;

namespace OxyPlot.Wpf
{
    public abstract class Axis : DependencyObject
    {
        public OxyPlot.Axis ModelAxis { get; protected set; }

        public abstract void UpdateModelProperties();
    }
}