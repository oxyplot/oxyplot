using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dx = SharpDX;
using D2DFactory = SharpDX.Direct2D1.Factory;
using DWFactory = SharpDX.DirectWrite.Factory;
using WICFactory = SharpDX.WIC.ImagingFactory;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using BitmapInterpolationMode = SharpDX.Direct2D1.BitmapInterpolationMode;
using SharpDX.WIC;
using System.IO;

namespace OxyPlot.SharpDX
{
    public class SharpDXRenderContext : IRenderContext, IDisposable
    {
        D2DFactory d2dFactory;
        DWFactory dwFactory;
        WICFactory wicFactory;
        
        RenderTarget renderTarget;

        /// <summary>
        /// All resources creating while render will be here
        /// </summary>
        List<IDisposable> garbage = new List<IDisposable>();
        

        /// <summary>
        /// The brush cache.
        /// </summary>
        private readonly Dictionary<OxyColor, Brush> brushCache = new Dictionary<OxyColor, Brush>();

        /// <summary>
        /// The images in use
        /// </summary>
        private readonly HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

        /// <summary>
        /// The image cache
        /// </summary>
        private readonly Dictionary<OxyImage, Bitmap> imageCache = new Dictionary<OxyImage, Bitmap>();

        /// <summary>
        /// The current tool tip
        /// </summary>
        private string currentToolTip;

        /// <summary>
        /// The clip rectangle.
        /// </summary>
        private  RectangleF clipRect;

        /// <summary>
        /// The clip flag.
        /// </summary>
       
        

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDXRenderContext" /> class.
        /// </summary>      
        public SharpDXRenderContext(D2DFactory factory)
        {
            this.d2dFactory = factory;
            this.dwFactory = new DWFactory();
            this.wicFactory = new WICFactory();

         

            this.RendersToScreen = true;
        }

     

        /// <summary>
        /// Gets a value indicating whether to paint the background.
        /// </summary>
        /// <value><c>true</c> if the background should be painted; otherwise, <c>false</c>.</value>
        public bool PaintBackground
        {
            get
            {
                return false;
            }
        }

     

        /// <summary>
        /// Gets or sets a value indicating whether the context renders to screen.
        /// </summary>
        /// <value><c>true</c> if the context renders to screen; otherwise, <c>false</c>.</value>
        public bool RendersToScreen { get; set; }

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        public void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            var el = rect.ToEllipse();
            if (stroke.IsVisible())
            {
                renderTarget.DrawEllipse(el, GetBrush(stroke),(float) thickness);
            }

            if (fill.IsVisible())
            {
                renderTarget.FillEllipse(el, GetBrush(fill));
            }
        }

        /// <summary>
        /// Draws the collection of ellipses, where all have the same stroke and fill.
        /// This performs better than calling DrawEllipse multiple times.
        /// </summary>
        /// <param name="rectangles">The rectangles.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        public void DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            var ellipseGeometries = rectangles.Select(x => new EllipseGeometry(this.d2dFactory, x.ToEllipse())).ToArray();
            var group = new GeometryGroup(this.d2dFactory,  FillMode.Alternate, ellipseGeometries);

            if (stroke.IsVisible())
                renderTarget.DrawGeometry(group, GetBrush(stroke), (float)thickness);

            if (fill.IsVisible())
                renderTarget.FillGeometry(group, GetBrush(fill));

