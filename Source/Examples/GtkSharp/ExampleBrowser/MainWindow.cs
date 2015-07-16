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
        HPaned paned;
        TreeView treeView;
        OxyPlot.GtkSharp.PlotView plotView;
        ExampleInfo selectedExample;

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
                this.plotView.Model = this.selectedExample != null ? this.selectedExample.PlotModel : null;
                this.plotView.Controller = this.selectedExample != null ? this.selectedExample.PlotController : null;
            }
        }

        private void InitializeComponent()
        {
            this.plotView = new OxyPlot.GtkSharp.PlotView();
            this.plotView.SetSizeRequest(300, 300);

            this.treeView = new TreeView();
            this.treeView.Visible = true;

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

            this.treeView.Model = treeModel;
            var exampleNameColumn = new TreeViewColumn { Title = "Example" };
            var exampleNameCell = new CellRendererText();
            exampleNameColumn.PackStart(exampleNameCell, true);
            this.treeView.AppendColumn(exampleNameColumn);
            exampleNameColumn.AddAttribute(exampleNameCell, "text", 0);

            this.treeView.Selection.Changed += (s, e) =>
            {
                TreeIter selectedNode;
                TreeModel selectedModel;
                if (treeView.Selection.GetSelected(out selectedModel, out selectedNode))
                {
                    string val1 = (string)selectedModel.GetValue(selectedNode, 0);
                    string val2 = (string)selectedModel.GetValue(selectedNode, 1);

                    var info = this.Examples.FirstOrDefault(ex => ex.Title == val1)
                            ?? this.Examples.FirstOrDefault(ex => ex.Category == val1);
                    if (info != null)
                    {
                        this.SelectedExample = info;
                    }
                }
            };

            var scrollwin = new ScrolledWindow ();
            scrollwin.Add (this.treeView);
            scrollwin.SetSizeRequest(250, 300);

            var txtSearch = new Entry ();
            treeView.SearchEntry = txtSearch;
            var treeVbox = new VBox (false, 0);
            treeVbox.BorderWidth = 6;
            treeVbox.PackStart (txtSearch, false, true, 0);
            treeVbox.PackStart (scrollwin, true, true, 0);

            this.paned = new HPaned ();
            this.paned.Pack1 (treeVbox, false, false);
            this.paned.Pack2 (this.plotView, true, false);
            this.paned.Position = 300;

            this.Add(this.paned);

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