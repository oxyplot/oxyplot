
using Microsoft.Wpf.Interop.DirectX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.DXGI.Factory;
using D2D1Factory = SharpDX.Direct2D1.Factory;
using RenderTarget = SharpDX.Direct2D1.RenderTarget;
using D2D1Resource = SharpDX.Direct2D1.Resource;
using DXGIResource = SharpDX.DXGI.Resource;
using ComObject = SharpDX.ComObject;
using RenderTargetProperties = SharpDX.Direct2D1.RenderTargetProperties;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;
using Ellipse = SharpDX.Direct2D1.Ellipse;
using Vector2 = SharpDX.Vector2;


namespace OxyPlot.SharpDX.WPF
{
    [TemplatePart(Name = PartGrid, Type = typeof(Grid))]
    public class PlotView : Control, IPlotView
    {


        static PlotView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlotView), new FrameworkPropertyMetadata(typeof(PlotView)));
        }


        /// <summary>
        /// Identifies the <see cref="Controller"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controller", typeof(IPlotController), typeof(PlotView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="DefaultTrackerTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultTrackerTemplateProperty =
            DependencyProperty.Register(
                "DefaultTrackerTemplate", typeof(ControlTemplate), typeof(PlotView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="HandleRightClicks"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HandleRightClicksProperty =
            DependencyProperty.Register("HandleRightClicks", typeof(bool), typeof(PlotView), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="IsMouseWheelEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsMouseWheelEnabledProperty =
            DependencyProperty.Register("IsMouseWheelEnabled", typeof(bool), typeof(PlotView), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="Model"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            "Model", typeof(PlotModel), typeof(PlotView), new PropertyMetadata(null, ModelChanged));

        /// <summary>
        /// Identifies the <see cref="ZoomRectangleTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomRectangleTemplateProperty =
            DependencyProperty.Register(
                "ZoomRectangleTemplate", typeof(ControlTemplate), typeof(PlotView), new PropertyMetadata(null));


        /// <summary>
        /// The mouse down point.
        /// </summary>
        private ScreenPoint mouseDownPoint;

        /// <summary>
        /// The Grid PART constant.
        /// </summary>
        private const string PartGrid = "PART_Grid";

        /// <summary>
        /// The model lock.
        /// </summary>
        private readonly object modelLock = new object();

        /// <summary>
        /// The tracker definitions.
        /// </summary>
        private readonly ObservableCollection<TrackerDefinition> trackerDefinitions;

        /// <summary>
        /// The canvas.
        /// </summary>
        private Image imageHost;

        /// <summary>
        /// The D3D surface
        /// </summary>
        private D3D11Image image;

        /// <summary>
        /// The current model.
        /// </summary>
        private PlotModel currentModel;

        /// <summary>
        /// The current tracker.
        /// </summary>
        private FrameworkElement currentTracker;

        /// <summary>
        /// The grid.
        /// </summary>
        private Grid grid;

        /// <summary>
        /// The default controller.
        /// </summary>
        private IPlotController defaultController;

        /// <summary>
        /// The overlays.
        /// </summary>
        private Canvas overlays;

        /// <summary>
        /// The render context
        /// </summary>
        private CacherRenderContext renderContext;

        /// <summary>
        /// The zoom control.
        /// </summary>
        private ContentControl zoomRectangle;

      

        /// <summary>
        /// Last visibility state
        /// </summary>
        private bool lastVisible;

        RenderTarget renderTarget;
        RenderTargetView renderTargetView;
        D2D1Factory d2dFactory;






        /// <summary>
        /// Gets or sets the PlotView controller.
        /// </summary>
        /// <value>The PlotView controller.</value>
        public IPlotController Controller
        {
            get { return (IPlotController)this.GetValue(ControllerProperty); }
            set { this.SetValue(ControllerProperty, value); }
        }

        /// <summary>
        /// Gets or sets the default tracker template.
        /// </summary>
        public ControlTemplate DefaultTrackerTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(DefaultTrackerTemplateProperty);
            }

            set
            {
                this.SetValue(DefaultTrackerTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to handle right clicks.
        /// </summary>
        public bool HandleRightClicks
        {
            get
            {
                return (bool)this.GetValue(HandleRightClicksProperty);
            }

            set
            {
                this.SetValue(HandleRightClicksProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsMouseWheelEnabled.
        /// </summary>
        public bool IsMouseWheelEnabled
        {
            get
            {
                return (bool)this.GetValue(IsMouseWheelEnabledProperty);
            }

            set
            {
                this.SetValue(IsMouseWheelEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="PlotModel" /> to show.
        /// </summary>
        /// <value>The <see cref="PlotModel" />.</value>
        public PlotModel Model
        {
            get
            {
                return (PlotModel)this.GetValue(ModelProperty);
            }

            set
            {
                this.SetValue(ModelProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the zoom rectangle template.
        /// </summary>
        /// <value>The zoom rectangle template.</value>
        public ControlTemplate ZoomRectangleTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(ZoomRectangleTemplateProperty);
            }

            set
            {
                this.SetValue(ZoomRectangleTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets the tracker definitions.
        /// </summary>
        /// <value>The tracker definitions.</value>
        public ObservableCollection<TrackerDefinition> TrackerDefinitions
        {
            get
            {
                return this.trackerDefinitions;
            }
        }

        /// <summary>
        /// Gets the actual model in the view.
        /// </summary>
        /// <value>
        /// The actual model.
        /// </value>
        Model IView.ActualModel
        {
            get
            {
                return this.Model;
            }
        }

        /// <summary>
        /// Gets the actual model.
        /// </summary>
        /// <value>The actual model.</value>
        public PlotModel ActualModel
        {
            get
            {
                return this.currentModel;
            }
        }

        /// <summary>
        /// Gets the actual controller.
        /// </summary>
        /// <value>
        /// The actual <see cref="IController" />.
        /// </value>
        IController IView.ActualController
        {
            get
            {
                return this.ActualController;
            }
        }

        /// <summary>
        /// Gets the coordinates of the client area of the view.
        /// </summary>
        public OxyRect ClientArea
        {
            get
            {
                return new OxyRect(0, 0, this.ActualWidth, this.ActualHeight);
            }
        }

        /// <summary>
        /// Gets the actual PlotView controller.
        /// </summary>
        /// <value>The actual PlotView controller.</value>
        public IPlotController ActualController
        {
            get
            {
                return this.Controller ?? (this.defaultController ?? (this.defaultController = new PlotController()));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "PlotView" /> class.
        /// </summary>
        public PlotView()
        {
            this.DefaultStyleKey = typeof(PlotView);

            this.trackerDefinitions = new ObservableCollection<TrackerDefinition>();
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            this.Loaded += this.OnLoaded;
            this.Unloaded += OnUnloaded;
            this.SizeChanged += this.OnSizeChanged;

        }



        /// <summary>
        /// Hides the tracker.
        /// </summary>
        public void HideTracker()
        {
            if (this.currentTracker != null)
            {
                this.overlays.Children.Remove(this.currentTracker);
                this.currentTracker = null;
            }
        }

        /// <summary>
        /// Hides the zoom rectangle.
        /// </summary>
        public void HideZoomRectangle()
        {
            this.zoomRectangle.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Invalidate the PlotView (not blocking the UI thread)
        /// </summary>
        /// <param name="update">if set to <c>true</c>, the data collections will be updated.</param>
        public void InvalidatePlot(bool update = true)
        {
            this.UpdateModel(update);
            if (renderContext != null && this.renderTarget!=null)
            {

                this.renderContext.ResetRenderUnits();

                ((IPlotModel)this.ActualModel).Render(this.renderContext, this.ActualWidth, this.ActualHeight);
            }


        }

        /// <summary>
        /// Sets the cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public void SetCursorType(CursorType cursor)
        {
          

            var type = Cursors.Arrow;
            switch (cursor)
            {
                case CursorType.Default:
                    type = null;//Cursors.Arrow;
                    break;
                case CursorType.Pan:
                    type = Cursors.Hand;
                    break;
                case CursorType.ZoomHorizontal:
                    type = Cursors.SizeWE;
                    break;
                case CursorType.ZoomVertical:
                    type = Cursors.SizeNS;
                    break;
                case CursorType.ZoomRectangle:
                    type = Cursors.SizeNWSE;
                    break;
            }

            Mouse.OverrideCursor = type;
           
        }

        /// <summary>
        /// Shows the tracker.
        /// </summary>
        /// <param name="trackerHitResult">The tracker data.</param>
        public void ShowTracker(TrackerHitResult trackerHitResult)
        {
            if (trackerHitResult == null)
            {
                this.HideTracker();
                return;
            }

            var trackerTemplate = this.DefaultTrackerTemplate;
            if (trackerHitResult.Series != null && !string.IsNullOrEmpty(trackerHitResult.Series.TrackerKey))
            {
                var match = this.TrackerDefinitions.FirstOrDefault(t => t.TrackerKey == trackerHitResult.Series.TrackerKey);
                if (match != null)
                {
                    trackerTemplate = match.TrackerTemplate;
                }
            }

            if (trackerTemplate == null)
            {
                this.HideTracker();
                return;
            }

            var tracker = new ContentControl { Template = trackerTemplate };

            if (tracker != this.currentTracker)
            {
                this.HideTracker();
                this.overlays.Children.Add(tracker);
                this.currentTracker = tracker;
            }

            if (this.currentTracker != null)
            {
                this.currentTracker.DataContext = trackerHitResult;
            }
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        public void ShowZoomRectangle(OxyRect r)
        {
            this.zoomRectangle.Width = r.Width;
            this.zoomRectangle.Height = r.Height;
            Canvas.SetLeft(this.zoomRectangle, r.Left);
            Canvas.SetTop(this.zoomRectangle, r.Top);
            this.zoomRectangle.Template = this.ZoomRectangleTemplate;
            this.zoomRectangle.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Renders the PlotView to a bitmap.
        /// </summary>
        /// <returns>A bitmap.</returns>
        public WriteableBitmap ToBitmap()
        {
            throw new NotImplementedException();

            // var bmp = new RenderTargetBitmap(
            // (int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            // bmp.Render(this);
            // return bmp;
        }

        /// <summary>
        /// Stores text on the clipboard.
        /// </summary>
        /// <param name="text">The text.</param>
        void IPlotView.SetClipboardText(string text)
        {
            Clipboard.SetText(text);
        }




        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            this.grid = this.GetTemplateChild(PartGrid) as Grid;
            if (this.grid == null)
            {
                return;
            }

            this.imageHost = new Image
            {
                IsHitTestVisible = false,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                
                 

            };

            this.Loaded += new RoutedEventHandler(this.Host_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(this.Host_SizeChanged);


            this.image = new D3D11Image();
            
            this.grid.Children.Add(this.imageHost);
            imageHost.Source = this.image;

            


            this.overlays = new Canvas();
            this.grid.Children.Add(this.overlays);

            this.zoomRectangle = new ContentControl();
            this.overlays.Children.Add(this.zoomRectangle);

            
        }

     

        /// <summary>
        /// Called when the control is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.InvalidatePlot();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.UninitializeRendering();
        }


        private void Host_Loaded(object sender, RoutedEventArgs e)
        {
            InitDevice();
            this.InitializeRendering();
        }

        private void Host_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double dpiScale = 1.0; // default value for 96 dpi

            // determine DPI
            // (as of .NET 4.6.1, this returns the DPI of the primary monitor, if you have several different DPIs)
            var hwndTarget = PresentationSource.FromVisual(this).CompositionTarget as HwndTarget;
            if (hwndTarget != null)
            {
                dpiScale = hwndTarget.TransformToDevice.M11;
            }

            int surfWidth = (int)(this.ActualWidth < 0 ? 0 : Math.Ceiling(this.ActualWidth * dpiScale));
            int surfHeight = (int)(this.ActualHeight < 0 ? 0 : Math.Ceiling(this.ActualHeight * dpiScale));

            // Notify the D3D11Image of the pixel size desired for the DirectX rendering.
            // The D3DRendering component will determine the size of the new surface it is given, at that point.
            image.SetPixelSize(surfWidth, surfHeight);

            // Stop rendering if the D3DImage isn't visible - currently just if width or height is 0
            // TODO: more optimizations possible (scrolled off screen, etc...)
            bool isVisible = (surfWidth != 0 && surfHeight != 0);
            if (lastVisible != isVisible)
            {
                lastVisible = isVisible;
                if (lastVisible)
                {
                    CompositionTarget.Rendering += CompositionTarget_Rendering;
                }
                else
                {
                    CompositionTarget.Rendering -= CompositionTarget_Rendering;
                }
            }
        }

        
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            //RenderingEventArgs args = (RenderingEventArgs)e;

            //// It's possible for Rendering to call back twice in the same frame 
            //// so only render when we haven't already rendered in this frame.
            //if (this.lastRender != args.RenderingTime)
            //{
                image.RequestRender();
            //    this.lastRender = args.RenderingTime;
            //}
        }

        private void InitializeRendering()
        {
            image.WindowOwner = (new WindowInteropHelper(Window.GetWindow(this))).Handle;
            image.OnRender = this.DoRender;
            d2dFactory = new D2D1Factory();
            renderContext = new CacherRenderContext(d2dFactory);
            
            // Start rendering now!
            image.RequestRender();
        }



        private void DoRender(IntPtr surface, bool isNewSurface)
        {
            if (this.imageHost == null || this.ActualModel == null)
            {
                return;
            }

            var backColor = OxyColors.Transparent;
            if (this.ActualModel != null && !this.ActualModel.Background.IsUndefined())
            {
                backColor = this.ActualModel.Background;
            }

            if (isNewSurface || renderTarget == null)
            {
              

                InitRenderTarget(surface);

                this.InvalidatePlot(false);


                //device.ImmediateContext.Rasterizer.SetViewport(0, 0, (float)this.ActualWidth, (float)this.ActualHeight, 0.0f, 1.0f);


                //device.ImmediateContext.OutputMerger.SetRenderTargets(renderTargetView);
                //device.ImmediateContext.ClearRenderTargetView(renderTargetView, backColor.ToColor4());
            }








            this.renderTarget.BeginDraw();
            renderTarget.Clear(backColor.ToDXColor());
            this.renderContext.Render();

            this.renderTarget.EndDraw();




            device.ImmediateContext.Flush();



        }

        private void UninitializeRendering()
        {
            Cleanup();

            CompositionTarget.Rendering -= this.CompositionTarget_Rendering;
        }

        private void Cleanup()
        {
            if (renderTargetView == null)
                return;
            renderTargetView.Dispose();
          //  backBuffer.Dispose();
            device.ImmediateContext.ClearState();
            device.ImmediateContext.Flush();
            device.Dispose();
            swapChain.Dispose();
            //factory.Dispose();
        }


        SwapChainDescription swapChainDescription;
        SwapChain swapChain;
        Device device;

        private void InitDevice()
        {
  

            swapChainDescription = new SwapChainDescription
            {
                OutputHandle = (new WindowInteropHelper(Window.GetWindow(this))).Handle,
                BufferCount = 1,
                Flags = SwapChainFlags.AllowModeSwitch,
                IsWindowed = true,
                ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.B8G8R8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput | Usage.Shared,
            };

            
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, swapChainDescription, out device, out swapChain);
            var d2dFactory = new D2D1Factory();
        }

        /// <summary>
        /// Should be called, if new D3D11Image surface OnRender
        /// </summary>
        void InitRenderTarget(IntPtr surfacePtr)
        {
            //MEMORY leaks here? dxgiResource, backbuffer etc?


            DXGIResource dxgiResource;
            using (var r = new ComObject(surfacePtr))
            {
                dxgiResource = r.QueryInterface<DXGIResource>();
            }

            var backBuffer = device.OpenSharedResource<Texture2D>(dxgiResource.SharedHandle);

            if (renderTargetView != null)
            {
                renderTargetView.Dispose();
                renderTargetView = null;
            }
            renderTargetView = new RenderTargetView(device, backBuffer);

            var surface = backBuffer.QueryInterface<Surface>();
            if (renderTarget != null)
                renderTarget.Dispose();



            renderTarget = new RenderTarget(d2dFactory, surface, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));

            renderContext.ResetRenderTarget(renderTarget);


            //dxgiResource.Dispose();
            //backBuffer.Dispose();
            //surface.Dispose();
            
        }

       
        



        /// <summary>
        /// Called when the model is changed.
        /// </summary>
        private void OnModelChanged()
        {
            lock (this.modelLock)
            {
                if (this.currentModel != null)
                {
                    ((IPlotModel)this.currentModel).AttachPlotView(null);
                    this.currentModel = null;
                }

                if (this.Model != null)
                {
                    ((IPlotModel)this.Model).AttachPlotView(this);
                    this.currentModel = this.Model;
                }
            }

            this.InvalidatePlot();
        }

        /// <summary>
        /// Called when the size of the control is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs" /> instance containing the event data.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Updates the model.
        /// </summary>
        /// <param name="update">if set to <c>true</c>, the data collections will be updated.</param>
        private void UpdateModel(bool update)
        {
            if (this.ActualModel != null)
            {
                ((IPlotModel)this.ActualModel).Update(update);
            }
        }

      
      























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
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseMove(this, e.ToMouseEventArgs(this));
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
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseLeave(this, e.ToMouseEventArgs(this));
        }




        /// <summary>
        /// Called when the <see cref="Model" /> property is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ModelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((PlotView)sender).OnModelChanged();
        }


        
        /// <summary>
        /// Invokes the specified action on the UI Thread (without blocking the calling thread).
        /// </summary>
        /// <param name="action">The action.</param>
        private void BeginInvoke(Action action)
        {
            this.Dispatcher.InvokeAsync(action, System.Windows.Threading.DispatcherPriority.Background);
            
        
        }
    }
}
