using System.Collections.ObjectModel;

namespace PlotModelAlreadyInUse
{
    public class ViewModel
    {
        public ViewModel()
        {
            Items = new ObservableCollection<Item>();

            for (var i = 0; i < 10; i++)
                Items.Add(new Item("MyName " + i));
        }

        public ObservableCollection<Item> Items { get; set; }
    }
}
