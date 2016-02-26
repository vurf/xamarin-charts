using System;
using System.Collections.Generic;
using UIKit;

namespace scrolling
{
    public interface IBarLineScatterCandleBubbleChartDataSet : IChartDataSet
    {
        // MARK: - Data functions and accessors

        // MARK: - Styling functions and accessors

        UIColor highlightColor { get; set; }
        nfloat highlightLineWidth { get; set; }
        nfloat highlightLineDashPhase { get; set; }
        List<nfloat> highlightLineDashLengths { get; set; }
    }
}