using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace scrolling
{
    public class RadarChartData : ChartData
    {
        public UIColor highlightColor = UIColor.FromRGB(255.0f/255.0f, 187.0f/255.0f, 115.0f/255.0f);
        public nfloat highlightLineWidth = 1.0f;
        public nfloat highlightLineDashPhase = 0.0f;
        public List<nfloat> highlightLineDashLengths;


        public RadarChartData()
        {
        }

        public RadarChartData(List<string> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
        }

        public RadarChartData(List<NSObject> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
        }


    }
}