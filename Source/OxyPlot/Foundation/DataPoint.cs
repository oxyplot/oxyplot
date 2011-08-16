namespace OxyPlot
{
    using System;
    using System.Globalization;

    /// <summary>
    /// DataPoint interface.
    /// </summary>
    public interface IDataPoint
    {
        #region Public Properties

        /// <summary>
        /// Gets the X.
        /// </summary>
        /// <value>The X.</value>
        double X { get; }

        /// <summary>
        /// Gets the Y.
        /// </summary>
        /// <value>The Y.</value>
        double Y { get; }

        #endregion
    }

    /// <summary>
    /// DataPoint value type.
    /// </summary>
    public struct DataPoint : IDataPoint, ICodeGenerating
    {
        #region Constants and Fields

        public static readonly DataPoint Undefined = new DataPoint(double.NaN, double.NaN);

        internal double x;

        internal double y;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public DataPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>
        /// The X.
        /// </value>
        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>
        /// The Y.
        /// </value>
        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns></returns>
        public string ToCode()
        {
            return String.Format(CultureInfo.InvariantCulture, "new DataPoint({0},{1})", this.x, this.y);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.x + " " + this.y;
        }

        #endregion
    }
}