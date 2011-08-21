namespace ExampleBrowser
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            OxyPlot.PlotModel plotModel1 = new OxyPlot.PlotModel();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.plot1 = new Oxyplot.WindowsForms.Plot();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(240, 682);
            this.treeView1.TabIndex = 0;
            // 
            // plot1
            // 
            this.plot1.BackColor = System.Drawing.Color.White;
            this.plot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plot1.Location = new System.Drawing.Point(240, 0);
            plotModel1.Annotations = ((System.Collections.ObjectModel.Collection<OxyPlot.IAnnotation>)(resources.GetObject("plotModel1.Annotations")));
            plotModel1.AutoAdjustPlotMargins = true;
            plotModel1.Axes = ((System.Collections.ObjectModel.Collection<OxyPlot.IAxis>)(resources.GetObject("plotModel1.Axes")));
            plotModel1.Background = null;
            plotModel1.BoxColor = ((OxyPlot.OxyColor)(resources.GetObject("plotModel1.BoxColor")));
            plotModel1.BoxThickness = 1D;
            plotModel1.DefaultColors = ((System.Collections.Generic.List<OxyPlot.OxyColor>)(resources.GetObject("plotModel1.DefaultColors")));
            plotModel1.IsLegendVisible = true;
            plotModel1.LegendBackground = ((OxyPlot.OxyColor)(resources.GetObject("plotModel1.LegendBackground")));
            plotModel1.LegendBorder = ((OxyPlot.OxyColor)(resources.GetObject("plotModel1.LegendBorder")));
            plotModel1.LegendBorderThickness = 1D;
            plotModel1.LegendColumnSpacing = 0D;
            plotModel1.LegendFont = null;
            plotModel1.LegendFontSize = 12D;
            plotModel1.LegendFontWeight = 400D;
            plotModel1.LegendItemAlignment = OxyPlot.HorizontalTextAlign.Left;
            plotModel1.LegendItemOrder = OxyPlot.LegendItemOrder.Normal;
            plotModel1.LegendItemSpacing = 24D;
            plotModel1.LegendMargin = 8D;
            plotModel1.LegendOrientation = OxyPlot.LegendOrientation.Vertical;
            plotModel1.LegendPadding = 8D;
            plotModel1.LegendPlacement = OxyPlot.LegendPlacement.Inside;
            plotModel1.LegendPosition = OxyPlot.LegendPosition.RightTop;
            plotModel1.LegendSymbolLength = 16D;
            plotModel1.LegendSymbolMargin = 4D;
            plotModel1.LegendSymbolPlacement = OxyPlot.LegendSymbolPlacement.Left;
            plotModel1.LegendTitle = null;
            plotModel1.LegendTitleFont = null;
            plotModel1.LegendTitleFontSize = 12D;
            plotModel1.LegendTitleFontWeight = 700D;
            plotModel1.PlotType = OxyPlot.PlotType.XY;
            plotModel1.Series = ((System.Collections.ObjectModel.Collection<OxyPlot.ISeries>)(resources.GetObject("plotModel1.Series")));
            plotModel1.Subtitle = null;
            plotModel1.SubtitleFont = null;
            plotModel1.SubtitleFontSize = 14D;
            plotModel1.SubtitleFontWeight = 400D;
            plotModel1.TextColor = ((OxyPlot.OxyColor)(resources.GetObject("plotModel1.TextColor")));
            plotModel1.Title = null;
            plotModel1.TitleFont = null;
            plotModel1.TitleFontSize = 18D;
            plotModel1.TitleFontWeight = 700D;
            plotModel1.TitlePadding = 6D;
            this.plot1.Model = plotModel1;
            this.plot1.Name = "plot1";
            this.plot1.Size = new System.Drawing.Size(1024, 682);
            this.plot1.TabIndex = 1;
            this.plot1.Text = "plot1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1264, 682);
            this.Controls.Add(this.plot1);
            this.Controls.Add(this.treeView1);
            this.Name = "Form1";
            this.Text = "Example Browser";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private Oxyplot.WindowsForms.Plot plot1;
    }
}

