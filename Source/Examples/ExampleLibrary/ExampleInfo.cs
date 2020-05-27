// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleInfo.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
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
        /// The un-modified example.
        /// </summary>
        private Example Example;

        /// <summary>
        /// The examples for this example info.
        /// </summary>
        private Dictionary<ExampleFlags, Example> examples;

        /// <summary>
        /// The options supported by this example.
        /// </summary>
        private ExampleFlags exampleSupport;

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
        /// Gets a value indicating whether the plot model is reversible.
        /// </summary>
        public bool IsReversible
        {
            get
            {
                this.EnsureInitialized();
                return exampleSupport.HasFlag(ExampleFlags.Reverse);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the plot model is transposable.
        /// </summary>
        public bool IsTransposable
        {
            get
            {
                this.EnsureInitialized();
                return exampleSupport.HasFlag(ExampleFlags.Transpose);
            }
        }

        /// <summary>
        /// Gets the code for the example with the given flags.
        /// </summary>
        /// <param name="flags">The flags for the example.</param>
        /// <returns>Code that produces the example.</returns>
        /// <remarks>Ignores unsupported flags.</remarks>
        public string GetCode(ExampleFlags flags)
        {
            return this.GetModel(flags)?.ToCode();
        }

        /// <summary>
        /// Gets the <see cref="PlotModel"/> for the example with the given flags.
        /// </summary>
        /// <param name="flags">The flags for the example.</param>
        /// <returns>The <see cref="PlotModel"/>.</returns>
        /// <remarks>Ignores unsupported flags.</remarks>
        public PlotModel GetModel(ExampleFlags flags)
        {
            return this.GetExample(flags).Model;
        }

        /// <summary>
        /// Gets the <see cref="IPlotController"/> for the example with the given flags.
        /// </summary>
        /// <param name="flags">The flags for the example.</param>
        /// <returns>The <see cref="IPlotController"/>.</returns>
        /// <remarks>Ignores unsupported flags.</remarks>
        public IPlotController GetController(ExampleFlags flags)
        {
            return this.GetExample(flags).Controller;
        }

        /// <summary>
        /// Gets the <see cref="Example"/> with the given flags.
        /// </summary>
        /// <param name="flags">The flags for the example.</param>
        /// <returns>The <see cref="Example"/>.</returns>
        /// <remarks>Ignores unsupported flags.</remarks>
        private Example GetExample(ExampleFlags flags)
        {
            this.EnsureInitialized();

            // ignore flags we don't support
            flags = FilterFlags(flags);

            if (!examples.TryGetValue(flags, out var example))
            {
                example = GetDefaultExample();

                if (flags.HasFlag(ExampleFlags.Transpose))
                {
                    example.Model.Transpose();
                }

                if (flags.HasFlag(ExampleFlags.Reverse))
                {
                    example.Model.ReverseAllAxes();
                }

                examples[flags] = example;
            }

            return example;
        }

        /// <summary>
        /// Gets the plot controller for the default example.
        /// </summary>
        /// <value>
        /// The plot controller.
        /// </value>
        public IPlotController PlotController
        {
            get
            {
                this.EnsureInitialized();
                return this.Example.Controller;
            }
        }

        /// <summary>
        /// Gets the plot model for the default example.
        /// </summary>
        /// <value>
        /// The plot model.
        /// </value>
        public PlotModel PlotModel
        {
            get
            {
                this.EnsureInitialized();
                return this.Example.Model;
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
        /// Prepares <see cref="ExampleFlags"/> from the given parameters.
        /// </summary>
        /// <param name="transpose">Whether to set the <see cref="ExampleFlags.Transpose"/> flag.</param>
        /// <param name="reverse">Whether to set the <see cref="ExampleFlags.Reverse"/> flag.</param>
        /// <returns>The <see cref="ExampleFlags"/>.</returns>
        public static ExampleFlags PrepareFlags(bool transpose, bool reverse)
        {
            var flags = (ExampleFlags)0;

            if (transpose)
            {
                flags |= ExampleFlags.Transpose;
            }

            if (reverse)
            {
                flags |= ExampleFlags.Reverse;
            }

            return flags;
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
            this.examples = new Dictionary<ExampleFlags, Example>();

            this.Example = GetDefaultExample();

            this.exampleSupport = PrepareFlags(this.Example.Model?.IsTransposable() == true, this.Example.Model?.IsReversible() == true);

            // remember the 'default' model: loads the transposed/reversed ones as we need them
            this.examples.Add(PrepareFlags(false, false), this.Example);
        }

        /// <summary>
        /// Gets a new instance of the default example.
        /// </summary>
        /// <returns>An <see cref="Example"/>.</returns>
        private Example GetDefaultExample()
        {
            var result = this.method.Invoke(null, null);

            if (result is null)
            {
                return new Example(null, null);
            }
            if (result is PlotModel plotModel)
            {
                return new Example(plotModel, null);
            }
            else if (result is Example example)
            {
                return example;
            }

            throw new Exception($"Unsupport type returned by example method for example {Category} > {Title}: {result.GetType().FullName}.");
        }

        /// <summary>
        /// Filters unsupported flags from the given flags.
        /// </summary>
        /// <param name="flags">The original set of flags.</param>
        /// <returns>The filtered flags.</returns>
        private ExampleFlags FilterFlags(ExampleFlags flags)
        {
            return flags & exampleSupport;
        }
    }
}
