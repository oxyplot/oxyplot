// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyDescription.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyTitle("OxyPlot for WPF")]
[assembly: AssemblyDescription("OxyPlot controls for WPF")]
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

[assembly: CLSCompliant(false)]

[assembly: XmlnsPrefix("http://oxyplot.org/wpf", "oxy")]
[assembly: XmlnsDefinition("http://oxyplot.org/wpf", "OxyPlot.Wpf")]