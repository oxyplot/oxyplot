using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace OxyPlot.Avalonia.Themes
{
    class Default : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Default"/> class.
        /// </summary>
        public Default()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
