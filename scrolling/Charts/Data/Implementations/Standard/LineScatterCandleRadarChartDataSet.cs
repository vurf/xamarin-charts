using System.Collections.Generic;

namespace scrolling
{
    public class LineScatterCandleRadarChartDataSet : BarLineScatterCandleBubbleChartDataSet, ILineScatterCandleRadarChartDataSet
    {
        public bool drawHorizontalHighlightIndicatorEnabled { get; set; }
        public bool drawVerticalHighlightIndicatorEnabled { get; set; }

        public bool isHorizontalHighlightIndicatorEnabled
        {
            get { return drawHorizontalHighlightIndicatorEnabled; }
        }

        public bool isVerticalHighlightIndicatorEnabled
        {
            get { return drawVerticalHighlightIndicatorEnabled; }
        }

        public void setDrawHighlightIndicators(bool enabled)
        {
            drawHorizontalHighlightIndicatorEnabled = enabled;
            drawVerticalHighlightIndicatorEnabled = enabled;
        }

        public LineScatterCandleRadarChartDataSet()
        {
        }

        public LineScatterCandleRadarChartDataSet(List<ChartDataEntry> yVals, string label) : base(yVals, label)
        {
        }
    }
}