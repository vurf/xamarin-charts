using System;
using System.Collections.Generic;
using Foundation;

namespace scrolling
{
    public class PieChartData : ChartData
    {
        public PieChartData()
        {
        }

        public PieChartData(List<string> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
        }

        public PieChartData(List<NSObject> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
        }

        PieChartDataSet dataSet
        {
            get { return dataSets.Count > 0 ? dataSets[0] as PieChartDataSet : null; }
            set
            {
                if (value != null)
                {
                    dataSets = new List<IChartDataSet>() {value};
                }
                else
                {
                    dataSets = new List<IChartDataSet>();
                }
            }
        }

        public override IChartDataSet getDataSetByIndex(int index)
        {
            if (index != 0)
            {
                return null;
            }
            return base.getDataSetByIndex(index);
        }

        public override IChartDataSet getDataSetByLabel(string label, bool ignorecase)
        {
            if (dataSets.Count == 0 || dataSets[0].label == null)
            {
                return null;
            }

            if (ignorecase)
            {
                if (label.caseInsensitiveCompare(dataSets[0].label) == NSComparisonResult.OrderedSame)
                {
                    return dataSets[0];
                }
            }
            else
            {
                if (label == dataSets[0].label)
                {
                    return dataSets[0];
                }
            }
            return null;
        }

        public override void addDataSet(IChartDataSet d)
        {
            if (_dataSets == null)
            {
                return;
            }

            base.addDataSet(d);
        }

        /// Removes the DataSet at the given index in the DataSet array from the data object.
        /// Also recalculates all minimum and maximum values.
        ///
        /// - returns: true if a DataSet was removed, false if no DataSet could be removed.
        public override bool removeDataSetByIndex(int index)
        {
            if (_dataSets == null || index >= _dataSets.Count || index < 0)
            {
                return false;
            }

            return false;
        }

        /// - returns: the total y-value sum across all DataSet objects the this object represents.
        public double yValueSum
        {
            get
            {
                double yValueSum = 0.0;

                if (_dataSets == null)
                {
                    return yValueSum;
                }
                foreach (var dataSet in _dataSets)
                {
                    yValueSum += Math.Abs((dataSet as IPieChartDataSet).yValueSum);
                }

                return yValueSum;
            }
        }

        /// - returns: the average value across all entries in this Data object (all entries from the DataSets this data object holds)
        public double average
        {
            get { return yValueSum/(double) (yValCount); }
        }
    }
}