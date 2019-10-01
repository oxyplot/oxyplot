// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotBase.Events.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    public partial class PlotBase
    {
        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.KeyDown" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Handled)
            {
                return;
            }

            var args = new OxyKeyEventArgs { ModifierKeys = Keyboard.GetModifierKeys(), Key = e.Key.Convert() };
            e.Handled = this.ActualController.HandleKeyDown(this, args);
        }

        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.ManipulationStarted" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        {
            base.OnManipulationStarted(e);
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleTouchStarted(this, e.ToTouchEventArgs(this));
        }

        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.ManipulationDelta" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            base.OnManipulationDelta(e);
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleTouchDelta(this, e.ToTouchEventArgs(this));
        }

        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.ManipulationCompleted" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            base.OnManipulationCompleted(e);
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleTouchCompleted(this, e.ToTouchEventArgs(this));
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseWheel" /> event occurs to provide handling for the event in a derived class without attaching a delegate.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Input.MouseWheelEventArgs" /> that contains the event data.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Handled || !this.IsMouseWheelEnabled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseWheel(this, e.ToMouseWheelEventArgs(this));
        }

        /// <summary>
        /// Invoked when an unhandled MouseDown attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Handled)
            {
                return;
            }

            this.Focus();
            this.CaptureMouse();

            // store the mouse down point, check it when mouse button is released to determine if the context menu should be shown
            this.mouseDownPoint = e.GetPosition(this).ToScreenPoint();

            e.Handled = this.ActualController.HandleMouseDown(this, e.ToMouseDownEventArgs(this));
        }

        /// <summary>
        /// Invoked when an unhandled MouseMove attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            UpdateToolTip();

            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseMove(this, e.ToMouseEventArgs(this));
        }

        /// <summary>
        /// Returns true if the event is handled.
        /// </summary>
        /// <returns></returns>
        private bool HandleTitleToolTip(ScreenPoint sp)
        {
            bool v = this.ActualModel.TitleArea.Contains(sp);

            if (v)
            {
                if (this.ToolTip == null || string.IsNullOrEmpty(this.ToolTip.ToString()))
                {
                    this.ToolTip = this.ActualModel.TitleToolTip;
                }

                this.previouslyHoveredPlotElement = null;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the event is handled.
        /// </summary>
        /// <returns></returns>
        private bool HandlePlotElementsToolTip(ScreenPoint sp)
        {
            bool found = false;

            // it may be possible that the 5 constant in this line needs to be replaced with some other value
            System.Collections.Generic.IEnumerable<HitTestResult> r = this.ActualModel.HitTest(new HitTestArguments(sp, 5));

            foreach (HitTestResult rtr in r)
            {
                if (rtr.Element != null)
                {
                    if (rtr.Element is PlotElement pe)
                    {
                        if (pe != this.previouslyHoveredPlotElement)
                        {
                            this.ToolTip = pe.ToolTip;
                        }
                        this.previouslyHoveredPlotElement = pe;
                        found = true;
                    }
                    break;
                }
            }

            if (!found)
            {
                this.previouslyHoveredPlotElement = null;
            }

            return found;
        }

        /// <summary>
        /// Updates the custom tooltip system's tooltip.
        /// </summary>
        private void UpdateToolTip()
        {
            if (this.ActualModel == null || !this.UseCustomToolTipSystem)
            {
                return;
            }

            ScreenPoint sp = Mouse.GetPosition(this).ToScreenPoint();


            bool handleTitle = HandleTitleToolTip(sp);
            bool handleOthers = HandlePlotElementsToolTip(sp);

            if (!handleTitle && !handleOthers)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    System.Windows.Controls.ToolTipService.SetIsEnabled(this, false);
                }), System.Windows.Threading.DispatcherPriority.Render);
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.ToolTip = null;
                }), System.Windows.Threading.DispatcherPriority.Render);
                return;
            }

            if (handleTitle)
            {
                UpdateToolTipVisibility();
            }
            else if (handleOthers)
            {
                UpdateToolTipVisibility();
            }
        }

        private void UpdateToolTipVisibility()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (this.ToolTip == null)
                {
                    System.Windows.Controls.ToolTipService.SetIsEnabled(this, false);
                }
                else
                {
                    System.Windows.Controls.ToolTipService.SetIsEnabled(this, true);
                }
            }), System.Windows.Threading.DispatcherPriority.Render);
        }

        /// <summary>
        /// Invoked when an unhandled MouseUp routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the mouse button was released.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Handled)
            {
                return;
            }

            this.ReleaseMouseCapture();

            e.Handled = this.ActualController.HandleMouseUp(this, e.ToMouseReleasedEventArgs(this));

            // Open the context menu
            var p = e.GetPosition(this).ToScreenPoint();
            var d = p.DistanceTo(this.mouseDownPoint);

            if (this.ContextMenu != null)
            {
                if (Math.Abs(d) < 1e-8 && e.ChangedButton == MouseButton.Right)
                {
                    // TODO: why is the data context not passed to the context menu??
                    this.ContextMenu.DataContext = this.DataContext;
                    this.ContextMenu.PlacementTarget = this;
                    this.ContextMenu.Visibility = System.Windows.Visibility.Visible;
                    this.ContextMenu.IsOpen = true;
                }
                else
                {
                    this.ContextMenu.Visibility = System.Windows.Visibility.Collapsed;
                    this.ContextMenu.IsOpen = false;
                }
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseEnter" /> attached event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            UpdateToolTip();
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseEnter(this, e.ToMouseEventArgs(this));
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseLeave" /> attached event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            UpdateToolTip();
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseLeave(this, e.ToMouseEventArgs(this));
        }
    }
}
