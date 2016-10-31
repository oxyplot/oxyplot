﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserControl1.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for UserControl1.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AvaloniaExamples.Examples.UserControlDemo
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : Avalonia.Controls.UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}