// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using System.Drawing;
    using System.Windows.Forms;

    using ExampleLibrary;

    public partial class MainForm : Form
    {
        readonly MainWindowViewModel vm = new MainWindowViewModel();

        public MainForm()
        {
            this.InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.InitTree();
        }

        private void InitTree()
        {
            TreeNode node = null;
            foreach (var ex in this.vm.Examples)
            {
                if (node == null || node.Text != ex.Category)
                {
                    node = new TreeNode(ex.Category);
                    this.treeView1.Nodes.Add(node);
                }

                var exNode = new TreeNode(ex.Title) { Tag = ex };
                node.Nodes.Add(exNode);
                if (ex == this.vm.SelectedExample)
                {
                    this.treeView1.SelectedNode = exNode;
                }
            }
            this.treeView1.AfterSelect += this.TreeView1AfterSelect;
        }

        void TreeView1AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.vm.SelectedExample = e.Node.Tag as ExampleInfo;
            this.InitPlot();

            this.transposedCheck.Enabled = this.vm.SelectedExample?.IsTransposable ?? false;
            this.reversedCheck.Enabled = this.vm.SelectedExample?.IsReversible ?? false;
        }

        private void InitPlot()
        {
            if (this.vm.SelectedExample == null)
            {
                this.plot1.Model = null;
                this.plot1.Controller = null;
            }
            else
            {
                var flags = ExampleInfo.PrepareFlags(
                    this.transposedCheck.Enabled && this.transposedCheck.Checked,
                    this.reversedCheck.Enabled && this.reversedCheck.Checked);

                this.plot1.Model = this.vm.SelectedExample.GetModel(flags);
                this.plot1.Controller = this.vm.SelectedExample.GetController(flags);
            }
        }

        private void transposedCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            InitPlot();
        }

        private void reversedCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            InitPlot();
        }
    }
}
