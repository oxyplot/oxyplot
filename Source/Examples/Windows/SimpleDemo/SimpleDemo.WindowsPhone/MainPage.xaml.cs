namespace SimpleDemo
{
    using Windows.UI.Xaml.Navigation;

    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = new MainViewModel();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
