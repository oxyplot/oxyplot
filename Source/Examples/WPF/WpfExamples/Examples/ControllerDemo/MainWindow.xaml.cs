// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControllerDemo
{
    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Shows how to define a Controller.")]
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.plotView2.ActualController.UnbindAll();
        }
   }
}