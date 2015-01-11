namespace SimpleDemo.WinPhone
{
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.WinPhone;

    public partial class MainPage : FormsApplicationPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            OxyPlot.XamarinFormsWinPhone.Forms.Init();
            Forms.Init();

            this.LoadApplication(new SimpleDemo.App());
        }
    }
}