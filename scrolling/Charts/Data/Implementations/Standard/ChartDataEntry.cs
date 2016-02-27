using System;
using Foundation;

namespace scrolling
{
    public class ChartDataEntry : NSObject
    {
        /// the actual value (y axis)
        public double _value = 0.0;

        public virtual double value 
        {
            get { return _value;  }
            set { _value = value; }
        }
        /// the index on the x-axis
        public int xIndex = 0;

        /// optional spot for additional data this Entry represents
        public object data;

        public ChartDataEntry() : base()
        {

        }

        public ChartDataEntry(double _value, int _xIndex)
        {
            value = _value;
            xIndex = _xIndex;
        }

        public ChartDataEntry(double _value, int _xIndex, object _data)
        {
            value = _value;
            xIndex = _xIndex;
            data = _data;
        }

        public override bool IsEqual(NSObject anObject)
        {
            if (anObject == null)
            {
                return false;
            }

            if (!anObject.IsKindOfClass(Class))
            {
                return false;
            }

            var current = anObject as ChartDataEntry;
            if (current == null)
            {
                return false;
            }
            else
            {
                if (current.data != data && !current.data.Equals(data))
                {
                    return false;
                }

                if (current.xIndex != xIndex)
                {
                    return false;
                }

                if (Math.Abs(current.value - value) > 0.00001)
                {
                    return false;
                }
                return true;
            }
        }

        // MARK: NSObject

        public override string Description
        {
            get { return string.Format("ChartDataEntry, xIndex: {0}, value {1}", xIndex, value); }
        }

        public static bool operator == (ChartDataEntry obj1, ChartDataEntry obj2)
        {
            if (obj1 == obj2)
            {
                return true;
            }
    
            if (!obj1.IsKindOfClass(obj2.Class))
            {
                return false;
            }
    
            if (obj1.data != obj2.data && !obj1.data.Equals(obj2.data))
            {
                return false;
            }
    
            if (obj1.xIndex != obj2.xIndex)
            {
                return false;
            }
    
            if (Math.Abs(obj1.value - obj2.value) > 0.00001)
            {
                return false;
            }

            return true;
        }

        public static bool operator !=(ChartDataEntry obj1, ChartDataEntry obj2)
        {
            return !(obj1 == obj2);
        }
    }
}