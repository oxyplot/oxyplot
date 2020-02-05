using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ExampleLibrary;
using OxyPlot;
using OxyPlot.WindowsForms;

namespace ExampleGenerator
{
    public static class Program
    {
        private static readonly OxyColor defaultBackground = OxyColor.Parse("#00000000");

        public static string OutputDirectory { get; set; }

        public static bool DoOptimizePng { get; set; }

        public static bool ExportPng { get; set; }

        public static bool ExportPdf { get; set; }

        public static bool ExportSvg { get; set; }

        public static void Main(string[] args)
        {
            ExportPng = true;
            ExportPdf = true;
            ExportSvg = true;
            //DoOptimizePng = true;
            OutputDirectory = @".";
            if (args.Length > 0)
            {
                OutputDirectory = args[0];
            }

            var exportTasks = new List<Task>();

            var exampleAssembly = typeof(DocumentationExampleAttribute).Assembly;
            foreach (var type in exampleAssembly.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
                {
                    var exportAttribute = method.GetCustomAttribute<DocumentationExampleAttribute>();
                    if (exportAttribute == null)
                    {
                        continue;
                    }

                    var model = (PlotModel)method.Invoke(null, null);
                    var exportTask = Export(model, exportAttribute.Filename.Replace('/', Path.DirectorySeparatorChar));
                    exportTasks.Add(exportTask);
                }
            }

            //Wait for exports to finish
            Task.WaitAll(exportTasks.ToArray());
        }

        private static async Task Export(PlotModel model, string name)
        {
            if (model.Background == defaultBackground) 
                model.Background = OxyColors.White;

            var fileName = Path.Combine(OutputDirectory, name + ".png");
            var directory = Path.GetDirectoryName(fileName) ?? ".";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (ExportPng)
            {
                Console.WriteLine(fileName);
                using (var stream = File.Create(fileName))
                {
                    var exporter = new PngExporter { Width = 600, Height = 400 };
                    exporter.Export(model, stream);
                }

                if (DoOptimizePng)
                    await OptimizePng(fileName);
            }

            if (ExportPdf)
            {
                fileName = Path.ChangeExtension(fileName, ".pdf");
                Console.WriteLine(fileName);
                using (var stream = File.Create(fileName))
                {
                    var exporter = new PdfExporter { Width = 600d * 72 / 96, Height = 400d * 72 / 96 };
                    exporter.Export(model, stream);
                }
            }

            if (ExportSvg)
            {
                fileName = Path.ChangeExtension(fileName, ".svg");
                Console.WriteLine(fileName);

                using (var stream = File.Create(fileName))
                {
                    using (var exporter = new OxyPlot.WindowsForms.SvgExporter { Width = 600, Height = 400, IsDocument = true })
                    {
                        exporter.Export(model, stream);
                    }
                }
            }
        }


        /* PNG Optimization */

        private static async Task OptimizePng(string pngFile)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                await OptimizePngWithOptiPNG(pngFile);
            }
            else
            {
                await OptimizePngWithTruePNG(pngFile);
            }
        }

        private static async Task OptimizePngWithTruePNG(string pngFile)
        {
            // /o max : optimization level
            // /nc : don't change ColorType and BitDepth
            // /md keep pHYs : keep pHYs metadata
            var psi = new ProcessStartInfo("TruePNG.exe", pngFile + " /o max /nc /md keep pHYs")
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            try
            {
                var p = Process.Start(psi);
                await Task.Run(() => p.WaitForExit());
            }
            catch (Win32Exception e)
            {
                throw new Win32Exception(
                    "Failed to run TruePNG optimization. Please ensure that TruePNG is installed and registered in the PATH variable.",
                    e);
            }
        }

        private static async Task OptimizePngWithOptiPNG(string pngFile)
        {
            var psi = new ProcessStartInfo("optipng", "-o7 " + pngFile)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            var p = Process.Start(psi);
            await Task.Run(() => p.WaitForExit());
        }
    }

}
