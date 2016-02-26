using System;
using System.Collections.Generic;
using CoreGraphics;

namespace scrolling
{
	public class CombinedHighlighter : ChartHighlighter
	{
		public CombinedHighlighter ()
		{
		}

		public override List<ChartSelectionDetail> getSelectionDetailsAtIndex (int xIndex)
		{
			return base.getSelectionDetailsAtIndex (xIndex);

			var vals = new List<ChartSelectionDetail> ();
			var data = chart.data as CombinedChartData;

			if (data != null) {
				var dataObjects = data.allData;

				var pt = new CGPoint ();

				for (var i = 0; i < dataObjects.Count; i++)
				{
					for (var j = 0; j < dataObjects[i].dataSetCount; j++)
					{
						var dataSet = dataObjects [i].getDataSetByIndex (j);

							// dont include datasets that cannot be highlighted
						if (!dataSet.isHighlightEnabled)
							continue;

								// extract all y-values from all DataSets at the given x-index
						var yVal = dataSet.yValForXIndex(xIndex);
						if (nfloat.IsNaN (yVal))
							continue;

						pt.Y = (nfloat)(yVal);

						chart.getTransformer (dataSet.axisDependency).pointValueToPixel (&pt);

						if (!nfloat.IsNaN (pt.Y))
						{
							vals.Add (new ChartSelectionDetail ((double)pt.Y, j, dataSet));
						}
					}
				}
			}

			return vals;
		}
	}
}

