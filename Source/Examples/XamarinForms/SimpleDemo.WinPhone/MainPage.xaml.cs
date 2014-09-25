namespace SimpleDemo.WinPhone
{
    using Microsoft.Phone.Controls;

    using Xamarin.Forms;

    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            OxyPlot.XamarinFormsWinPhone.Forms.Init();
            Forms.Init();
            this.Content = SimpleDemo.App.GetMainPage().ConvertPageToUIElement(this);
        }
    }
}