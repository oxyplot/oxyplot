using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;

// --------------------------------------------------------------------------------------------------------------------
// <copyright filename="GraphicsRenderContext.cs" company="OxyPlot">
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
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Core.Drawing
{
   /// <summary>
   /// The graphics render context.
   /// </summary>
   internal class GraphicsRenderContext : RenderContextBase, IDisposable
   {
      /// <summary>
      /// The font size factor.
      /// </summary>
      private const float FontsizeFactor = 0.8f;

      /// <summary>
      /// The GDI+ drawing surface.
      /// </summary>
      private Graphics g;

      /// <summary>
      /// Initializes a new instance of the <see cref="GraphicsRenderContext"/> class.
      /// </summary>
      public GraphicsRenderContext(Graphics g)
      {
         this.g = g;
      }

      /// <summary>
      /// Sets the graphics target.
      /// </summary>
      /// <param name="graphics">The graphics surface.</param>
      public void SetGraphicsTarget(Graphics graphics)
      {
         g = graphics;
      }

      /// <summary>
      /// Draws the ellipse.
      /// </summary>
      /// <param name="rect">The rect.</param>
      /// <param name="fill">The fill.</param>
      /// <param name="stroke">The stroke.</param>
      /// <param name="thickness">The thickness.</param>
      public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
      {
         if (fill != null)
         {
            g.FillEllipse(
               fill.ToBrush(), (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
         }

         if (stroke == null || thickness <= 0)
         {
            return;
         }

         using (var pen = new Pen(stroke.ToColor(), (float)thickness))
         {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawEllipse(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
         }
      }

      /// <summary>
      /// Draws the line.
      /// </summary>
      /// <param name="points">The points.</param>
      /// <param name="stroke">The stroke.</param>
      /// <param name="thickness">The thickness.</param>
      /// <param name="dashArray">The dash array.</param>
      /// <param name="lineJoin">The line join.</param>
      /// <param name="aliased">if set to <c>true</c> [aliased].</param>
      public override void DrawLine(
         IList<ScreenPoint> points,
         OxyColor stroke,
         double thickness,
         double[] dashArray,
         LineJoin lineJoin,
         bool aliased)
      {
         if (stroke == null || thickness <= 0 || points.Count < 2)
         {
            return;
         }

         g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;
         using (var pen = new Pen(stroke.ToColor(), (float)thickness))
         {

            if (dashArray != null)
            {
               pen.DashPattern = ToFloatArray(dashArray);
            }

            switch (lineJoin)
            {
               case LineJoin.Round:
                  pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                  break;
               case LineJoin.Bevel:
                  pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                  break;

               // The default LineJoin is Miter
            }

            g.DrawLines(pen, ToPoints(points));
         }
      }

      /// <summary>
      /// Draws the polygon.
      /// </summary>
      /// <param name="points">The points.</param>
      /// <param name="fill">The fill.</param>
      /// <param name="stroke">The stroke.</param>
      /// <param name="thickness">The thickness.</param>
      /// <param name="dashArray">The dash array.</param>
      /// <param name="lineJoin">The line join.</param>
      /// <param name="aliased">if set to <c>true</c> [aliased].</param>
      public override void DrawPolygon(
         IList<ScreenPoint> points,
         OxyColor fill,
         OxyColor stroke,
         double thickness,
         double[] dashArray,
         LineJoin lineJoin,
         bool aliased)
      {
         if (points.Count < 2)
         {
            return;
         }

         g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;

         PointF[] pts = ToPoints(points);
         if (fill != null)
         {
            g.FillPolygon(fill.ToBrush(), pts);
         }

         if (stroke != null && thickness > 0)
         {
            using (var pen = new Pen(stroke.ToColor(), (float)thickness))
            {

               if (dashArray != null)
               {
                  pen.DashPattern = ToFloatArray(dashArray);
               }

               switch (lineJoin)
               {
                  case LineJoin.Round:
                     pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                     break;
                  case LineJoin.Bevel:
                     pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                     break;

                  // The default LineJoin is Miter
               }

               g.DrawPolygon(pen, pts);
            }
         }
      }

      /// <summary>
      /// The draw rectangle.
      /// </summary>
      /// <param name="rect">
      /// The rect.
      /// </param>
      /// <param name="fill">
      /// The fill.
      /// </param>
      /// <param name="stroke">
      /// The stroke.
      /// </param>
      /// <param name="thickness">
      /// The thickness.
      /// </param>
      public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
      {
         if (fill != null)
         {
            g.FillRectangle(
               fill.ToBrush(), (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
         }

         if (stroke == null || thickness <= 0)
         {
            return;
         }

         using (var pen = new Pen(stroke.ToColor(), (float)thickness))
         {
            g.DrawRectangle(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
         }
      }

      /// <summary>
      /// The draw text.
      /// </summary>
      /// <param name="p">
      /// The p.
      /// </param>
      /// <param name="text">
      /// The text.
      /// </param>
      /// <param name="fill">
      /// The fill.
      /// </param>
      /// <param name="fontFamily">
      /// The font family.
      /// </param>
      /// <param name="fontSize">
      /// The font size.
      /// </param>
      /// <param name="fontWeight">
      /// The font weight.
      /// </param>
      /// <param name="rotate">
      /// The rotate.
      /// </param>
      /// <param name="halign">
      /// The halign.
      /// </param>
      /// <param name="valign">
      /// The valign.
      /// </param>
      /// <param name="maxSize">
      /// The maximum size of the text.
      /// </param>
      public override void DrawText(
         ScreenPoint p,
         string text,
         OxyColor fill,
         string fontFamily,
         double fontSize,
         double fontWeight,
         double rotate,
         HorizontalAlignment halign,
         VerticalAlignment valign,
         OxySize? maxSize)
      {
         var fs = FontStyle.Regular;
         if (fontWeight >= 700)
         {
            fs = FontStyle.Bold;
         }

         using (var font = new Font(fontFamily, (float)fontSize * FontsizeFactor, fs))
         {
            using (var sf = new StringFormat { Alignment = StringAlignment.Near })
            {
               var size = g.MeasureString(text, font);
               if (maxSize != null)
               {
                  if (size.Width > maxSize.Value.Width)
                  {
                     size.Width = (float)maxSize.Value.Width;
                  }

                  if (size.Height > maxSize.Value.Height)
                  {
                     size.Height = (float)maxSize.Value.Height;
                  }
               }

               float dx = 0;
               if (halign == HorizontalAlignment.Center)
               {
                  dx = -size.Width / 2;
               }

               if (halign == HorizontalAlignment.Right)
               {
                  dx = -size.Width;
               }

               float dy = 0;
               sf.LineAlignment = StringAlignment.Near;
               if (valign == VerticalAlignment.Middle)
               {
                  dy = -size.Height / 2;
               }

               if (valign == VerticalAlignment.Bottom)
               {
                  dy = -size.Height;
               }

               g.TranslateTransform((float)p.X, (float)p.Y);
               if (Math.Abs(rotate) > double.Epsilon)
               {
                  g.RotateTransform((float)rotate);
               }

               g.TranslateTransform(dx, dy);

               var layoutRectangle = new RectangleF(0, 0, size.Width, size.Height);
               g.DrawString(text, font, fill.ToBrush(), layoutRectangle, sf);

               g.ResetTransform();
            }
         }
      }

      /// <summary>
      /// The measure text.
      /// </summary>
      /// <param name="text">The text.</param>
      /// <param name="fontFamily">The font family.</param>
      /// <param name="fontSize">The font size.</param>
      /// <param name="fontWeight">The font weight.</param>
      /// <returns>The size of the text.</returns>
      public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
      {
         if (text == null)
         {
            return OxySize.Empty;
         }

         var fs = FontStyle.Regular;
         if (fontWeight >= 700)
         {
            fs = FontStyle.Bold;
         }

         using (var font = new Font(fontFamily, (float)fontSize * FontsizeFactor, fs))
         {
            var size = g.MeasureString(text, font);
            return new OxySize(size.Width, size.Height);
         }
      }



      /// <summary>
      /// Converts a double array to a float array.
      /// </summary>
      /// <param name="a">
      /// The a.
      /// </param>
      /// <returns>
      /// The float array.
      /// </returns>
      private float[] ToFloatArray(double[] a)
      {
         if (a == null)
         {
            return null;
         }

         var r = new float[a.Length];
         for (int i = 0; i < a.Length; i++)
         {
            r[i] = (float)a[i];
         }

         return r;
      }

      /// <summary>
      /// Converts a list of point to an array of PointF.
      /// </summary>
      /// <param name="points">
      /// The points.
      /// </param>
      /// <returns>
      /// An array of points.
      /// </returns>
      private PointF[] ToPoints(IList<ScreenPoint> points)
      {
         if (points == null)
         {
            return null;
         }

         var r = new PointF[points.Count()];
         int i = 0;
         foreach (ScreenPoint p in points)
         {
            r[i++] = new PointF((float)p.X, (float)p.Y);
         }

         return r;
      }

      public override void CleanUp()
      {
         var imagesToRelease = imageCache.Keys.Where(i => !imagesInUse.Contains(i));
         foreach (var i in imagesToRelease)
         {
            var image = GetImage(i);
            image.Dispose();
            imageCache.Remove(i);
         }

         imagesInUse.Clear();
      }

      //public override OxyImageInfo GetImageInfo(OxyImage source)
      //{
      //    var image = this.GetImage(source);
      //    return image == null ? null : new OxyImageInfo { Width = (uint)image.Width, Height = (uint)image.Height, DpiX = image.HorizontalResolution, DpiY = image.VerticalResolution };
      //}

      //public override void DrawImage(OxyImage source, uint srcX, uint srcY, uint srcWidth, uint srcHeight, double x, double y, double w, double h, double opacity, bool interpolate)
      //{
      //    var image = this.GetImage(source);
      //    if (image != null)
      //    {
      //        ImageAttributes ia = null;
      //        if (opacity < 1)
      //        {
      //            var cm = new ColorMatrix
      //                         {
      //                             Matrix00 = 1f,
      //                             Matrix11 = 1f,
      //                             Matrix22 = 1f,
      //                             Matrix33 = 1f,
      //                             Matrix44 = (float)opacity
      //                         };

      //            ia = new ImageAttributes();
      //            ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
      //        }

      //        g.InterpolationMode = interpolate ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
      //        int sx = (int)Math.Round(x);
      //        int sy = (int)Math.Round(y);
      //        int sw = (int)Math.Round(x + w) - sx;
      //        int sh = (int)Math.Round(y + h) - sy;
      //        g.DrawImage(image, new Rectangle(sx, sy, sw, sh), srcX, srcY, srcWidth, srcHeight, GraphicsUnit.Pixel, ia);
      //    }
      //}

      private HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

      private Dictionary<OxyImage, Image> imageCache = new Dictionary<OxyImage, Image>();


      private Image GetImage(OxyImage source)
      {
         if (source == null)
         {
            return null;
         }

         if (!imagesInUse.Contains(source))
         {
            imagesInUse.Add(source);
         }

         Image src;
         if (imageCache.TryGetValue(source, out src))
         {
            return src;
         }

         if (source != null)
         {
            Image btm;
            using (var ms = new MemoryStream(source.GetData()))
            {
               btm = Image.FromStream(ms);
            }

            imageCache.Add(source, btm);
            return btm;
         }

         return null;
      }

      public override bool SetClip(OxyRect rect)
      {
         g.SetClip(rect.ToRect());
         return true;
      }

      public override void ResetClip()
      {
         g.ResetClip();
      }

      #region Implementation of IDisposable

      public void Dispose()
      {
         // dispose images
         foreach (var i in imageCache)
         {
            i.Value.Dispose();
         }
      }

      #endregion
   }
}