            garbage.Add(group);
            garbage.AddRange(ellipseGeometries);
        }

        /// <summary>
        /// Draws the polyline from the specified points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">if set to <c>true</c> the shape will be aliased.</param>
        public void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {
            var path = new PathGeometry(d2dFactory);
            var sink = path.Open();
            sink.BeginFigure(points[0].ToVector2(aliased), new FigureBegin());

            sink.AddLines(points.Skip(1).Select(pt =>(dx.Mathematics.Interop.RawVector2) pt.ToVector2(aliased)).ToArray());
            sink.EndFigure(new FigureEnd());
            sink.Close();
            

            var strokeStyle = GetStroke(dashArray, lineJoin);

            renderTarget.DrawGeometry(path, GetBrush(stroke),(float) thickness, strokeStyle);
            sink.Dispose();
            garbage.Add(path);
            garbage.Add(strokeStyle);




        }

        /// <summary>
        /// Draws the multiple line segments defined by points (0,1) (2,3) (4,5) etc.
        /// This should have better performance than calling DrawLine for each segment.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">if set to <c>true</c> the shape will be aliased.</param>
        public void DrawLineSegments(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {

            var path = new PathGeometry(d2dFactory);
            var sink = path.Open();
            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                sink.BeginFigure(points[i].ToVector2(aliased), new FigureBegin());

                sink.AddLine(points[i + 1].ToVector2(aliased));
                sink.EndFigure(new FigureEnd());
            }
            sink.Close();
            

            var strokeStyle = GetStroke(dashArray, lineJoin);

            renderTarget.DrawGeometry(path, GetBrush(stroke), (float)thickness, strokeStyle);
            sink.Dispose();
            garbage.Add(path);
            garbage.Add(strokeStyle);



        }

        /// <summary>
        /// Draws the polygon from the specified points. The polygon can have stroke and/or fill.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">if set to <c>true</c> the shape will be aliased.</param>
        public void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {


            var path = new PathGeometry(d2dFactory);
            var sink = path.Open();
            sink.BeginFigure(points[0].ToVector2(aliased), new FigureBegin());

            sink.AddLines(points.Skip(1).Select(pt => (dx.Mathematics.Interop.RawVector2)pt.ToVector2(aliased)).ToArray());
            sink.EndFigure(new FigureEnd());
            sink.Close();

            var strokeStyle = GetStroke(dashArray, lineJoin);

            

            if (fill.IsVisible())
            {
                renderTarget.FillGeometry(path, GetBrush(fill));
            }
            if (stroke.IsVisible())
            {
                renderTarget.DrawGeometry(path, GetBrush(fill), (float)thickness, strokeStyle);
            }

            sink.Dispose();
            garbage.Add(path);
            garbage.Add(strokeStyle);

        }

        /// <summary>
        /// Draws a collection of polygons, where all polygons have the same stroke and fill.
        /// This performs better than calling DrawPolygon multiple times.
        /// </summary>
        /// <param name="polygons">The polygons.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">if set to <c>true</c> the shape will be aliased.</param>
        public void DrawPolygons(
            IList<IList<ScreenPoint>> polygons,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {




            var path = new PathGeometry(d2dFactory);
            var sink = path.Open();
            foreach (var points in polygons)
            {
                sink.BeginFigure(points[0].ToVector2(aliased), new FigureBegin());

                sink.AddLines(points.Skip(1).Select(pt => (dx.Mathematics.Interop.RawVector2)pt.ToVector2(aliased)).ToArray());
                sink.EndFigure(new FigureEnd());
            }
            sink.Close();

            var strokeStyle = GetStroke(dashArray, lineJoin);

            



            if (fill.IsVisible())
            {
                renderTarget.FillGeometry(path, GetBrush(fill));
            }
            if (stroke.IsVisible())
            {
                renderTarget.DrawGeometry(path, GetBrush(fill), (float)thickness, strokeStyle);
            }

            sink.Dispose();
            garbage.Add(path);
            garbage.Add(strokeStyle);


            
        }

        /// <summary>
        /// Draws the rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        public void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {

           






            var r = rect.ToRectangleF();
            if (stroke.IsVisible())
            {
                renderTarget.DrawRectangle(r, GetBrush(stroke), (float)thickness);
            }

            if (fill.IsVisible())
            {
                renderTarget.FillRectangle(r, GetBrush(fill));
            }
        }

        /// <summary>
        /// Draws a collection of rectangles, where all have the same stroke and fill.
        /// This performs better than calling DrawRectangle multiple times.
        /// </summary>
        /// <param name="rectangles">The rectangles.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        public void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            var rectangleGeometries = rectangles.Select(x => new RectangleGeometry(this.d2dFactory, x.ToRectangleF())).ToArray();
            var group = new GeometryGroup(this.d2dFactory, FillMode.Winding, rectangleGeometries);

            if (stroke.IsVisible())
                renderTarget.DrawGeometry(group, GetBrush(stroke), (float)thickness);

            if (fill.IsVisible())
                renderTarget.FillGeometry(group, GetBrush(fill));

            garbage.Add(group);
            garbage.AddRange(rectangleGeometries);
                        
        }

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="p">The position.</param>
        /// <param name="text">The text.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotate">The rotation angle.</param>
        /// <param name="halign">The horizontal alignment.</param>
        /// <param name="valign">The vertical alignment.</param>
        /// <param name="maxSize">The maximum size of the text.</param>
        public void DrawText(
            ScreenPoint p,
            string text,
            OxyColor fill,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double rotate,
            OxyPlot.HorizontalAlignment halign,
            OxyPlot.VerticalAlignment valign,
            OxySize? maxSize)
        {
            if (string.IsNullOrWhiteSpace(fontFamily))
                fontFamily = "Arial";
            if (text == null)
                text = string.Empty;

            var format = new TextFormat(dwFactory, fontFamily, GetFontWeight(fontWeight), FontStyle.Normal, FontStretch.Normal, (float)fontSize);
            var maxWidth = 1000f;
            var maxHeight = 1000f;
            if (maxSize != null)
            {
                maxHeight=(float) maxSize.Value.Height;
                maxWidth = (float)maxSize.Value.Width;

            }





            var layout = new TextLayout(dwFactory, text, format, maxWidth, maxHeight);

            var size = new Size2F(layout.Metrics.Width, layout.Metrics.Height);
            if (maxSize != null)
            {
                if (size.Width > maxSize.Value.Width)
                {
                    size.Width =(float) maxSize.Value.Width;
                }

                if (size.Height > maxSize.Value.Height)
                {
                    size.Height =(float) maxSize.Value.Height;
                }
                
            }


            float dx = 0;
            if (halign == OxyPlot.HorizontalAlignment.Center)
            {
                dx = -size.Width / 2;
            }

            if (halign == OxyPlot.HorizontalAlignment.Right)
            {
                dx = -size.Width;
            }

            float dy = 0;
            if (valign == OxyPlot.VerticalAlignment.Middle)
            {
                dy = -size.Height / 2;
            }

            if (valign == OxyPlot.VerticalAlignment.Bottom)
            {
                dy = -size.Height;
            }


            var currentTransform = renderTarget.Transform;
            renderTarget.Transform = Matrix3x2.Translation(dx, dy)* Matrix3x2.Rotation((float)rotate) * Matrix3x2.Translation(p.ToVector2());

            renderTarget.DrawTextLayout(new Vector2(), layout, GetBrush(fill));

            renderTarget.Transform = currentTransform;


            garbage.Add(layout);
            garbage.Add(format);

            

        }

        /// <summary>
        /// Measures the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The text size.</returns>
        public OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (string.IsNullOrWhiteSpace(fontFamily))
                fontFamily = "Arial";
            if (text == null)
                text = string.Empty;

            var format = new TextFormat(dwFactory, fontFamily, GetFontWeight(fontWeight), FontStyle.Normal, FontStretch.Normal, (float)fontSize);




            var layout = new TextLayout(dwFactory, text, format, 1000f, 1000f);
            var res = new OxySize(layout.Metrics.Width, layout.Metrics.Height);

            format.Dispose();
            layout.Dispose();
            

            return res;
        }

        /// <summary>
        /// Sets the tool tip for the following items.
        /// </summary>
        /// <param name="text">The text in the tooltip.</param>
        public void SetToolTip(string text)
        {
            this.currentToolTip = text;
        }

        /// <summary>
        /// Draws the specified portion of the specified <see cref="OxyImage" /> at the specified location and with the specified size.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw.</param>
        /// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw.</param>
        /// <param name="srcWidth">Width of the portion of the source image to draw.</param>
        /// <param name="srcHeight">Height of the portion of the source image to draw.</param>
        /// <param name="destX">The x-coordinate of the upper-left corner of drawn image.</param>
        /// <param name="destY">The y-coordinate of the upper-left corner of drawn image.</param>
        /// <param name="destWidth">The width of the drawn image.</param>
        /// <param name="destHeight">The height of the drawn image.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="interpolate">interpolate if set to <c>true</c>.</param>
        public void DrawImage(
            OxyImage source,
            double srcX,
            double srcY,
            double srcWidth,
            double srcHeight,
            double destX,
            double destY,
            double destWidth,
            double destHeight,
            double opacity,
            bool interpolate)
        {
            if (destWidth <= 0 || destHeight <= 0 || srcWidth <= 0 || srcHeight <= 0)
            {
                return;
            }

          

          
            var bmp = this.GetBitmap(source);

            renderTarget.DrawBitmap(
                bmp,
                new RectangleF((float)destX, (float)destY, (float)destWidth, (float)destHeight),
                (float)opacity,
                interpolate ? BitmapInterpolationMode.Linear : BitmapInterpolationMode.NearestNeighbor,
                new RectangleF((float)srcX, (float)srcY, (float)srcWidth, (float)srcHeight));
       
        }

        /// <summary>
        /// Sets the clipping rectangle.
        /// </summary>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <returns>True if the clipping rectangle was set.</returns>
        public bool SetClip(OxyRect clippingRect)
        {
            this.clipRect = clippingRect.ToRectangleF();
            return true;
        }

        /// <summary>
        /// Resets the clipping rectangle.
        /// </summary>
        public void ResetClip()
        {
            //this.clip = false;
        }

        /// <summary>
        /// Cleans up resources not in use.
        /// </summary>
        /// <remarks>This method is called at the end of each rendering.</remarks>
        public void CleanUp()
        {
            // Find the images in the cache that has not been used since last call to this method
            var imagesToRelease = this.imageCache.Keys.Where(i => !this.imagesInUse.Contains(i)).ToList();

            // Remove the images from the cache
            foreach (var i in imagesToRelease)
            {
                this.imageCache.Remove(i);
            }

            this.imagesInUse.Clear();


            foreach (var item in garbage)
            {
                item.Dispose();
            }

            garbage.Clear();
        }

      

        /// <summary>
        /// Gets the font weight.
        /// </summary>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>A <see cref="FontWeight" /></returns>
        private static FontWeight GetFontWeight(double fontWeight)
        {
            return fontWeight >(int) FontWeight.Normal ? FontWeight.Bold : FontWeight.Normal;
        }




        public void BeginDraw(RenderTarget renderTarget)
        {
            if (this.renderTarget != null && this.renderTarget != renderTarget)
            {
                foreach (var brush in brushCache.Values)
                    brush.Dispose();
                brushCache.Clear();


                foreach (var item in imageCache.Values)
                    item.Dispose();

                imageCache.Clear();
            }
            this.renderTarget = renderTarget;

        }

        public void EndDraw()
        {
           // this.renderTarget = null;
        }



        StrokeStyle GetStroke(double[] dashArray, LineJoin lineJoin)
        {
            if (dashArray == null)
                return new StrokeStyle(d2dFactory, new StrokeStyleProperties { LineJoin = lineJoin.ToDXLineJoin() });
            return new StrokeStyle(d2dFactory, new StrokeStyleProperties { LineJoin = lineJoin.ToDXLineJoin(), DashStyle=DashStyle.Custom }, dashArray.Select(x => (float)x).ToArray());
        }


        Brush GetBrush(OxyColor color)
        {
            Brush brush;
            if (!this.brushCache.TryGetValue(color, out brush))
            {
                brush = new SolidColorBrush(renderTarget, color.ToDXColor());
                this.brushCache.Add(color, brush);
            }

            return brush;
        }
        Bitmap GetBitmap(OxyImage image)
        {
            if (image == null)
            {
                return null;
            }

            if (!this.imagesInUse.Contains(image))
            {
                this.imagesInUse.Add(image);
            }

            Bitmap res;
            using (var stream = new MemoryStream(image.GetData()))
            {

                var decoder = new BitmapDecoder(wicFactory, stream, DecodeOptions.CacheOnDemand);
                var frame = decoder.GetFrame(0);
                var converter = new FormatConverter(wicFactory);
                converter.Initialize(frame,dx.WIC.PixelFormat.Format32bppPRGBA);

                res= Bitmap.FromWicBitmap(renderTarget, converter);



            }
            this.imageCache.Add(image, res);
            return res;
        }



        public void Dispose()
        {
            this.CleanUp();
            //foreach (var item in garbage)
            //{
            //    item.Dispose();
            //}

            foreach (var item in brushCache.Values)
                item.Dispose();

            foreach (var item in imageCache.Values)
                item.Dispose();

            imageCache.Clear();
            brushCache.Clear();
            if (renderTarget != null)
                renderTarget.Dispose();

           // d2dFactory.Dispose();
            dwFactory.Dispose();
            wicFactory.Dispose();

            renderTarget = null;
            d2dFactory = null;
            dwFactory = null;
            wicFactory = null;

           
        }
        
       
    }
}
