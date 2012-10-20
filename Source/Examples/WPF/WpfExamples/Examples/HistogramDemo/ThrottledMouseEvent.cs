// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrottledMouseEvent.cs" company="OxyPlot">
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
// <summary>
//   Creates a 'throttled' MouseMove event which ensures that the UI
//   rendering is not starved.
//   see: http://www.scottlogic.co.uk/blog/colin/2010/06/throttling-silverlight-mouse-events-to-keep-the-ui-responsive/
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace HistogramDemo
{
  /// <summary>
  /// Creates a 'throttled' MouseMove event which ensures that the UI
  /// rendering is not starved.
  /// see: http://www.scottlogic.co.uk/blog/colin/2010/06/throttling-silverlight-mouse-events-to-keep-the-ui-responsive/
  /// </summary>
  public class ThrottledMouseMoveEvent
  {
    private bool _awaitingRender = false;

    private UIElement _element;

    public ThrottledMouseMoveEvent(UIElement element)
    {
      _element = element;
      element.MouseMove += new MouseEventHandler(Element_MouseMove);
    }

    public event MouseEventHandler ThrottledMouseMove;

    private void Element_MouseMove(object sender, MouseEventArgs e)
    {
      if (!_awaitingRender)
      {
        // if we are not awaiting a render as a result of a previously handled event
        // raise a ThrottledMouseMove event, and add a Rendering handler so that
        // we can determine when this event has been acted upon.
        OnThrottledMouseMove(e);
        _awaitingRender = true;
        CompositionTarget.Rendering += CompositionTarget_Rendering;
      }
    }

    private void CompositionTarget_Rendering(object sender, EventArgs e)
    {
      _awaitingRender = false;
      CompositionTarget.Rendering -= CompositionTarget_Rendering;
    }

    /// <summary>
    /// Raises the ThrottledMouseMove event
    /// </summary>
    protected void OnThrottledMouseMove(MouseEventArgs args)
    {
      if (ThrottledMouseMove != null)
      {
        ThrottledMouseMove(_element, args);
      }
    }
  }
}