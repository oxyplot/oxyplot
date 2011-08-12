namespace $safeprojectname$ {
    using System.ComponentModel.Composition;

    [Export(typeof(IShell))]
    public class ShellViewModel : IShell {}
}