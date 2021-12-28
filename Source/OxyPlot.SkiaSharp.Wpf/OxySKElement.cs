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

Mmodifications include
 - A workaround for https://github.com/mono/SkiaSharp/issues/1236. Once the issue is fixed on their side, we should remove this file and replace usages by SKElement from the SkiaSharp.Views.WPF nuget package.
 - Simple RenderTransform detection to allow for non-unity scaling per https://github.com/oxyplot/oxyplot/issues/1785.

*/

namespace OxyPlot.SkiaSharp.Wpf
{
    using global::SkiaSharp;
    using global::SkiaSharp.Views.Desktop;
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Provides a surface on which to render with the SkiaSharp graphics APIs.
    /// </summary>
    public class OxySKElement : FrameworkElement
    {
        /// <summary>
        /// A value indicating whether the element is being presented in design mode. 
        /// </summary>
        private readonly bool designMode;

        /// <summary>
        /// The bitmap to which to render.
        /// </summary>
        private WriteableBitmap bitmap;

        /// <summary>
        /// A value indicating whether to ignore pixel scaling when determining the render buffer bitmap dimensions.
        /// </summary>
        private bool ignorePixelScaling;

        /// <summary>
        /// Initialises an instance of the <see cref="OxySKElement"/> class.
        /// </summary>
        public OxySKElement()
        {
            this.designMode = DesignerProperties.GetIsInDesignMode(this);
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
        }

        /// <summary>
        /// Invoked when the surface is painting.
        /// </summary>
        [Category("Appearance")]
        public event EventHandler<SKPaintSurfaceEventArgs> PaintSurface;

        /// <summary>
        /// Gets the size of the render buffer bitmap, or <c>SKSize.Empty</c> if there is no current render buffer bitmap.
        /// </summary>
        [Bindable(false)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SKSize CanvasSize => this.bitmap == null ? SKSize.Empty : new SKSize(this.bitmap.PixelWidth, this.bitmap.PixelHeight);

        /// <summary>
        /// Gets or sets a value indicating whether to ignore pixel scaling when determining the render buffer bitmap dimensions.
        /// </summary>
        public bool IgnorePixelScaling
        {
            get { return this.ignorePixelScaling; }
            set
            {
                this.ignorePixelScaling = value;
                this.InvalidateVisual();
            }
        }

        /// <summary>
        /// Raises the <see cref="PaintSurface"/> event with the given <see cref="SKPaintSurfaceEventArgs"/>.
        /// </summary>
        /// <param name="e">The skia surface and associated information ready for drawing.</param>
        protected virtual void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            // invoke the event
            PaintSurface?.Invoke(this, e);
        }

        /// <inheritdoc/>
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
            var renderScale = this.GetRenderScale();

            // scale bitmap according to the renderScale
            var bitmapPixelWidth = (int)(size.Width * renderScale);
            var bitmapPixelHeight = (int)(size.Height * renderScale);

            if (size.Width <= 0 || size.Height <= 0)
            {
                return;
            }

            var info = new SKImageInfo(bitmapPixelWidth, bitmapPixelHeight, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

            // reset the bitmap if the size has changed
            if (this.bitmap == null || info.Width != this.bitmap.PixelWidth || info.Height != this.bitmap.PixelHeight)
            {
                this.bitmap = new WriteableBitmap(bitmapPixelWidth, bitmapPixelHeight, 96 * scaleX, 96 * scaleY, PixelFormats.Pbgra32, null);
            }

            // draw on the bitmap
            this.bitmap.Lock();
            using (var surface = SKSurface.Create(info, this.bitmap.BackBuffer, this.bitmap.BackBufferStride))
            {
                this.OnPaintSurface(new SKPaintSurfaceEventArgs(surface, info));
            }

            // draw the bitmap to the screen
            this.bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmapPixelWidth, bitmapPixelHeight));
            this.bitmap.Unlock();

            // get window to screen offset
            var ancestor = GetAncestorVisualFromVisualTree(this);
            var visualOffset = ancestor != null ? this.TransformToAncestor(ancestor).Transform(default) : default;

            // calculate offset to physical pixels
            var offsetX = ((visualOffset.X * scaleX) % 1) / scaleX;
            var offsetY = ((visualOffset.Y * scaleY) % 1) / scaleY;

            // draw, scaling back down from the (rounded) bitmap dimensions
            drawingContext.DrawImage(this.bitmap, new Rect(-offsetX, -offsetY, this.bitmap.Width / renderScale, this.bitmap.Height / renderScale));
        }

        /// <inheritdoc/>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            this.InvalidateVisual();
        }

        /// <summary>
        /// Determines the size of the render buffer bitmap, and the scale at which to draw.
        /// </summary>
        /// <param name="scaleX">The horizontal scale.</param>
        /// <param name="scaleY">The vertical scale.</param>
        /// <returns>The size in pixels of the bitmap.</returns>
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

            var compositionTarget = PresentationSource.FromVisual(this)?.CompositionTarget;
            if (compositionTarget != null)
            {
                var m = compositionTarget.TransformToDevice;
                scaleX = m.M11;
                scaleY = m.M22;
            }
            return new SKSizeI((int)(w * scaleX), (int)(h * scaleY));

            static bool IsPositive(double value)
            {
                return !double.IsNaN(value) && !double.IsInfinity(value) && value > 0;
            }
        }

        /// <summary>
        /// Returns a reference to the visual object that hosts the dependency object in the visual tree.
        /// </summary>
        /// <returns> The host visual from the visual tree.</returns>
        private Visual GetAncestorVisualFromVisualTree(DependencyObject startElement)
        {
            DependencyObject child = startElement;
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                child = parent;
                parent = VisualTreeHelper.GetParent(child);
            }
            return child is Visual visualChild ? visualChild : Window.GetWindow(this);
        }

        /// <summary>
        /// Determines the scaling transform applied to the control.
        /// </summary>
        /// <returns>The scale factor.</returns>
        public virtual double GetRenderScale()
        {
            var transform = VisualTreeHelper.GetTransform(this)?.Value ?? Matrix.Identity;
            DependencyObject control = VisualTreeHelper.GetParent(this);
            while (control != null)
            {
                if (control is Visual v && VisualTreeHelper.GetTransform(v) is Transform vt)
                {
                    transform *= vt.Value;
                }
                control = VisualTreeHelper.GetParent(control);
            }

            return Math.Max(transform.M11, transform.M22);
        }
    }
}
