using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace scrolling
{
    public class PieChartDataSet : ChartDataSet, IPieChartDataSet
    {
        private double _yValueSum = 0.0;
        private nfloat _sliceSpace = 0.0f;
        private nfloat _selectionShift = 18.0f;

        public double yValueSum
        {
            get { return _yValueSum; }
        }

        public double average
        {
            get { return yValueSum/(valueCount); }
        }

        public nfloat sliceSpace
        {
            get { return _sliceSpace; }
            set
            {
                var space = value;
                if (space > 20.0f)
                {
                    space = 20.0f;
                }
                if (space < 0.0f)
                {
                    space = 0.0f;
                }
                _sliceSpace = space;
            }
        }

        public nfloat selectionShift
        {
            get { return _selectionShift; }
            set { _selectionShift = value; }
        }

        private void initialize()
        {
            valueTextColor = UIColor.White;
            valueFont = UIFont.SystemFontOfSize(13.0f);

            calcYValueSum();
        }

        public PieChartDataSet()
        {
            initialize();
        }

        public PieChartDataSet(List<ChartDataEntry> yVals, string label) : base(yVals, label)
        {
            initialize();
        }

        private void calcYValueSum()
        {
            _yValueSum = 0;

            for (var i = 0; i < _yVals.Count; i++)
            {
                _yValueSum += Math.Abs(_yVals[i].value);
            }
        }

        public override void notifyDataSetChanged()
        {
            base.notifyDataSetChanged();
            calcYValueSum();
        }

        public override bool addEntry(ChartDataEntry e)
        {
            if (base.addEntry(e))
            {
                _yValueSum += e.value;
                return true;
            }

            return false;
        }

        public override bool addEntryOrdered(ChartDataEntry e)
        {
             if (base.addEntryOrdered(e))
             {
                 _yValueSum += e.value;
                 return true;
             }

            return false;

        }

        public override bool removeEntry(ChartDataEntry entry)
        {
            if (base.removeEntry(entry))
            {
                _yValueSum -= entry.value;
                return true;
            }

            return false;
        }

        public override bool removeEntry(int xIndex)
        {
            var index = entryIndex(xIndex);
            if (index > -1)
            {
                var e = _yVals[index];

                if (base.removeEntry(e))
                {
                    _yValueSum -= e.value;
                    return true;
                }
            }

            return false;
        }


        public override bool removeFirst()
        {
            ChartDataEntry entry = _yVals != null ? _yVals[0] : null;
            if (entry != null && base.removeFirst())
            {
                _yValueSum -= entry.value;
                return true;
            }
            return false;
        }

        public override bool removeLast()
        {
            ChartDataEntry entry = _yVals != null ? _yVals[_yVals.Count - 1] : null;
            if (entry != null)
            {
                if (base.removeLast())
                {
                    _yValueSum -= entry.value;
                    return true;
                }
            }

            return false;
        }
    }
}