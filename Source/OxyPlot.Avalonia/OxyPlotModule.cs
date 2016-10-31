namespace OxyPlot.Avalonia
{
    using global::Avalonia;
    using global::Avalonia.Styling;

    public static class OxyPlotModule
    {
        public static void Initialize()
        {
            AvaloniaLocator.Current.GetService<IGlobalStyles>().Styles.AddRange(new Themes.Default());
        }
    }
}
