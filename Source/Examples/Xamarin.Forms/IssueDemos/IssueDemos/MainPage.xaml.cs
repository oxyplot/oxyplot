using System.Collections.Generic;
using System.Reflection;

namespace IssueDemos
{
    using Xamarin.Forms;

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            var demoPages = new List<DemoInfo>();
            foreach (var type in typeof(MainPage).GetTypeInfo().Assembly.ExportedTypes)
            {
                var ti = type.GetTypeInfo();
                if (ti.GetCustomAttribute<DemoPageAttribute>() != null)
                {
                    demoPages.Add(new DemoInfo(type));
                }
            }

            this.list1.ItemsSource = demoPages;
        }

        private async void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var demoInfo = (DemoInfo)e.Item;
            var page = demoInfo.CreatePage();
            page.Title = demoInfo.Title;
            await Navigation.PushAsync(page);
        }
    }
}
