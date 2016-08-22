using OxyPlot;

namespace PlotModelAlreadyInUse
{
    public class Item
    {
        public Item(string name)
        {
            this.Name = name;
            this.Model = new PlotModel();
        }

        public string Name { get; set; }

        public PlotModel Model { get; set; }
    }
}
