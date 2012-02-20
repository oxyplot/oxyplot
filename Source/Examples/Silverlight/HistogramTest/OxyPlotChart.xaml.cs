using System.Windows.Controls;
using OxyPlot.Silverlight;

namespace Visiblox.Charts.Examples
{
    public partial class OxyPlotChart : UserControl
    {
        public OxyPlotChart()
        {
            InitializeComponent();
        }

        public Plot Chart
        {
            get { return chart; }
        }
    }
}