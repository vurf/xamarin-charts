using System;
using System.Collections.Generic;
using UIKit;

namespace scrolling
{
    public interface ILineChartDataSet : ILineRadarChartDataSet
    {
        // MARK: - Data functions and accessors

        // MARK: - Styling functions and accessors

        /// Intensity for cubic lines (min = 0.05, max = 1)
        ///
        /// **default**: 0.2
        nfloat cubicIntensity { get; set; }

        /// If true, cubic lines are drawn instead of linear
        bool drawCubicEnabled { get; set; }

        /// - returns: true if drawing cubic lines is enabled, false if not.
        bool isDrawCubicEnabled { get; }

        /// The radius of the drawn circles.
        nfloat circleRadius { get; set; }

        List<UIColor> circleColors { get; set; }

        /// - returns: the color at the given index of the DataSet's circle-color array.
        /// Performs a IndexOutOfBounds check by modulus.
        UIColor getCircleColor(int index);

        /// Sets the one and ONLY color that should be used for this DataSet.
        /// Internally, this recreates the colors array and adds the specified color.
        void setCircleColor(UIColor color);

        /// Resets the circle-colors array and creates a new one
        void resetCircleColors(int index);

        /// If true, drawing circles is enabled
        bool drawCirclesEnabled { get; set; }

        /// - returns: true if drawing circles for this DataSet is enabled, false if not
        bool isDrawCirclesEnabled { get; }

        /// The color of the inner circle (the circle-hole).
        UIColor circleHoleColor { get; set; }

        /// True if drawing circles for this DataSet is enabled, false if not
        bool drawCircleHoleEnabled { get; set; }

        /// - returns: true if drawing the circle-holes is enabled, false if not.
        bool isDrawCircleHoleEnabled { get; }

        /// This is how much (in pixels) into the dash pattern are we starting from.
        nfloat lineDashPhase { get; }

        /// This is the actual dash pattern.
        /// I.e. [2, 3] will paint [--   --   ]
        /// [1, 3, 4, 2] will paint [-   ----  -   ----  ]
        List<nfloat> lineDashLengths { get; set; }

        /// Sets a custom FillFormatter to the chart that handles the position of the filled-line for each DataSet. Set this to null to use the default logic.
        ChartFillFormatter fillFormatter { get; set; }
    }
}