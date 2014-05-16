// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
    using System.Collections.Generic;
    using System.Linq;

    using ExampleLibrary;

    using Gtk;

    public partial class MainWindow : Window
    {
        private HBox hbox1;

        private TreeView treeView1;

        private OxyPlot.GtkSharp.PlotView plotView1;

        private ExampleInfo selectedExample;

        public MainWindow()
            : base("Example Browser")
        {
            this.Examples = ExampleLibrary.Examples.GetList().OrderBy(e => e.Category).ToList();
            this.InitializeComponent();
            this.SelectedExample = this.Examples.FirstOrDefault();
        }

        public IList<ExampleInfo> Examples { get; private set; }

        public ExampleInfo SelectedExample
        {
            get
            {
                return this.selectedExample;
            }

            set
            {
                this.selectedExample = value;
                this.plotView1.Model = this.selectedExample != null ? this.selectedExample.PlotModel : null;
                this.plotView1.Controller = this.selectedExample != null ? this.selectedExample.PlotController : null;
            }
        }

        private void InitializeComponent()
        {
            this.plotView1 = new OxyPlot.GtkSharp.PlotView();
            this.plotView1.SetSizeRequest(625, 554);

            this.treeView1 = new TreeView();
            this.treeView1.SetSizeRequest(314, 554);
            this.treeView1.Visible = true;

            var treeModel = new TreeStore(typeof(string), typeof(string));
            var iter = new TreeIter();
            string last = null;
            foreach (var ex in this.Examples)
            {
                if (last == null || last != ex.Category)
                {
                    iter = treeModel.AppendValues(ex.Category);
                    last = ex.Category;
                }

                treeModel.AppendValues(iter, ex.Title);
            }

            this.treeView1.Model = treeModel;
            var exampleNameColumn = new TreeViewColumn { Title = "Example" };
            var exampleNameCell = new CellRendererText();
            exampleNameColumn.PackStart(exampleNameCell, true);
            this.treeView1.AppendColumn(exampleNameColumn);
            exampleNameColumn.AddAttribute(exampleNameCell, "text", 0);

            this.treeView1.Selection.Changed += (s, e) =>
            {
                TreeIter selectedNode;
                TreeModel selectedModel;
                if (treeView1.Selection.GetSelected(out selectedModel, out selectedNode))
                {
                    string val1 = (string)selectedModel.GetValue(selectedNode, 0);
                    string val2 = (string)selectedModel.GetValue(selectedNode, 1);

                    var info = this.Examples.FirstOrDefault(ex => ex.Title == val1);
                    if (info != null)
                    {
                        this.SelectedExample = info;
                    }
                }
            };

            this.hbox1 = new HBox(false, 6);
            this.hbox1.SetSizeRequest(943, 554);

            this.hbox1.PackStart(this.treeView1, false, false, 6);
            this.hbox1.PackStart(this.plotView1, false, false, 6);

            this.Add(this.hbox1);

            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //this.AutoScaleMode = Gtk.AutoScaleMode.Font;
            //this.ClientSize = new System.Drawing.Size(943, 554);
            this.Title = "OxyPlot.GtkSharp Example Browser";
            this.DeleteEvent += (s, a) =>
            {
                Application.Quit();
                a.RetVal = true;
            };
        }
    }
}