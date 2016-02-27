using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace scrolling
{
    public class CandleChartDataSet : LineScatterCandleRadarChartDataSet, ICandleChartDataSet
    {
        private nfloat _barSpace = 0.1f;
        private bool _showCandleBar = true;
        private nfloat _shadowWidth = 1.5f;
        private bool _decreasingFilled = true;

        public CandleChartDataSet()
        {
        }

        public CandleChartDataSet(List<ChartDataEntry> yVals, string label) : base(yVals, label)
        {

        }

        public override void calcMinMax(int start, int end)
        {
            var yValCount = this.entryCount;

            if (yValCount == 0)
            {
                return;
            }

            var entries = yVals.Cast<CandleChartDataEntry>().ToList();

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

            _yMin = double.MaxValue;
            _yMax = -double.MaxValue;

            for (var i = start; i <= endValue; i++)
            {
                var e = entries[i];

                if (e.low < _yMin)
                {
                    _yMin = e.low;
                }

                if (e.high > _yMax)
                {
                    _yMax = e.high;
                }
            }
        }

        public nfloat barSpace
        {
            get { return _barSpace; }
            set
            {
                if (value < 0.0f)
                {
                    _barSpace = 0.0f;
                }
                else if (value > 0.45f)
                {
                    _barSpace = 0.45f;
                }
                else
                {
                    _barSpace = value;
                }
            }
        }

        public bool showCandleBar
        {
            get { return _showCandleBar; }
            set { _showCandleBar = value; }
        }

        public nfloat shadowWidth
        {
            get { return _shadowWidth; }
            set { _shadowWidth = value; }
        }

        public UIColor shadowColor { get; set; }
        public bool shadowColorSameAsCandle { get; set; }

        public bool isShadowColorSameAsCandle
        {
            get { return shadowColorSameAsCandle; }
        }

        public UIColor neutralColor { get; set; }
        public UIColor increasingColor { get; set; }
        public UIColor decreasingColor { get; set; }
        public bool increasingFilled { get; set; }

        public bool isIncreasingFilled
        {
            get { return increasingFilled; }
        }

        public bool decreasingFilled
        {
            get { return _decreasingFilled; }
            set { _decreasingFilled = value; }
        }

        public bool isDecreasingFilled
        {
            get { return decreasingFilled; }
        }
    }
}