// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;

using Xamarin.Forms;

[assembly: AssemblyTitle("OxyPlot.XamarinFormsIOS")]
[assembly: AssemblyDescription("OxyPlot for Xamarin.Forms/Xamarin.iOS")]

[assembly: ExportRenderer(typeof(OxyPlot.XamarinForms.PlotView), typeof(OxyPlot.XamarinFormsIOS.PlotViewRenderer))]
