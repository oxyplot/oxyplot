// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearBarView.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AnimationsDemo
{
    using System.Windows;

    public partial class AnimationSettingsControl
    {
        public AnimationSettingsControl()
        {
            this.InitializeComponent();
        }

        private async void OnAnimateClick(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as IAnimationViewModel;
            if (vm != null)
            {
                await vm.AnimateAsync();
            }
        }
    }
}