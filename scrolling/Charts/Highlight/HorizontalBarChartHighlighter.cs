using System;
using CoreGraphics;

namespace scrolling
{
	public class HorizontalBarChartHighlighter : BarChartHighlighter
	{
		public HorizontalBarChartHighlighter ()
		{
		}

		public override ChartHighlight getHighlight (double x, double y)
		{
			var h = base.getHighlight (x, y);
			if (h == null) {
				return h;
			} else {
				var setValue = chart.data.getDataSetByIndex (h.dataSetIndex) as BarChartDataSet;
				if (setValue != null) {
					if (setValue.isStacked) {
						var pt = new CGPoint ();
						pt.X = (nfloat)y;
						chart.getTransformer (setValue.axisDependency).pixelToValue (&pt);
						return getStackedHighlight (h, set, h.xIndex, h.dataSetIndex, (double)(pt.X));
					}
				}
				return h;
			}
		}

		public override int getXIndex (double x)
		{
			var barChartData = chart.data as BarChartData;
			if (barChartData != null) {
				if (!barChartData.isGrouped) {
					// create an array of the touch-point
					var pt = new CGPoint (0.0f, x);

					// take any transformer to determine the x-axis value
					chart.getTransformer (ChartYAxis.AxisDependency.Left).pixelToValue (&pt);

					return (int)(Math.Round (pt.Y));
				} else {
					var baseNoSpace = getBase (x);

					var setCount = barChartData.dataSetCount;
					var xIndex = (int)(baseNoSpace) / setCount;

					var valCount = barChartData.xValCount;

					if (xIndex < 0)
						xIndex = 0;
					else if (xIndex >= valCount)
						xIndex = valCount - 1;

					return xIndex;
				}
			} else {
				return 0;
			}
		}

		public override double getBase (double x)
		{
			var barChartData = chart.data as BarChartData;
			if (barChartData != null) {
				var pt = new CGPoint ();
				pt.Y = (nfloat)x;

					// take any transformer to determine the x-axis value
				chart.getTransformer(ChartYAxis.AxisDependency.Left).pixelToValue(&pt);
				var yVal = (double)pt.Y;

				var setCount = barChartData.dataSetCount ?? 0;

					// calculate how often the group-space appears
				var steps = (int)(yVal / ((double)(setCount) + (double)(barChartData.groupSpace)));

				var groupSpaceSum = (double) (barChartData.groupSpace) * (double) (steps);

				var baseNoSpace = yVal - groupSpaceSum;

				return baseNoSpace;
			} else {
				return 0.0d;
			}
		}

	}
}

