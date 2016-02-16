using System.Collections.ObjectModel;

namespace PlotModelIssue
{
    public class ViewModel
    {
        public ViewModel()
        {
            for (int i = 0; i < 10; i++)
                MyItems.Add(new Item("MyName" + i));
        }

        private ObservableCollection<Item> myItems = new ObservableCollection<Item>();
        public ObservableCollection<Item> MyItems
        {
            get { return myItems; }
            set { myItems = value; }
        }
    }
}
