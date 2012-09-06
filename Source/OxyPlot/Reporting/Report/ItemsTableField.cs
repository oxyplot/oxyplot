// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsTableField.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System;
    using System.Reflection;

    /// <summary>
    /// The alignment.
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        ///   The left.
        /// </summary>
        Left, 

        /// <summary>
        ///   The right.
        /// </summary>
        Right, 

        /// <summary>
        ///   The center.
        /// </summary>
        Center
    }

    /// <summary>
    /// Represents a field in an items table.
    /// </summary>
    public class ItemsTableField
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsTableField"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="stringFormat">
        /// The string format.
        /// </param>
        /// <param name="alignment">
        /// The alignment.
        /// </param>
        public ItemsTableField(
            string header, string path, string stringFormat = null, Alignment alignment = Alignment.Center)
        {
            this.Header = header;
            this.Path = path;
            this.StringFormat = stringFormat;
            this.Alignment = alignment;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets Alignment.
        /// </summary>
        public Alignment Alignment { get; set; }

        /// <summary>
        ///   Gets or sets Header.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        ///   Gets or sets Path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///   Gets or sets StringFormat.
        /// </summary>
        public string StringFormat { get; set; }

        /// <summary>
        ///   Gets or sets Width.
        /// </summary>
        public double Width { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <returns>
        /// The text.
        /// </returns>
        public string GetText(object item, IFormatProvider formatProvider)
        {
            PropertyInfo pi = item.GetType().GetProperty(this.Path);
            object o = pi.GetValue(item, null);
            var of = o as IFormattable;
            if (of != null)
            {
                return of.ToString(this.StringFormat, formatProvider);
            }

            return o != null ? o.ToString() : null;
        }

        #endregion
    }
}