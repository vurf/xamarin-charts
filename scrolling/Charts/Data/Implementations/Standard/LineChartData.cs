using System.Collections.Generic;
using Foundation;

namespace scrolling
{
    public class LineChartData : ChartData
    {
        public LineChartData()
        {
        }

        public LineChartData(List<string> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
        }

        public LineChartData(List<NSObject> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
        }

      
    }
}