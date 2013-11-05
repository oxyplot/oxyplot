// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using System.Drawing;
    using System.Linq;
    using Gtk;
    using Gdk;
    using OxyPlot;

    using ExampleLibrary;

    public partial class MainWindow : Gtk.Window
    {
        readonly MainWindowViewModel vm = new MainWindowViewModel();


        private void InitializeComponent()
        {
            OxyPlot.PlotModel plotModel1 = new OxyPlot.PlotModel();
            this.hbox1 = new Gtk.HBox(false, 6);
            this.treeView1 = new Gtk.TreeView();
            this.plot1 = new OxyPlot.GtkSharp.Plot();
            this.hbox1.SetSizeRequest(943, 554);
            this.hbox1.Name = "hbox1";
            //this.hbox1.SplitterDistance = 314;
            //this.hbox1.TabIndex = 0;
            // 
            // treeView1
            // 
            //this.treeView1.Dock = Gtk.DockStyle.Fill;
            //this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SetSizeRequest(314, 554);
            this.treeView1.Visible = true;

            //this.treeView1.TabIndex = 1;
            // 
            // plot1
            // 
            //this.plot1.Dock = Gtk.DockStyle.Fill;
            this.plot1.KeyboardPanHorizontalStep = 0.1D;
            this.plot1.KeyboardPanVerticalStep = 0.1D;
            //this.plot1.Location = new System.Drawing.Point(0, 0);
            plotModel1.Annotations = null;
            plotModel1.AutoAdjustPlotMargins = true;
            plotModel1.Axes = null;
            plotModel1.AxisTierDistance = 4D;
            plotModel1.Background = OxyColors.Transparent;
            plotModel1.Culture = null;
            plotModel1.DefaultColors = null;
            plotModel1.DefaultFont = "Segoe UI";
            plotModel1.DefaultFontSize = 12D;
            plotModel1.IsLegendVisible = true;
            plotModel1.LegendBackground = OxyColors.Undefined;
            plotModel1.LegendBorder = OxyColors.Undefined;
            plotModel1.LegendBorderThickness = 1D;
            plotModel1.LegendColumnSpacing = 0D;
            plotModel1.LegendFont = null;
            plotModel1.LegendFontSize = 12D;
            plotModel1.LegendFontWeight = 400D;
            plotModel1.LegendItemAlignment = OxyPlot.HorizontalAlignment.Left;
            plotModel1.LegendItemOrder = OxyPlot.LegendItemOrder.Normal;
            plotModel1.LegendItemSpacing = 24D;
            plotModel1.LegendMargin = 8D;
            plotModel1.LegendMaxWidth = double.NaN;
            plotModel1.LegendOrientation = OxyPlot.LegendOrientation.Vertical;
            plotModel1.LegendPadding = 8D;
            plotModel1.LegendPlacement = OxyPlot.LegendPlacement.Inside;
            plotModel1.LegendPosition = OxyPlot.LegendPosition.RightTop;
            plotModel1.LegendSymbolLength = 16D;
            plotModel1.LegendSymbolMargin = 4D;
            plotModel1.LegendSymbolPlacement = OxyPlot.LegendSymbolPlacement.Left;
            plotModel1.LegendTextColor = OxyColors.Undefined;
            plotModel1.LegendTitle = null;
            plotModel1.LegendTitleColor = OxyColors.Undefined;
            plotModel1.LegendTitleFont = null;
            plotModel1.LegendTitleFontSize = 12D;
            plotModel1.LegendTitleFontWeight = 700D;
            plotModel1.PlotAreaBackground = OxyColors.Undefined;
            plotModel1.PlotAreaBorderColor = OxyColors.Undefined;
            plotModel1.PlotAreaBorderThickness = 1D;
            plotModel1.PlotType = OxyPlot.PlotType.XY;
            plotModel1.SelectionColor = OxyColors.Undefined;
            plotModel1.Series = null;
            plotModel1.Subtitle = null;
            plotModel1.SubtitleColor = OxyColors.Undefined;
            plotModel1.SubtitleFont = null;
            plotModel1.SubtitleFontSize = 14D;
            plotModel1.SubtitleFontWeight = 400D;
            plotModel1.TextColor = OxyColors.Undefined;
            plotModel1.Title = null;
            plotModel1.TitleColor = OxyColors.Undefined;
            plotModel1.TitleFont = null;
            plotModel1.TitleFontSize = 18D;
            plotModel1.TitleFontWeight = 700D;
            plotModel1.TitlePadding = 6D;
            this.plot1.Model = plotModel1;
            this.plot1.Name = "plot1";
            //this.plot1.PanCursor = Gtk.Cursors.Hand;
            this.plot1.SetSizeRequest(625, 554);
            //this.plot1.TabIndex = 0;
            //this.plot1.Text = "plot1";
            this.plot1.ZoomHorizontalCursor = new Gdk.Cursor(Gdk.CursorType.Sizing); // Cursors.SizeWE;
            this.plot1.ZoomRectangleCursor = new Gdk.Cursor(Gdk.CursorType.Sizing); //Gtk.Cursors.SizeNWSE;
            this.plot1.ZoomVerticalCursor = new Gdk.Cursor(Gdk.CursorType.Sizing); //Gtk.Cursors.SizeNS;
            this.plot1.Visible = true;

            vm.SelectedExample = vm.Examples.FirstOrDefault();
            InitPlot();

            var treeModel = new TreeStore(typeof(string), typeof(string));
            TreeIter iter = new TreeIter();
            string last = null;
            foreach (var ex in vm.Examples)
            {
                if (last == null || last != ex.Category)
                {
                    iter = treeModel.AppendValues(ex.Category);
                    last = ex.Category;
                }
                treeModel.AppendValues(iter, ex.Title);
            }
            treeView1.Model = treeModel;
            Gtk.TreeViewColumn exampleNameColumn = new Gtk.TreeViewColumn();
            exampleNameColumn.Title = "Example";
            Gtk.CellRendererText exampleNameCell = new Gtk.CellRendererText();

            exampleNameColumn.PackStart(exampleNameCell, true);
            treeView1.AppendColumn(exampleNameColumn);
            exampleNameColumn.AddAttribute(exampleNameCell, "text", 0);

            treeView1.Selection.Changed += (delegate(object sender, System.EventArgs ev)
            {
                TreeIter selectedNode;
                TreeModel selectedModel;
                if (treeView1.Selection.GetSelected(out selectedModel, out selectedNode))
                {
                    string val1 = (string)selectedModel.GetValue(selectedNode, 0);
                    string val2 = (string)selectedModel.GetValue(selectedNode, 1);

                    //treeView1.GetselectedNode
                    //vm.SelectedExample = selectedNode.;
                    //.Node.Tag as ExampleInfo;
                    ExampleInfo info = vm.Examples.FirstOrDefault(ex => ex.Title == val1);
                    if (info != null)
                    {
                        vm.SelectedExample = info;
                        InitPlot();
                    }
                }
            });

            this.hbox1.PackStart(treeView1, false, false, 6);
            this.hbox1.PackStart(plot1, true, true, 6);
            treeView1.Show();
            hbox1.Show();
            // 
            // MainWindow
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //this.AutoScaleMode = Gtk.AutoScaleMode.Font;
            //this.ClientSize = new System.Drawing.Size(943, 554);
            this.Add(this.hbox1);
            this.Name = "MainWindow";
            this.Title = "OxyPlot.GtkSharp Example Browser";
            this.DeleteEvent += (delegate(object sender, DeleteEventArgs a)
            {
                Application.Quit();
                a.RetVal = true;
            });
        }

        private Gtk.HBox hbox1;
        private Gtk.TreeView treeView1;
        private OxyPlot.GtkSharp.Plot plot1;
        public MainWindow() : base("Example Browser")
        {
            this.InitializeComponent();
            //this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }


        private void InitPlot()
        {
            plot1.Model = vm.SelectedExample != null ? vm.SelectedExample.PlotModel : null;
            //plot1.BackColor = vm.PlotBackground;
        }
    }
}