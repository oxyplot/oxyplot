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
        public OxyPlotPropertiesDialog(Plot plot)
        {
            InitializeComponent();
            PropertiesControl.Plot = plot;
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
