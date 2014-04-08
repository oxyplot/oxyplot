// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Gtk;
using System;
using OxyPlot;
using OxyPlot.Series;

class Hello
{

    static void Main()
    {
        Application.Init();

        Window window = new Window("helloworld");
        var pm = new PlotModel
        {
            Title = "Trigonometric functions",
            Subtitle = "Example using the FunctionSeries",
            PlotType = PlotType.Cartesian,
            Background = OxyColors.White
        };
        pm.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)") { Color = OxyColors.Black });
        pm.Series.Add(new FunctionSeries(Math.Cos, -10, 10, 0.1, "cos(x)") { Color = OxyColors.Green });
        pm.Series.Add(new FunctionSeries(t => 5 * Math.Cos(t), t => 5 * Math.Sin(t), 0, 2 * Math.PI, 0.1, "cos(t),sin(t)") { Color = OxyColors.Yellow });
        var plot = new OxyPlot.GtkSharp.Plot() { Model = pm };
        plot.SetSizeRequest(400, 400);
        plot.Visible = true;
        window.SetSizeRequest(600, 600);
        window.Add(plot);
        window.Focus = plot;
        window.Show();
        window.DeleteEvent += (delegate(object sender, DeleteEventArgs a)
        {
            Application.Quit();
            a.RetVal = true;
        });

        Application.Run();

    }
}