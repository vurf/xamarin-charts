using System;
using UIKit;

namespace scrolling
{
    public interface ICandleChartDataSet : ILineScatterCandleRadarChartDataSet
    {
        // MARK: - Data functions and accessors

        // MARK: - Styling functions and accessors

        /// the space that is left out on the left and right side of each candle,
        /// **default**: 0.1 (10%), max 0.45, min 0.0
        nfloat barSpace { get; set; }

        /// should the candle bars show?
        /// when false, only "ticks" will show
        ///
        /// **default**: true
        bool showCandleBar { get; set; }

        /// the width of the candle-shadow-line in pixels.
        ///
        /// **default**: 3.0
        nfloat shadowWidth { get; set; }

        /// the color of the shadow line
        UIColor shadowColor { get; set; }

        /// use candle color for the shadow
        bool shadowColorSameAsCandle { get; set; }

        /// Is the shadow color same as the candle color?
        bool isShadowColorSameAsCandle { get; }

        /// color for open == close
        UIColor neutralColor { get; set; }

        /// color for open > close
        UIColor increasingColor { get; set; }

        /// color for open < close
        UIColor decreasingColor { get; set; }

        /// Are increasing values drawn as filled?
        bool increasingFilled { get; set; }

        /// Are increasing values drawn as filled?
        bool isIncreasingFilled { get; }

        /// Are decreasing values drawn as filled?
        bool decreasingFilled { get; set; }

        /// Are decreasing values drawn as filled?
        bool isDecreasingFilled { get; }
    }
}