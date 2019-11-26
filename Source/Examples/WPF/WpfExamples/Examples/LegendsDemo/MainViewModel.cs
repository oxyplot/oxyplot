// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LegendsDemo
{
    using System;
    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    using PropertyTools.DataAnnotations;
    using WpfExamples;

    using HorizontalAlignment = OxyPlot.HorizontalAlignment;

    public class MainViewModel : Observable
    {
        private HorizontalAlignment legendItemAlignment = HorizontalAlignment.Left;
        private LegendItemOrder legendItemOrder;
        private LegendOrientation legendOrientation;
        private LegendPlacement legendPlacement;
        private LegendPosition legendPosition;
        private LegendSymbolPlacement legendSymbolPlacement;
        private PlotModel model;
        private int numberOfSeries = 20;
        private double maxHeight = double.NaN;
        private double maxWidth = double.NaN;

        public MainViewModel()
        {
            this.PropertiesChanged();
        }

        [DisplayName("Position"), Category("Legend properties")]
        public LegendPosition LegendPosition
        {
            get { return this.legendPosition; }
            set
            {
                this.SetValue(ref this.legendPosition, value);
                this.PropertiesChanged();
            }
        }

        [DisplayName("Placement")]
        public LegendPlacement LegendPlacement
        {
            get { return this.legendPlacement; }
            set
            {
                this.SetValue(ref this.legendPlacement, value);
                this.PropertiesChanged();
            }
        }

        [DisplayName("Orientation")]
        public LegendOrientation LegendOrientation
        {
            get { return this.legendOrientation; }
            set
            {
                this.SetValue(ref this.legendOrientation, value);
                this.PropertiesChanged();
            }
        }

        [DisplayName("ItemOrder")]
        public LegendItemOrder LegendItemOrder
        {
            get { return this.legendItemOrder; }
            set
            {
                this.SetValue(ref this.legendItemOrder, value);
                this.PropertiesChanged();
            }
        }

        [DisplayName("ItemAlignment")]
        public HorizontalAlignment LegendItemAlignment
        {
            get { return this.legendItemAlignment; }
            set
            {
                this.SetValue(ref this.legendItemAlignment, value);
                this.PropertiesChanged();
            }
        }

        [DisplayName("SymbolPlacement")]
        public LegendSymbolPlacement LegendSymbolPlacement
        {
            get { return this.legendSymbolPlacement; }
            set
            {
                this.SetValue(ref this.legendSymbolPlacement, value);
                this.PropertiesChanged();
            }
        }

        [DisplayName("MaxWidth"), Optional]
        public double LegendMaxWidth
        {
            get { return this.maxWidth; }
            set
            {
                this.SetValue(ref this.maxWidth, value);
                this.PropertiesChanged();
            }
        }

        [DisplayName("MaxHeight"), Optional]
        public double LegendMaxHeight
        {
            get { return this.maxHeight; }
            set
            {
                this.SetValue(ref this.maxHeight, value);
                this.PropertiesChanged();
            }
        }

        [DisplayName("Curves"), Slidable(1, 32)]
        public int NumberOfSeries
        {
            get { return this.numberOfSeries; }
            set
            {
                this.SetValue(ref this.numberOfSeries, value);
                this.PropertiesChanged();
            }
        }

        [Browsable(false)]
        public PlotModel Model
        {
            get { return this.model; }
            set
            {
                this.SetValue(ref this.model, value);
            }
        }

        private void PropertiesChanged()
        {
            this.Model = this.CreateModel(this.NumberOfSeries);
        }

        private PlotModel CreateModel(int n)
        {
            var newModel = new PlotModel
            {
                Title = "LineSeries"
            };

            var l = new Legend
            {
                LegendBorder = OxyColors.Black,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendPosition = this.LegendPosition,
                LegendPlacement = this.LegendPlacement,
                LegendOrientation = this.LegendOrientation,
                LegendItemOrder = this.LegendItemOrder,
                LegendItemAlignment = this.LegendItemAlignment,
                LegendSymbolPlacement = this.LegendSymbolPlacement,
                LegendMaxWidth = this.LegendMaxWidth,
                LegendMaxHeight = this.LegendMaxHeight
            };

            newModel.Legends.Add(l);

            for (int i = 1; i <= n; i++)
            {
                var s = new LineSeries { Title = "Series " + i };
                newModel.Series.Add(s);
                for (double x = 0; x < 2 * Math.PI; x += 0.1)
                    s.Points.Add(new DataPoint(x, Math.Sin(x * i) / i + i));
            }
            return newModel;
        }
    }
}
