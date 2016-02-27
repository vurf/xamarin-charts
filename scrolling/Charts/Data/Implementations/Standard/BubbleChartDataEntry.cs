using System;

namespace scrolling
{
    public class BubbleChartDataEntry : ChartDataEntry
    {
        public nfloat size = 0.0f;

        public BubbleChartDataEntry() : base()
        {

        }

        /// - parameter xIndex: The index on the x-axis.
        /// - parameter val: The value on the y-axis.
        /// - parameter size: The size of the bubble.
        public BubbleChartDataEntry(int xIndex, double value, nfloat size) : base(value, xIndex)
        {
            this.size = size;
        }

        /// - parameter xIndex: The index on the x-axis.
        /// - parameter val: The value on the y-axis.
        /// - parameter size: The size of the bubble.
        /// - parameter data: Spot for additional data this Entry represents.
        public BubbleChartDataEntry(int xIndex, double value, nfloat size, object data) : base(value, xIndex, data)
        {
            this.size = size;
        }
    }
}