// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Example.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples
{
    using Avalonia.Media.Imaging;
    using System;
    using System.Diagnostics;

    public class Example
    {
        public Example(Type mainWindowType, string title = null, string description = null)
        {
            this.MainWindowType = mainWindowType;
            this.Title = title ?? mainWindowType.Namespace;
            this.Description = description;
            try
            {
                this.Thumbnail = new Bitmap("resm:Images/" + this.ThumbnailFileName);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public string Title { get; private set; }
        public string Description { get; set; }
        private Type MainWindowType { get; set; }

        public IBitmap Thumbnail { get; set; }

        public string ThumbnailFileName
        {
            get
            {
                return this.MainWindowType.Namespace + ".png";
            }
        }

        public override string ToString()
        {
            return this.Title;
        }

        public Avalonia.Controls.Window Create()
        {
            return Activator.CreateInstance(this.MainWindowType) as Avalonia.Controls.Window;
        }
    }
}