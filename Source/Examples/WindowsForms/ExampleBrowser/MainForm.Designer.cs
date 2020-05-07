// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.Designer.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: MIT
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using OxyPlot;

    partial class MainForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.plot1 = new OxyPlot.WindowsForms.PlotView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.transposedCheck = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.plot1);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(943, 554);
            this.splitContainer1.SplitterDistance = 314;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(314, 554);
            this.treeView1.TabIndex = 1;
            // 
            // plot1
            // 
            this.plot1.BackColor = System.Drawing.Color.White;
            this.plot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plot1.Location = new System.Drawing.Point(0, 0);
            plotModel1.AxisTierDistance = 4D;
            plotModel1.Culture = null;
            plotModel1.DefaultColors = null;
            plotModel1.DefaultFont = "Segoe UI";
            plotModel1.DefaultFontSize = 12D;
            plotModel1.EdgeRenderingMode = OxyPlot.EdgeRenderingMode.Automatic;
            plotModel1.IsLegendVisible = true;
            plotModel1.PlotType = OxyPlot.PlotType.XY;
            plotModel1.RenderingDecorator = null;
            plotModel1.Subtitle = null;
            plotModel1.SubtitleFont = null;
            plotModel1.SubtitleFontSize = 14D;
            plotModel1.SubtitleFontWeight = 400D;
            plotModel1.Title = null;
            plotModel1.TitleFont = null;
            plotModel1.TitleFontSize = 18D;
            plotModel1.TitleFontWeight = 700D;
            plotModel1.TitleHorizontalAlignment = OxyPlot.TitleHorizontalAlignment.CenteredWithinPlotArea;
            plotModel1.TitlePadding = 6D;
            plotModel1.TitleToolTip = null;
            this.plot1.Model = plotModel1;
            this.plot1.Name = "plot1";
            this.plot1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plot1.Size = new System.Drawing.Size(625, 525);
            this.plot1.TabIndex = 0;
            this.plot1.Text = "plot1";
            this.plot1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plot1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plot1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.transposedCheck);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 525);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(625, 29);
            this.panel1.TabIndex = 1;
            // 
            // transposedCheck
            // 
            this.transposedCheck.AutoSize = true;
            this.transposedCheck.Location = new System.Drawing.Point(531, 6);
            this.transposedCheck.Name = "transposedCheck";
            this.transposedCheck.Size = new System.Drawing.Size(82, 17);
            this.transposedCheck.TabIndex = 0;
            this.transposedCheck.Text = "Transposed";
            this.transposedCheck.UseVisualStyleBackColor = true;
            this.transposedCheck.CheckedChanged += new System.EventHandler(this.transposedCheck_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 554);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "OxyPlot.WindowsForms Example Browser";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private OxyPlot.WindowsForms.PlotView plot1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox transposedCheck;
    }
}
