using System;
using Foundation;

namespace scrolling
{
	public class ChartHighlight : NSObject
	{
		public ChartHighlight ()
		{
		}

		/// the x-index of the highlighted value
		private int _xIndex = 0;

			/// the index of the dataset the highlighted value is in
		private int _dataSetIndex = 0;

			/// index which value of a stacked bar entry is highlighted
			/// 
			/// **default**: -1
		private int _stackIndex = -1;

			/// the range of the bar that is selected (only for stacked-barchart)
		private ChartRange _range;



		public ChartHighlight(int x, int dataSetIndex)
		{
			_xIndex = x;
			_dataSetIndex = dataSetIndex;
		}

		public ChartHighlight(int x, int dataSetIndex, int stackIndex)
		{
			_xIndex = x;
			_dataSetIndex = dataSetIndex;
			_stackIndex = stackIndex;
		}

			/// Constructor, only used for stacked-barchart.
			///
			/// - parameter x: the index of the highlighted value on the x-axis
			/// - parameter dataSet: the index of the DataSet the highlighted value belongs to
			/// - parameter stackIndex: references which value of a stacked-bar entry has been selected
			/// - parameter range: the range the selected stack-value is in
		public ChartHighlight(int x, int dataSetIndex,int stackIndex,ChartRange range) : this (x, dataSetIndex, stackIndex)
		{
			
			_range = range;
		}

		public int dataSetIndex {
			get { 
				return _dataSetIndex; 
			}
		}

		public int xIndex {
			get { 
				return _xIndex; 
			}
		}

		public int stackIndex {
			get { 
				return _stackIndex;
			}
		}

			/// - returns: the range of values the selected value of a stacked bar is in. (this is only relevant for stacked-barchart)
		public ChartRange range {
			get { 
				return _range;
			}
		}

			// MARK: NSObject
		public override string Description {
			get {
				return string.Format ("Highlight, xIndex: {0}, dataSetIndex: {1}, stackIndex (only stacked barentry): {2}", 
					xIndex, _dataSetIndex, _stackIndex);
			}
		}

		public override bool IsEqual (NSObject anObject)
		{
			if (anObject == null)
				return false;

			if (!anObject.IsKindOfClass (this.Class))
				return false;

			var any = anObject as ChartHighlight;
			if (any != null) {
				if (any.xIndex != _xIndex)
					return false;

				if (any.dataSetIndex != _dataSetIndex)
					return false;

				if (any.stackIndex != _stackIndex)
					return false;
			}
			return true;
		}

		public static bool operator ==(ChartHighlight lhs, ChartHighlight rhs) {
			if (lhs == rhs)
				return true;

			if (!lhs.IsKindOfClass (rhs.Class))
				return false;

			if (lhs._xIndex != rhs._xIndex)
				return false;

			if (lhs._dataSetIndex != rhs._dataSetIndex)
				return false;

			if (lhs._stackIndex != rhs._stackIndex)
				return false;

			return true;
		}
	}
}

