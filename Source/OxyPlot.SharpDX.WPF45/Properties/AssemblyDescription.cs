// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyDescription.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyTitle("OxyPlot for WPF with SharpDX based renderer")]
[assembly: AssemblyDescription("OxyPlot controls for WPF with SharpDX based renderer")]
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

[assembly: CLSCompliant(false)]

[assembly: XmlnsPrefix("http://oxyplot.org/sharpdx/wpf", "oxydx")]
[assembly: XmlnsDefinition("http://oxyplot.org/sharpdx/wpf", "OxyPlot.SharpDX.WPF")]