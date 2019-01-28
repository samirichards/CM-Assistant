using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;

namespace CM_Assistant_UWP.Classes.Viewmodels
{
    class TestPlotViewModel
    {
        public PlotModel PlotModel { get; set; }

        public TestPlotViewModel()
        {
            PlotModel = new PlotModel { Title = "Test" };
            PlotModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
        }
    }
}
