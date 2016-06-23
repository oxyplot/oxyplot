// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotImage.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SharpDX.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using OxyPlot.SharpDX;
    using global::SharpDX;
    using global::SharpDX.Direct3D;
    using global::SharpDX.Direct3D11;
    using global::SharpDX.DXGI;
    using AlphaMode = global::SharpDX.Direct2D1.AlphaMode;
    using D3D11Device = global::SharpDX.Direct3D11.Device;
    using D3D9CreateFlags = global::SharpDX.Direct3D9.CreateFlags;
    using D3D9Device = global::SharpDX.Direct3D9.DeviceEx;
    using D3D9DeviceType = global::SharpDX.Direct3D9.DeviceType;
    using D3D9PresentInterval = global::SharpDX.Direct3D9.PresentInterval;
    using D3D9PresentParameters = global::SharpDX.Direct3D9.PresentParameters;
    using D3D9SwapEffect = global::SharpDX.Direct3D9.SwapEffect;
    using Direct3D = global::SharpDX.Direct3D9.Direct3DEx;
    using DXGIResource = global::SharpDX.DXGI.Resource;
    using PixelFormat = global::SharpDX.Direct2D1.PixelFormat;
    using RenderTarget = global::SharpDX.Direct2D1.RenderTarget;
    using RenderTargetProperties = global::SharpDX.Direct2D1.RenderTargetProperties;

    /// <summary>
    /// Control, that renders Plot using SharpDX renderer, renders Tracker using WPF controls.
    /// Adds scrolling support.
    /// </summary>
    public class PlotImage : FrameworkElement, System.Windows.Controls.Primitives.IScrollInfo
    {
        /// <summary>
        /// DependencyProperty as the backing store for PlotWidth
        /// </summary>
        public static readonly DependencyProperty PlotWidthProperty =
            DependencyProperty.Register(nameof(PlotWidth), typeof(double), typeof(PlotImage), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PlotHeight.
        /// </summary>
        public static readonly DependencyProperty PlotHeightProperty =
            DependencyProperty.Register(nameof(PlotHeight), typeof(double), typeof(PlotImage), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// DependencyProperty as the backing store for PlotModel
        /// </summary>
        public static readonly DependencyProperty PlotModelProperty =
            DependencyProperty.Register(nameof(PlotModel), typeof(IPlotModel), typeof(PlotImage), new PropertyMetadata(null));

        // stolen from scrollviewer MS source code

        // Scrolling physical "line" metrics.

        /// <summary>
        /// The scroll line delta.
        /// </summary>
        internal const double ScrollLineDelta = 16.0;   // Default physical amount to scroll with one Up/Down/Left/Right key

        /// <summary>
        /// The mouse wheel delta/
        /// </summary>
        internal const double MouseWheelDelta = 48.0;   // Default physical amount to scroll with one MouseWheel.

        /// <summary>
        /// The Direct3D11 device.
        /// </summary>
        private D3D11Device d3d11Device;

        /// <summary>
        /// The Direct3D9 device.
        /// </summary>
        private D3D9Device d3d9Device;

        /// <summary>
        /// The render target.
        /// </summary>
        private RenderTarget renderTarget;

        /// <summary>
        /// The oxy render target.
        /// </summary>
        private CacherRenderContext oxyRenderContext;

        /// <summary>
        /// The image source.
        /// </summary>
        private D3D11Image imageSource;

        /// <summary>
        /// The overlay transform.
        /// </summary>
        private TranslateTransform overlayTransform;

        /// <summary>
        /// The extent.
        /// </summary>
        private Size extent;

        /// <summary>
        /// The viewport.
        /// </summary>
        private Size viewport;

        /// <summary>
        /// The offset vector.
        /// </summary>
        private Vector offset;

        /// <summary>
        /// The placeholder image for design time.
        /// </summary>
        private BitmapImage designModeImage;

        /// <summary>
        ///  Initializes a new instance of the <see cref = "PlotImage" /> class.
        /// </summary>
        public PlotImage()
        {
            this.oxyRenderContext = new CacherRenderContext();
            this.overlayTransform = new TranslateTransform();
            this.Overlay = new System.Windows.Controls.Canvas
            {
                RenderTransform = this.overlayTransform,
            };

            this.AddVisualChild(this.Overlay);
            this.AddLogicalChild(this.Overlay);

            this.Loaded += (s, e) => this.Dispatcher.Invoke(this.InvalidateVisual, System.Windows.Threading.DispatcherPriority.Background);
            this.Unloaded += (s, e) => this.OnUnloaded();
        }

        /// <summary>
        /// Gets or sets  the plot model.
        /// </summary>
        public IPlotModel PlotModel
        {
            get { return (IPlotModel)GetValue(PlotModelProperty); }
            set { this.SetValue(PlotModelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the plot height.
        /// </summary>
        public double PlotHeight
        {
            get { return (double)GetValue(PlotHeightProperty); }
            set { this.SetValue(PlotHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the plot width.
        /// </summary>
        public double PlotWidth
        {
            get { return (double)GetValue(PlotWidthProperty); }
            set { this.SetValue(PlotWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether scrolling on the vertical axis is possible.
        /// </summary>
        public bool CanVerticallyScroll
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether scrolling on the vertical axis is possible.
        /// </summary>
        public bool CanHorizontallyScroll
        {
            get; set;
        }

        /// <summary>
        /// Gets the horizontal size of the extent.
        /// </summary>
        public double ExtentWidth
        {
            get
            {
                return this.extent.Width;
            }
        }

        /// <summary>
        /// Gets the vertical size of the extent.
        /// </summary>
        public double ExtentHeight
        {
            get
            {
                return this.extent.Height;
            }
        }

        /// <summary>
        /// Gets the horizontal size of the viewport for this content.
        /// </summary>
        public double ViewportWidth
        {
            get
            {
                return this.viewport.Width;
            }
        }

        /// <summary>
        /// Gets the vertical size of the viewport for this content.
        /// </summary>
        public double ViewportHeight
        {
            get
            {
                return this.viewport.Height;
            }
        }

        /// <summary>
        /// Gets the horizontal offset of the scrolled content.
        /// </summary>
        public double HorizontalOffset
        {
            get
            {
                return this.offset.X;
            }
        }

        /// <summary>
        /// Gets the vertical offset of the scrolled content.
        /// </summary>
        public double VerticalOffset
        {
            get
            {
                return this.offset.Y;
            }
        }

        /// <summary>
        ///  Gets the offset vector of the scrolled content.
        /// </summary>
        public Vector Offset
        {
            get
            {
                return this.offset;
            }
        }

        /// <summary>
        /// Gets or sets a System.Windows.Controls.ScrollViewer element that controls scrolling behavior.
        /// </summary>
        public ScrollViewer ScrollOwner
        {
            get; set;
        }

        /// <summary>
        /// Gets the overlay canvas.
        /// </summary>
        internal System.Windows.Controls.Canvas Overlay
        {
            get; private set;
        }

        /// <summary>
        /// Gets the number of visual child elements within this element.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Renders <see cref="PlotModel"/> to <see cref="D3D11Image"/> surface.
        /// </summary>
        /// <param name="invalidateSurface">Indicates whether on not surface should be recreated.</param>
        /// <param name="invalidateUnits">Indicates whether on not render units should be recreated.</param>
        public void Render(bool invalidateSurface, bool invalidateUnits)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            if (!this.IsLoaded)
            {
                return;
            }

            if (invalidateSurface || this.renderTarget == null)
            {
                this.CleanResources();
                this.InitRendering();

                invalidateUnits = true;
            }

            if (this.PlotModel == null)
            {
                return;
            }

            if (invalidateUnits)
            {
                this.oxyRenderContext.ResetRenderUnits();

                // Creates renderUnits that can be rendered later
                this.PlotModel.Render(this.oxyRenderContext, this.extent.Width, this.extent.Height);
            }

            var backColor = OxyColors.White;
            if (!this.PlotModel.Background.IsUndefined())
            {
                backColor = this.PlotModel.Background;
            }

            this.renderTarget.BeginDraw();
            this.renderTarget.Clear(backColor.ToDXColor());
            this.oxyRenderContext.Render(new RectangleF((float)this.offset.X, (float)this.offset.Y, (float)this.viewport.Width, (float)this.viewport.Height)); // TODO: add clip rectangle
            this.renderTarget.EndDraw();

            this.d3d11Device.ImmediateContext.Flush();

            this.imageSource.Lock();
            this.imageSource.AddDirtyRect(new Int32Rect()
            {
                X = 0,
                Y = 0,
                Height = this.imageSource.PixelHeight,
                Width = this.imageSource.PixelWidth
            });
            this.imageSource.Unlock();
        }

        /// <summary>
        /// Scrolls up within content by one logical unit.
        /// </summary>
        public void LineUp()
        {
            this.AddOffset(0, -ScrollLineDelta);
        }

        /// <summary>
        /// Scrolls down within content by one logical unit.
        /// </summary>
        public void LineDown()
        {
            this.AddOffset(0, ScrollLineDelta);
        }

        /// <summary>
        /// Scrolls left within content by one logical unit.
        /// </summary>
        public void LineLeft()
        {
            this.AddOffset(-ScrollLineDelta, 0);
        }

        /// <summary>
        /// Scrolls right within content by one logical unit.
        /// </summary>
        public void LineRight()
        {
            this.AddOffset(ScrollLineDelta, 0);
        }

        /// <summary>
        /// Scrolls up within content by one page.
        /// </summary>
        public void PageUp()
        {
            this.AddOffset(0, -this.viewport.Height);
        }

        /// <summary>
        /// Scrolls down within content by one page.
        /// </summary>
        public void PageDown()
        {
            this.AddOffset(0, this.viewport.Height);
        }

        /// <summary>
        /// Scrolls left within content by one page.
        /// </summary>
        public void PageLeft()
        {
            this.AddOffset(-this.viewport.Height, 0);
        }

        /// <summary>
        /// Scrolls right within content by one page.
        /// </summary>
        public void PageRight()
        {
            this.AddOffset(this.viewport.Height, 0);
        }

        /// <summary>
        /// Scrolls up within content after a user clicks the wheel button on a mouse.
        /// </summary>
        public void MouseWheelUp()
        {
            this.AddOffset(0, -MouseWheelDelta);
        }

        /// <summary>
        /// Scrolls down within content after a user clicks the wheel button on a mouse.
        /// </summary>
        public void MouseWheelDown()
        {
            this.AddOffset(0, MouseWheelDelta);
        }

        /// <summary>
        /// Scrolls left within content after a user clicks the wheel button on a mouse.
        /// </summary>
        public void MouseWheelLeft()
        {
            this.AddOffset(-MouseWheelDelta, 0);
        }

        /// <summary>
        /// Scrolls right within content after a user clicks the wheel button on a mouse.
        /// </summary>
        public void MouseWheelRight()
        {
            this.AddOffset(MouseWheelDelta, 0);
        }

        /// <summary>
        /// Sets the amount of horizontal offset.
        /// </summary>
        /// <param name="offset">The degree to which content is horizontally offset from the containing viewport.</param>
        public void SetHorizontalOffset(double offset)
        {
            this.offset.X = offset;
            this.InvalidateVisual();
        }

        /// <summary>
        /// Sets the amount of vertical offset.
        /// </summary>
        /// <param name="offset">The degree to which content is vertically offset from the containing viewport.</param>
        public void SetVerticalOffset(double offset)
        {
            this.offset.Y = offset;
            this.InvalidateVisual();
        }

        /// <summary>
        /// Forces content to scroll until the coordinate space of a System.Windows.Media.Visual
        /// object is visible.
        /// </summary>
        /// <param name="visual">A System.Windows.Media.Visual that becomes visible.</param>
        /// <param name="rectangle">A bounding rectangle that identifies the coordinate space to make visible.</param>
        /// <returns>A <see cref="System.Windows.Rect"/> that is visible.</returns>
        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            return rectangle;

            // TODO: implement this????????
        }

        /// <summary>
        /// Invalidates <see cref="PlotImage"/> current instance.
        /// </summary>
        internal void Invalidate()
        {
            this.Render(false, true);
            this.InvalidateVisual();
        }

        /// <summary>
        /// Overrides System.Windows.Media.Visual.GetVisualChild(<see cref="int"/>), and returns a child at the specified index from a collection of child elements.
        /// </summary>
        /// <param name="index">The zero-based index of the requested child element in the collection.</param>
        /// <returns>The requested child element. This should not return null; if the provided index is out of range, an exception is thrown.</returns>
        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.Overlay;
        }

        /// <summary>
        /// When overridden in a derived class, participates in rendering operations that
        /// are directed by the layout system. The rendering instructions for this element
        /// are not used directly when this method is invoked, and are instead preserved
        /// for later asynchronous use by layout and drawing.
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                if (this.designModeImage == null)
                {
                    var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OxyPlot.SharpDX.Wpf.Resources.designmode.png");

                    this.designModeImage = new BitmapImage();

                    this.designModeImage.BeginInit();
                    this.designModeImage.StreamSource = stream;
                    this.designModeImage.EndInit();
                }

                drawingContext.DrawImage(this.designModeImage, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            }

            if (this.imageSource != null)
            {
                drawingContext.DrawImage(this.imageSource, new Rect(0, 0, this.imageSource.Width, this.imageSource.Height));
            }
        }

        /// <summary>
        /// When overridden in a derived class, measures the size in layout required for
        /// child elements and determines a size for the System.Windows.FrameworkElement-derived
        /// class.
        /// </summary>
        /// <param name="availableSize">The available size that this element can give to child elements. Infinity can
        /// be specified as a value to indicate that the element will size to whatever content
        /// is available.
        /// </param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations
        /// of child element sizes.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            double desiredHeight;
            double desiredWidth;

            if (!double.IsNaN(this.PlotHeight))
            {
                this.extent.Height = this.PlotHeight;
                desiredHeight = Math.Min(this.PlotHeight, availableSize.Height);
            }
            else
            {
                this.extent.Height = !double.IsInfinity(availableSize.Height) ? availableSize.Height : 1;
                desiredHeight = this.extent.Height;
            }

            if (!double.IsNaN(this.PlotWidth))
            {
                this.extent.Width = this.PlotWidth;
                desiredWidth = Math.Min(this.PlotWidth, availableSize.Width);
            }
            else
            {
                this.extent.Width = !double.IsInfinity(availableSize.Width) ? availableSize.Width : 1;
                desiredWidth = this.extent.Width;
            }

            var desired = new Size(desiredWidth, desiredHeight);

            this.Overlay.Measure(desired);
            if (this.ScrollOwner != null)
            {
                this.ScrollOwner.InvalidateScrollInfo();
            }

            return desired;
        }

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a
        /// size for a System.Windows.FrameworkElement derived class.
        /// </summary>
        /// <param name="finalSize">
        /// The final area within the parent that this element should use to arrange itself
        /// and its children.
        /// </param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var overlaySize = this.extent;

            if (!this.CanVerticallyScroll)
            {
                overlaySize.Height = finalSize.Height;
                this.offset.Y = 0;
            }
            else
            {
                this.overlayTransform.Y = -this.offset.Y;
            }

            if (!this.CanHorizontallyScroll)
            {
                overlaySize.Width = finalSize.Width;
                this.offset.X = 0;
            }
            else
            {
                this.overlayTransform.X = -this.offset.X;
            }

            this.Overlay.Arrange(new Rect(overlaySize));

            bool sizeChanged = this.viewport != finalSize;

            this.viewport = finalSize;

            if (this.ScrollOwner != null)
            {
                this.ScrollOwner.InvalidateScrollInfo();
            }

            this.Render(sizeChanged, false);

            return finalSize;
        }

        /// <summary>
        /// Initializes DirectX rendering resources.
        /// </summary>
        private void InitRendering()
        {
            double dpiScale = 1.0; // default value for 96 dpi

            var hwndTarget = PresentationSource.FromVisual(this).CompositionTarget as HwndTarget;
            if (hwndTarget != null)
            {
                dpiScale = hwndTarget.TransformToDevice.M11;
            }

            int surfWidth = (int)(this.viewport.Width < 0 ? 0 : Math.Ceiling(this.viewport.Width * dpiScale));
            int surfHeight = (int)(this.viewport.Height < 0 ? 0 : Math.Ceiling(this.viewport.Height * dpiScale));

            var windowHandle = (new System.Windows.Interop.WindowInteropHelper(System.Windows.Window.GetWindow(this))).Handle;

            this.d3d11Device = new D3D11Device(
                DriverType.Hardware,
                DeviceCreationFlags.BgraSupport,
                FeatureLevel.Level_11_0,
                FeatureLevel.Level_10_1,
                FeatureLevel.Level_10_0,
                FeatureLevel.Level_9_3,
                FeatureLevel.Level_9_2,
                FeatureLevel.Level_9_1);

            var backBuffer = new Texture2D(
                this.d3d11Device, 
                new Texture2DDescription
                {
                    Height = surfHeight,
                    Width = surfWidth,
                    ArraySize = 1,
                    MipLevels = 1,
                    Format = Format.B8G8R8A8_UNorm,
                    SampleDescription = { Count = 1, Quality = 0 },
                    Usage = ResourceUsage.Default,
                    OptionFlags = ResourceOptionFlags.Shared,
                    BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                    CpuAccessFlags = 0
                });

            var surface = backBuffer.QueryInterface<Surface>();
            this.renderTarget = new RenderTarget(
                this.oxyRenderContext.D2DFactory, 
                surface,
                new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));

            this.d3d9Device = new D3D9Device(
                new Direct3D(),
                0,
                D3D9DeviceType.Hardware,
                windowHandle,
                D3D9CreateFlags.HardwareVertexProcessing | D3D9CreateFlags.Multithreaded | D3D9CreateFlags.FpuPreserve,
                new D3D9PresentParameters(surfWidth, surfHeight)
                {
                    PresentationInterval = D3D9PresentInterval.Immediate,
                    DeviceWindowHandle = windowHandle,
                    SwapEffect = D3D9SwapEffect.Discard,
                    Windowed = true
                });

            this.imageSource = new D3D11Image(this.d3d9Device, backBuffer);

            this.oxyRenderContext.ResetRenderTarget(this.renderTarget);
        }

        /// <summary>
        /// Free used resources.
        /// </summary>
        private void CleanResources()
        {
            this.oxyRenderContext.ResetRenderTarget(null);

            this.d3d11Device?.ImmediateContext.ClearState();
            this.d3d11Device?.ImmediateContext.Flush();

            this.imageSource?.Dispose();
            this.d3d9Device?.Dispose();
            this.renderTarget?.Dispose();
            this.d3d11Device?.Dispose();

            this.imageSource = null;
            this.d3d9Device = null;
            this.renderTarget = null;
            this.d3d11Device = null;
        }

        /// <summary>
        /// Called when control unloaded.
        /// </summary>
        private void OnUnloaded()
        {
            this.CleanResources();
        }

        /// <summary>
        /// Changes offset by <paramref name="deltaX"/>, <paramref name="deltaY"/> vector.
        /// </summary>
        /// <param name="deltaX">The X axis delta.</param>
        /// <param name="deltaY">The Y axis delta.</param>
        private void AddOffset(double deltaX, double deltaY)
        {
            this.offset.X += deltaX;
            this.offset.Y += deltaY;

            if (this.offset.X < 0)
            {
                this.offset.X = 0;
            }

            if (this.offset.X + this.viewport.Width > this.extent.Width)
            {
                this.offset.X = Math.Max(0, this.extent.Width - this.viewport.Width);
            }

            if (this.offset.Y < 0)
            {
                this.offset.Y = 0;
            }

            if (this.offset.Y + this.viewport.Height > this.extent.Height)
            {
                this.offset.Y = Math.Max(0, this.extent.Height - this.viewport.Height);
            }

            this.InvalidateVisual();
        } 
    }
}
