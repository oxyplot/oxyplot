// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ColorBindingDemo
{
    using System.Windows.Input;

    using OxyPlot;

    using WpfExamples;

    using ICommand = System.Windows.Input.ICommand;

    public class MainViewModel : Observable
    {
        private OxyColor background;

        private OxyColor plotAreaBorderColor;

        private OxyColor textColor;

        private OxyColor ticklineColor;

        public MainViewModel()
        {
            this.SetBlackCommand = new DelegateCommand(
                () =>
                    {
                        this.Background = OxyColors.Black;
                        this.TextColor = OxyColors.White;
                        this.TicklineColor = OxyColors.White;
                        this.PlotAreaBorderColor = OxyColors.White;
                    });
            this.SetWhiteCommand = new DelegateCommand(
                () =>
                    {
                        this.Background = OxyColors.White;
                        this.TextColor = OxyColors.Black;
                        this.TicklineColor = OxyColors.Black;
                        this.PlotAreaBorderColor = OxyColors.Black;
                    });
            this.SetRastaCommand = new DelegateCommand(
                () =>
                    {
                        this.Background = OxyColors.Black;
                        this.TextColor = OxyColors.Yellow;
                        this.PlotAreaBorderColor = OxyColors.Green;
                        this.TicklineColor = OxyColors.Red;
                    });

            this.SetBlackCommand.Execute(null);
        }

        public ICommand SetBlackCommand { get; private set; }

        public ICommand SetWhiteCommand { get; private set; }

        public ICommand SetRastaCommand { get; private set; }

        public OxyColor TextColor
        {
            get
            {
                return this.textColor;
            }

            set
            {
                this.SetValue(ref this.textColor, value);
                this.textColor = value;
            }
        }

        public OxyColor TicklineColor
        {
            get
            {
                return this.ticklineColor;
            }

            set
            {
                this.SetValue(ref this.ticklineColor, value);
            }
        }

        public OxyColor PlotAreaBorderColor
        {
            get
            {
                return this.plotAreaBorderColor;
            }

            set
            {
                this.SetValue(ref this.plotAreaBorderColor, value);
            }
        }

        public OxyColor Background
        {
            get
            {
                return this.background;
            }

            set
            {
                this.SetValue(ref this.background, value);
            }
        }
    }
}
