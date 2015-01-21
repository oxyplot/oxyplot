using System;
using System.Collections.Generic;
using System.Linq;
using ExampleLibrary;
using OxyPlot.Xwt;
using Xwt;

namespace ExampleBrowser
{
	public class MainWindow: Window
	{
		OxyPlot.Xwt.PlotView plotView;
		TreeView treeView;

		ExampleInfo selectedExample;
		DataField<string> nameCol = new DataField<string> ();

		public IList<ExampleInfo> Examples { get; private set; }

		public ExampleInfo SelectedExample
		{
			get
			{
				return selectedExample;
			}

			set
			{
				selectedExample = value;
				plotView.Model = selectedExample != null ? selectedExample.PlotModel : null;
				plotView.Controller = selectedExample != null ? selectedExample.PlotController : null;
			}
		}

		public MainWindow ()
		{
			this.Examples = ExampleLibrary.Examples.GetList().OrderBy(e => e.Category).ToList();

			this.plotView = new PlotView();
			this.plotView.MinHeight = 554;
			this.plotView.MinWidth = 625;
			this.plotView.DefaultTrackerSettings.Enabled = true;
			this.plotView.DefaultTrackerSettings.Background = Xwt.Drawing.Colors.AliceBlue.WithAlpha (0.9).ToOxyColor();

			this.treeView = new TreeView();
			this.treeView.MinWidth = 314;
			this.treeView.Visible = true;

			var treeModel = new TreeStore(nameCol);
			TreePosition categoryNode = null;
			string categoryName = null;
			foreach (var ex in this.Examples)
			{
				if (categoryName == null || categoryName != ex.Category)
				{
					categoryNode = treeModel.AddNode ().SetValue (nameCol, ex.Category).CurrentPosition;
					categoryName = ex.Category;
				}

				treeModel.AddNode (categoryNode).SetValue (nameCol, ex.Title);
			}

			treeView.Columns.Add ("Example", nameCol);
			this.treeView.DataSource = treeModel;

			this.treeView.SelectionChanged += (s, e) =>
			{

				if (treeView.SelectedRow != null) {
					var sample = treeModel.GetNavigatorAt (treeView.SelectedRow).GetValue (nameCol);

					var info = this.Examples.FirstOrDefault(ex => ex.Title == sample);
					if (info != null)
					{
						this.SelectedExample = info;
					}
				}
			};

			var hbox = new HBox();
			hbox.Spacing = 6;
			hbox.MinHeight = 554;
			hbox.MinWidth = 943;

			hbox.PackStart(this.treeView);
			hbox.PackStart(this.plotView, true);

			Content = hbox;

			this.SelectedExample = this.Examples.FirstOrDefault();

			this.Title = "OxyPlot.Xwt Example Browser";
			this.CloseRequested += (s, a) => Application.Exit ();
		}
	}
}

