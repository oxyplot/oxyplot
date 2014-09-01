// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Example.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WpfExamples
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class Example
    {
        public Example(Type mainWindowType, string title = null, string description = null)
        {
            this.MainWindowType = mainWindowType;
            this.Title = title ?? mainWindowType.Namespace;
            this.Description = description;
            try
            {
                this.Thumbnail = new BitmapImage(new Uri("pack://application:,,,/Images/" + this.ThumbnailFileName));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public string Title { get; private set; }
        public string Description { get; set; }
        private Type MainWindowType { get; set; }

        public ImageSource Thumbnail { get; set; }

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

        public Window Create()
        {
            return Activator.CreateInstance(this.MainWindowType) as Window;
        }
    }
}