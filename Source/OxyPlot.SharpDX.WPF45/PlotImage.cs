using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using WICFactory = SharpDX.WIC.ImagingFactory;
//using DXGIFactory = SharpDX.DXGI.Factory;
//using DWFactory = SharpDX.DirectWrite.Factory;

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

namespace OxyPlot.SharpDX.WPF
{
    public class PlotImage: FrameworkElement, System.Windows.Controls.Primitives.IScrollInfo
    {
        //AffectsMeasure/AffectsArrange

        // Using a DependencyProperty as the backing store for PlotWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlotWidthProperty =
            DependencyProperty.Register("PlotWidth", typeof(double), typeof(PlotImage), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure));

        // Using a DependencyProperty as the backing store for PlotHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlotHeightProperty =
            DependencyProperty.Register("PlotHeight", typeof(double), typeof(PlotImage), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure));

             

        // Using a DependencyProperty as the backing store for PlotModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlotModelProperty =
            DependencyProperty.Register("PlotModel", typeof(IPlotModel), typeof(PlotImage), new PropertyMetadata(null));


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






        internal System.Windows.Controls.Canvas Overlay
        {
            get; private set;
        }


        public IPlotModel PlotModel
        {
            get { return (IPlotModel)GetValue(PlotModelProperty); }
            set { SetValue(PlotModelProperty, value); }
        }

        public double PlotHeight
        {
            get { return (double)GetValue(PlotHeightProperty); }
            set { SetValue(PlotHeightProperty, value); }
        }

        public double PlotWidth
        {
            get { return (double)GetValue(PlotWidthProperty); }
            set { SetValue(PlotWidthProperty, value); }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        public bool CanVerticallyScroll
        {
            get; set;
        }

        public bool CanHorizontallyScroll
        {
            get; set;
        }

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



            //this.Loaded += (s, e) => OnLoaded();
            //this.SizeChanged += (s, e) => OnResize();
            this.Unloaded += (s, e) => OnUnloaded();
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            return this.Overlay;
         //   return base.GetVisualChild(index);
        }

       


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (_imageSource != null)
                drawingContext.DrawImage(_imageSource, new Rect(0, 0, _imageSource.Width, _imageSource.Height));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double desiredHeight;
            double desiredWidth;

            if (!double.IsNaN(PlotHeight))
            {
                _extent.Height = PlotHeight;
                desiredHeight = Math.Min(PlotHeight, availableSize.Height);
                //Viewport?
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
                //Viewport?
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

        protected override Size ArrangeOverride(Size finalSize)
        {
            var overlaySize = _extent;

            if (!CanVerticallyScroll)
            {
                overlaySize.Height = finalSize.Height;
                _offset.Y = 0;
            }
            else
                this._overlayTransform.X = -_offset.X;


            if (!CanHorizontallyScroll)
            {
                overlaySize.Width = finalSize.Width;
                _offset.X = 0;
            }
            else
                this._overlayTransform.Y = -_offset.Y;


            this.Overlay.Arrange(new Rect(overlaySize));

            bool sizeChanged = _viewport != finalSize;

            _viewport = finalSize;
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


       

            _d3d11Device = new D3D11Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport, FeatureLevel.Level_11_0, FeatureLevel.Level_10_1);

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
            

            //D3D11Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, swapChainDescription, out _d3d11Device, out _swapChain);



            //var backBuffer = Texture2D.FromSwapChain<Texture2D>(_swapChain, 0);
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
         //   _imageControl.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("D:\\tits.jpg"));

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
            //if (!this.IsLoaded)
            //    return;

            if (invalidateSurface || _renderTarget==null)
            {
                try
                {
                    CleanResources();
                    InitRendering();
                }
                catch
                {
                    return;
                }

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


     
        

        public void Invalidate()
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

            if (this._offset.X > _extent.Width)
                this._offset.X = _extent.Width;

            if (this._offset.Y < 0)
                this._offset.Y = 0;

            if (this._offset.Y > _extent.Height)
                this._offset.Y = _extent.Height;

            this.InvalidateVisual();
        }


        //Scroll Info implementation

        public void LineUp()
        {
            AddOffset( 0,- _scrollLineDelta);
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
