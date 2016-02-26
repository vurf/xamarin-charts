using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace scrolling
{
    class ChartDefaultXAxisValueFormatter : NSObject, ChartXAxisValueFormatter
    {
        public string stringForXValue(int index, string original, ChartViewPortHandler viewPortHandler)
        {
            return original; // just return original, no adjustments
        }
    }
}