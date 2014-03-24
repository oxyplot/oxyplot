// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageEncoderOptions.cs" company="OxyPlot">
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
//   Provides an abstract base class for image encoder options.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides an abstract base class for image encoder options.
    /// </summary>
    public abstract class ImageEncoderOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageEncoderOptions" /> class.
        /// </summary>
        protected ImageEncoderOptions()
        {
            this.DpiX = 96;
            this.DpiY = 96;
        }

        /// <summary>
        /// Gets or sets the horizontal resolution (in dots per inch).
        /// </summary>
        /// <value>The resolution. The default value is 96 dpi.</value>
        public double DpiX { get; set; }

        /// <summary>
        /// Gets or sets the vertical resolution (in dots per inch).
        /// </summary>
        /// <value>The resolution. The default value is 96 dpi.</value>
        public double DpiY { get; set; }
    }
}