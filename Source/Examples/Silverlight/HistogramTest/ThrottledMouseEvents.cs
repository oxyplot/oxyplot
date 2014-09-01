// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrottledMouseEvents.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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

namespace Histogram
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