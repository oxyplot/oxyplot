
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
            Initialize();
        }

        public PlotView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
        }

        private readonly object renderingLock = new object();
        private readonly object modelLock = new object();

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
                    var rc = new CanvasRenderContext(canvas);
                    this.model.Render(rc);
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