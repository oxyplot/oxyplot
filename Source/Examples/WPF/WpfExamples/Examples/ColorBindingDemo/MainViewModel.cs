// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ColorBindingDemo
{
    using System.Windows.Input;

    using OxyPlot;

    using WpfExamples;

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
                this.textColor = value;
                this.RaisePropertyChanged(() => this.TextColor);
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
                this.ticklineColor = value;
                this.RaisePropertyChanged(() => this.TicklineColor);
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
                this.plotAreaBorderColor = value;
                this.RaisePropertyChanged(() => this.PlotAreaBorderColor);
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
                this.background = value;
                this.RaisePropertyChanged(() => this.Background);
            }
        }
    }
}