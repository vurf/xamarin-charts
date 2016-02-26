using System;
using Foundation;
using CoreGraphics;
using System.Collections.Generic;

namespace scrolling
{
	public class ChartHighlighter : NSObject
	{
		public ChartHighlighter ()
		{
		}

		public BarLineChartViewBase chart;

		public ChartHighlighter(BarLineChartViewBase _chart)
		{
			chart = _chart;
		}

		/// Returns a Highlight object corresponding to the given x- and y- touch positions in pixels.
		/// - parameter x:
		/// - parameter y:
		/// - returns:
		public virtual ChartHighlight getHighlight(double x, double y)
		{
			var xIndex = getXIndex (x);
			if (xIndex == -int.MaxValue)
				return null;

			var dataSetIndex = getDataSetIndex (xIndex, x, y);
			if (dataSetIndex == -int.MaxValue)
				return null;

			return new ChartHighlight (xIndex, dataSetIndex);
		}

		/// Returns the corresponding x-index for a given touch-position in pixels.
		/// - parameter x:
		/// - returns:
		public virtual int getXIndex(double x)
		{
			// create an array of the touch-point
			var pt = new CGPoint(x, 0.0f);

				// take any transformer to determine the x-axis value
			chart.getTransformer(ChartYAxis.AxisDependency.Left).pixelToValue(&pt);

			return (int) (Math.Round (pt.X));
		}

		/// Returns the corresponding dataset-index for a given xIndex and xy-touch position in pixels.
		/// - parameter xIndex:
		/// - parameter x:
		/// - parameter y:
		/// - returns:
		public virtual int getDataSetIndex(int xIndex, double x, double y)
		{
			var valsAtIndex = getSelectionDetailsAtIndex (xIndex);

			var leftdist = ChartUtils.getMinimumDistance (valsAtIndex, val: y, axis: ChartYAxis.AxisDependency.Left);
			var rightdist = ChartUtils.getMinimumDistance (valsAtIndex, val: y, axis: ChartYAxis.AxisDependency.Right);

			var axis = leftdist < rightdist ? ChartYAxis.AxisDependency.Left : ChartYAxis.AxisDependency.Right;

			var dataSetIndex = ChartUtils.closestDataSetIndex (valsAtIndex, value: y, axis: axis);

			return dataSetIndex;
		}

		/// Returns a list of SelectionDetail object corresponding to the given xIndex.
		/// - parameter xIndex:
		/// - returns:
		public virtual List<ChartSelectionDetail> getSelectionDetailsAtIndex(int xIndex)
		{
			var vals = new List<ChartSelectionDetail> ();
			var pt = new CGPoint ();

			for (var i = 0, dataSetCount = chart?.data?.dataSetCount; i < dataSetCount; i++)
			{
				var dataSet = chart.data.getDataSetByIndex(i);

					// dont include datasets that cannot be highlighted
				if (!dataSet.isHighlightEnabled)
					continue;

						// extract all y-values from all DataSets at the given x-index
				var yVal = dataSet.yValForXIndex(xIndex);

				if (nfloat.IsNaN (yVal))
					continue;

				pt.Y = (nfloat)yVal;

				chart.getTransformer (dataSet.axisDependency).pointValueToPixel (&pt);

				if (!nfloat.IsNaN (pt.Y)) 
					vals.Add(new ChartSelectionDetail((double)pt.Y, i, dataSet));
			}

			return vals;
		}
		
	}
}

