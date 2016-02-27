using System;
using System.Collections.Generic;
using UIKit;

namespace scrolling
{
    public class BarLineScatterCandleBubbleChartDataSet : ChartDataSet, IBarLineScatterCandleBubbleChartDataSet
    {
         // MARK: - Data functions and accessors

        public BarLineScatterCandleBubbleChartDataSet()
        {
            
        }

        public BarLineScatterCandleBubbleChartDataSet(List<ChartDataEntry> yVals, string label) : base(yVals, label)
        {
        }

        // MARK: - Styling functions and accessors

        UIColor _highlightColor = UIColor.FromRGB(255.0f/255.0f, 187.0f/255.0f, 115.0f/255.0f);
        nfloat _highlightLineWidth = 0.5f;
        nfloat _highlightLineDashPhase = 0.0f;
        List<nfloat> _highlightLineDashLengths;

        public UIColor highlightColor
        {
            get { return _highlightColor; }
            set { _highlightColor = value; }
        }

        public nfloat highlightLineWidth
        {
            get { return _highlightLineWidth; }
            set { _highlightLineWidth = value; }
        }

        public nfloat highlightLineDashPhase
        {
            get { return _highlightLineDashPhase; }
            set { _highlightLineDashPhase = value; }
        }

        public List<nfloat> highlightLineDashLengths
        {
            get { return _highlightLineDashLengths; }
            set { _highlightLineDashLengths = value; }
        }
    }
}