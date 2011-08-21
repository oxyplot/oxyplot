using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExampleBrowser
{
    using ExampleLibrary;

    public partial class Form1 : Form
    {
        MainWindowViewModel vm = new MainWindowViewModel();

        public Form1()
        {
            InitializeComponent();
            InitTree();
        }

        private void InitTree()
        {
            TreeNode node = null;
            string cat = null;
            foreach (var ex in vm.Examples)
            {
                if (node == null || node.Text != ex.Category)
                {
                    node = new TreeNode(ex.Category);
                    treeView1.Nodes.Add(node);
                }
                node.Nodes.Add(new TreeNode(ex.Title) { Tag = ex });
            }
            treeView1.AfterSelect += this.treeView1_AfterSelect;
        }

        void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            vm.SelectedExample = e.Node.Tag as ExampleInfo;
            InitPlot();
        }

        private void InitPlot()
        {
            plot1.Model = vm.SelectedExample != null ? vm.SelectedExample.PlotModel : null;
            plot1.BackColor = vm.PlotBackground;
        }
    }
}
