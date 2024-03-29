﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TileMapAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an annotation that shows a tile based map.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Threading;

    /// <summary>
    /// Provides an annotation that shows a tile based map.
    /// </summary>
    /// <remarks>The longitude and latitude range of the map is defined by the range of the x and y axis, respectively.</remarks>
    public class TileMapAnnotation : Annotation
    {
        /// <summary>
        /// The image cache.
        /// </summary>
        private readonly Dictionary<string, OxyImage> images = new Dictionary<string, OxyImage>();

        /// <summary>
        /// The download queue.
        /// </summary>
        private readonly Queue<string> queue = new Queue<string>();

        /// <summary>
        /// The current number of downloads
        /// </summary>
        private int numberOfDownloads;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMapAnnotation" /> class.
        /// </summary>
        public TileMapAnnotation()
        {
            this.TileSize = 256;
            this.MinZoomLevel = 0;
            this.MaxZoomLevel = 20;
            this.Opacity = 1.0;
            this.MaxNumberOfDownloads = 8;
            this.UserAgent = "OxyPlotExampleLibrary";
        }

        /// <summary>
        /// Gets or sets the max number of simultaneous downloads.
        /// </summary>
        /// <value>The max number of downloads.</value>
        public int MaxNumberOfDownloads { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the copyright notice.
        /// </summary>
        /// <value>The copyright notice.</value>
        public string CopyrightNotice { get; set; }

        /// <summary>
        /// Gets or sets the size of the tiles.
        /// </summary>
        /// <value>The size of the tiles.</value>
        public int TileSize { get; set; }

        /// <summary>
        /// Gets or sets the min zoom level.
        /// </summary>
        /// <value>The min zoom level.</value>
        public int MinZoomLevel { get; set; }

        /// <summary>
        /// Gets or sets the max zoom level.
        /// </summary>
        /// <value>The max zoom level.</value>
        public int MaxZoomLevel { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the user agent used for requests.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Renders the annotation on the specified context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        public override void Render(IRenderContext rc)
        {
            var lon0 = this.XAxis.ActualMinimum;
            var lon1 = this.XAxis.ActualMaximum;
            var lat0 = this.YAxis.ActualMinimum;
            var lat1 = this.YAxis.ActualMaximum;

            // the desired number of tiles horizontally
            double tilesx = this.PlotModel.Width / this.TileSize;

            // calculate the desired zoom level
            var n = tilesx / (((lon1 + 180) / 360) - ((lon0 + 180) / 360));
            var zoom = (int)Math.Round(Math.Log(n) / Math.Log(2));
            if (zoom < this.MinZoomLevel)
            {
                zoom = this.MinZoomLevel;
            }

            if (zoom > this.MaxZoomLevel)
            {
                zoom = this.MaxZoomLevel;
            }

            // find tile coordinates for the corners
            double x0, y0;
            LatLonToTile(lat0, lon0, zoom, out x0, out y0);
            double x1, y1;
            LatLonToTile(lat1, lon1, zoom, out x1, out y1);

            double xmax = Math.Max(x0, x1);
            double xmin = Math.Min(x0, x1);
            double ymax = Math.Max(y0, y1);
            double ymin = Math.Min(y0, y1);

            var clippingRectangle = this.GetClippingRect();

            // Add the tiles
            for (var x = (int)xmin; x < xmax; x++)
            {
                for (var y = (int)ymin; y < ymax; y++)
                {
                    string uri = this.GetTileUri(x, y, zoom);
                    var img = this.GetImage(uri, rc.RendersToScreen);

                    if (img == null)
                    {
                        continue;
                    }

                    // transform from tile coordinates to lat/lon
                    double latitude0, latitude1, longitude0, longitude1;
                    TileToLatLon(x, y, zoom, out latitude0, out longitude0);
                    TileToLatLon(x + 1, y + 1, zoom, out latitude1, out longitude1);

                    // transform from lat/lon to screen coordinates
                    var s00 = this.Transform(longitude0, latitude0);
                    var s11 = this.Transform(longitude1, latitude1);

                    var r = OxyRect.Create(s00.X, s00.Y, s11.X, s11.Y);

                    // draw the image
                    rc.DrawImage(img, r.Left, r.Top, r.Width, r.Height, this.Opacity, true);
                }
            }

            // draw the copyright notice
            var p = new ScreenPoint(clippingRectangle.Right - 5, clippingRectangle.Bottom - 5);
            var textSize = rc.MeasureText(this.CopyrightNotice, this.ActualFont, this.ActualFontSize, this.ActualFontWeight);
            rc.DrawRectangle(
                new OxyRect(p.X - textSize.Width - 2, p.Y - textSize.Height - 2, textSize.Width + 4, textSize.Height + 4), 
                OxyColor.FromAColor(200, OxyColors.White), 
                OxyColors.Undefined,
                0,
                this.EdgeRenderingMode);

            rc.DrawText(
                p,
                this.CopyrightNotice,
                OxyColors.Black,
                this.ActualFont,
                this.ActualFontSize,
                this.ActualFontWeight,
                0,
                HorizontalAlignment.Right,
                VerticalAlignment.Bottom);
        }

        /// <summary>
        /// Transforms a position to a tile coordinate.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="zoom">The zoom.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        private static void LatLonToTile(double latitude, double longitude, int zoom, out double x, out double y)
        {
            // http://wiki.openstreetmap.org/wiki/Slippy_map_tilenames
            int n = 1 << zoom;
            double lat = latitude / 180 * Math.PI;
            x = (longitude + 180.0) / 360.0 * n;
            y = (1.0 - (Math.Log(Math.Tan(lat) + (1.0 / Math.Cos(lat))) / Math.PI)) / 2.0 * n;
        }

        /// <summary>
        /// Transforms a tile coordinate (x,y) to a position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="zoom">The zoom.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        private static void TileToLatLon(double x, double y, int zoom, out double latitude, out double longitude)
        {
            int n = 1 << zoom;
            longitude = (x / n * 360.0) - 180.0;
            double lat = Math.Atan(Math.Sinh(Math.PI * (1 - (2 * y / n))));
            latitude = lat * 180.0 / Math.PI;
        }

        /// <summary>
        /// Gets the image from the specified uri.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="asyncLoading">Get the image asynchronously if set to <c>true</c>. The plot model will be invalidated when the image has been downloaded.</param>
        /// <returns>The image.</returns>
        /// <remarks>This method gets the image from cache, or starts an async download.</remarks>
        private OxyImage GetImage(string uri, bool asyncLoading)
        {
            OxyImage img;
            if (this.images.TryGetValue(uri, out img))
            {
                return img;
            }

            if (!asyncLoading)
            {
                return this.Download(uri);
            }

            lock (this.queue)
            {
                // 'reserve' an image (otherwise multiple downloads of the same uri may happen)
                this.images[uri] = null;
                this.queue.Enqueue(uri);
            }

            this.BeginDownload();
            return null;
        }

        /// <summary>
        /// Downloads the image from the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The image</returns>
        private OxyImage Download(string uri)
        {
            OxyImage img = null;
            var mre = new ManualResetEvent(false);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.BeginGetResponse(
               r =>
               {
                   try
                   {
                       if (request.HaveResponse)
                       {
                           var response = request.EndGetResponse(r);
                           var stream = response.GetResponseStream();

                           var ms = new MemoryStream();
                           stream.CopyTo(ms);
                           var buffer = ms.ToArray();

                           img = new OxyImage(buffer);
                           this.images[uri] = img;
                       }
                   }
                   catch (Exception e)
                   {
                       var ie = e;
                       while (ie != null)
                       {
                           System.Diagnostics.Debug.WriteLine(ie.Message);
                           ie = ie.InnerException;
                       }
                   }
                   finally
                   {
                       mre.Set();
                   }
               },
               request);

            mre.WaitOne();
            return img;
        }

        /// <summary>
        /// Starts the next download in the queue.
        /// </summary>
        private void BeginDownload()
        {
            if (this.numberOfDownloads >= this.MaxNumberOfDownloads)
            {
                return;
            }

            string uri = this.queue.Dequeue();
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";

#if NETFRAMEWORK
            // unavailable in NET Standard 1.0
            request.UserAgent = this.UserAgent;
#else
            // compiles but does not run under NET Framework
            request.Headers["User-Agent"] = this.UserAgent;
#endif

            Interlocked.Increment(ref this.numberOfDownloads);
            request.BeginGetResponse(
                r =>
                {
                    Interlocked.Decrement(ref this.numberOfDownloads);
                    try
                    {
                        if (request.HaveResponse)
                        {
                            var response = request.EndGetResponse(r);
                            var stream = response.GetResponseStream();
                            this.DownloadCompleted(uri, stream);
                        }
                    }
                    catch (Exception e)
                    {
                        var ie = e;
                        while (ie != null)
                        {
                            System.Diagnostics.Debug.WriteLine(ie.Message);
                            ie = ie.InnerException;
                        }
                    }
                },
                request);
        }

        /// <summary>
        /// The download completed, set the image.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="result">The result.</param>
        private void DownloadCompleted(string uri, Stream result)
        {
            if (result == null)
            {
                return;
            }

            var ms = new MemoryStream();
            result.CopyTo(ms);
            var buffer = ms.ToArray();

            var img = new OxyImage(buffer);
            this.images[uri] = img;

            lock (this.queue)
            {
                // Clear old items in the queue, new ones will be added when the plot is refreshed
                foreach (var queuedUri in this.queue)
                {
                    // Remove the 'reserved' image
                    this.images.Remove(queuedUri);
                }

                this.queue.Clear();
            }

            this.PlotModel.InvalidatePlot(false);
            if (this.queue.Count > 0)
            {
                this.BeginDownload();
            }
        }

        /// <summary>
        /// Gets the tile URI.
        /// </summary>
        /// <param name="x">The tile x.</param>
        /// <param name="y">The tile y.</param>
        /// <param name="zoom">The zoom.</param>
        /// <returns>The uri.</returns>
        private string GetTileUri(int x, int y, int zoom)
        {
            string url = this.Url.Replace("{X}", x.ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{Y}", y.ToString(CultureInfo.InvariantCulture));
            return url.Replace("{Z}", zoom.ToString(CultureInfo.InvariantCulture));
        }
    }
}
