using System.ComponentModel;
using System.Windows;
using OxyPlot.Wpf;

namespace Demo_OxyPlotControls
{
    /// <summary>
    /// Interaction logic for OxyPlotPropertiesDialog.xaml
    /// </summary>
    public partial class OxyPlotPropertiesDialog : Window
    {
        public OxyPlotPropertiesDialog(PlotView plotView)
        {
            InitializeComponent();
            PropertiesControl.PlotModel = plotView.Model;
        }

        private void OxyPlotPropertiesDialog_Closing(object sender, CancelEventArgs e)
        {
            if (Owner != null)
            {
                Owner.Activate();
            }
        }
    }
}
