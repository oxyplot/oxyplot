// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileNameUtilities.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides file name utilities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf.Tests
{
    /// <summary>
    /// Provides file name utilities.
    /// </summary>
    public static class FileNameUtilities
    {
        /// <summary>
        /// Creates a valid file name.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>A file name.</returns>
        public static string CreateValidFileName(string title, string extension)
        {
            string validFileName = title.Trim();
            var invalidFileNameChars = "/?<>\\:*|\0\t\r\n".ToCharArray();
            foreach (var invalChar in invalidFileNameChars)
            {
                validFileName = validFileName.Replace(invalChar.ToString(), string.Empty);
            }

            foreach (var invalChar in invalidFileNameChars)
            {
                validFileName = validFileName.Replace(invalChar.ToString(), string.Empty);
            }

            if (validFileName.Length > 160)
            {
                // safe value threshold is 260
                validFileName = validFileName.Remove(156) + "...";
            }

            return validFileName + extension;
        }
    }
}
