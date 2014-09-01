// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleInfo.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Reflection;

    using OxyPlot;

    public class ExampleInfo
    {
        private readonly MethodInfo method;

        private object result;

        public ExampleInfo(string category, string title, MethodInfo method)
        {
            this.Category = category;
            this.Title = title;
            this.method = method;
        }

        public string Category { get; set; }

        public string Title { get; set; }

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

        public IPlotController PlotController
        {
            get
            {
                var example = this.Result as Example;
                return example != null ? example.Controller : null;
            }
        }

        public string Code
        {
            get
            {
                return this.PlotModel.ToCode();
            }
        }

        private object Result
        {
            get
            {
                return this.result ?? (this.result = this.method.Invoke(null, null));
            }
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}