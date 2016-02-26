using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace scrolling
{
    class ChartDataBaseFilter : NSObject
    {
        
        public virtual List<ChartDataEntry> filter(List<ChartDataEntry> points)
        {
            Console.WriteLine("filter() cannot be called on ChartDataBaseFilter");
        }
    }
}