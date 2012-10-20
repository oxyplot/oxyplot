// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotFrame.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
#if RENDER_BY_DRAWINGCONTEXT
namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    // <summary>
    // Represents a control that renders a PlotModel using a DrawingContext.
    // This should give the highest performance with WPF.
    // The problem is currently to mix aliased (axes) and anti-aliased (curves) elements...
    /// Currently we are rendering to two different PlotFrames, with and without the Alias property set.
    /// </summary>
    internal class PlotFrame : FrameworkElement
    {
        private readonly bool aliased;

        public bool Aliased
        {
            get { return aliased; }
        }

        public PlotFrame(bool aliased = false)
        {
            this.aliased = aliased;
            SetValue(RenderOptions.ClearTypeHintProperty, ClearTypeHint.Enabled);

            if (this.aliased)
                SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        }

        public PlotModel Model { get; set; }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (Model == null)
                return;

            var drc = new DrawingRenderContext(dc, ActualWidth, ActualHeight);

            if (aliased)
            {
                Model.RenderInit(drc);
                Model.RenderAxes(drc);
                Model.RenderBox(drc);
            }
            else
            {
                Model.RenderSeries(drc);
            }
        }
    }
}
#endif