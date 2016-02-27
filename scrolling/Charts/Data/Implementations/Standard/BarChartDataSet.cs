using System;
using System.Collections.Generic;
using UIKit;

namespace scrolling
{
    public class BarChartDataSet : BarLineScatterCandleBubbleChartDataSet, IBarChartDataSet
    {
        private nfloat _barSpace = 0.15f;
        private bool _isStacked;
        private int _stackSize = 1;
        private int _entryCountStacks = 0;
        private UIColor _barShadowColor = UIColor.FromRGB(215.0f / 255.0f, 215.0f / 255.0f, 215.0f / 255.0f);
        private nfloat _highlightAlpha = 120.0f / 255.0f;
        private List<string> _stackLabels = new List<string>() { "Stack" };

        public nfloat barSpace
        {
            get { return _barSpace; }
            set { _barSpace = value; }
        }

        public bool isStacked
        {
            get { return _stackSize > 1; }
        }

        public int stackSize
        {
            get { return _stackSize; }
        }

        public int entryCountStacks
        {
            get { return _entryCountStacks; }
        }

        public UIColor barShadowColor
        {
            get { return _barShadowColor; }
            set { _barShadowColor = value; }
        }

        public nfloat highlightAlpha
        {
            get { return _highlightAlpha; }
            set { _highlightAlpha = value; }
        }

        public List<string> stackLabels
        {
            get { return _stackLabels; }
            set { _stackLabels = value; }
        }

        private void initialize()
        {
            highlightColor = UIColor.Black;

            calcStackSize(yVals as List<BarChartDataEntry>);
            calcEntryCountIncludingStacks(yVals as List<BarChartDataEntry>);
        }

        public BarChartDataSet() : base()
        {
            initialize();
        }

        public BarChartDataSet(List<ChartDataEntry> yVals, string label) : base(yVals, label)
        {
            initialize();
        }

        /// Calculates the total number of entries this DataSet represents, including
        /// stacks. All values belonging to a stack are calculated separately.
        private void calcEntryCountIncludingStacks(List<BarChartDataEntry> yVals)
        {
            _entryCountStacks = 0;

            for (var i = 0; i < yVals.Count; i++)
            {
                var vals = yVals[i].values;

                if (vals == null)
                {
                    _entryCountStacks++;
                }
                else
                {
                    _entryCountStacks += vals.Count;
                }
            }
        }

        private void calcStackSize(List<BarChartDataEntry> yVals)
        {
            for (var i = 0; i < yVals.Count; i++)
            {
                var vals = yVals[i].values;
                if (vals != null)
                {
                    if (vals.Count > _stackSize)
                    {
                        _stackSize = vals.Count;
                    }
                }
            }
        }


        public override void calcMinMax(int start, int end)
        {
            var yValCount = _yVals.Count;

            if (yValCount == 0)
            {
                return;
            }

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
            _lastEnd = endValue;

            _yMin = double.MaxValue;
            _yMax = -double.MaxValue;

            for (var i = start; i <= endValue; i++)
            {
                var e = _yVals[i] as BarChartDataEntry;
                if (e != null)
                {
                    if (!double.IsNaN(e.Value))
                    {
                        if (e.values == null)
                        {
                            if (e.Value < _yMin)
                            {
                                _yMin = e.Value;
                            }

                            if (e.Value > _yMax)
                            {
                                _yMax = e.Value;
                            }
                        }
                        else
                        {
                            if (-e.negativeSum < _yMin)
                            {
                                _yMin = -e.negativeSum;
                            }

                            if (e.positiveSum > _yMax)
                            {
                                _yMax = e.positiveSum;
                            }
                        }
                    }
                }
            }

            if (_yMin == double.MaxValue)
            {
                _yMin = 0.0;
                _yMax = 0.0;
            }
        }



    }
}