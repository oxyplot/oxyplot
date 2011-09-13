namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Input;

    public class InputBindingX : InputBinding
    {
        public InputGesture Gezture
        {
            get { return (InputGesture)GetValue(GeztureProperty); }
            set { SetValue(GeztureProperty, value); }
        }

        public static readonly DependencyProperty GeztureProperty =
            DependencyProperty.Register("Gezture", typeof(InputGesture), typeof(InputBindingX), new UIPropertyMetadata(null, GeztureChanged));

        private static void GeztureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((InputBindingX)d).OnGeztureChanged();
        }

        protected virtual void OnGeztureChanged()
        {
            this.Gesture = Gezture;
        }
    }
}