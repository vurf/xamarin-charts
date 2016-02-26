using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace scrolling
{
    class ChartLimitLine : ChartComponentBase
    {
        public enum ChartLimitLabelPosition
        {
            LeftTop,
            LeftBottom,
            RightTop,
            RightBottom
        }

        /// limit / maximum (the y-value or xIndex)
        public double limit = 0.0d;

        private nfloat _lineWidth = 2.0f;
        public UIColor lineColor = UIColor.FromRGB(237.0f/255.0f, 91.0f/255.0f, 91.0f/255.0f);
        public nfloat lineDashPhase = 0.0f;
        public List<nfloat> lineDashLengths;
        public UIColor valueTextColor = UIColor.Black;
        public UIFont valueFont = UIFont.SystemFontOfSize(13.0f);
        public string label = "";
        public ChartLimitLabelPosition labelPosition = ChartLimitLabelPosition.RightTop;

        public ChartLimitLine()
        {
        }

        public ChartLimitLine(double limit)
        {
            this.limit = limit;
        }

        public ChartLimitLine(double limit, string label)
        {
            this.limit = limit;
            this.label = label;
        }


        /// set the line width of the chart (min = 0.2, max = 12); default 2
        public nfloat lineWidth
        {
            get { return _lineWidth; }
            set
            {
                if (value < 0.2f)
                {
                    _lineWidth = 0.2f;
                }
                else if (value > 12.0f)
                {
                    _lineWidth = 12.0f;
                }
                else
                {
                    _lineWidth = value;
                }
            }
        }

    }
}