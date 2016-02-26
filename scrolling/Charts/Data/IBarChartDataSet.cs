using System;
using System.Collections.Generic;
using UIKit;

namespace scrolling
{
    public interface IBarChartDataSet : IBarLineScatterCandleBubbleChartDataSet
    {
        // MARK: - Data functions and accessors

        // MARK: - Styling functions and accessors

        /// space indicator between the bars in percentage of the whole width of one value (0.15 == 15% of bar width)
        nfloat barSpace { get; set; }

        /// - returns: true if this DataSet is stacked (stacksize > 1) or not.
        bool isStacked { get; }

        /// - returns: the maximum number of bars that can be stacked upon another in this DataSet.
        int stackSize { get; }

        /// the color used for drawing the bar-shadows. The bar shadows is a surface behind the bar that indicates the maximum value
        UIColor barShadowColor { get; set; }

        /// the alpha value (transparency) that is used for drawing the highlight indicator bar. min = 0.0 (fully transparent), max = 1.0 (fully opaque)
        nfloat highlightAlpha { get; set; }

        /// array of labels used to describe the different values of the stacked bars
        List<string> stackLabels { get; set; }
    }
}