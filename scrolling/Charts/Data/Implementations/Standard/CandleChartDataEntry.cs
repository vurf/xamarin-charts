using System;

namespace scrolling
{
    public class CandleChartDataEntry : ChartDataEntry
    {
        public double high = 0.0;

        /// shadow-low value
        public double low = 0.0;

        /// close value
        public double close = 0.0;

        /// open value
        public double open = 0.0;


        public CandleChartDataEntry()
        {

        }

        public CandleChartDataEntry(int xIndex, double shadowH, double shadowL, double open, double close)
            : base((shadowH + shadowL)/2.0, xIndex)
        {
            this.high = shadowH;
            this.low = shadowL;
            this.open = open;
            this.close = close;
        }

        public CandleChartDataEntry(int xIndex, double shadowH, double shadowL, double open, double close, object data)
            : base((shadowH + shadowL)/2.0, xIndex, data)
        {

            this.high = shadowH;
            this.low = shadowL;
            this.open = open;
            this.close = close;
        }

        /// - returns: the overall range (difference) between shadow-high and shadow-low.
        public double shadowRange
        {
            get { return Math.Abs(high - low); }
        }

        /// - returns: the body size (difference between open and close).
        public double bodyRange
        {
            get { return Math.Abs(open - close); }
        }

        /// the center value of the candle. (Middle value between high and low)
        public override double value
        {
            get { return base.value; }
            set { base.value = (high + low)/2.0; }
        }

    }
}