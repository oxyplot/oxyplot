// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using ExampleLibrary.Utilities;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("AreaSeries"), Tags("Series")]
    public static class AreaSeriesExamples
    {
        [Example("Default style")]
        [DocumentationExample("Series/AreaSeries")]
        public static PlotModel DefaultStyle()
        {
            var plotModel1 = new PlotModel { Title = "AreaSeries with default style" };
            plotModel1.Series.Add(CreateExampleAreaSeries());
            return plotModel1;
        }

        [Example("Different stroke colors")]
        public static PlotModel DifferentColors()
        {
            var plotModel1 = new PlotModel { Title = "AreaSeries with different stroke colors" };
            var areaSeries1 = CreateExampleAreaSeries();
            areaSeries1.Color = OxyColors.Red;
            areaSeries1.Color2 = OxyColors.Blue;
            plotModel1.Series.Add(areaSeries1);
            return plotModel1;
        }

        [Example("Crossing lines")]
        public static PlotModel CrossingLines()
        {
            var plotModel1 = new PlotModel { Title = "AreaSeries with crossing lines" };
            var areaSeries1 = new AreaSeries();
            areaSeries1.Points.Add(new DataPoint(0, 50));
            areaSeries1.Points.Add(new DataPoint(10, 140));
            areaSeries1.Points.Add(new DataPoint(20, 60));
            areaSeries1.Points2.Add(new DataPoint(0, 60));
            areaSeries1.Points2.Add(new DataPoint(5, 80));
            areaSeries1.Points2.Add(new DataPoint(20, 70));
            plotModel1.Series.Add(areaSeries1);
            return plotModel1;
        }

        [Example("Custom TrackerFormatString")]
        public static PlotModel TrackerFormatString()
        {
            var model = new PlotModel { Title = "AreaSeries with custom TrackerFormatString" };

            // the axis titles will be used in the default tracker format string
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis" });

            var areaSeries1 = CreateExampleAreaSeries();
            areaSeries1.Title = "X={2:0.0} Y={4:0.0}";
            areaSeries1.TrackerFormatString = "X={2:0.0} Y={4:0.0}";
            model.Series.Add(areaSeries1);
            return model;
        }

        [Example("Constant baseline (empty Points2)")]
        public static PlotModel ConstantBaseline()
        {
            var plotModel1 = new PlotModel { Title = "AreaSeries with constant baseline", Subtitle = "Empty Points2, ConstantY2 = 0 (default)" };
            var areaSeries1 = new AreaSeries();
            areaSeries1.Points.Add(new DataPoint(0, 50));
            areaSeries1.Points.Add(new DataPoint(10, 140));
            areaSeries1.Points.Add(new DataPoint(20, 60));
            plotModel1.Series.Add(areaSeries1);
            return plotModel1;
        }

        [Example("Constant baseline (empty Points2, ConstantY2=NaN)")]
        public static PlotModel ConstantBaselineNaN()
        {
            var plotModel1 = new PlotModel { Title = "AreaSeries with constant baseline", Subtitle = "Empty Points2, ConstantY2 = NaN" };
            var areaSeries1 = new AreaSeries();
            areaSeries1.Points.Add(new DataPoint(0, 50));
            areaSeries1.Points.Add(new DataPoint(10, 140));
            areaSeries1.Points.Add(new DataPoint(20, 60));
            areaSeries1.ConstantY2 = double.NaN;
            plotModel1.Series.Add(areaSeries1);
            return plotModel1;
        }

        [Example("Constant baseline (ItemsSource and DataField2 not set)")]
        public static PlotModel ConstantBaselineItemsSource()
        {
            var plotModel1 = new PlotModel { Title = "AreaSeries with constant baseline", Subtitle = "ItemsSource and DataField2 not set, ConstantY2 = -20" };
            var areaSeries1 = new AreaSeries();
            var points = new[] { new DataPoint(0, 50), new DataPoint(10, 140), new DataPoint(20, 60) };
            areaSeries1.ItemsSource = points;
            areaSeries1.DataFieldX = "X";
            areaSeries1.DataFieldY = "Y";
            areaSeries1.ConstantY2 = -20;
            plotModel1.Series.Add(areaSeries1);
            return plotModel1;
        }

        [Example("LineSeries and AreaSeries")]
        public static PlotModel LineSeriesAndAreaSeries()
        {
            var plotModel1 = new PlotModel { Title = "LineSeries and AreaSeries" };
            var linearAxis1 = new LinearAxis { Position = AxisPosition.Bottom };
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis2);
            var areaSeries1 = new AreaSeries
            {
                Fill = OxyColors.LightBlue,
                DataFieldX2 = "Time",
                DataFieldY2 = "Minimum",
                Color = OxyColors.Red,
                StrokeThickness = 0,
                MarkerFill = OxyColors.Transparent,
                DataFieldX = "Time",
                DataFieldY = "Maximum"
            };
            areaSeries1.Points2.Add(new DataPoint(0, -5.04135905692417));
            areaSeries1.Points2.Add(new DataPoint(2.5, -4.91731850813018));
            areaSeries1.Points2.Add(new DataPoint(5, -4.45266314658926));
            areaSeries1.Points2.Add(new DataPoint(7.5, -3.87303874542613));
            areaSeries1.Points2.Add(new DataPoint(10, -3.00101110255393));
            areaSeries1.Points2.Add(new DataPoint(12.5, -2.17980725503518));
            areaSeries1.Points2.Add(new DataPoint(15, -1.67332229254456));
            areaSeries1.Points2.Add(new DataPoint(17.5, -1.10537158549082));
            areaSeries1.Points2.Add(new DataPoint(20, -0.6145459544447));
            areaSeries1.Points2.Add(new DataPoint(22.5, 0.120028106039404));
            areaSeries1.Points2.Add(new DataPoint(25, 1.06357270435597));
            areaSeries1.Points2.Add(new DataPoint(27.5, 1.87301405606466));
            areaSeries1.Points2.Add(new DataPoint(30, 2.57569854952195));
            areaSeries1.Points2.Add(new DataPoint(32.5, 3.59165537664278));
            areaSeries1.Points2.Add(new DataPoint(35, 4.87991958133872));
            areaSeries1.Points2.Add(new DataPoint(37.5, 6.36214537958714));
            areaSeries1.Points2.Add(new DataPoint(40, 7.62564585126268));
            areaSeries1.Points2.Add(new DataPoint(42.5, 8.69606320261772));
            areaSeries1.Points2.Add(new DataPoint(45, 10.0118704438265));
            areaSeries1.Points2.Add(new DataPoint(47.5, 11.0434480519236));
            areaSeries1.Points2.Add(new DataPoint(50, 11.9794171576758));
            areaSeries1.Points2.Add(new DataPoint(52.5, 12.9591851832621));
            areaSeries1.Points2.Add(new DataPoint(55, 14.172107889304));
            areaSeries1.Points2.Add(new DataPoint(57.5, 15.5520057698488));
            areaSeries1.Points2.Add(new DataPoint(60, 17.2274942386092));
            areaSeries1.Points2.Add(new DataPoint(62.5, 18.6983982186757));
            areaSeries1.Points2.Add(new DataPoint(65, 20.4560332001448));
            areaSeries1.Points2.Add(new DataPoint(67.5, 22.4867327382261));
            areaSeries1.Points2.Add(new DataPoint(70, 24.5319674302041));
            areaSeries1.Points2.Add(new DataPoint(72.5, 26.600547815813));
            areaSeries1.Points2.Add(new DataPoint(75, 28.5210891459701));
            areaSeries1.Points2.Add(new DataPoint(77.5, 30.6793080755413));
            areaSeries1.Points2.Add(new DataPoint(80, 33.0546651200646));
            areaSeries1.Points2.Add(new DataPoint(82.5, 35.3256065179713));
            areaSeries1.Points2.Add(new DataPoint(85, 37.6336074839968));
            areaSeries1.Points2.Add(new DataPoint(87.5, 40.2012266359763));
            areaSeries1.Points2.Add(new DataPoint(90, 42.8923555399256));
            areaSeries1.Points2.Add(new DataPoint(92.5, 45.8665211907432));
            areaSeries1.Points2.Add(new DataPoint(95, 48.8200195945427));
            areaSeries1.Points2.Add(new DataPoint(97.5, 51.8304284402311));
            areaSeries1.Points2.Add(new DataPoint(100, 54.6969868542147));
            areaSeries1.Points2.Add(new DataPoint(102.5, 57.7047292990632));
            areaSeries1.Points2.Add(new DataPoint(105, 60.4216644602929));
            areaSeries1.Points2.Add(new DataPoint(107.5, 62.926258762519));
            areaSeries1.Points2.Add(new DataPoint(110, 65.1829734629407));
            areaSeries1.Points2.Add(new DataPoint(112.5, 67.2365592083133));
            areaSeries1.Points2.Add(new DataPoint(115, 69.5713628691022));
            areaSeries1.Points2.Add(new DataPoint(117.5, 71.7267046705944));
            areaSeries1.Points2.Add(new DataPoint(120, 73.633463102781));
            areaSeries1.Points2.Add(new DataPoint(122.5, 75.4660150158061));
            areaSeries1.Points2.Add(new DataPoint(125, 77.5669292504745));
            areaSeries1.Points2.Add(new DataPoint(127.5, 79.564218544664));
            areaSeries1.Points2.Add(new DataPoint(130, 81.8631309028078));
            areaSeries1.Points2.Add(new DataPoint(132.5, 83.9698189969034));
            areaSeries1.Points2.Add(new DataPoint(135, 86.3847886532009));
            areaSeries1.Points2.Add(new DataPoint(137.5, 88.5559348267764));
            areaSeries1.Points2.Add(new DataPoint(140, 91.0455050418365));
            areaSeries1.Points2.Add(new DataPoint(142.5, 93.6964157585504));
            areaSeries1.Points2.Add(new DataPoint(145, 96.284336864941));
            areaSeries1.Points2.Add(new DataPoint(147.5, 98.7508602689723));
            areaSeries1.Points2.Add(new DataPoint(150, 100.904510594255));
            areaSeries1.Points2.Add(new DataPoint(152.5, 103.266136681506));
            areaSeries1.Points2.Add(new DataPoint(155, 105.780951269521));
            areaSeries1.Points2.Add(new DataPoint(157.5, 108.032859257065));
            areaSeries1.Points2.Add(new DataPoint(160, 110.035478448093));
            areaSeries1.Points2.Add(new DataPoint(162.5, 112.10655731615));
            areaSeries1.Points2.Add(new DataPoint(165, 114.37480786097));
            areaSeries1.Points2.Add(new DataPoint(167.5, 116.403992550869));
            areaSeries1.Points2.Add(new DataPoint(170, 118.61663988727));
            areaSeries1.Points2.Add(new DataPoint(172.5, 120.538730287384));
            areaSeries1.Points2.Add(new DataPoint(175, 122.515721057177));
            areaSeries1.Points2.Add(new DataPoint(177.5, 124.474386629124));
            areaSeries1.Points2.Add(new DataPoint(180, 126.448283293214));
            areaSeries1.Points2.Add(new DataPoint(182.5, 128.373811322299));
            areaSeries1.Points2.Add(new DataPoint(185, 130.33627914667));
            areaSeries1.Points2.Add(new DataPoint(187.5, 132.487933658477));
            areaSeries1.Points2.Add(new DataPoint(190, 134.716989778456));
            areaSeries1.Points2.Add(new DataPoint(192.5, 136.817287595392));
            areaSeries1.Points2.Add(new DataPoint(195, 139.216488664698));
            areaSeries1.Points2.Add(new DataPoint(197.5, 141.50803227574));
            areaSeries1.Points2.Add(new DataPoint(200, 143.539586683614));
            areaSeries1.Points2.Add(new DataPoint(202.5, 145.535911545221));
            areaSeries1.Points2.Add(new DataPoint(205, 147.516964978686));
            areaSeries1.Points2.Add(new DataPoint(207.5, 149.592416731684));
            areaSeries1.Points2.Add(new DataPoint(210, 151.600983566512));
            areaSeries1.Points2.Add(new DataPoint(212.5, 153.498210993362));
            areaSeries1.Points2.Add(new DataPoint(215, 155.512606828247));
            areaSeries1.Points2.Add(new DataPoint(217.5, 157.426564302774));
            areaSeries1.Points2.Add(new DataPoint(220, 159.364474964172));
            areaSeries1.Points2.Add(new DataPoint(222.5, 161.152806492128));
            areaSeries1.Points2.Add(new DataPoint(225, 162.679069434562));
            areaSeries1.Points2.Add(new DataPoint(227.5, 163.893622036741));
            areaSeries1.Points2.Add(new DataPoint(230, 165.475827621238));
            areaSeries1.Points2.Add(new DataPoint(232.5, 167.303960444734));
            areaSeries1.Points2.Add(new DataPoint(235, 169.259393394952));
            areaSeries1.Points2.Add(new DataPoint(237.5, 171.265193646758));
            areaSeries1.Points2.Add(new DataPoint(240, 173.074304345192));
            areaSeries1.Points2.Add(new DataPoint(242.5, 174.975492766814));
            areaSeries1.Points2.Add(new DataPoint(245, 176.684088218484));
            areaSeries1.Points2.Add(new DataPoint(247.5, 178.406887247603));
            areaSeries1.Points.Add(new DataPoint(0, 5.0184649433561));
            areaSeries1.Points.Add(new DataPoint(2.5, 5.27685959268215));
            areaSeries1.Points.Add(new DataPoint(5, 5.81437064628786));
            areaSeries1.Points.Add(new DataPoint(7.5, 6.51022475040994));
            areaSeries1.Points.Add(new DataPoint(10, 7.49921246878766));
            areaSeries1.Points.Add(new DataPoint(12.5, 8.41941631823751));
            areaSeries1.Points.Add(new DataPoint(15, 9.09826907222079));
            areaSeries1.Points.Add(new DataPoint(17.5, 9.89500750098145));
            areaSeries1.Points.Add(new DataPoint(20, 10.6633345249404));
            areaSeries1.Points.Add(new DataPoint(22.5, 11.6249613445368));
            areaSeries1.Points.Add(new DataPoint(25, 12.8816391467497));
            areaSeries1.Points.Add(new DataPoint(27.5, 13.9665185705603));
            areaSeries1.Points.Add(new DataPoint(30, 14.8501816818724));
            areaSeries1.Points.Add(new DataPoint(32.5, 16.0683128022441));
            areaSeries1.Points.Add(new DataPoint(35, 17.5378799723172));
            areaSeries1.Points.Add(new DataPoint(37.5, 19.1262752954039));
            areaSeries1.Points.Add(new DataPoint(40, 20.4103953650735));
            areaSeries1.Points.Add(new DataPoint(42.5, 21.5430627723891));
            areaSeries1.Points.Add(new DataPoint(45, 22.9105459463366));
            areaSeries1.Points.Add(new DataPoint(47.5, 23.9802361888719));
            areaSeries1.Points.Add(new DataPoint(50, 24.8659461235003));
            areaSeries1.Points.Add(new DataPoint(52.5, 25.7303194442439));
            areaSeries1.Points.Add(new DataPoint(55, 26.7688545912359));
            areaSeries1.Points.Add(new DataPoint(57.5, 28.0545112571933));
            areaSeries1.Points.Add(new DataPoint(60, 29.7036634266394));
            areaSeries1.Points.Add(new DataPoint(62.5, 31.2273634344467));
            areaSeries1.Points.Add(new DataPoint(65, 33.1038196356519));
            areaSeries1.Points.Add(new DataPoint(67.5, 35.2639893610328));
            areaSeries1.Points.Add(new DataPoint(70, 37.434293559489));
            areaSeries1.Points.Add(new DataPoint(72.5, 39.7109359368267));
            areaSeries1.Points.Add(new DataPoint(75, 41.7573881676222));
            areaSeries1.Points.Add(new DataPoint(77.5, 44.0460374479862));
            areaSeries1.Points.Add(new DataPoint(80, 46.5098714746581));
            areaSeries1.Points.Add(new DataPoint(82.5, 48.7754012129155));
            areaSeries1.Points.Add(new DataPoint(85, 51.1619816926597));
            areaSeries1.Points.Add(new DataPoint(87.5, 53.9036778414639));
            areaSeries1.Points.Add(new DataPoint(90, 56.7448825012636));
            areaSeries1.Points.Add(new DataPoint(92.5, 59.9294987878434));
            areaSeries1.Points.Add(new DataPoint(95, 63.0148831289797));
            areaSeries1.Points.Add(new DataPoint(97.5, 66.0721745989622));
            areaSeries1.Points.Add(new DataPoint(100, 68.8980036274521));
            areaSeries1.Points.Add(new DataPoint(102.5, 71.7719322611447));
            areaSeries1.Points.Add(new DataPoint(105, 74.4206055336728));
            areaSeries1.Points.Add(new DataPoint(107.5, 76.816198386632));
            areaSeries1.Points.Add(new DataPoint(110, 79.0040432726983));
            areaSeries1.Points.Add(new DataPoint(112.5, 80.9617606926066));
            areaSeries1.Points.Add(new DataPoint(115, 83.1345574620341));
            areaSeries1.Points.Add(new DataPoint(117.5, 85.0701022046479));
            areaSeries1.Points.Add(new DataPoint(120, 86.8557530286516));
            areaSeries1.Points.Add(new DataPoint(122.5, 88.5673387745243));
            areaSeries1.Points.Add(new DataPoint(125, 90.6003321543338));
            areaSeries1.Points.Add(new DataPoint(127.5, 92.439864576254));
            areaSeries1.Points.Add(new DataPoint(130, 94.5383744861178));
            areaSeries1.Points.Add(new DataPoint(132.5, 96.4600166864507));
            areaSeries1.Points.Add(new DataPoint(135, 98.6091052949006));
            areaSeries1.Points.Add(new DataPoint(137.5, 100.496459351478));
            areaSeries1.Points.Add(new DataPoint(140, 102.705767030085));
            areaSeries1.Points.Add(new DataPoint(142.5, 105.009994476992));
            areaSeries1.Points.Add(new DataPoint(145, 107.31287026052));
            areaSeries1.Points.Add(new DataPoint(147.5, 109.584842542272));
            areaSeries1.Points.Add(new DataPoint(150, 111.641435600837));
            areaSeries1.Points.Add(new DataPoint(152.5, 113.988459973544));
            areaSeries1.Points.Add(new DataPoint(155, 116.50349048027));
            areaSeries1.Points.Add(new DataPoint(157.5, 118.753612704274));
            areaSeries1.Points.Add(new DataPoint(160, 120.801728924085));
            areaSeries1.Points.Add(new DataPoint(162.5, 122.902486914165));
            areaSeries1.Points.Add(new DataPoint(165, 125.104391935796));
            areaSeries1.Points.Add(new DataPoint(167.5, 127.06056966547));
            areaSeries1.Points.Add(new DataPoint(170, 129.217086578495));
            areaSeries1.Points.Add(new DataPoint(172.5, 131.151968896274));
            areaSeries1.Points.Add(new DataPoint(175, 133.159906275133));
            areaSeries1.Points.Add(new DataPoint(177.5, 135.065263957561));
            areaSeries1.Points.Add(new DataPoint(180, 137.041870026822));
            areaSeries1.Points.Add(new DataPoint(182.5, 138.937477489811));
            areaSeries1.Points.Add(new DataPoint(185, 140.776914926282));
            areaSeries1.Points.Add(new DataPoint(187.5, 142.786975776398));
            areaSeries1.Points.Add(new DataPoint(190, 144.862762377347));
            areaSeries1.Points.Add(new DataPoint(192.5, 146.89654967049));
            areaSeries1.Points.Add(new DataPoint(195, 149.204343821204));
            areaSeries1.Points.Add(new DataPoint(197.5, 151.369748673527));
            areaSeries1.Points.Add(new DataPoint(200, 153.324438580137));
            areaSeries1.Points.Add(new DataPoint(202.5, 155.173148715344));
            areaSeries1.Points.Add(new DataPoint(205, 157.0501827528));
            areaSeries1.Points.Add(new DataPoint(207.5, 159.109122278359));
            areaSeries1.Points.Add(new DataPoint(210, 161.044446932778));
            areaSeries1.Points.Add(new DataPoint(212.5, 162.942364031841));
            areaSeries1.Points.Add(new DataPoint(215, 164.966769883021));
            areaSeries1.Points.Add(new DataPoint(217.5, 166.89711806788));
            areaSeries1.Points.Add(new DataPoint(220, 168.906874949069));
            areaSeries1.Points.Add(new DataPoint(222.5, 170.85692034995));
            areaSeries1.Points.Add(new DataPoint(225, 172.602125010408));
            areaSeries1.Points.Add(new DataPoint(227.5, 173.964258466598));
            areaSeries1.Points.Add(new DataPoint(230, 175.629908385654));
            areaSeries1.Points.Add(new DataPoint(232.5, 177.495778359378));
            areaSeries1.Points.Add(new DataPoint(235, 179.432933300749));
            areaSeries1.Points.Add(new DataPoint(237.5, 181.400180771342));
            areaSeries1.Points.Add(new DataPoint(240, 183.232300309899));
            areaSeries1.Points.Add(new DataPoint(242.5, 185.225502661441));
            areaSeries1.Points.Add(new DataPoint(245, 186.979590140413));
            areaSeries1.Points.Add(new DataPoint(247.5, 188.816640077725));
            areaSeries1.Title = "Maximum/Minimum";
            plotModel1.Series.Add(areaSeries1);

            var lineSeries1 = new LineSeries
            {
                Color = OxyColors.Blue,
                MarkerFill = OxyColors.Transparent,
                DataFieldX = "Time",
                DataFieldY = "Value"
            };
            lineSeries1.Points.Add(new DataPoint(0, -0.011447056784037));
            lineSeries1.Points.Add(new DataPoint(2.5, 0.179770542275985));
            lineSeries1.Points.Add(new DataPoint(5, 0.6808537498493));
            lineSeries1.Points.Add(new DataPoint(7.5, 1.31859300249191));
            lineSeries1.Points.Add(new DataPoint(10, 2.24910068311687));
            lineSeries1.Points.Add(new DataPoint(12.5, 3.11980453160117));
            lineSeries1.Points.Add(new DataPoint(15, 3.71247338983811));
            lineSeries1.Points.Add(new DataPoint(17.5, 4.39481795774531));
            lineSeries1.Points.Add(new DataPoint(20, 5.02439428524784));
            lineSeries1.Points.Add(new DataPoint(22.5, 5.87249472528812));
            lineSeries1.Points.Add(new DataPoint(25, 6.97260592555283));
            lineSeries1.Points.Add(new DataPoint(27.5, 7.91976631331247));
            lineSeries1.Points.Add(new DataPoint(30, 8.71294011569719));
            lineSeries1.Points.Add(new DataPoint(32.5, 9.82998408944345));
            lineSeries1.Points.Add(new DataPoint(35, 11.208899776828));
            lineSeries1.Points.Add(new DataPoint(37.5, 12.7442103374955));
            lineSeries1.Points.Add(new DataPoint(40, 14.0180206081681));
            lineSeries1.Points.Add(new DataPoint(42.5, 15.1195629875034));
            lineSeries1.Points.Add(new DataPoint(45, 16.4612081950815));
            lineSeries1.Points.Add(new DataPoint(47.5, 17.5118421203978));
            lineSeries1.Points.Add(new DataPoint(50, 18.4226816405881));
            lineSeries1.Points.Add(new DataPoint(52.5, 19.344752313753));
            lineSeries1.Points.Add(new DataPoint(55, 20.47048124027));
            lineSeries1.Points.Add(new DataPoint(57.5, 21.8032585135211));
            lineSeries1.Points.Add(new DataPoint(60, 23.4655788326243));
            lineSeries1.Points.Add(new DataPoint(62.5, 24.9628808265612));
            lineSeries1.Points.Add(new DataPoint(65, 26.7799264178984));
            lineSeries1.Points.Add(new DataPoint(67.5, 28.8753610496295));
            lineSeries1.Points.Add(new DataPoint(70, 30.9831304948466));
            lineSeries1.Points.Add(new DataPoint(72.5, 33.1557418763199));
            lineSeries1.Points.Add(new DataPoint(75, 35.1392386567962));
            lineSeries1.Points.Add(new DataPoint(77.5, 37.3626727617638));
            lineSeries1.Points.Add(new DataPoint(80, 39.7822682973613));
            lineSeries1.Points.Add(new DataPoint(82.5, 42.0505038654434));
            lineSeries1.Points.Add(new DataPoint(85, 44.3977945883283));
            lineSeries1.Points.Add(new DataPoint(87.5, 47.0524522387201));
            lineSeries1.Points.Add(new DataPoint(90, 49.8186190205946));
            lineSeries1.Points.Add(new DataPoint(92.5, 52.8980099892933));
            lineSeries1.Points.Add(new DataPoint(95, 55.9174513617612));
            lineSeries1.Points.Add(new DataPoint(97.5, 58.9513015195966));
            lineSeries1.Points.Add(new DataPoint(100, 61.7974952408334));
            lineSeries1.Points.Add(new DataPoint(102.5, 64.738330780104));
            lineSeries1.Points.Add(new DataPoint(105, 67.4211349969828));
            lineSeries1.Points.Add(new DataPoint(107.5, 69.8712285745755));
            lineSeries1.Points.Add(new DataPoint(110, 72.0935083678195));
            lineSeries1.Points.Add(new DataPoint(112.5, 74.0991599504599));
            lineSeries1.Points.Add(new DataPoint(115, 76.3529601655682));
            lineSeries1.Points.Add(new DataPoint(117.5, 78.3984034376212));
            lineSeries1.Points.Add(new DataPoint(120, 80.2446080657163));
            lineSeries1.Points.Add(new DataPoint(122.5, 82.0166768951652));
            lineSeries1.Points.Add(new DataPoint(125, 84.0836307024042));
            lineSeries1.Points.Add(new DataPoint(127.5, 86.002041560459));
            lineSeries1.Points.Add(new DataPoint(130, 88.2007526944628));
            lineSeries1.Points.Add(new DataPoint(132.5, 90.2149178416771));
            lineSeries1.Points.Add(new DataPoint(135, 92.4969469740507));
            lineSeries1.Points.Add(new DataPoint(137.5, 94.5261970891274));
            lineSeries1.Points.Add(new DataPoint(140, 96.875636035961));
            lineSeries1.Points.Add(new DataPoint(142.5, 99.3532051177711));
            lineSeries1.Points.Add(new DataPoint(145, 101.798603562731));
            lineSeries1.Points.Add(new DataPoint(147.5, 104.167851405622));
            lineSeries1.Points.Add(new DataPoint(150, 106.272973097546));
            lineSeries1.Points.Add(new DataPoint(152.5, 108.627298327525));
            lineSeries1.Points.Add(new DataPoint(155, 111.142220874895));
            lineSeries1.Points.Add(new DataPoint(157.5, 113.39323598067));
            lineSeries1.Points.Add(new DataPoint(160, 115.418603686089));
            lineSeries1.Points.Add(new DataPoint(162.5, 117.504522115157));
            lineSeries1.Points.Add(new DataPoint(165, 119.739599898383));
            lineSeries1.Points.Add(new DataPoint(167.5, 121.732281108169));
            lineSeries1.Points.Add(new DataPoint(170, 123.916863232882));
            lineSeries1.Points.Add(new DataPoint(172.5, 125.845349591829));
            lineSeries1.Points.Add(new DataPoint(175, 127.837813666155));
            lineSeries1.Points.Add(new DataPoint(177.5, 129.769825293343));
            lineSeries1.Points.Add(new DataPoint(180, 131.745076660018));
            lineSeries1.Points.Add(new DataPoint(182.5, 133.655644406055));
            lineSeries1.Points.Add(new DataPoint(185, 135.556597036476));
            lineSeries1.Points.Add(new DataPoint(187.5, 137.637454717438));
            lineSeries1.Points.Add(new DataPoint(190, 139.789876077902));
            lineSeries1.Points.Add(new DataPoint(192.5, 141.856918632941));
            lineSeries1.Points.Add(new DataPoint(195, 144.210416242951));
            lineSeries1.Points.Add(new DataPoint(197.5, 146.438890474634));
            lineSeries1.Points.Add(new DataPoint(200, 148.432012631876));
            lineSeries1.Points.Add(new DataPoint(202.5, 150.354530130282));
            lineSeries1.Points.Add(new DataPoint(205, 152.283573865743));
            lineSeries1.Points.Add(new DataPoint(207.5, 154.350769505022));
            lineSeries1.Points.Add(new DataPoint(210, 156.322715249645));
            lineSeries1.Points.Add(new DataPoint(212.5, 158.220287512602));
            lineSeries1.Points.Add(new DataPoint(215, 160.239688355634));
            lineSeries1.Points.Add(new DataPoint(217.5, 162.161841185327));
            lineSeries1.Points.Add(new DataPoint(220, 164.135674956621));
            lineSeries1.Points.Add(new DataPoint(222.5, 166.004863421039));
            lineSeries1.Points.Add(new DataPoint(225, 167.640597222485));
            lineSeries1.Points.Add(new DataPoint(227.5, 168.928940251669));
            lineSeries1.Points.Add(new DataPoint(230, 170.552868003446));
            lineSeries1.Points.Add(new DataPoint(232.5, 172.399869402056));
            lineSeries1.Points.Add(new DataPoint(235, 174.346163347851));
            lineSeries1.Points.Add(new DataPoint(237.5, 176.33268720905));
            lineSeries1.Points.Add(new DataPoint(240, 178.153302327545));
            lineSeries1.Points.Add(new DataPoint(242.5, 180.100497714128));
            lineSeries1.Points.Add(new DataPoint(245, 181.831839179449));
            lineSeries1.Points.Add(new DataPoint(247.5, 183.611763662664));
            lineSeries1.Title = "Average";
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

        private static AreaSeries CreateExampleAreaSeries()
        {
            var areaSeries1 = new AreaSeries();
            areaSeries1.Points.Add(new DataPoint(0, 50));
            areaSeries1.Points.Add(new DataPoint(10, 40));
            areaSeries1.Points.Add(new DataPoint(20, 60));
            areaSeries1.Points2.Add(new DataPoint(0, 60));
            areaSeries1.Points2.Add(new DataPoint(5, 80));
            areaSeries1.Points2.Add(new DataPoint(20, 70));
            return areaSeries1;
        }
    }
}
