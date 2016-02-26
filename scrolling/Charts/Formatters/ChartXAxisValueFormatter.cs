using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace scrolling
{
    public interface  ChartXAxisValueFormatter
    {
        string stringForXValue(int index, string original, ChartViewPortHandler viewPortHandler);
    }
}