using System;
using System.Collections.Generic;
using CoreGraphics;

namespace scrolling
{
	public class BarChartHighlighter : ChartHighlighter
	{
		public BarChartHighlighter ()
		{
		}

		public override ChartHighlight getHighlight (double x, double y)
		{
			var h = base.getHighlight (x, y);
			if (h == null)
				return h;
			else {
				var set1 = chart.data.getDataSetByIndex (h.dataSetIndex) as BarChartHighlighter;
				if (set1 != null) {
					if (set1.isStacked) {
						
					}
				}
			}
		}

		public override int getXIndex (double x)
		{
			
			var barChartData = chart.data as BarChartData;
			if (barChartData != null) {
				
				if (!barChartData.isGrouped)
					return base.getXIndex (x);
				else
				{
					var baseNoSpace = getBase (x);

					var setCount = barChartData.dataSetCount;
					var xIndex = (int)baseNoSpace / setCount;

					var valCount = barChartData.xValCount;
					if (xIndex < 0)
						xIndex = 0;
					else if (xIndex >= valCount) 
						xIndex = valCount - 1;
					
					return xIndex;
				}
			}
			else
				return 0;

		}

		public override int getDataSetIndex (int xIndex, double x, double y)
		{
			var barChartData = chart.data as BarChartData;
			if (barChartData != null) {
				if (!barChartData.isGrouped)
					return 0;
				else
				{
					var baseNoSpace = getBase (x);

					var setCount = barChartData.dataSetCount;
					var dataSetIndex = (int)baseNoSpace % setCount;

					if (dataSetIndex < 0)
						dataSetIndex = 0;
					else if (dataSetIndex >= setCount)
						dataSetIndex = setCount - 1;

					return dataSetIndex;
				}
			} else {
				return 0;
			}
		}


		/// This method creates the Highlight object that also indicates which value of a stacked BarEntry has been selected.
		/// - parameter old: the old highlight object before looking for stacked values
		/// - parameter set:
		/// - parameter xIndex:
		/// - parameter dataSetIndex:
		/// - parameter yValue:
		/// - returns:
		public ChartHighlight getStackedHighlight(ChartHighlight old, BarChartDataSet setValue , int xIndex, int dataSetIndex, double yValue)
		{
			var entry = setValue.entryForXIndex(xIndex) as BarChartDataEntry;

			if (entry.values == null)
				return old;

			var ranges = getRanges (entry);
			if (ranges != null) {
				var stackIndex = getClosestStackIndex (ranges, yValue);
				var h = new ChartHighlight (xIndex, dataSetIndex, stackIndex, ranges [stackIndex]);
				return h;
			}
			return null;
		}
		
	

		/// Returns the index of the closest value inside the values array / ranges (stacked barchart) to the value given as a parameter.
		/// - parameter entry:
		/// - parameter value:
		/// - returns:
		public int getClosestStackIndex(List<ChartRange> ranges, double value)
		{
			if (ranges == null)
				return 0;

			var stackIndex = 0;
			foreach (var range in ranges) {
				if (range.Contains (value))
					return stackIndex;
				else
					stackIndex++;
			}

			var length = Math.Max (ranges.Count - 1, 0);

			return (value > ranges[length].to) ? length : 0;
		}

		/// Returns the base x-value to the corresponding x-touch value in pixels.
		/// - parameter x:
		/// - returns:
		public virtual double getBase(double x)
		{
			var barChartData = chart.data as BarChartData;
			if (barChartData != null)
			{
				// create an array of the touch-point
				var pt = new CGPoint();
				pt.X = (nfloat)x;

					// take any transformer to determine the x-axis value
				chart.getTransformer(ChartYAxis.AxisDependency.Left).pixelToValue(&pt);
				var xVal = (double)pt.X;

				var setCount = barChartData.dataSetCount ?? 0;

					// calculate how often the group-space appears
				var steps = (int)(xVal / ((double)(setCount) + (double)(barChartData.groupSpace)));

				var groupSpaceSum = (double)(barChartData.groupSpace) * (double) (steps);

				var baseNoSpace = xVal - groupSpaceSum;

				return baseNoSpace;
			} else {
				return 0.0d;
			}
		}

		/// Splits up the stack-values of the given bar-entry into Range objects.
		/// - parameter entry:
		/// - returns:
		public List<ChartRange> getRanges(BarChartDataEntry entry)
		{
			var values = entry.values;
			if (values == null) {
				return null;
			}

			var negRemain = -entry.negativeSum;
			var posRemain = 0.0d;

			var ranges = new List<ChartRange> ();
			ranges.reserveCapacity(values.Count);

			for (var i = 0, count = values.Count; i < count; i++)
			{
				var value = values [i];

				if (value < 0) {
					ranges.Add (new ChartRange (negRemain, negRemain + Math.Abs(value)));
					negRemain += Math.Abs (value);
				} else {
					ranges.Add (new ChartRange (posRemain, posRemain + value));
					posRemain += value;
				}
			}

			return ranges;
		}
		

	}
}

