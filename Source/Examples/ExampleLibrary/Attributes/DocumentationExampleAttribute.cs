using System;

namespace ExampleLibrary
{
    /// <summary>
    /// Marks the model as documentation example to be exported by the ExampleGenerator program.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DocumentationExampleAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">The filename of the documentation file without extension. For sub folders, use '/' as path delimiter.</param>
        public DocumentationExampleAttribute(string filename)
        {
            this.Filename = filename;
        }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <value>The filename.</value>
        /// <remarks>
        /// For sub folders, use '/' as path delimiter.
        /// This is then replaced with the current platforms path separator later in the process.
        /// </remarks>
        public string Filename { get; private set; }
    }
}
