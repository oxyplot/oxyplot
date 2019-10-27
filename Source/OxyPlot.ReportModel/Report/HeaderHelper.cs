// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderHelper.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The header helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// The header helper.
    /// </summary>
    public class HeaderHelper
    {
        /// <summary>
        /// The header level.
        /// </summary>
        private readonly int[] headerLevel = new int[10];

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <param name="level">The header level.</param>
        /// <returns>The header.</returns>
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
    }
}