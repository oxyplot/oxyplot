namespace IconGenerator
{
    using System;

    /// <summary>
    /// Provides functions for the icon generator.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Initializes static members of the <see cref="Functions"/> class.
        /// </summary>
        static Functions()
        {
            Peaks = (x, y) => 3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1)) - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y) - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);
        }

        /// <summary>
        /// Gets the peaks function.
        /// </summary>
        public static Func<double, double, double> Peaks { get; private set; }
    }
}