// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
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
//   Represents a view that can show a PlotModel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.XamarinAndroid
{
	using Android.Content;
	using Android.Graphics;
	using Android.Util;
	using Android.Views;

	using OxyPlot;

	/// <summary>
	/// Represents a view that can show a <see cref="PlotModel"/>.
	/// </summary>
	public class PlotView : View, IPlotControl
	{
		/// <summary>
		/// The rendering lock object.
		/// </summary>
		private readonly object renderingLock = new object();

		/// <summary>
		/// The invalidation lock object.
		/// </summary>
		private readonly object invalidateLock = new object();

		/// <summary>
		/// The current model.
		/// </summary>
		private PlotModel model;

		/// <summary>
		/// The current render context.
		/// </summary>
		private CanvasRenderContext rc;

		/// <summary>
		/// The model invalidated flag.
		/// </summary>
		private bool isModelInvalidated;

		/// <summary>
		/// The update data flag.
		/// </summary>
		private bool updateDataFlag = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="PlotView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="attrs">The attribute set.</param>
		public PlotView(Context context, IAttributeSet attrs) :
		base(context, attrs)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PlotView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="attrs">The attribute set.</param>
		/// <param name="defStyle">The definition style.</param>
		public PlotView(Context context, IAttributeSet attrs, int defStyle) :
		base(context, attrs, defStyle)
		{
		}

		/// <summary>
		/// Gets or sets the plot model.
		/// </summary>
		/// <value>
		/// The model.
		/// </value>
		public PlotModel Model
		{
			get
			{
				return this.model;
			}

			set
			{
				if (this.model != value)
				{
					this.model = value;
					this.InvalidatePlot(true);
				}
			}
		}

		/// <summary>
		/// Gets the actual <see cref="PlotModel" /> of the control.
		/// </summary>
		public PlotModel ActualModel
		{
			get
			{
				return this.Model;
			}
		}

		/// <summary>
		/// Hides the tracker.
		/// </summary>
		public void HideTracker()
		{
		}

		/// <summary>
		/// Hides the zoom rectangle.
		/// </summary>
		public void HideZoomRectangle()
		{
		}

		/// <summary>
		/// Invalidates the plot (not blocking the UI thread)
		/// </summary>
		/// <param name="updateData">if set to <c>true</c>, all data bindings will be updated.</param>
		public void InvalidatePlot(bool updateData)
		{
			lock (this.invalidateLock)
			{
				this.isModelInvalidated = true;
				this.updateDataFlag = this.updateDataFlag || updateData;
			}

			this.Invalidate();
		}

		/// <summary>
		/// Refreshes the plot immediately (blocking UI thread)
		/// </summary>
		/// <param name="updateData">if set to <c>true</c>, all data collections will be updated.</param>
		public void RefreshPlot(bool updateData)
		{
			this.InvalidatePlot(updateData);
		}

		/// <summary>
		/// Sets the cursor type.
		/// </summary>
		/// <param name="cursorType">The cursor type.</param>
		public void SetCursorType(CursorType cursorType)
		{
		}

		/// <summary>
		/// Shows the tracker.
		/// </summary>
		/// <param name="trackerHitResult">The tracker data.</param>
		public void ShowTracker(TrackerHitResult trackerHitResult)
		{
		}

		/// <summary>
		/// Shows the zoom rectangle.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		public void ShowZoomRectangle(OxyRect rectangle)
		{
		}

		/// <summary>
		/// Stores text on the clipboard.
		/// </summary>
		/// <param name="text">The text.</param>
		public void SetClipboardText(string text)
		{
		}

		/// <summary>
		/// Draws the content of the control.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);
			var actualModel = this.ActualModel;
			if (actualModel == null)
			{
				return;
			}

			var background = actualModel.Background.IsVisible () ? actualModel.Background : OxyColors.White;
			canvas.DrawColor(background.ToColor());

			lock (this.invalidateLock)
			{
				if (this.isModelInvalidated)
				{
					actualModel.Update(this.updateDataFlag);
					this.updateDataFlag = false;
					this.isModelInvalidated = false;
				}
			}

			lock (this.renderingLock)
			{
				if (this.rc == null)
				{
					this.rc = new CanvasRenderContext();
				}

				this.rc.SetTarget(canvas);
				using (var bounds = new Rect())
				{
					canvas.GetClipBounds(bounds);
					actualModel.Render(this.rc, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
				}
			}
		}
	}
}