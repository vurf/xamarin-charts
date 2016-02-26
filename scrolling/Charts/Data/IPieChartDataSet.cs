using System;

namespace scrolling
{
    public interface IPieChartDataSet : IChartDataSet
    {
        // MARK: - Data functions and accessors

        double yValueSum { get; }

        /// - returns: the average value across all entries in this DataSet.
        double average { get; }

        // MARK: - Styling functions and accessors

        /// the space in pixels between the pie-slices
        /// **default**: 0
        /// **maximum**: 20
        nfloat sliceSpace { get; set; }

        /// indicates the selection distance of a pie slice
        nfloat selectionShift { get; set; }
    }
}