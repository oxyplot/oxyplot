// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleInfo.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Reflection;

    using OxyPlot;

    /// <summary>
    /// Provides information about an example.
    /// </summary>
    public class ExampleInfo
    {
        /// <summary>
        /// The method to invoke.
        /// </summary>
        private readonly MethodInfo method;

        /// <summary>
        /// The result of the method call.
        /// </summary>
        private object result;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleInfo"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="title">The title.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="method">The method.</param>
        public ExampleInfo(string category, string title, string[] tags, MethodInfo method)
        {
            this.Category = category;
            this.Title = title;
            this.Tags = tags;
            this.method = method;
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; private set; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public string[] Tags { get; private set; }

        /// <summary>
        /// Gets the plot model.
        /// </summary>
        /// <value>
        /// The plot model.
        /// </value>
        public PlotModel PlotModel
        {
            get
            {
                var plotModel = this.Result as PlotModel;
                if (plotModel != null)
                {
                    return plotModel;
                }

                var example = this.Result as Example;
                return example != null ? example.Model : null;
            }
        }

        /// <summary>
        /// Gets the plot controller.
        /// </summary>
        /// <value>
        /// The plot controller.
        /// </value>
        public IPlotController PlotController
        {
            get
            {
                var example = this.Result as Example;
                return example != null ? example.Controller : null;
            }
        }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code
        {
            get
            {
                return this.PlotModel != null ? this.PlotModel.ToCode() : null;
            }
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        private object Result
        {
            get
            {
                return this.result ?? (this.result = this.method.Invoke(null, null));
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Title;
        }
    }
}