using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace scrolling
{
    public interface ChartFillFormatter
    {
        nfloat getFillLinePosition(ILineChartDataSet dataSet, LineChartDataProvider dataProvider);
    }

    class ChartDefaultFillFormatter : NSObject, ChartFillFormatter
    {
        public nfloat getFillLinePosition(ILineChartDataSet dataSet, LineChartDataProvider dataProvider)
        {
            nfloat fillMin = 0.0f;

            if (dataSet.yMax > 0.0 && dataSet.yMin < 0.0)
            {
                fillMin = 0.0f;
            }
            else
            {
                var data = dataProvider.data;
                if (data != null)
                {
                    double max, min;

                    if (data.yMax > 0.0)
                    {
                        max = 0.0;
                    }
                    else
                    {
                        max = dataProvider.chartYMax;
                    }

                    if (data.yMin < 0.0)
                    {
                        min = 0.0;
                    }
                    else
                    {
                        min = dataProvider.chartYMin;
                    }

                    fillMin = (nfloat) (dataSet.yMin >= 0.0 ? min : max);
                }
            }

            return fillMin;
        }
    }
}