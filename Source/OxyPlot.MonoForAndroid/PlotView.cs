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
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.MonoForAndroid
{
    using Android.Content;
    using Android.Graphics;
    using Android.Util;
    using Android.Views;

    using OxyPlot;

    public class PlotView : View
    {
        private PlotModel model;

        public PlotModel Model
        {
            get
            {
                return this.model;
            }
            set
            {
                if (this.model == value)
                {
                    return;
                }

                this.model = value;
                this.OnModelChanged();
            }
        }

        private void OnModelChanged()
        {
            this.InvalidatePlot(true);
        }

        private readonly object invalidateLock = new object();
        private bool isModelInvalidated;
        private bool updateDataFlag = true;

        public void InvalidatePlot(bool updateData)
        {
            lock (this.invalidateLock)
            {
                this.isModelInvalidated = true;
                this.updateDataFlag = this.updateDataFlag || updateData;
            }

            this.Invalidate();
        }

        public PlotView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            this.Initialize();
        }

        public PlotView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            this.Initialize();
        }

        private CanvasRenderContext rc;

        private void Initialize()
        {
            this.rc = new CanvasRenderContext();
        }

        private readonly object renderingLock = new object();

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawColor(Color.White);

            lock (this.invalidateLock)
            {
                if (this.isModelInvalidated)
                {
                    if (this.model != null)
                    {
                        this.model.Update(this.updateDataFlag);
                        this.updateDataFlag = false;
                    }

                    this.isModelInvalidated = false;
                }
            }

            lock (this.renderingLock)
            {
                if (this.model != null)
                {
                    this.rc.SetTarget(canvas);
                    using (var bounds = new Rect())
                    {
                        canvas.GetClipBounds(bounds);

                        // Render the background
                        if (this.model.Background != null)
                        {
                            // TODO: can we set this.Background instead?
                            rc.DrawRectangle(new OxyRect(bounds.Left, bounds.Top, bounds.Width(), bounds.Height()), this.model.Background, null, 0);
                        }
                        
                        this.model.Render(rc, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
                    }
                }
            }

            /*
            canvas.DrawColor(Color.White);

            using (var paint = new Paint())
            {
                paint.AntiAlias = true;
                paint.Color = new Color(0, 0, 0, 220);
                paint.StrokeWidth = 2;
                canvas.DrawLine(10f, 10f, canvas.Width - 10, h - 10, paint);
                canvas.DrawLine(canvas.Width - 10, 10f, 10, h - 10, paint);
                paint.TextSize = 24;
                paint.TextAlign = Paint.Align.Left;
                var textbounds = new Rect();
                paint.GetTextBounds("ABC", 0, 1, textbounds);
                var tw = paint.MeasureText("ABC");
                // canvas.DrawText(this.Text, 40, 200, paint);
                canvas.Translate(40, 40);
                canvas.DrawText("ABC=" + tw + "/" + textbounds, 0, 0, paint);
                canvas.Rotate(20);
                canvas.DrawText("ABC=" + tw, 0, 0, paint);
            }*/
        }
    }
}