using System;
using CoreGraphics;
using UIKit;

namespace scrolling
{
    public interface IScatterChartDataSet : ILineScatterCandleRadarChartDataSet
    {
        // MARK: - Data functions and accessors

        // MARK: - Styling functions and accessors

        // The size the scatter shape will have
        nfloat scatterShapeSize { get; set; }

        // The type of shape that is set to be drawn where the values are at
        // - default: .Square
        ScatterChartDataSet.ScatterShape scatterShape { get; set; }

        // The radius of the hole in the shape (applies to Square, Circle and Triangle)
        // Set this to <= 0 to remove holes.
        // - default: 0.0
        nfloat scatterShapeHoleRadius { get; set; }

        // Color for the hole in the shape. Setting to `nil` will behave as transparent.
        // - default: nil
        UIColor scatterShapeHoleColor { get; set; }

        // Custom path object to draw where the values are at.
        // This is used when shape is set to Custom.
        CGPath customScatterShape { get; set; }
    }
}