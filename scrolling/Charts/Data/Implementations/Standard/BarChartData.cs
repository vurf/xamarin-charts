using System;
using System.Collections.Generic;
using Foundation;

namespace scrolling
{
    public class BarChartData : BarLineScatterCandleBubbleChartData
    {

        public BarChartData() : base()
        {

        }

        public BarChartData(List<string> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {

        }

        public BarChartData(List<NSObject> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {

        }

        private nfloat _groupSpace = 0.8f;

        /// The spacing is relative to a full bar width
        public nfloat groupSpace
        {
            get
            {
                if (_dataSets.Count <= 1)
                {
                    return 0.0f;
                }
                return _groupSpace;
            }
            set { _groupSpace = value; }
        }

        /// - returns: true if this BarData object contains grouped DataSets (more than 1 DataSet).
        public bool isGrouped
        {
            get { return _dataSets.Count > 1 ? true : false; }
        }
    }
}