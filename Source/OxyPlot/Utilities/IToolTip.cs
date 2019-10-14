namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IToolTip
    {
        void Show();
        void Hide();
        string Text { get; set; }
        void Dispose();
    }
}
