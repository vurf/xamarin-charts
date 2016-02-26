using System;
using Foundation;

namespace scrolling
{
	public class ChartComponentBase : NSObject
	{
		public ChartComponentBase ()
		{
		}

		public bool enabled = true;

		public nfloat xOffset = 5.0f;

		public nfloat yOffset = 5.0f;

		public bool isEnabled {
			get {
				return enabled;
			}
		}
	}
}

