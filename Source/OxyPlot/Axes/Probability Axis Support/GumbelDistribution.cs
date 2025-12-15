using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxyPlot.Axes
{
    public class GumbelDistribution
    {

        /// <summary>
        /// Constructs a standard Gumbel (Extreme Value Type I) distribution.
        /// </summary>
        public GumbelDistribution(){ }

        /// <summary>
        /// Constructs a Gumbel (Extreme Value Type I) distribution.
        /// </summary>
        /// <param name="location">The location parameter ξ (Xi).</param>
        /// <param name="scale">The scale parameter α (alpha).</param>
        public GumbelDistribution(double location, double scale)
        {
            _xi = location;
            _alpha = scale;
        }

        private double _xi = 0d;
        private double _alpha = 1d;

        /// <summary>
        /// Gets the location parameter ξ (Xi).
        /// </summary>
        public double Xi
        {
            get
            {
                return _xi;
            }
        }

        /// <summary>
        /// Gets the scale parameter α (alpha).
        /// </summary>
        public double Alpha
        {
            get
            {
                return _alpha;
            }
        }


        /// <summary>
        /// Gets the Probability Density Function (PDF) of the distribution evaluated at a point X.
        /// </summary>
        /// <param name="X">A single point in the distribution range.</param>
        /// <returns>
        /// The probability of X occurring in the distribution.
        /// </returns>
        /// <remarks>
        /// The Probability Density Function (PDF) describes the probability that X will occur.
        /// </remarks>
        public double PDF(double X)
        {
            double z = (X - Xi) / Alpha;
            return 1 / Alpha * Math.Exp(-(z + Math.Exp(-z)));
        }

        /// <summary>
        /// Gets the Cumulative Distribution Function (CDF) for the distribution evaluated at a point X.
        /// </summary>
        /// <param name="X">A single point in the distribution range.</param>
        /// <returns>
        /// The non-exceedance probability given a point X.
        /// </returns>
        /// <remarks>
        /// The Cumulative Distribution Function (CDF) describes the cumulative probability that a given value or any value smaller than it will occur.
        /// </remarks>
        public double CDF(double X)
        {
            double z = (X - Xi) / Alpha;
            return Math.Exp(-Math.Exp(-z));
        }

        /// <summary>
        /// Gets the Inverse Cumulative Distribution Function (ICFD) of the distribution evaluated at a probability.
        /// </summary>
        /// <param name="probability">Probability between 0 and 1.</param>
        /// <returns>
        /// Returns for a given probability in the probability distribution of a random variable,
        /// the value at which the probability of the random variable is less than or equal to the
        /// given probability.
        /// </returns>
        /// <remarks>
        /// This function is also know as the Quantile Function.
        /// </remarks>
        public double InverseCDF(double probability)
        {
            // Validate probability
            if (probability < 0.0 || probability > 1.0)
                throw new ArgumentOutOfRangeException("probability", "Probability must be between 0 and 1."); 
            return Xi - Alpha * Math.Log(-Math.Log(probability));
        }

    }
}
