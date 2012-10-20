// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotControl.cs" company="OxyPlot">
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
//   Gets the series that is nearest the specified point (in screen coordinates).
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace OxyPlot.Wpf
{
    [ContentProperty("Series")]
    public class PlotControl : Control, IPlotControl
    {
        public static readonly DependencyProperty PlotMarginsProperty =
            DependencyProperty.Register("PlotMargins", typeof(Thickness?), typeof(PlotControl),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty BoxColorProperty =
            DependencyProperty.Register("BoxColor", typeof(Color), typeof(PlotControl),
                                        new UIPropertyMetadata(Colors.Black));

        public static readonly DependencyProperty BoxThicknessProperty =
            DependencyProperty.Register("BoxThickness", typeof(double), typeof(PlotControl),
                                        new UIPropertyMetadata(1.0));

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(PlotModel), typeof(PlotControl),
                                        new UIPropertyMetadata(null, ModelChanged));

        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register("Subtitle", typeof(string), typeof(PlotControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PlotControl),
                                        new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty RenderToCanvasProperty =
            DependencyProperty.Register("RenderToCanvas", typeof(bool), typeof(PlotControl),
                                        new UIPropertyMetadata(true, RenderToCanvasChanged));

        public static readonly DependencyProperty SliderTemplateProperty =
            DependencyProperty.Register("SliderTemplate", typeof(DataTemplate), typeof(PlotControl),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty ZoomRectangleTemplateProperty =
            DependencyProperty.Register("ZoomRectangleTemplate", typeof(ControlTemplate), typeof(PlotControl),
                                        new UIPropertyMetadata(null));

        public LegendPosition LegendPosition
        {
            get { return (LegendPosition)GetValue(LegendPositionProperty); }
            set { SetValue(LegendPositionProperty, value); }
        }

        public static readonly DependencyProperty LegendPositionProperty =
            DependencyProperty.Register("LegendPosition", typeof(LegendPosition), typeof(PlotControl),
                                        new UIPropertyMetadata(LegendPosition.TopRight));

        public bool IsLegendOutsidePlotArea
        {
            get { return (bool)GetValue(IsLegendOutsidePlotAreaProperty); }
            set { SetValue(IsLegendOutsidePlotAreaProperty, value); }
        }

        public static readonly DependencyProperty IsLegendOutsidePlotAreaProperty =
            DependencyProperty.Register("IsLegendOutsidePlotArea", typeof(bool), typeof(PlotControl),
                                        new UIPropertyMetadata(false));

        private const string PART_GRID = "PART_Grid";

        private Canvas canvas;
        private Grid grid;
        private Canvas overlays;

        private PlotModel internalModel;

        private PlotFrame plotAliasedFrame;
        private PlotFrame plotFrame;
        private Slider slider;

        public List<MouseAction> MouseActions { get; private set; }

        private readonly PanAction panAction;
        private readonly SliderAction sliderAction;
        private readonly ZoomAction zoomAction;

        private ContentControl zoomControl;
        private ScreenPoint mouseDownPoint;

        private bool isPlotInvalidated;

        static PlotControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlotControl),
                                                     new FrameworkPropertyMetadata(typeof(PlotControl)));
        }

        public PlotControl()
        {
            Background = Brushes.Transparent;

            panAction = new PanAction(this);
            zoomAction = new ZoomAction(this);
            sliderAction = new SliderAction(this);

            MouseActions = new List<MouseAction> { panAction, zoomAction, sliderAction };

            series = new ObservableCollection<DataSeries>();
            axes = new ObservableCollection<Axis>();
            series.CollectionChanged += OnSeriesChanged;
            axes.CollectionChanged += OnAxesChanged;

            Loaded += OnLoaded;
            DataContextChanged += OnDataContextChanged;
            SizeChanged += OnSizeChanged;

            CompositionTarget.Rendering += CompositionTargetRendering;

            // CommandBindings.Add(new KeyBinding())
        }

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            lock (this)
            {
                if (isPlotInvalidated)
                {
                    isPlotInvalidated = false;
                    Refresh();
                }
            }
        }

        public void InvalidatePlot()
        {
            lock (this)
            {
                isPlotInvalidated = true;
            }
        }

        private void OnSeriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SyncLogicalTree(e);
        }

        private void OnAxesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SyncLogicalTree(e);
        }

        private void SyncLogicalTree(NotifyCollectionChangedEventArgs e)
        {
            // In order to get DataContext and binding to work with the series and axes
            // we add the items to the logical tree
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    AddLogicalChild(item);
                }
            }
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    RemoveLogicalChild(item);
                }
            }
        }

        public ControlTemplate ZoomRectangleTemplate
        {
            get { return (ControlTemplate)GetValue(ZoomRectangleTemplateProperty); }
            set { SetValue(ZoomRectangleTemplateProperty, value); }
        }

        public Color BoxColor
        {
            get { return (Color)GetValue(BoxColorProperty); }
            set { SetValue(BoxColorProperty, value); }
        }

        public DataTemplate SliderTemplate
        {
            get { return (DataTemplate)GetValue(SliderTemplateProperty); }
            set { SetValue(SliderTemplateProperty, value); }
        }

        public bool RenderToCanvas
        {
            get { return (bool)GetValue(RenderToCanvasProperty); }
            set { SetValue(RenderToCanvasProperty, value); }
        }

        public Thickness? PlotMargins
        {
            get { return (Thickness?)GetValue(PlotMarginsProperty); }
            set { SetValue(PlotMarginsProperty, value); }
        }

        public double BoxThickness
        {
            get { return (double)GetValue(BoxThicknessProperty); }
            set { SetValue(BoxThicknessProperty, value); }
        }

        private readonly ObservableCollection<DataSeries> series;

        public ObservableCollection<DataSeries> Series
        {
            get { return series; }
        }

        private readonly ObservableCollection<Axis> axes;

        public ObservableCollection<Axis> Axes
        {
            get { return axes; }
        }

        public PlotModel Model
        {
            get { return (PlotModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public string Subtitle
        {
            get { return (string)GetValue(SubtitleProperty); }
            set { SetValue(SubtitleProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            bool control = (Keyboard.IsKeyDown(Key.LeftCtrl));
            bool shift = (Keyboard.IsKeyDown(Key.LeftShift));
            var p = e.GetPosition(this).ToScreenPoint();
            foreach (var a in MouseActions)
                a.OnMouseWheel(p, e.Delta, control, shift);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            Focus();
            CaptureMouse();

            bool control = (Keyboard.IsKeyDown(Key.LeftCtrl));
            bool shift = (Keyboard.IsKeyDown(Key.LeftShift));

            var button = OxyMouseButton.Left;
            if (e.MiddleButton == MouseButtonState.Pressed)
                button = OxyMouseButton.Middle;
            if (e.RightButton == MouseButtonState.Pressed)
                button = OxyMouseButton.Right;
            if (e.XButton1 == MouseButtonState.Pressed)
                button = OxyMouseButton.XButton1;
            if (e.XButton2 == MouseButtonState.Pressed)
                button = OxyMouseButton.XButton2;

            var p = e.GetPosition(this).ToScreenPoint();
            foreach (var a in MouseActions)
                a.OnMouseDown(p, button, e.ClickCount, control, shift);

            mouseDownPoint = p;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            bool control = (Keyboard.IsKeyDown(Key.LeftCtrl));
            bool shift = (Keyboard.IsKeyDown(Key.LeftShift));
            var p = e.GetPosition(this).ToScreenPoint();
            foreach (var a in MouseActions)
                a.OnMouseMove(p, control, shift);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            foreach (var a in MouseActions)
                a.OnMouseUp();
            ReleaseMouseCapture();
            var p = e.GetPosition(this).ToScreenPoint();

            double d = p.DistanceTo(mouseDownPoint);
            if (ContextMenu != null)
            {
                if (d == 0 && e.ChangedButton == MouseButton.Right)
                {
                    ContextMenu.Visibility = Visibility.Visible;
                    // todo: The contextmenu has the wrong placement after panning
                    //ContextMenu.Placement = PlacementMode.Relative;
                    //ContextMenu.PlacementTarget = this;
                    //ContextMenu.PlacementRectangle=new Rect(e.GetPosition(this),new Size(0,0));
                }
                else
                    ContextMenu.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                e.Handled = true;
                ZoomAll();
            }
            base.OnKeyDown(e);
        }

        public void ZoomAll()
        {
            foreach (var a in internalModel.Axes)
                a.Reset();
            Refresh();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            grid = Template.FindName(PART_GRID, this) as Grid;

            if (grid == null)
                return;

            OnRenderToCanvasChanged();

            // Overlays
            overlays = new Canvas();
            grid.Children.Add(overlays);

            // Slider
            slider = new Slider();
            overlays.Children.Add(slider);

            zoomControl = new ContentControl();
            overlays.Children.Add(zoomControl);
        }

        private static void RenderToCanvasChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotControl)d).OnRenderToCanvasChanged();
        }

        private void OnRenderToCanvasChanged()
        {
            if (grid == null)
                return;
            if (RenderToCanvas)
            {
                if (canvas == null)
                {
                    canvas = new Canvas();
                    grid.Children.Insert(0, canvas);
                    canvas.UpdateLayout();
                }
                if (plotFrame != null)
                {
                    grid.Children.Remove(plotFrame);
                    grid.Children.Remove(plotAliasedFrame);
                    plotFrame = null;
                    plotAliasedFrame = null;
                }
            }
            else
            {
                if (plotFrame == null)
                {
                    plotAliasedFrame = new PlotFrame(true);
                    plotFrame = new PlotFrame(false);
                    grid.Children.Insert(0, plotAliasedFrame);
                    grid.Children.Insert(1, plotFrame);
                    plotFrame.UpdateLayout();
                    plotAliasedFrame.UpdateLayout();
                }
                if (canvas != null)
                {
                    grid.Children.Remove(canvas);
                    canvas = null;
                }
            }
            UpdateVisuals();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnModelChanged();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnModelChanged();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateVisuals();
        }

        private static void VisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotControl)d).UpdateVisuals();
        }

        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotControl)d).OnModelChanged();
        }

        private void OnModelChanged()
        {
            Refresh();
        }

        private void UpdateModel()
        {
            // If no model is set, create an internal model and copy the
            // axes/series/properties from the WPF objects to the internal model
            if (Model == null)
            {
                // Create an internal model
                if (internalModel == null)
                    internalModel = new PlotModel();

                // Transfer axes, series and properties from
                // the WPF dependency objects to the internal model
                if (Series != null)
                {
                    internalModel.Series.Clear();
                    foreach (var s in Series)
                    {
                        internalModel.Series.Add(s.CreateModel());
                    }
                }
                if (Axes != null && Axes.Count > 0)
                {
                    internalModel.Axes.Clear();

                    foreach (var a in Axes)
                    {
                        a.UpdateModelProperties();
                        internalModel.Axes.Add(a.ModelAxis);
                    }
                }

                if (PlotMargins.HasValue)
                {
                    internalModel.PlotMargins = new OxyThickness(
                        PlotMargins.Value.Left, PlotMargins.Value.Top,
                        PlotMargins.Value.Right, PlotMargins.Value.Bottom);
                }

                // Box around the plot area
                internalModel.BoxColor = BoxColor.ToOxyColor();
                internalModel.BoxThickness = BoxThickness;

                internalModel.LegendPosition = LegendPosition;
                internalModel.IsLegendOutsidePlotArea = IsLegendOutsidePlotArea;
            }
            else
                internalModel = Model;

            internalModel.UpdateData();
        }

        public void Refresh(bool refreshData = true)
        {
            if (refreshData)
                UpdateModel();
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (canvas != null)
                canvas.Children.Clear();

            if (internalModel == null)
                return;

            if (Title != null)
                internalModel.Title = Title;

            if (Subtitle != null)
                internalModel.Subtitle = Subtitle;

            if (canvas != null)
            {
                var wrc = new ShapesRenderContext(canvas);
                internalModel.Render(wrc);
            }

            if (plotFrame != null)
            {
                plotFrame.Model = internalModel;
                plotAliasedFrame.Model = internalModel;
                plotFrame.InvalidateVisual();
                plotAliasedFrame.InvalidateVisual();
            }
        }

        public void SaveBitmap(string fileName)
        {
            var bmp = ToBitmap();

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (var s = File.Create(fileName))
            {
                encoder.Save(s);
            }
        }

        public void SaveXaml(string fileName)
        {
            using (var w = new StreamWriter(fileName))
            {
                w.Write(ToXaml());
            }
        }

        public RenderTargetBitmap ToBitmap()
        {
            var bmp = new RenderTargetBitmap((int)ActualWidth, (int)ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(this);
            return bmp;
        }

        public string ToXaml()
        {
            var sb = new StringBuilder();
            var tw = new StringWriter(sb);
            var xw = new XmlTextWriter(tw) { Formatting = Formatting.Indented };
            if (canvas != null)
                XamlWriter.Save(canvas, xw);
            xw.Close();
            return sb.ToString();
        }

        public void GetAxesFromPoint(ScreenPoint pt, out AxisBase xaxis, out AxisBase yaxis)
        {
            internalModel.GetAxesFromPoint(pt, out xaxis, out yaxis);
        }

        public void ShowZoomRectangle(OxyRect r)
        {
            zoomControl.Width = r.Width;
            zoomControl.Height = r.Height;
            Canvas.SetLeft(zoomControl, r.Left);
            Canvas.SetTop(zoomControl, r.Top);
            zoomControl.Template = ZoomRectangleTemplate;
            zoomControl.Visibility = Visibility.Visible;
        }

        public void HideZoomRectangle()
        {
            zoomControl.Visibility = Visibility.Hidden;
        }

        public OxyRect GetPlotArea()
        {
            if (internalModel != null)
                return internalModel.PlotArea;
            // todo
            return new OxyRect(0, 0, ActualWidth, ActualHeight);
        }

        /// <summary>
        /// Gets the series that is nearest the specified point (in screen coordinates).
        /// </summary>
        /// <param name="pt">The point.</param>
        /// <param name="limit">The maximum distance, if this is exceeded the method will return null.</param>
        /// <returns>The closest DataSeries</returns>
        public OxyPlot.DataSeries GetSeriesFromPoint(ScreenPoint pt, double limit = 100)
        {
            return internalModel.GetSeriesFromPoint(pt, limit);
        }

        public void ShowSlider(OxyPlot.DataSeries s, DataPoint dp)
        {
            slider.ContentTemplate = SliderTemplate;
            slider.SetPosition(dp, s);
        }

        public void HideSlider()
        {
            slider.Hide();
        }

        public void Pan(AxisBase axis, double dx)
        {
            if (Model == null)
            {
                var a = FindModelAxis(axis);
                if (a != null)
                    a.Pan(dx);
            }
            // Modify min/max of the PlotModel's axis
            axis.Pan(dx);
        }

        private Axis FindModelAxis(AxisBase a)
        {
            return Axes.FirstOrDefault(axis => axis.ModelAxis == a);
        }

        public void Reset(AxisBase axis)
        {
            if (Model == null)
            {
                var a = FindModelAxis(axis);
                if (a != null)
                    a.Reset();
            }
            axis.Reset();
        }

        public void Zoom(AxisBase axis, double p1, double p2)
        {
            var a = FindModelAxis(axis);
            if (a != null)
                a.Zoom(p1, p2);
            else
                axis.Zoom(p1, p2);
        }

        public void ZoomAt(AxisBase axis, double factor, double x)
        {
            var a = FindModelAxis(axis);
            if (a != null)
                a.ZoomAt(factor, x);
            else
                axis.ZoomAt(factor, x);
        }
    }
}