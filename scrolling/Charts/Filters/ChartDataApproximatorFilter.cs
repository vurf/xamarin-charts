using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace scrolling
{
    class ChartDataApproximatorFilter : ChartDataBaseFilter
    {
        public enum ApproximatorType
        {
            None,
            RamerDouglasPeucker
        }

        /// the type of filtering algorithm to use
        public ApproximatorType type = ApproximatorType.None;

        /// the tolerance to be filtered with
        /// When using the Douglas-Peucker-Algorithm, the tolerance is an angle in degrees, that will trigger the filtering
        public double tolerance = 0;

        public double scaleRatio = 1;
        public double deltaRatio = 1;

        public ChartDataApproximatorFilter(ApproximatorType type, double tolerance)
        {
            this.type = type;
            this.tolerance = tolerance;
        }



        /// Sets the ratios for x- and y-axis, as well as the ratio of the scale levels
        public void setRatios(double deltaRatio, double scaleRatio)
        {
            this.deltaRatio = deltaRatio;
            this.scaleRatio = scaleRatio;
        }

        /// Filters according to type. Uses the pre set set tolerance
        ///
        /// - parameter points: the points to filter
        public override List<ChartDataEntry> filter(List<ChartDataEntry> points)
        {
            return filter(points, tolerance);
        }

        /// Filters according to type.
        ///
        /// - parameter points: the points to filter
        /// - parameter tolerance: the angle in degrees that will trigger the filtering
        public List<ChartDataEntry> filter(List<ChartDataEntry> points, double tolerance)
        {
            if (tolerance <= 0)
            {
                return points;
            }

            switch (type)
            {
                case ApproximatorType.RamerDouglasPeucker:
                    return reduceWithDouglasPeuker(points, tolerance);
                case ApproximatorType.None:
                    return points;
            }
        }

        /// uses the douglas peuker algorithm to reduce the given arraylist of entries
        private List<ChartDataEntry> reduceWithDouglasPeuker(List<ChartDataEntry> entries, double epsilon)
        {
            // if a shape has 2 or less points it cannot be reduced
            if (epsilon <= 0 || entries.Count < 3)
            {
                return entries;
            }

            var keep = new List<bool>(entries.Count);

            // first and last always stay
            keep[0] = true;
            keep[entries.Count - 1] = true;

            // first and last entry are entry point to recursion
            algorithmDouglasPeucker(entries, epsilon, 0, entries.Count - 1, &keep);

            // create a new array with series, only take the kept ones
            var reducedEntries = new List<ChartDataEntry>();
            for (var i = 0; i < entries.Count; i++)
            {
                if (keep[i])
                {
                    var curEntry = entries[i];
                    reducedEntries.Add(new ChartDataEntry(curEntry.value, curEntry.xIndex));
                }
            }
            return reducedEntries;
        }

        /// apply the Douglas-Peucker-Reduction to an ArrayList of Entry with a given epsilon (tolerance)
        ///
        /// - parameter entries:
        /// - parameter epsilon: as y-value
        /// - parameter start:
        /// - parameter end:
        private void algorithmDouglasPeucker(List<ChartDataEntry> entries, double epsilon, int start, int end,
            List<bool> keep)
        {
            if (end <= start + 1)
            {
                // recursion finished
                return;
            }

            // find the greatest distance between start and endpoint
            var maxDistIndex = 0;
            double distMax = 0;

            var firstEntry = entries[start];
            var lastEntry = entries[end];

            for (var i = start + 1; i < end; i++)
            {
                var dist = calcAngleBetweenLines(firstEntry, lastEntry, firstEntry, entries[i]);

                // keep the point with the greatest distance
                if (dist > distMax)
                {
                    distMax = dist;
                    maxDistIndex = i;
                }
            }

            if (distMax > epsilon)
            {
                // keep max dist point
                keep[maxDistIndex] = true;

                // recursive call
                algorithmDouglasPeucker(entries, epsilon, start, maxDistIndex, &keep);
                algorithmDouglasPeucker(entries, epsilon, maxDistIndex, end, &keep);
            } // else don't keep the point...
        }

        /// calculate the distance between a line between two entries and an entry (point)
        ///
        /// - parameter startEntry: line startpoint
        /// - parameter endEntry: line endpoint
        /// - parameter entryPoint: the point to which the distance is measured from the line
        private double calcPointToLineDistance(ChartDataEntry startEntry, ChartDataEntry endEntry,
            ChartDataEntry entryPoint)
        {
            var xDiffEndStart = (double) (endEntry.xIndex) - (double) (startEntry.xIndex);
            var xDiffEntryStart = (double) (entryPoint.xIndex) - (double) (startEntry.xIndex);

            var normalLength = Math.Sqrt((xDiffEndStart)
                                         *(xDiffEndStart)
                                         + (endEntry.value - startEntry.value)
                                         *(endEntry.value - startEntry.value));

            return Math.Abs((xDiffEntryStart)
                            *(endEntry.value - startEntry.value)
                            - (entryPoint.value - startEntry.value)
                            *(xDiffEndStart))/normalLength;
        }

        /// Calculates the angle between two given lines. The provided entries mark the starting and end points of the lines.
        private double calcAngleBetweenLines(ChartDataEntry start1, ChartDataEntry end1, ChartDataEntry start2,
            ChartDataEntry end2)
        {
            var angle1 = calcAngleWithRatios(start1, end1);
            var angle2 = calcAngleWithRatios(start2, end2);

            return Math.Abs(angle1 - angle2);
        }

        /// calculates the angle between two entries (points) in the chart taking ratios into consideration
        private double calcAngleWithRatios(ChartDataEntry p1, ChartDataEntry p2)
        {
            var dx = (double) (p2.xIndex)*(double) (deltaRatio) - (double) (p1.xIndex)*(double) (deltaRatio);
            var dy = p2.value*scaleRatio - p1.value*scaleRatio;
            return Math.Atan2(dy, dx)*ChartUtils.Math2.RAD2DEG;
        }

        // calculates the angle between two entries (points) in the chart
        private double calcAngle(ChartDataEntry p1, ChartDataEntry p2)
        {
            var dx = p2.xIndex - p1.xIndex;
            var dy = p2.value - p1.value;
            return Math.Atan2(dy, dx)*ChartUtils.Math2.RAD2DEG;
        }
    }
}