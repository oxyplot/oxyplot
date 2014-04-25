// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SilverlightRenderContext.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
//   Rendering Silverlight shapes to a Canvas
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WP8
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Shapes;

	using FontWeights = OxyPlot.FontWeights;

	/// <summary>
	/// Rendering Silverlight shapes to a Canvas
	/// </summary>
	public class PhoneRenderContext : IRenderContext
	{
		/// <summary>
		/// The brush cache.
		/// </summary>
		private readonly Dictionary<OxyColor, Brush> brushCache = new Dictionary<OxyColor, Brush>();

		/// <summary>
		/// The canvas.
		/// </summary>
		private readonly Canvas canvas;

		/// <summary>
		/// The images in use
		/// </summary>
		private readonly HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

		/// <summary>
		/// The image cache
		/// </summary>
		private readonly Dictionary<OxyImage, BitmapSource> imageCache = new Dictionary<OxyImage, BitmapSource>();

		/// <summary>
		/// The current tool tip
		/// </summary>
		private string currentToolTip;

		/// <summary>
		/// The clip rectangle.
		/// </summary>
		private Rect clipRect;

		/// <summary>
		/// The clip flag.
		/// </summary>
		private bool clip;

		/// <summary>
		/// Initializes a new instance of the <see cref="SilverlightRenderContext" /> class.
		/// </summary>
		/// <param name="canvas">The canvas.</param>
		public PhoneRenderContext(Canvas canvas)
		{
			this.canvas = canvas;
			this.Width = canvas.ActualWidth;
			this.Height = canvas.ActualHeight;
			this.RendersToScreen = true;
		}

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public double Height { get; private set; }

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
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		public double Width { get; private set; }

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
			var el = new Ellipse();
			if (stroke.IsVisible())
			{
				el.Stroke = new SolidColorBrush(stroke.ToColor());
				el.StrokeThickness = thickness;
			}

			if (fill.IsVisible())
			{
				el.Fill = new SolidColorBrush(fill.ToColor());
			}

			el.Width = rect.Width;
			el.Height = rect.Height;
			Canvas.SetLeft(el, rect.Left);
			Canvas.SetTop(el, rect.Top);
			this.Add(el, rect.Left, rect.Top);
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
			var path = new Path();
			this.SetStroke(path, stroke, thickness);
			if (fill.IsVisible())
			{
				path.Fill = this.GetCachedBrush(fill);
			}

			var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
			foreach (var rect in rectangles)
			{
				gg.Children.Add(
					new EllipseGeometry
					{
						Center = new Point(rect.Left + (rect.Width / 2), rect.Top + (rect.Height / 2)),
						RadiusX = rect.Width / 2,
						RadiusY = rect.Height / 2
					});
			}

			path.Data = gg;
			this.Add(path);
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
			OxyPenLineJoin lineJoin,
			bool aliased)
		{
			var e = new Polyline();
			this.SetStroke(e, stroke, thickness, lineJoin, dashArray, aliased);

			var pc = new PointCollection();
			foreach (var p in points)
			{
				pc.Add(p.ToPoint(aliased));
			}

			e.Points = pc;

			this.Add(e);
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
			OxyPenLineJoin lineJoin,
			bool aliased)
		{
			var path = new Path();
			this.SetStroke(path, stroke, thickness, lineJoin, dashArray, aliased);
			var pg = new PathGeometry();
			for (int i = 0; i + 1 < points.Count; i += 2)
			{
				// if (points[i].Y==points[i+1].Y)
				// {
				// var line = new Line();

				// line.X1 = 0.5+(int)points[i].X;
				// line.X2 = 0.5+(int)points[i+1].X;
				// line.Y1 = 0.5+(int)points[i].Y;
				// line.Y2 = 0.5+(int)points[i+1].Y;
				// SetStroke(line, OxyColors.DarkRed, thickness, lineJoin, dashArray, aliased);
				// Add(line);
				// continue;
				// }
				var figure = new PathFigure { StartPoint = points[i].ToPoint(aliased), IsClosed = false };
				figure.Segments.Add(new LineSegment { Point = points[i + 1].ToPoint(aliased) });
				pg.Figures.Add(figure);
			}

			path.Data = pg;
			this.Add(path);
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
			OxyPenLineJoin lineJoin,
			bool aliased)
		{
			var po = new Polygon();
			this.SetStroke(po, stroke, thickness, lineJoin, dashArray, aliased);

			if (fill.IsVisible())
			{
				po.Fill = this.GetCachedBrush(fill);
			}

			var pc = new PointCollection();
			foreach (var p in points)
			{
				pc.Add(p.ToPoint(aliased));
			}

			po.Points = pc;

			this.Add(po);
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
			OxyPenLineJoin lineJoin,
			bool aliased)
		{
			var path = new Path();
			this.SetStroke(path, stroke, thickness, lineJoin, dashArray, aliased);
			if (fill.IsVisible())
			{
				path.Fill = this.GetCachedBrush(fill);
			}

			var pg = new PathGeometry { FillRule = FillRule.Nonzero };
			foreach (var polygon in polygons)
			{
				var figure = new PathFigure { IsClosed = true };
				bool first = true;
				foreach (var p in polygon)
				{
					if (first)
					{
						figure.StartPoint = p.ToPoint(aliased);
						first = false;
					}
					else
					{
						figure.Segments.Add(new LineSegment { Point = p.ToPoint(aliased) });
					}
				}

				pg.Figures.Add(figure);
			}

			path.Data = pg;
			this.Add(path);
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
			var el = new Rectangle();
			if (stroke.IsVisible())
			{
				el.Stroke = new SolidColorBrush(stroke.ToColor());
				el.StrokeThickness = thickness;
			}

			if (fill.IsVisible())
			{
				el.Fill = new SolidColorBrush(fill.ToColor());
			}

			el.Width = rect.Width;
			el.Height = rect.Height;
			Canvas.SetLeft(el, rect.Left);
			Canvas.SetTop(el, rect.Top);
			this.Add(el, rect.Left, rect.Top);
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
			var path = new Path();
			this.SetStroke(path, stroke, thickness);
			if (fill.IsVisible())
			{
				path.Fill = this.GetCachedBrush(fill);
			}

			var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
			foreach (var rect in rectangles)
			{
				gg.Children.Add(new RectangleGeometry { Rect = rect.ToRect(true) });
			}

			path.Data = gg;
			this.Add(path);
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
			var tb = new TextBlock { Text = text, Foreground = new SolidColorBrush(fill.ToColor()) };

			// tb.SetValue(TextOptions.TextHintingModeProperty, TextHintingMode.Animated);
			if (fontFamily != null)
			{
				tb.FontFamily = new FontFamily(fontFamily);
			}

			if (fontSize > 0)
			{
				tb.FontSize = fontSize;
			}

			tb.FontWeight = GetFontWeight(fontWeight);

			tb.Measure(new Size(1000, 1000));
			var size = new Size(tb.ActualWidth, tb.ActualHeight);
			if (maxSize != null)
			{
				if (size.Width > maxSize.Value.Width)
				{
					size.Width = maxSize.Value.Width;
				}

				if (size.Height > maxSize.Value.Height)
				{
					size.Height = maxSize.Value.Height;
				}

				tb.Clip = new RectangleGeometry { Rect = new Rect(0, 0, size.Width, size.Height) };
			}

			double dx = 0;
			if (halign == OxyPlot.HorizontalAlignment.Center)
			{
				dx = -size.Width / 2;
			}

			if (halign == OxyPlot.HorizontalAlignment.Right)
			{
				dx = -size.Width;
			}

			double dy = 0;
			if (valign == OxyPlot.VerticalAlignment.Middle)
			{
				dy = -size.Height / 2;
			}

			if (valign == OxyPlot.VerticalAlignment.Bottom)
			{
				dy = -size.Height;
			}

			var transform = new TransformGroup();
			transform.Children.Add(new TranslateTransform { X = (int)dx, Y = (int)dy });
			if (!rotate.Equals(0))
			{
				transform.Children.Add(new RotateTransform { Angle = rotate });
			}

			transform.Children.Add(new TranslateTransform { X = (int)p.X, Y = (int)p.Y });
			tb.RenderTransform = transform;
			this.ApplyTooltip(tb);

			if (this.clip)
			{
				// add a clipping container that is not rotated
				var c = new Canvas();
				c.Children.Add(tb);
				this.Add(c);
			}
			else
			{
				this.Add(tb);
			}
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
			if (string.IsNullOrEmpty(text))
			{
				return OxySize.Empty;
			}

			var tb = new TextBlock { Text = text };

			if (fontFamily != null)
			{
				tb.FontFamily = new FontFamily(fontFamily);
			}

			if (fontSize > 0)
			{
				tb.FontSize = fontSize;
			}

			tb.FontWeight = GetFontWeight(fontWeight);

			tb.Measure(new Size(1000, 1000));

			return new OxySize(tb.ActualWidth, tb.ActualHeight);
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

			var image = new Image();
			var bmp = this.GetImageSource(source);

			if (srcX.Equals(0) && srcY.Equals(0) && srcWidth.Equals(bmp.PixelWidth) && srcHeight.Equals(bmp.PixelHeight))
			{
				// do not crop
			}
			else
			{
				// TODO: cropped image not available in Silverlight??
				// bmp = new CroppedBitmap(bmp, new Int32Rect((int)srcX, (int)srcY, (int)srcWidth, (int)srcHeight));
				return;
			}

			image.Opacity = opacity;
			image.Width = destWidth;
			image.Height = destHeight;
			image.Stretch = Stretch.Fill;

			// TODO: not available in Silverlight??
			// RenderOptions.SetBitmapScalingMode(image, interpolate ? BitmapScalingMode.HighQuality : BitmapScalingMode.NearestNeighbor);
			// Canvas.SetLeft(image, x);
			// Canvas.SetTop(image, y);
			image.RenderTransform = new TranslateTransform { X = destX, Y = destY };
			image.Source = bmp;
			this.ApplyTooltip(image);
			this.Add(image, destX, destY);
		}

		/// <summary>
		/// Sets the clipping rectangle.
		/// </summary>
		/// <param name="clippingRect">The clipping rectangle.</param>
		/// <returns>True if the clipping rectangle was set.</returns>
		public bool SetClip(OxyRect clippingRect)
		{
			this.clipRect = clippingRect.ToRect(false);
			this.clip = true;
			return true;
		}

		/// <summary>
		/// Resets the clipping rectangle.
		/// </summary>
		public void ResetClip()
		{
			this.clip = false;
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
		}

		/// <summary>
		/// Creates the dash array collection.
		/// </summary>
		/// <param name="dashArray">The dash array.</param>
		/// <returns>A DoubleCollection.</returns>
		private static DoubleCollection CreateDashArrayCollection(IEnumerable<double> dashArray)
		{
			var dac = new DoubleCollection();
			foreach (double v in dashArray)
			{
				dac.Add(v);
			}

			return dac;
		}

		/// <summary>
		/// Gets the font weight.
		/// </summary>
		/// <param name="fontWeight">The font weight.</param>
		/// <returns>A <see cref="FontWeight" /></returns>
		private static FontWeight GetFontWeight(double fontWeight)
		{
			return fontWeight > FontWeights.Normal ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Normal;
		}

		/// <summary>
		/// Adds the specified element to the canvas.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="clipOffsetX">The clip offset X.</param>
		/// <param name="clipOffsetY">The clip offset Y.</param>
		private void Add(UIElement element, double clipOffsetX = 0, double clipOffsetY = 0)
		{
			if (this.clip)
			{
				this.ApplyClip(element, clipOffsetX, clipOffsetY);
			}

			this.canvas.Children.Add(element);
		}

		/// <summary>
		/// Applies the tooltip to the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		private void ApplyTooltip(DependencyObject element)
		{
			if (!string.IsNullOrEmpty(this.currentToolTip))
			{
				ToolTipService.SetToolTip(element, this.currentToolTip);
			}
		}

		/// <summary>
		/// Gets the cached brush.
		/// </summary>
		/// <param name="stroke">The stroke.</param>
		/// <returns>The brush.</returns>
		private Brush GetCachedBrush(OxyColor stroke)
		{
			Brush brush;
			if (!this.brushCache.TryGetValue(stroke, out brush))
			{
				brush = new SolidColorBrush(stroke.ToColor());
				this.brushCache.Add(stroke, brush);
			}

			return brush;
		}

		/// <summary>
		/// Sets the stroke of the specified shape.
		/// </summary>
		/// <param name="shape">The shape.</param>
		/// <param name="stroke">The stroke.</param>
		/// <param name="thickness">The thickness.</param>
		/// <param name="lineJoin">The line join.</param>
		/// <param name="dashArray">The dash array.</param>
		/// <param name="aliased">aliased if set to <c>true</c>.</param>
		private void SetStroke(
			Shape shape,
			OxyColor stroke,
			double thickness,
			OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter,
			IEnumerable<double> dashArray = null,
			bool aliased = false)
		{
			if (stroke.IsVisible() && thickness > 0)
			{
				shape.Stroke = this.GetCachedBrush(stroke);

				switch (lineJoin)
				{
					case OxyPenLineJoin.Round:
						shape.StrokeLineJoin = PenLineJoin.Round;
						break;
					case OxyPenLineJoin.Bevel:
						shape.StrokeLineJoin = PenLineJoin.Bevel;
						break;

					// The default StrokeLineJoin is Miter
				}

				shape.StrokeThickness = thickness;

				if (dashArray != null)
				{
					shape.StrokeDashArray = CreateDashArrayCollection(dashArray);
				}

				if (aliased)
				{
					// shape.UseLayoutRounding = aliased;
				}
			}
		}

		/// <summary>
		/// Applies the clip rectangle.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="x">The x offset of the element.</param>
		/// <param name="y">The y offset of the element.</param>
		private void ApplyClip(UIElement image, double x, double y)
		{
			image.Clip = new RectangleGeometry { Rect = new Rect(this.clipRect.X - x, this.clipRect.Y - y, this.clipRect.Width, this.clipRect.Height) };
		}

		/// <summary>
		/// Gets the bitmap source.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <returns>The bitmap source.</returns>
		private BitmapSource GetImageSource(OxyImage image)
		{
			if (image == null)
			{
				return null;
			}

			if (!this.imagesInUse.Contains(image))
			{
				this.imagesInUse.Add(image);
			}

			BitmapSource src;
			if (this.imageCache.TryGetValue(image, out src))
			{
				return src;
			}

			using (var ms = new System.IO.MemoryStream(image.GetData()))
			{
				var bitmapImage = new BitmapImage();
				bitmapImage.SetSource(ms);
				this.imageCache.Add(image, bitmapImage);
				return bitmapImage;
			}
		}
	}
}