using System;
using Foundation;

namespace scrolling
{
	public class ChartRange : NSObject
	{
		public ChartRange ()
		{
		}

		public double from;
		public double to;

		public ChartRange(double _from, double _to)
		{
			from = _from;
			to = _to;
		}

		/// Returns true if this range contains (if the value is in between) the given value, false if not.
		/// - parameter value:
		public bool contains(double value)
		{
			if (value > from && value <= to)
				return true;
			else
				return false;
		}

		public bool isLarger(double value) {
			return value > to;
		}

		public bool isSmaller(double value)
		{
			return value < from
			}
		
	}
}

