// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window6ViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a view-model for <see cref="Window6" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RefreshDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    using OxyPlot;

    /// <summary>
    /// Represents a view-model for <see cref="Window6" />.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class Window6ViewModel : INotifyPropertyChanged
    {
        private readonly Task task;

        private int refresh;

        private string title;

        private bool complete;

        public Window6ViewModel()
        {
            this.Points = new List<DataPoint>();
            this.task = Task.Factory.StartNew(
                () =>
                {
                    double x = 0;
                    while (!complete)
                    {
                        this.Title = "Plot updated: " + DateTime.Now;
                        this.Points.Add(new DataPoint(x, Math.Sin(x)));

                        // Change the refresh flag, this will trig InvalidatePlot() on the Plot control
                        this.Refresh++;

                        x += 0.1;
                        Thread.Sleep(100);
                    }
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IList<DataPoint> Points { get; set; }

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.RaisePropertyChanged("Title");
            }
        }

        public int Refresh
        {
            get
            {
                return this.refresh;
            }

            set
            {
                if (this.refresh == value)
                {
                    return;
                }

                this.refresh = value;
                this.RaisePropertyChanged("Refresh");
            }
        }

        public void Close()
        {
            this.complete = true;
            this.task.Wait();
        }

        protected void RaisePropertyChanged(string property)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}