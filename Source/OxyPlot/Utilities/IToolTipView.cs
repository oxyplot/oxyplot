// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IToolTip.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// <summary>
//   A wrapper around native ToolTip objects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Threading;

    /// <summary>
    /// A wrapper around native ToolTip objects.
    /// </summary>
    public interface IToolTipView : System.IDisposable
    {
        /// <summary>
        /// Gets or sets the string representation of the tooltip.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the length of time before a tooltip opens.
        /// </summary>
        int InitialShowDelay { get; set; }

        /// <summary>
        /// Gets or sets the amount of time that a tooltip remains visible.
        /// </summary>
        /// <remarks>
        /// On both WinForms and WPF, the actual duration of the tooltip is longer than this with about 2 seconds, maybe because of the animations.
        /// </remarks>
        int ShowDuration { get; set; }

        /// <summary>
        /// Gets or sets the maximum time between the display of two tooltips where the second tooltip appears without a delay.
        /// </summary>
        int BetweenShowDelay { get; set; }

        /// <summary>
        /// Shows the tooltip with the initial delay.
        /// </summary>
        void ShowWithInitialDelay(CancellationToken ct);

        /// <summary>
        /// Hides the tooltip.
        /// </summary>
        void Hide();
    }
}
