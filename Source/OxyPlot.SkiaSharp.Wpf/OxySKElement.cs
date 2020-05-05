// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxySKElement.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

/*

This is a modified copy of https://github.com/mono/SkiaSharp/blob/2550d14410839ebba2d541ec5b423261b9236197/source/SkiaSharp.Views/SkiaSharp.Views.WPF/SKElement.cs.
License of the original file:

------------------------------------------

Copyright (c) 2015-2016 Xamarin, Inc.
Copyright (c) 2017-2018 Microsoft Corporation.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

------------------------------------------

The modifications are a workaround for https://github.com/mono/SkiaSharp/issues/1236.
Once the issue is fixed on their side, we should remove this file and replace usages by SKElement from the SkiaSharp.Views.WPF nuget package.

*/

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace OxyPlot.SkiaSharp.Wpf
{
    using global::SkiaSharp;
    using global::SkiaSharp.Views.Desktop;
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class OxySKElement : FrameworkElement
    {
        private readonly bool designMode;
        private WriteableBitmap bitmap;
        private bool ignorePixelScaling;

        public OxySKElement()
        {
            this.designMode = DesignerProperties.GetIsInDesignMode(this);
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
        }

        [Category("Appearance")]
        public event EventHandler<SKPaintSurfaceEventArgs> PaintSurface;

        [Bindable(false)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SKSize CanvasSize => this.bitmap == null ? SKSize.Empty : new SKSize(this.bitmap.PixelWidth, this.bitmap.PixelHeight);

        public bool IgnorePixelScaling
        {
            get { return this.ignorePixelScaling; }
            set
            {
                this.ignorePixelScaling = value;
                this.InvalidateVisual();
            }
        }

        protected virtual void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            // invoke the event
            PaintSurface?.Invoke(this, e);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.designMode)
            {
                return;
            }

            if (this.Visibility != Visibility.Visible)
            {
                return;
            }

            var size = this.CreateSize(out var scaleX, out var scaleY);
            if (size.Width <= 0 || size.Height <= 0)
            {
                return;
            }

            var info = new SKImageInfo(size.Width, size.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

            // reset the bitmap if the size has changed
            if (this.bitmap == null || info.Width != this.bitmap.PixelWidth || info.Height != this.bitmap.PixelHeight)
            {
                this.bitmap = new WriteableBitmap(size.Width, size.Height, 96 * scaleX, 96 * scaleY, PixelFormats.Pbgra32, null);
            }

            // draw on the bitmap
            this.bitmap.Lock();
            using (var surface = SKSurface.Create(info, this.bitmap.BackBuffer, this.bitmap.BackBufferStride))
            {
                this.OnPaintSurface(new SKPaintSurfaceEventArgs(surface, info));
            }

            // draw the bitmap to the screen
            this.bitmap.AddDirtyRect(new Int32Rect(0, 0, size.Width, size.Height));
            this.bitmap.Unlock();

            // get window to screen offset
            var visualOffset = this.TransformToAncestor(Window.GetWindow(this)).Transform(default);

            // calculate offset to physical pixels
            var offsetX = ((visualOffset.X * scaleX) % 1) / scaleX;
            var offsetY = ((visualOffset.Y * scaleY) % 1) / scaleY;

            drawingContext.DrawImage(this.bitmap, new Rect(-offsetX, -offsetY, this.bitmap.Width, this.bitmap.Height));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            this.InvalidateVisual();
        }

        private SKSizeI CreateSize(out double scaleX, out double scaleY)
        {
            scaleX = 1.0;
            scaleY = 1.0;

            var w = this.ActualWidth;
            var h = this.ActualHeight;

            if (!IsPositive(w) || !IsPositive(h))
            {
                return SKSizeI.Empty;
            }

            if (this.IgnorePixelScaling)
            {
                return new SKSizeI((int)w, (int)h);
            }

            var m = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            scaleX = m.M11;
            scaleY = m.M22;
            return new SKSizeI((int)(w * scaleX), (int)(h * scaleY));

            static bool IsPositive(double value)
            {
                return !double.IsNaN(value) && !double.IsInfinity(value) && value > 0;
            }
        }
    }
}
