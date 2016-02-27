using System.Collections.Generic;
using Foundation;

namespace scrolling
{
    public class CombinedChartData : BarLineScatterCandleBubbleChartData
    {
        private LineChartData _lineData;
        private BarChartData _barData;
        private ScatterChartData _scatterData;
        private CandleChartData _candleData;
        private BubbleChartData _bubbleData;

        public CombinedChartData()
        {
        }

        public CombinedChartData(List<string> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
        }

        public CombinedChartData(List<NSObject> xVals, List<IChartDataSet> dataSets) : base(xVals, dataSets)
        {
        }


        public LineChartData lineData
        {
            get { return _lineData; }
            set
            {
                _lineData = value;
                foreach (var dataSet in value.dataSets)
                {
                    _dataSets.Add(dataSet);
                }

                checkIsLegal(value.dataSets);

                calcMinMax(_lastStart, _lastEnd);
                calcYValueCount();

                calcXValAverageLength();
            }
        }

        public BarChartData barData
        {
            get { return _barData; }
            set
            {
                _barData = value;
                foreach (var dataSet in value.dataSets)
                {
                    _dataSets.Add(dataSet);
                }

                checkIsLegal(value.dataSets);

                calcMinMax(_lastStart, _lastEnd);
                calcYValueCount();

                calcXValAverageLength();
            }
        }

        public ScatterChartData scatterData
        {
            get { return _scatterData; }
            set
            {
                _scatterData = value;
                foreach (var dataSet in value.dataSets)
                {
                    _dataSets.Add(dataSet);
                }

                checkIsLegal(value.dataSets);

                calcMinMax(_lastStart, _lastEnd);
                calcYValueCount();

                calcXValAverageLength();
            }
        }

        public CandleChartData candleData
        {
            get { return _candleData; }
            set
            {
                _candleData = value;
                foreach (var dataSet in value.dataSets)
                {
                    _dataSets.Add(dataSet);
                }

                checkIsLegal(value.dataSets);

                calcMinMax(_lastStart, _lastEnd);
                calcYValueCount();

                calcXValAverageLength();
            }
        }

        public BubbleChartData bubbleData
        {
            get { return _bubbleData; }
            set
            {
                _bubbleData = value;
                foreach (var dataSet in value.dataSets)
                {
                    _dataSets.Add(dataSet);
                }

                checkIsLegal(value.dataSets);

                calcMinMax(_lastStart, _lastEnd);
                calcYValueCount();

                calcXValAverageLength();
            }
        }

        /// - returns: all data objects in row: line-bar-scatter-candle-bubble if not null.
        public List<ChartData> allData
        {
            get
            {
                var data = new List<ChartData>();

                if (lineData != null)
                {
                    data.Add(lineData);
                }
                if (barData != null)
                {
                    data.Add(barData);
                }
                if (scatterData != null)
                {
                    data.Add(scatterData);
                }
                if (candleData != null)
                {
                    data.Add(candleData);
                }
                if (bubbleData != null)
                {
                    data.Add(bubbleData);
                }

                return data;
            }
        }


        public override void notifyDataChanged()
        {
            if (_lineData != null)
            {
                _lineData.notifyDataChanged();
            }
            if (_barData != null)
            {
                _barData.notifyDataChanged();
            }
            if (_scatterData != null)
            {
                _scatterData.notifyDataChanged();
            }
            if (_candleData != null)
            {
                _candleData.notifyDataChanged();
            }
            if (_bubbleData != null)
            {
                _bubbleData.notifyDataChanged();
            }

            base.notifyDataChanged(); // recalculate everything
        }
    }
}