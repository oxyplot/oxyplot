open System
open System.Windows

[<STAThread>]
[<EntryPoint>]
let main argv = 

    let mainWindow = Application.LoadComponent(
                        new System.Uri("/SimpleDemoFsharp;component/MainWindow.xaml", UriKind.Relative)) :?> Window
    
    let application = new Application()
    application.Run(mainWindow)