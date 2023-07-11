namespace OxyPlot.ImageSharp
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing.Processors;

    /// <summary>
    /// Performs a 'mirror' (clamp) along the edge of the image, which is used to assist with drawing non-pixel aligned and interpolate images.
    /// </summary>
    internal class MirrorPadProcessor : IImageProcessor
    {
        private class MirrorPadImplementation<TPixel> : ImageProcessor<TPixel> where TPixel : unmanaged, IPixel<TPixel>
        {
            public MirrorPadImplementation(Configuration configuration, Image<TPixel> source, Rectangle sourceRectangle)
                : base(configuration, source, sourceRectangle)
            {
            }

            protected override void OnFrameApply(ImageFrame<TPixel> source)
            {
                source[0, 0] = source[1, 1];
                source[source.Width - 1, 0] = source[source.Width - 2, 1];
                source[0, source.Height - 1] = source[1, source.Height - 2];
                source[source.Width - 1, source.Height - 1] = source[source.Width - 2, source.Height - 2];

                for (int x = 1; x < source.Width - 1; x++)
                {
                    source[x, 0] = source[x, 1];
                    source[x, source.Height - 1] = source[x, source.Height - 2];
                }

                for (int y = 1; y < source.Height - 1; y++)
                {
                    source[0, y] = source[1, y];
                    source[source.Width - 1, y] = source[source.Width - 2, y];
                }
            }
        }

        public IImageProcessor<TPixel> CreatePixelSpecificProcessor<TPixel>(Configuration configuration, Image<TPixel> source, Rectangle sourceRectangle) where TPixel : unmanaged, IPixel<TPixel>
        {
            return new MirrorPadImplementation<TPixel>(configuration, source, sourceRectangle);
        }
    }
}
