using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxyPlot.Axes
{
    public class NormalDistribution
    {

        /// <summary>
        /// Constructs a Normal (Gaussian) distribution with a mean of 0 and standard deviation of 1.
        /// </summary>
        public NormalDistribution() {}

        /// <summary>
        /// Constructs a Normal (Gaussian) distribution.
        /// </summary>
        /// <param name="mu">The mean.</param>
        /// <param name="sigma">The standard deviation.</param>
        public NormalDistribution(double mu, double sigma)
        {
            _mu = mu;
            _sigma = sigma;
        }

       
        // Private variables
        private double _mu = 0.0;
        private double _sigma = 1.0;

        /// <summary>
        /// Gets the location parameter µ (Mu).
        /// </summary>
        public double Mu
        {
            get
            {
                return _mu;
            }
        }

        /// <summary>
        /// Gets the scale parameter σ (sigma).
        /// </summary>
        public double Sigma
        {
            get
            {
                return _sigma;
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
            double d = (X - Mu) / Sigma;
            return Math.Exp(-0.5 * d * d) / (Math.Sqrt(2.0 * Math.PI) * Sigma);
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
            return 0.5 * (1.0 + Erf((X - Mu) / (Sigma * Math.Sqrt(2.0))));
        }

        /// <summary>
        /// Constants used in the rational approximation.
        /// </summary>
        private static double[] inverse_P0 = new[] { -59.963350101410789, 98.001075418599967, -56.676285746907027, 13.931260938727968, -1.2391658386738125 };
        private static double[] inverse_Q0 = new[] { 1.9544885833814176, 4.6762791289888153, 86.360242139089053, -225.46268785411937, 200.26021238006067, -82.037225616833339, 15.90562251262117, -1.1833162112133 };
        private static double[] inverse_P1 = new[] { 4.0554489230596245, 31.525109459989388, 57.162819224642128, 44.080507389320083, 14.684956192885803, 2.1866330685079025, -0.14025607917135449, -0.035042462682784818, -0.00085745678515468545 };
        private static double[] inverse_Q1 = new[] { 15.779988325646675, 45.390763512887922, 41.3172038254672, 15.04253856929075, 2.5046494620830941, -0.14218292285478779, -0.038080640769157827, -0.00093325948089545744 };
        private static double[] inverse_P2 = new[] { 3.2377489177694603, 6.9152288906898418, 3.9388102529247444, 1.3330346081580755, 0.20148538954917908, 0.012371663481782003, 0.00030158155350823543, 0.0000026580697468673755, 0.0000000062397453918498331 };
        private static double[] inverse_Q2 = new[] { 6.02427039364742, 3.6798356385616087, 1.3770209948908132, 0.21623699359449664, 0.013420400608854318, 0.00032801446468212774, 0.0000028924786474538068, 0.0000000067901940800998127 };

        /// <summary>
        /// A rational approximation for the Normal (Gaussian) inverse cumulative distribution function.
        /// </summary>
        /// <param name="probability">Probability between 0 and 1.</param>
        /// <remarks>
        /// <see href = "http://accord-framework.net"/>
        /// </remarks>
        /// <returns>
        /// Returns the value, <c>x</c>, for which the area under the Normal (Gaussian)
        /// probability density function (integrated from minus infinity to <c>x</c>) Is
        /// equal to the argument <c>y</c> (assumes mean Is zero, variance Is one).
        /// </returns>
        private double RationalApproximation(double probability)
        {
            double s2pi = Math.Sqrt(2.0 * Math.PI);
            int code = 1;
            double y = probability;
            double x;

            if (y > 0.8646647167633873)
            {
                y = 1.0 - y;
                code = 0;
            }

            if (y > 0.1353352832366127)
            {
                y -= 0.5;
                double y2 = y * y;
                x = y + y * (y2 * PolynomialRev(inverse_P0, y2) / PolynomialRev_1(inverse_Q0, y2));
                x *= s2pi;
                return x;
            }


            x = Math.Sqrt(-2.0 * Math.Log(y));
            double x0 = x - Math.Log(x) / x;
            double z = 1.0 / x;
            double x1;

            if (x < 8.0)
                x1 = z * PolynomialRev(inverse_P1, z) / PolynomialRev_1(inverse_Q1, z);
            else
                x1 = z * PolynomialRev(inverse_P2, z) / PolynomialRev_1(inverse_Q2, z);

            x = x0 - x1;

            if (code != 0)
                x = -x;

            return x;
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
            if (probability < 0.0 | probability > 1.0)
                throw new ArgumentOutOfRangeException("probability", "Probability must be between 0 and 1.");
            return Mu + Sigma * RationalApproximation(probability);
        }


        /// <summary>
        /// Error function.
        /// </summary>
        public double Erf(double X)
        {
            if (X < 0)
                return -GammaLowerIncomplete(0.5, X * X);
            else
                return GammaLowerIncomplete(0.5, X * X);
        }

        /// <summary>
        /// Natural logarithm of the gamma function.
        /// </summary>
        /// <para>
        /// References: Based algorithm ACM291, Commun. Assoc. Comput. Mach. (1966)
        /// </para>
        public static double LogGamma(double X)
        {
            double SMALL = 0.0000001;
            double CRIT = 13;
            double BIG = 1000000000.0;
            double TOOBIG = 2.0E+36;

            double C0 = 0.5 * Math.Log(2 * Math.PI);
            double C1 = 0.083333333333333329;
            double C2 = -0.0027777777777777779;
            double C3 = 0.00079365079365079365;
            double C4 = -0.00059523809523809529;
            double C5 = 0.00084175084175084171;
            double C6 = -0.0019175269175269176;
            double C7 = 0.00641025641025641;
            double S1 = -0.57721566490153287;
            double S2 = 0.8224670334241132;
            double _logGamma = 0.0;
            double XX;
            double Y;
            double Z;
            double SUM1;
            double SUM2;

            if (X <= 0.0)
                throw new ArgumentOutOfRangeException("X", "X must be greater than zero.");

            if (X > TOOBIG)
                throw new ArgumentOutOfRangeException("X", "X is too big. It must be less than 2E36.");

            if (Math.Abs(X - 2.0) > SMALL)
            {
                if (Math.Abs(X - 1.0) > SMALL)
                {
                    if (X > SMALL)
                    {

                        // reduce to LogGamma(X+N) where X+N >= CRIT
                        SUM1 = 0.0;
                        Y = X;

                        if (Y < CRIT)
                        {
                            Z = 1.0;
                            while (Y < CRIT)
                            {
                                Z = Z * Y;
                                Y = Y + 1.0;
                            }
                            SUM1 = SUM1 - Math.Log(Z);
                        }

                        // use asymptotic expansion if Y >= CRIT
                        SUM1 = SUM1 + (Y - 0.5) * Math.Log(Y) - Y + C0;
                        SUM2 = 0.0;

                        if (Y >= BIG)
                            _logGamma = SUM1 + SUM2;
                        else
                        {
                            Z = 1.0 / (Y * Y);
                            SUM2 = ((((((C7 * Z + C6) * Z + C5) * Z + C4) * Z + C3) * Z + C2) * Z + C1) / Y;
                            _logGamma = SUM1 + SUM2;
                        }
                    }
                    else
                        _logGamma = -Math.Log(X) + S1 * X;
                }
                else
                {
                    XX = X - 1.0;
                    _logGamma = XX * (S1 + XX * S2);
                }
            }
            else
            {
                XX = X - 2.0;
                _logGamma = Math.Log(X - 1.0) + XX * (S1 + XX * S2);
            }

            return _logGamma;
        }

        /// <summary>
        /// Upper incomplete regularized Gamma function Q
        /// (a.k.a the incomplete complemented Gamma function)
        /// </summary>
        /// <remarks>
        /// This function is equivalent to Q(x) = Γ(s, x) / Γ(s).
        /// </remarks>
        public double GammaUpperIncomplete(double a, double x)
        {
            const double LogMax = 709.782712893384;
            const double DoubleEpsilon = 0.000000000000000111022302462516;
            const double big = 4.5035996273705E+15;
            const double biginv = 0.000000000000000222044604925031;
            double ans;
            double ax;
            double c;
            double yc;
            double r;
            double t;
            double y;
            double z;
            double pk;
            double pkm1;
            double pkm2;
            double qk;
            double qkm1;
            double qkm2;

            if (x <= 0 || a <= 0)
                return 1.0;

            if (x < 1.0 || x < a)
                return 1.0 - GammaLowerIncomplete(a, x);

            if (double.IsPositiveInfinity(x))
                return 0;

            ax = a * Math.Log(x) - x - LogGamma(a);

            if (ax < -LogMax)
                return 0.0;

            ax = Math.Exp(ax);

            // continued fraction
            y = 1.0 - a;
            z = x + y + 1.0;
            c = 0.0;
            pkm2 = 1.0;
            qkm2 = x;
            pkm1 = x + 1.0;
            qkm1 = z * x;
            ans = pkm1 / qkm1;

            do
            {
                c += 1.0;
                y += 1.0;
                z += 2.0;
                yc = y * c;
                pk = pkm1 * z - pkm2 * yc;
                qk = qkm1 * z - qkm2 * yc;
                if (qk != 0)
                {
                    r = pk / qk;
                    t = Math.Abs((ans - r) / r);
                    ans = r;
                }
                else
                    t = 1.0;
                pkm2 = pkm1;
                pkm1 = pk;
                qkm2 = qkm1;
                qkm1 = qk;
                if (Math.Abs(pk) > big)
                {
                    pkm2 *= biginv;
                    pkm1 *= biginv;
                    qkm2 *= biginv;
                    qkm1 *= biginv;
                }
            }
            while (t > DoubleEpsilon);

            return ans * ax;
        }

        /// <summary>
        /// Lower incomplete regularized gamma function P
        /// (a.k.a. the incomplete Gamma function).
        /// </summary>
        /// <remarks>
        /// This function is equivalent to P(x) = γ(s, x) / Γ(s).
        /// </remarks>
        public double GammaLowerIncomplete(double a, double x)
        {
            const double LogMax = 709.782712893384;
            const double DoubleEpsilon = 0.000000000000000111022302462516;

            if (a <= 0)
                return 1.0;

            if (x <= 0)
                return 0.0;

            if (x > 1.0 && x > a)
                return 1.0 - GammaUpperIncomplete(a, x);

            double ax = a * Math.Log(x) - x - LogGamma(a);

            if (ax < -LogMax)
                return 0.0;

            ax = Math.Exp(ax);

            double r = a;
            double c = 1.0;
            double ans = 1.0;

            do
            {
                r += 1.0;
                c *= x / r;
                ans += c;
            }
            while (c / ans > DoubleEpsilon);

            return ans * ax / a;
        }

        /// <summary>
        /// Evaluates a double precision polynomial. Coefficients are in reverse order.
        /// </summary>
        /// <param name="coefficients"></param>
        /// <param name="x"></param>
        public double PolynomialRev(double[] coefficients, double x)
        {
            int n = coefficients.Length;

            double value = coefficients[0];
            for (int i = 1, loopTo = n - 1; i <= loopTo; i++)
            {
                value *= x;
                value += coefficients[i];
            }
            return value;
        }

        /// <summary>
        /// Evaluates a double precision polynomial. Coefficients are in reverse order, and coefficient(N) = 1.0.
        /// </summary>
        /// <param name="coefficients"></param>
        /// <param name="x"></param>
        public double PolynomialRev_1(double[] coefficients, double x)
        {
            int n = coefficients.Length;

            double value = x + coefficients[0];
            for (int i = 1, loopTo = n - 1; i <= loopTo; i++)
            {
                value *= x;
                value += coefficients[i];
            }
            return value;
        }
       
    }
}
