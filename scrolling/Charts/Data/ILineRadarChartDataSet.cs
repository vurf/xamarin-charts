using System;
using UIKit;

namespace scrolling
{
    public interface ILineRadarChartDataSet : ILineScatterCandleRadarChartDataSet
    {
        // MARK: - Data functions and accessors

        // MARK: - Styling functions and accessors

        /// The color that is used for filling the line surface area.
        UIColor fillColor { get; set; }

        /// Returns the object that is used for filling the area below the line.
        /// - default: nil
        ChartFill fill { get; set; }

        /// The alpha value that is used for filling the line surface.
        /// - default: 0.33
        nfloat fillAlpha { get; set; }

        /// line width of the chart (min = 0.2, max = 10)
        ///
        /// **default**: 1
        nfloat lineWidth { get; set; }

        bool drawFilledEnabled { get; set; }

        bool isDrawFilledEnabled { get; }
    }
}