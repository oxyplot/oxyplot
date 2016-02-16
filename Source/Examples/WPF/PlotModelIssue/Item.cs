using OxyPlot;

namespace PlotModelIssue
{
    public class Item
    {
        public Item(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        private PlotModel model = new PlotModel();

        public PlotModel Model
        {
            get { return model; }
            set { model = value; }
        }
    }
}
