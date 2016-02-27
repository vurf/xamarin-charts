using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;

namespace scrolling
{
    public class BubbleChartData : BarLineScatterCandleBubbleChartData
    {
        public BubbleChartData(List<string> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
            
        }

        public BubbleChartData(List<NSObject> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
            
        }

        /// Sets the width of the circle that surrounds the bubble when highlighted for all DataSet objects this data object contains
        public void setHighlightCircleWidth(nfloat width)
        {
            var temp = dataSets.Cast<IBubbleChartDataSet>().ToList();
            //FIXME debug cast
            foreach (var set in temp)
            {
                set.highlightCircleWidth = width;
            }
        }
    }
}