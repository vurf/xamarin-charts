using System;
using System.Collections.Generic;
using System.Linq;

namespace scrolling
{
    public class BubbleChartDataSet : BarLineScatterCandleBubbleChartDataSet, IBubbleChartDataSet
    {
        private double _xMin = 0;
        private double _xMax = 0;
        private nfloat _maxSize = 0f;
        private nfloat _highlightCircleWidth = 2.5f;


        public double xMin
        {
            get { return _xMin; }
        }

        public double xMax
        {
            get { return _xMax; }
        }

        public nfloat maxSize
        {
            get { return _maxSize; }
        }

        public nfloat highlightCircleWidth
        {
            get { return _highlightCircleWidth; }
            set { _highlightCircleWidth = value; }
        }

        public override void calcMinMax(int start, int end)
        {
            var yValCount = this.entryCount;

            if (yValCount == 0)
            {
                return;
            }
            //FIXME debug cast
            var entries = yVals.Cast<BubbleChartDataEntry>().ToList();

            // need chart width to guess this properly

            int endValue = 0;

            if (end == 0 || end >= yValCount)
            {
                endValue = yValCount - 1;
            }
            else
            {
                endValue = end;
            }

            _lastStart = start;
            _lastEnd = end;

            _yMin = yMin(entries[start]);
            _yMax = yMax(entries[start]);

            for (var i = start; i <= endValue; i++)
            {
                var entry = entries[i];

                var ymin = yMin(entry);
                var ymax = yMax(entry);

                if (ymin < _yMin)
                {
                    _yMin = ymin;
                }

                if (ymax > _yMax)
                {
                    _yMax = ymax;
                }

                var xmin = xMinget(entry);
                var xmax = xMaxget(entry);

                if (xmin < _xMin)
                {
                    _xMin = xmin;
                }

                if (xmax > _xMax)
                {
                    _xMax = xmax;
                }

                var size = largestSize(entry);

                if (size > _maxSize)
                {
                    _maxSize = size;
                }
            }
        }

        private double yMin(BubbleChartDataEntry entry)
        {
            return entry.Value;
        }

        private double yMax(BubbleChartDataEntry entry)
        {
            return entry.Value;
        }

        private double xMinget(BubbleChartDataEntry entry)
        {
            return (double) (entry.xIndex);
        }

        private double xMaxget(BubbleChartDataEntry entry)
        {
            return (double) (entry.xIndex);
        }

        private nfloat largestSize(BubbleChartDataEntry entry)
        {
            return entry.size;
        }
    }
}