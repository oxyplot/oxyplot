// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using System.Collections.Generic;
    using System.Linq;

    using ExampleLibrary;

    using Gtk;

    #if GTK3
    using TreeModel = Gtk.ITreeModel;
    #endif

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

            var scrollwin = new ScrolledWindow ();
            scrollwin.Add (this.treeView1);

            this.hbox1.PackStart(scrollwin, false, false, 6);
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