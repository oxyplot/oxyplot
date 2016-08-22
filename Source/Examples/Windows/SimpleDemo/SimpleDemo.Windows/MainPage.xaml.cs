namespace SimpleDemo
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
