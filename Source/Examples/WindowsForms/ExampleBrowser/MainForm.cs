// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="OxyPlot">
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

                node.Nodes.Add(new TreeNode(ex.Title) { Tag = ex });
            }

            this.treeView1.AfterSelect += this.TreeView1AfterSelect;
        }

        void TreeView1AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.vm.SelectedExample = e.Node.Tag as ExampleInfo;
            this.InitPlot();
        }

        private void InitPlot()
        {
            this.plot1.Model = this.vm.SelectedExample != null ? this.vm.SelectedExample.PlotModel : null;
            this.plot1.Controller = this.vm.SelectedExample != null ? this.vm.SelectedExample.PlotController : null;
        }
    }
}