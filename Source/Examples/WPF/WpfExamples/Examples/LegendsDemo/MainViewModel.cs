// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LegendsDemo
{
    using System;
    using System.ComponentModel;

    using OxyPlot;
    using OxyPlot.Series;

    using PropertyTools.DataAnnotations;
    using WpfExamples;

    using HorizontalAlignment = OxyPlot.HorizontalAlignment;

    public class MainViewModel : Observable
    {
        private HorizontalAlignment legendItemAlignment;
        private LegendItemOrder legendItemOrder;
        private LegendOrientation legendOrientation;
        private LegendPlacement legendPlacement;
        private LegendPosition legendPosition;
        private LegendSymbolPlacement legendSymbolPlacement;
        private PlotModel model;
        private int numberOfSeries;

        public MainViewModel()
        {
            NumberOfSeries = 20;
            LegendItemAlignment = HorizontalAlignment.Left;
            PropertiesChanged();
        }

        [DisplayName("Position"), Category("Legend properties")]
        public LegendPosition LegendPosition
        {
            get { return legendPosition; }
            set
            {
                legendPosition = value;
                PropertiesChanged();
            }
        }

        [DisplayName("Placement")]
        public LegendPlacement LegendPlacement
        {
            get { return legendPlacement; }
            set
            {
                legendPlacement = value;
                PropertiesChanged();
            }
        }

        [DisplayName("Orientation")]
        public LegendOrientation LegendOrientation
        {
            get { return legendOrientation; }
            set
            {
                legendOrientation = value;
                PropertiesChanged();
            }
        }

        [DisplayName("ItemOrder")]
        public LegendItemOrder LegendItemOrder
        {
            get { return legendItemOrder; }
            set
            {
                legendItemOrder = value;
                PropertiesChanged();
            }
        }

        [DisplayName("ItemAlignment")]
        public HorizontalAlignment LegendItemAlignment
        {
            get { return legendItemAlignment; }
            set
            {
                legendItemAlignment = value;
                PropertiesChanged();
            }
        }

        [DisplayName("SymbolPlacement")]
        public LegendSymbolPlacement LegendSymbolPlacement
        {
            get { return legendSymbolPlacement; }
            set
            {
                legendSymbolPlacement = value;
                PropertiesChanged();
            }
        }

        [DisplayName("Curves"), Slidable(1, 32)]
        public int NumberOfSeries
        {
            get { return numberOfSeries; }
            set
            {
                numberOfSeries = value;
                PropertiesChanged();
            }
        }

        [Browsable(false)]
        public PlotModel Model
        {
            get { return model; }
            set
            {
                if (model != value)
                {
                    model = value;
                    RaisePropertyChanged(() => Model);
                }
            }
        }

        private void PropertiesChanged()
        {
            Model = CreateModel(NumberOfSeries);
        }

        private PlotModel CreateModel(int n)
        {
            var newModel = new PlotModel
            {
                Title = "LineSeries",
                LegendBorder = OxyColors.Black,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendPosition = LegendPosition,
                LegendPlacement = LegendPlacement,
                LegendOrientation = LegendOrientation,
                LegendItemOrder = LegendItemOrder,
                LegendItemAlignment = LegendItemAlignment,
                LegendSymbolPlacement = LegendSymbolPlacement
            };

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