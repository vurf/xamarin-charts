using System.Collections.Generic;
using Foundation;

namespace scrolling
{
    public class CandleChartData : BarLineScatterCandleBubbleChartData
    {
        public CandleChartData() : base()
        {
        }

        public CandleChartData(List<string> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
        }

        public CandleChartData(List<NSObject> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
        }

    }
}