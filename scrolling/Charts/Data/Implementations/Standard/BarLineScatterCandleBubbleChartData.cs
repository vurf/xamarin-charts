using System.Collections.Generic;
using Foundation;

namespace scrolling
{
    public class BarLineScatterCandleBubbleChartData : ChartData
    {
        public BarLineScatterCandleBubbleChartData() : base()
        {
            
        }

        public BarLineScatterCandleBubbleChartData(List<string> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
            
        }

        public BarLineScatterCandleBubbleChartData(List<NSObject> xVals, List<IChartDataSet> dataSets)
            : base(xVals, dataSets)
        {
            
        }

    }
}