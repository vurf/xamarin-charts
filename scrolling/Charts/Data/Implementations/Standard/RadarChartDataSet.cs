using System;
using System.Collections.Generic;
using UIKit;

namespace scrolling
{
    public class RadarChartDataSet: LineRadarChartDataSet, IRadarChartDataSet
    {
         private void initialize()
         {
             valueFont = UIFont.SystemFontOfSize(13.0f);
         }

        public RadarChartDataSet()
        {
            initialize();
        }

        public RadarChartDataSet(List<ChartDataEntry> yVals, string label) : base(yVals, label)
        {
            initialize();
        }

    }
}