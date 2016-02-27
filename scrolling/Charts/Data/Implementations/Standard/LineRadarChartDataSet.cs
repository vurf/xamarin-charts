using System;
using System.Collections.Generic;
using UIKit;

namespace scrolling
{
    public class LineRadarChartDataSet : LineScatterCandleRadarChartDataSet, ILineRadarChartDataSet
    {
        private UIColor _fillColor = UIColor.FromRGB(140.0f/255.0f, 234.0f/255.0f, 255.0f/255.0f);
        private nfloat _fillAlpha = 0.33f;
        private nfloat _lineWidth = 1f;

        public UIColor fillColor
        {
            get { return _fillColor; }
            set { _fillColor = value;
                fill = null; 
            }
        }

        public ChartFill fill { get; set; }

        public nfloat fillAlpha
        {
            get { return _fillAlpha; }
            set { _fillAlpha = value; }
        }

        public nfloat lineWidth
        {
            get { return _lineWidth; }
            set
            {
                if (value < 0.2f)
                {
                    _lineWidth = 0.2f;
                }
                else if (value > 10.0f)
                {
                    _lineWidth = 10.0f;
                }
                else
                {
                    _lineWidth = value;
                }
            }
        }

        public bool drawFilledEnabled { get; set; }

        public bool isDrawFilledEnabled
        {
            get { return drawFilledEnabled; }
        }

        public LineRadarChartDataSet()
        {
        }

        public LineRadarChartDataSet(List<ChartDataEntry> yVals, string label) : base(yVals, label)
        {
        }
    }
}