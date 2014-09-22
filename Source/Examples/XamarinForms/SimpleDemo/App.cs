namespace SimpleDemo
{

    using Xamarin.Forms;

    using OxyPlot;
    using OxyPlot.XamarinForms;

    public class App
    {
        public static Page GetMainPage ()
        {   
            var plotModel = new PlotModel { Title = "Hello Xamarin.Forms" };

            return new ContentPage { 
                Content = new PlotView {
                    Model = plotModel,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                },
            };
        }
    }
}

