namespace IssueDemos.Pages
{
    using System;

    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Xamarin.Forms;

    using Xamarin.Forms;

    [DemoPage ("TabbedPage with ItemTemplate", "Pages defined in ItemTemplate")]
    public class TabbedPageWithItemTemplate : TabbedPage
    {
        public TabbedPageWithItemTemplate ()
        {
            this.Title = "TabbedPage with ItemTemplate";

            this.ItemsSource = new[] {
                new PageDefinition { Name = "Page 1", Color = Color.Red },
                new PageDefinition { Name = "Page 2", Color = Color.Yellow },
                new PageDefinition { Name = "Page 3", Color = Color.Green },
            };

            this.ItemTemplate = new DataTemplate (() => new SubPage ());
        }

        public class PageDefinition
        {
            public string Name { get; set; }

            public Color Color { get; set; }

            public override string ToString ()
            {
                return Name;
            }
        }

        public class SubPage : ContentPage
        {
            public SubPage ()
            {
                this.SetBinding (Page.TitleProperty, "Name");

                var plotModel = new PlotModel { Title = "f(x)=Math.Sqrt(x)" };
                plotModel.Series.Add (new FunctionSeries (Math.Sqrt, 0, 40, 200));

                var plotView = new PlotView {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Model = plotModel
                };

                plotView.SetBinding (VisualElement.BackgroundColorProperty, "Color");

                this.Content = plotView;
            }
        }
    }
}
