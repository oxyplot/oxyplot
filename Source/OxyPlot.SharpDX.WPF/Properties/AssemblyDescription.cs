// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyDescription.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyTitle("OxyPlot for WPF with SharpDX based renderer")]
[assembly: AssemblyDescription("OxyPlot controls for WPF with SharpDX based renderer")]
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

[assembly: CLSCompliant(true)]

[assembly: XmlnsPrefix("http://oxyplot.org/sharpdx/wpf", "oxydx")]
[assembly: XmlnsDefinition("http://oxyplot.org/sharpdx/wpf", "OxyPlot.SharpDX.Wpf")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("92129d21-61e9-4952-ab75-6e5b27c8d3f7")]