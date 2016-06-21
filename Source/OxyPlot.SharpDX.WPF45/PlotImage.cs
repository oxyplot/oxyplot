using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3D11Device = SharpDX.Direct3D11.Device;
using DXGIResource = SharpDX.DXGI.Resource;

using D3D9Device = SharpDX.Direct3D9.DeviceEx;
using Direct3D = SharpDX.Direct3D9.Direct3DEx;
using D3D9DeviceType = SharpDX.Direct3D9.DeviceType;
using D3D9CreateFlags = SharpDX.Direct3D9.CreateFlags;
using D3D9PresentParameters = SharpDX.Direct3D9.PresentParameters;
using D3D9PresentInterval = SharpDX.Direct3D9.PresentInterval;
using D3D9SwapEffect = SharpDX.Direct3D9.SwapEffect;

using RenderTarget = SharpDX.Direct2D1.RenderTarget;
using RenderTargetProperties = SharpDX.Direct2D1.RenderTargetProperties;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;

using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.Direct3D;
using SharpDX;
using OxyPlot.SharpDX;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OxyPlot.SharpDX.WPF
{
    /// <summary>
    /// Control, that renders Plot using SharpDX renderer, renders Tracker using WPF controls.
    /// Adds scrolling support.
    /// </summary>
    public class PlotImage: FrameworkElement, System.Windows.Controls.Primitives.IScrollInfo
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

        //stolen from scrollviewer MS source code

        // Scrolling physical "line" metrics.
        internal const double _scrollLineDelta = 16.0;   // Default physical amount to scroll with one Up/Down/Left/Right key
        internal const double _mouseWheelDelta = 48.0;   // Default physical amount to scroll with one MouseWheel.

        D3D11Device _d3d11Device;
        D3D9Device _d3d9Device;       
        RenderTarget _renderTarget;

        CacherRenderContext _oxyRenderContext;
        D3D11Image _imageSource;

        TranslateTransform _overlayTransform;

        Size _extent;
        Size _viewport;
        Vector _offset;

        BitmapImage _designModeImage;

        internal System.Windows.Controls.Canvas Overlay
        {
            get; private set;
        }

        /// <summary>
        /// The Plot Model
        /// </summary>
        public IPlotModel PlotModel
        {
            get { return (IPlotModel)GetValue(PlotModelProperty); }
            set { SetValue(PlotModelProperty, value); }
        }

        /// <summary>
        /// The Plot Heigth
        /// </summary>
        public double PlotHeight
        {
            get { return (double)GetValue(PlotHeightProperty); }
            set { SetValue(PlotHeightProperty, value); }
        }

        /// <summary>
        /// The Plot Width
        /// </summary>
        public double PlotWidth
        {
            get { return (double)GetValue(PlotWidthProperty); }
            set { SetValue(PlotWidthProperty, value); }
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
        /// 
        /// </summary>
        public bool CanVerticallyScroll
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanHorizontallyScroll
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public double ExtentWidth
        {
            get
            {
                return _extent.Width;
            }
        }

        public double ExtentHeight
        {
            get
            {
                return _extent.Height;
            }
        }

        public double ViewportWidth
        {
            get
            {
                return _viewport.Width;
            }
        }

        public double ViewportHeight
        {
            get
            {
                return _viewport.Height;
            }
        }

        public double HorizontalOffset
        {
            get
            {
                return _offset.X;
            }
        }

        public double VerticalOffset
        {
            get
            {
                return _offset.Y;
            }
        }

        public Vector Offset
        {
            get
            {
                return _offset;
            }
        }

        public ScrollViewer ScrollOwner
        {
            get; set;
        }

        public PlotImage()
        {
            _oxyRenderContext = new CacherRenderContext();
            _overlayTransform = new TranslateTransform();
            Overlay = new System.Windows.Controls.Canvas
            {
                RenderTransform = _overlayTransform,
            };


            this.AddVisualChild(Overlay);
            this.AddLogicalChild(Overlay);

            this.Loaded += (s, e) => Dispatcher.Invoke(InvalidateVisual, System.Windows.Threading.DispatcherPriority.Background);
            this.Unloaded += (s, e) => OnUnloaded();
        }
               
        /// <summary>
        /// Overrides System.Windows.Media.Visual.GetVisualChild(System.Int32), and returns a child at the specified index from a collection of child elements.
        /// </summary>
        /// <param name="index">The zero-based index of the requested child element in the collection.</param>
        /// <returns>The requested child element. This should not return null; if the provided index is out of range, an exception is thrown.</returns>
        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            return this.Overlay;
        }
       
        /// <summary>
        /// When overridden in a derived class, participates in rendering operations that
        /// are directed by the layout system. The rendering instructions for this element
        /// are not used directly when this method is invoked, and are instead preserved
        /// for later asynchronous use by layout and drawing.
        /// </summary>
        /// <param name="drawingContext"> The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                if (_designModeImage == null)
                {
                   var stream= System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OxyPlot.SharpDX.WPF.Resources.designmode.png");
                    
                    _designModeImage = new BitmapImage();
                    
                    _designModeImage.BeginInit();
                    _designModeImage.StreamSource = stream;
                    _designModeImage.EndInit();
                }

                drawingContext.DrawImage(_designModeImage, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            }

            if (_imageSource != null)
                drawingContext.DrawImage(_imageSource, new Rect(0, 0, _imageSource.Width, _imageSource.Height));
        }

        //
        // Summary:
        //     When overridden in a derived class, measures the size in layout required for
        //     child elements and determines a size for the System.Windows.FrameworkElement-derived
        //     class.
        //
        // Parameters:
        //   availableSize:
        //     The available size that this element can give to child elements. Infinity can
        //     be specified as a value to indicate that the element will size to whatever content
        //     is available.
        //
        // Returns:
        //     The size that this element determines it needs during layout, based on its calculations
        //     of child element sizes.
        protected override Size MeasureOverride(Size availableSize)
        {
            double desiredHeight;
            double desiredWidth;

            if (!double.IsNaN(PlotHeight))
            {
                _extent.Height = PlotHeight;
                desiredHeight = Math.Min(PlotHeight, availableSize.Height);
            }
            else
            {
                _extent.Height = !double.IsInfinity(availableSize.Height) ? availableSize.Height : 1;
                desiredHeight = _extent.Height;
            }
            
            if (!double.IsNaN(PlotWidth))
            {
                _extent.Width = PlotWidth;
                desiredWidth = Math.Min(PlotWidth, availableSize.Width);
            }
            else
            {
                _extent.Width = !double.IsInfinity(availableSize.Width) ? availableSize.Width : 1;
                desiredWidth = _extent.Width;
            }

            var desired = new Size(desiredWidth, desiredHeight);

            this.Overlay.Measure(desired);
            if (this.ScrollOwner != null)
                this.ScrollOwner.InvalidateScrollInfo();
            return desired;
        }

        //
        // Summary:
        //     When overridden in a derived class, positions child elements and determines a
        //     size for a System.Windows.FrameworkElement derived class.
        //
        // Parameters:
        //   finalSize:
        //     The final area within the parent that this element should use to arrange itself
        //     and its children.
        //
        // Returns:
        //     The actual size used.
        protected override Size ArrangeOverride(Size finalSize)
        {
            var overlaySize = _extent;

            if (!CanVerticallyScroll)
            {
                overlaySize.Height = finalSize.Height;
                _offset.Y = 0;
            }
            else
            {
                this._overlayTransform.Y = -_offset.Y;
            }

            if (!CanHorizontallyScroll)
            {
                overlaySize.Width = finalSize.Width;
                _offset.X = 0;
            }
            else
            {
                this._overlayTransform.X = -_offset.X;
            }

            this.Overlay.Arrange(new Rect(overlaySize));

            bool sizeChanged = _viewport != finalSize;

            _viewport =finalSize;
            
            if (this.ScrollOwner != null)
                this.ScrollOwner.InvalidateScrollInfo();

            Render(sizeChanged, false);

            return finalSize;
        }

        void InitRendering()
        {           
            double dpiScale = 1.0; // default value for 96 dpi
         
            var hwndTarget = PresentationSource.FromVisual(this).CompositionTarget as HwndTarget;
            if (hwndTarget != null)
            {
                dpiScale = hwndTarget.TransformToDevice.M11;
            }

            int surfWidth = (int)(_viewport.Width < 0 ? 0 : Math.Ceiling(_viewport.Width * dpiScale));
            int surfHeight = (int)(_viewport.Height < 0 ? 0 : Math.Ceiling(_viewport.Height * dpiScale));

            var windowHandle = (new System.Windows.Interop.WindowInteropHelper(System.Windows.Window.GetWindow(this))).Handle;

            _d3d11Device = new D3D11Device(
                DriverType.Hardware, 
                DeviceCreationFlags.BgraSupport,
                FeatureLevel.Level_11_0, 
                FeatureLevel.Level_10_1,
                FeatureLevel.Level_10_0,
                FeatureLevel.Level_9_3,
                FeatureLevel.Level_9_2,
                FeatureLevel.Level_9_1);

            var backBuffer = new Texture2D(_d3d11Device, new Texture2DDescription
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
            _renderTarget = new RenderTarget(_oxyRenderContext.D2DFactory, surface,
                                                            new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
            _d3d9Device = new D3D9Device(
                new Direct3D(),
                0, 
                D3D9DeviceType.Hardware,
                windowHandle,
                D3D9CreateFlags.HardwareVertexProcessing | D3D9CreateFlags.Multithreaded |D3D9CreateFlags.FpuPreserve,
                new D3D9PresentParameters(surfWidth, surfHeight) {
                    PresentationInterval = D3D9PresentInterval.Immediate,
                    DeviceWindowHandle = windowHandle,
                    SwapEffect = D3D9SwapEffect.Discard,
                    Windowed = true
                });
            
            _imageSource = new D3D11Image(_d3d9Device, backBuffer);

            _oxyRenderContext.ResetRenderTarget(_renderTarget);
        }

        void CleanResources()
        {
            _oxyRenderContext.ResetRenderTarget(null);

            _d3d11Device?.ImmediateContext.ClearState();
            _d3d11Device?.ImmediateContext.Flush();

            _imageSource?.Dispose();
            _d3d9Device?.Dispose();
            _renderTarget?.Dispose();
            _d3d11Device?.Dispose();

            _imageSource = null;
            _d3d9Device = null;
            _renderTarget = null;
            _d3d11Device = null;
        }

        void Render(bool invalidateSurface, bool invalidateUnits)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;
            if (!this.IsLoaded)
                return;

            if (invalidateSurface || _renderTarget == null)
            {
                CleanResources();
                InitRendering();

                invalidateUnits = true;
            }

            if (this.PlotModel == null)
                return;

            if (invalidateUnits)
            {
                _oxyRenderContext.ResetRenderUnits();

                //Creates renderUnits that can be rendered later
                this.PlotModel.Render(this._oxyRenderContext, this._extent.Width, this._extent.Height);
            }


            var backColor = OxyColors.White;
            if (!this.PlotModel.Background.IsUndefined())
            {
                backColor = this.PlotModel.Background;
            }


            _renderTarget.BeginDraw();
            _renderTarget.Clear(backColor.ToDXColor());
            _oxyRenderContext.Render(new RectangleF((float)_offset.X, (float)_offset.Y, (float)_viewport.Width, (float)_viewport.Height));//todo: add clip rectangle
            _renderTarget.EndDraw();

            _d3d11Device.ImmediateContext.Flush();
            
            _imageSource.Lock();
            _imageSource.AddDirtyRect(new Int32Rect()
            {
                X = 0,
                Y = 0,
                Height = _imageSource.PixelHeight,
                Width = _imageSource.PixelWidth
            });
            _imageSource.Unlock();
        }

        internal void Invalidate()
        {
            Render(false, true);
            InvalidateVisual();
        }
                
        void OnUnloaded()
        {
            CleanResources();
        }
        
        void AddOffset(double deltaX, double deltaY)
        {
            this._offset.X += deltaX;
            this._offset.Y += deltaY;

            if (this._offset.X < 0)
                this._offset.X = 0;

            if (this._offset.X + _viewport.Width > _extent.Width)
                this._offset.X = Math.Max(0, _extent.Width - _viewport.Width);

            if (this._offset.Y < 0)
                this._offset.Y = 0;

            if (this._offset.Y + _viewport.Height > _extent.Height)
                this._offset.Y = Math.Max(0, _extent.Height - _viewport.Height);

            this.InvalidateVisual();
        }
        
        //Scroll Info implementation
        public void LineUp()
        {
            AddOffset(0, -_scrollLineDelta);
        }

        public void LineDown()
        {
            AddOffset(0, _scrollLineDelta);
        }

        public void LineLeft()
        {
            AddOffset(-_scrollLineDelta, 0);
        }

        public void LineRight()
        {
            AddOffset(_scrollLineDelta, 0);
        }

        public void PageUp()
        {
            AddOffset(0, -_viewport.Height);
        }

        public void PageDown()
        {
            AddOffset(0, _viewport.Height);
        }

        public void PageLeft()
        {
            AddOffset(-_viewport.Height,0);
        }

        public void PageRight()
        {
            AddOffset(_viewport.Height, 0);
        }

        public void MouseWheelUp()
        {
            AddOffset(0,-_mouseWheelDelta);
        }

        public void MouseWheelDown()
        {
            AddOffset(0, _mouseWheelDelta);
        }

        public void MouseWheelLeft()
        {
            AddOffset(-_mouseWheelDelta, 0);
        }

        public void MouseWheelRight()
        {
            AddOffset(_mouseWheelDelta, 0);
        }

        public void SetHorizontalOffset(double offset)
        {
            this._offset.X = offset;
            this.InvalidateVisual();
        }

        public void SetVerticalOffset(double offset)
        {
            this._offset.Y = offset;
            this.InvalidateVisual();
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            return rectangle;
            //TODO: implement this????????
        }
    }
}
