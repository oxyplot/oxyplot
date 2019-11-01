// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleInfo.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Reflection;

    using ExampleLibrary.Utilities;

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
        /// Indicates whether this instance was already initialized.
        /// </summary>
        private bool initialized;

        /// <summary>
        /// The plot model.
        /// </summary>
        private PlotModel model;

        /// <summary>
        /// The plot controller.
        /// </summary>
        private IPlotController plotController;

        /// <summary>
        /// The transposed plot model.
        /// </summary>
        private PlotModel transposedModel;

        /// <summary>
        /// The plot controller for the transposed model.
        /// </summary>
        private IPlotController transposedPlotController;

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
        public string Category { get; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code => this.PlotModel?.ToCode();

        /// <summary>
        /// Gets a value indicating whether the plot model is transposable.
        /// </summary>
        public bool IsTransposable
        {
            get
            {
                this.EnsureInitialized();
                return this.transposedModel != null;
            }
        }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string TransposedCode => this.TransposedPlotModel?.ToCode();

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
                this.EnsureInitialized();
                return this.plotController;
            }
        }

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
                this.EnsureInitialized();
                return this.model;
            }
        }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public string[] Tags { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; }

        /// <summary>
        /// Gets the plot controller for the transposed plot model.
        /// </summary>
        /// <value>
        /// The plot controller for the transposed plot model, or null if the plot model is not transposable.
        /// </value>
        public IPlotController TransposedPlotController
        {
            get
            {
                this.EnsureInitialized();
                return this.transposedPlotController;
            }
        }

        /// <summary>
        /// Gets the transposed plot model.
        /// </summary>
        /// <value>
        /// The transposed plot model, or null if the plot model is not transposable.
        /// </value>
        public PlotModel TransposedPlotModel
        {
            get
            {
                this.EnsureInitialized();
                return this.transposedModel;
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

        /// <summary>
        /// Initializes this instance if it is not already initialized.
        /// </summary>
        private void EnsureInitialized()
        {
            if (this.initialized)
            {
                return;
            }

            this.initialized = true;

            var result = this.GetResult();
            if (result is PlotModel plotModel)
            {
                this.model = plotModel;
            }
            else if (result is Example example)
            {
                this.model = example.Model;
                this.plotController = example.Controller;
            }

            if (this.model?.IsTransposable() != true)
            {
                return;
            }

            var result2 = this.GetResult();
            if (result2 is PlotModel plotModel2)
            {
                this.transposedModel = plotModel2.Transpose();
            }
            else if (result2 is Example example2)
            {
                this.transposedModel = example2.Model.Transpose();
                this.transposedPlotController = example2.Controller;
            }
        }

        /// <summary>
        /// Gets the result of the method call.
        /// </summary>
        /// <returns>Either a <see cref="Example"/> or a <see cref="OxyPlot.PlotModel"/>.</returns>
        private object GetResult()
        {
            return this.method.Invoke(null, null);
        }
    }
}
