using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using OxyPlot;

namespace AxesDemo
{
    public class MainViewModel
    {
        public Collection<DataPoint> Data1 { get; set; }

        public MainViewModel()
        {
            Data1 = new Collection<DataPoint> { new DataPoint(10, 4), new DataPoint(12, 7), new DataPoint(16, 3), new DataPoint(20, 9) };
        }
    }
}
