// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderHelper.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// The header helper.
    /// </summary>
    public class HeaderHelper
    {
        #region Constants and Fields

        /// <summary>
        ///   The header level.
        /// </summary>
        private readonly int[] headerLevel = new int[10];

        #endregion

        #region Public Methods

        /// <summary>
        /// The get header.
        /// </summary>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <returns>
        /// The get header.
        /// </returns>
        public string GetHeader(int level)
        {
            for (int i = level - 1; i > 0; i--)
            {
                if (this.headerLevel[i] == 0)
                {
                    this.headerLevel[i] = 1;
                }
            }

            this.headerLevel[level]++;
            for (int i = level + 1; i < 10; i++)
            {
                this.headerLevel[i] = 0;
            }

            string levelString = string.Empty;
            for (int i = 1; i <= level; i++)
            {
                if (i > 1)
                {
                    levelString += ".";
                }

                levelString += this.headerLevel[i];
            }

            return levelString;
        }

        #endregion
    }
}