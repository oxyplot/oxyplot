// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using OxyPlot;
using PropertyTools.Wpf;

namespace LegendsDemo
{
    using PropertyTools.DataAnnotations;

    public class MainViewModel : Observable
    {
        private HorizontalTextAlign legendItemAlignment;
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
            LegendItemAlignment = HorizontalTextAlign.Left;
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
        public HorizontalTextAlign LegendItemAlignment
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
            var newModel = new PlotModel("LineSeries")
                            {
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
                var s = new LineSeries("Series " + i);
                newModel.Series.Add(s);
                for (double x = 0; x < 2 * Math.PI; x += 0.1)
                    s.Points.Add(new DataPoint(x, Math.Sin(x * i) / i + i));
            }
            return newModel;
        }
    }
}