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

        private void OnAnimateClick(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as LinearBarViewModel;
            if (vm != null)
            {
                vm.Animate();
            }
        }
    }
}