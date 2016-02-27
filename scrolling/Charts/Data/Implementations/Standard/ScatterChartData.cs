using System;
using System.Collections.Generic;
using Foundation;

namespace scrolling
{
    public class ScatterChartData : BarLineScatterCandleBubbleChartData
    {
        public ScatterChartData()
        {
        }

        public ScatterChartData(List<string> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
        }

        public ScatterChartData(List<NSObject> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
        }

        public nfloat getGreatestShapeSize()
        {
            nfloat max = 0.0f;

            foreach (var set in dataSets)
            {
                var scatterDataSet = set as IScatterChartDataSet;

                if (scatterDataSet == null)
                {
                    Console.WriteLine("ScatterChartData: Found a DataSet which is not a ScatterChartDataSet");
                }
                else
                {
                    var size = scatterDataSet.scatterShapeSize;

                    if (size > max)
                    {
                        max = size;
                    }
                }
            }

            return max;
        }
    }
}