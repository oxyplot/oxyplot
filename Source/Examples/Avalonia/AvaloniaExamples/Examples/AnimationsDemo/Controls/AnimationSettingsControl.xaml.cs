// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearBarView.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.AnimationsDemo.Controls
{
    using Avalonia.Controls;

    public partial class AnimationSettingsControl : UserControl
    {
        public AnimationSettingsControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
            this.FindNameScope().Get<Button>("Animate").Click += OnAnimateClick;
        }

        private async void OnAnimateClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var vm = this.DataContext as IAnimationViewModel;
            if (vm != null)
            {
                await vm.AnimateAsync();
            }
        }
    }
}