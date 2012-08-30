// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleInfo.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using OxyPlot;

namespace ExampleLibrary
{
    public class ExampleInfo
    {
        public string Category { get; set; }
        public string Title { get; set; }
        private MethodInfo Method { get; set; }

        private PlotModel plotModel;

        public PlotModel PlotModel
        {
            get
            {
                if (plotModel == null)
                    plotModel = Method.Invoke(null, null) as PlotModel;
                return plotModel;
            }
        }

        public string Code
        {
            get
            {
                return PlotModel.ToCode();
            }
        }

        public ExampleInfo(string category, string title, MethodInfo method)
        {
            this.Category = category;
            this.Title = title;
            this.Method = method;
        }
        public override string ToString()
        {
            return Title;
        }
    }
}