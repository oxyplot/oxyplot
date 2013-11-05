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
        var pm = new PlotModel("Trigonometric functions", "Example using the FunctionSeries")
                     {
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
