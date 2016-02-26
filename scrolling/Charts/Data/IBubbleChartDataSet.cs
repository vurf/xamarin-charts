using System;

namespace scrolling
{
    public interface IBubbleChartDataSet : IBarLineScatterCandleBubbleChartDataSet
    {
        // MARK: - Data functions and accessors

        double xMin { get; }
        double xMax { get; }
        nfloat maxSize { get; }

        // MARK: - Styling functions and accessors

        /// Sets/gets the width of the circle that surrounds the bubble when highlighted
        nfloat highlightCircleWidth { get; set; }
    }
}