namespace SimpleDemoFsharp

open OxyPlot
open OxyPlot.Series

type MainViewModel() =
    let myModel = PlotModel()
    do
        myModel.Series.Add(FunctionSeries(cos, 0.0, 10.0, 0.1, "cos(x)"))
    member mainWindow.MyModel with get() = myModel