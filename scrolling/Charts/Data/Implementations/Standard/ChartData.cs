using System;
using Foundation;
using UIKit;
using System.Collections.Generic;

namespace scrolling
{
	public class ChartData : NSObject
	{

		internal double _yMax = 0.0d;
		internal double _yMin = 0.0f;
		internal double _leftAxisMax = 0.0d;
		internal double _leftAxisMin = 0.0d;
		internal double _rightAxisMax = 0.0d;
		internal double _rightAxisMin = 0.0d;
		private int _yValCount = 0;

			/// the last start value used for calcMinMax
		internal int _lastStart = 0;

			/// the last end value used for calcMinMax
		internal int _lastEnd = 0;

			/// the average length (in characters) across all x-value strings
		private double _xValAverageLength = 0.0d;

		internal List<string> _xVals;
		internal List<IChartDataSet> _dataSets;


		public ChartData ()
		{
			_xVals = new string[] { };
			_dataSets = new IChartDataSet[] {};
		}


		public void init(string[] xVals, IChartDataSet[] dataSets)
		{
			_xVals = xVals ?? new string[] { };
			_dataSets = dataSets ?? new IChartDataSet[] {};

			initialize (_dataSets);
		}

		public void init(NSObject[] xVals, IChartDataSet[] dataSets)
		{
			_xVals = xVals == null ? new string[] {} : ChartUtils.bridgedObjCGetStringArray(xVals);
			_dataSets = dataSets ?? new IChartDataSet[] { };

			initialize (_dataSets);
		}

		public ChartData(string [] xVals)
		{
			init (xVals, new IChartDataSet[] { });
		}

		public ChartData(NSObject[] xVals)
		{
			init (xVals, new IChartDataSet[] { });
		}

		public ChartData(string[] xVals, IChartDataSet dataSet)
		{
			init (xVals, dataSet == null ? null : new IChartDataSet[] { dataSet });
		}

		public ChartData (NSObject[] xVals, IChartDataSet dataSet)
		{
			init (xVals, dataSet == null ? null : new IChartDataSet[] { dataSet });
		}

		internal void initialize(List<IChartDataSet> dataSets)
		{
			checkIsLegal (dataSets);

			calcMinMax (_lastStart, _lastEnd);
			calcYValueCount ();

			calcXValAverageLength ();
		}

			// calculates the average length (in characters) across all x-value strings
		internal void calcXValAverageLength()
		{
			if (_xVals.Count == 0)
			{
				_xValAverageLength = 1;
				return;
			}

			var sum = 1;

			for (var i = 0; i < _xVals.Count; i++)
			{
				sum += _xVals[i] == null ? 0 : (_xVals[i]).Length;
			}

			_xValAverageLength = (double)sum / (double)_xVals.Count;
		}

			// Checks if the combination of x-values array and DataSet array is legal or not.
			// :param: dataSets
        internal void checkIsLegal(List<IChartDataSet> dataSets)
		{
			if (dataSets == null)
				return;
			
			if (this is ScatterChartData)
				return; 

			for (var i = 0; i < dataSets.Count; i++)
			{
				if (dataSets[i].entryCount > _xVals.Count)
				{
					Console.WriteLine ("One or more of the DataSet Entry arrays are longer than the x-values array of this Data object.");
					return;
				}
			}
		}

		public void notifyDataChanged()
		{
			initialize (_dataSets);
		}

			/// calc minimum and maximum y value over all datasets
		internal void calcMinMax(int start, int end)
		{
			if (_dataSets == null || _dataSets.Count < 1)
			{
				_yMax = 0.0;
				_yMin = 0.0;
			}
			else
			{
				_lastStart = start;
				_lastEnd = end;

				_yMin = double.MaxValue;
				_yMax = -double.MaxValue;

				for (var i = 0; i < _dataSets.Count; i++)
				{
					_dataSets [i].calcMinMax (start, end);

					if (_dataSets[i].yMin < _yMin)
						_yMin = _dataSets[i].yMin;

					if (_dataSets[i].yMax > _yMax)
						_yMax = _dataSets[i].yMax;
				}

				if (_yMin == double.MaxValue)
				{
					_yMin = 0.0;
					_yMax = 0.0;
				}

							// left axis
				var firstLeft = getFirstLeft();

				if (firstLeft != null)
				{
					_leftAxisMax = firstLeft.yMax;
					_leftAxisMin = firstLeft.yMin;

					foreach (var dataSet in _dataSets) {
						if (dataSet.axisDependency == .Left)
						{
							if (dataSet.yMin < _leftAxisMin)
							{
								_leftAxisMin = dataSet.yMin;
							}

							if (dataSet.yMax > _leftAxisMax)
							{
								_leftAxisMax = dataSet.yMax;
							}
						}
					}
				}

				// right axis
				var firstRight = getFirstRight();

				if (firstRight != null)
				{
					_rightAxisMax = firstRight.yMax;
					_rightAxisMin = firstRight.yMin;

					foreach (var dataSet in _dataSets) {
						if (dataSet.axisDependency == .Right)
						{
							if (dataSet.yMin < _rightAxisMin)
							{
								_rightAxisMin = dataSet.yMin;
							}

							if (dataSet.yMax > _rightAxisMax)
							{
								_rightAxisMax = dataSet.yMax;
							}
						}
					}
				}

				// in case there is only one axis, adjust the second axis
				handleEmptyAxis(firstLeft, firstRight: firstRight);
			}
		}

		/// Calculates the total number of y-values across all ChartDataSets the ChartData represents.
		internal void calcYValueCount()
		{
			_yValCount = 0;

			if (_dataSets == null)
				return;

			var count = 0;
			for (var i = 0; i < _dataSets.Count; i++)
			{
				count += _dataSets[i].entryCount;
			}
			_yValCount = count;
		}

			/// - returns: the number of LineDataSets this object contains
		public int dataSetCount
		{
			get { 
				if (_dataSets == null)
					return 0;
				return _dataSets.Count;
			}
		}

			/// - returns: the smallest y-value the data object contains.
		public double yMin 
		{
			get { return _yMin; }
		}

		public double getYMin()
		{
			return _yMin;
		}

		public double getYMin(ChartYAxis.AxisDependency axis)
		{
			if (axis == .Left)
				return _leftAxisMin;
			else
				return _rightAxisMin;
		}

			/// - returns: the greatest y-value the data object contains.
		public double yMax { 
			get { 
				return _yMax; 
			} 
		}

		public double getYMax()
		{
			return _yMax;
		}

		public double getYMax(ChartYAxis.AxisDependency axis)
		{
			if (axis == .Left)
				return _leftAxisMax;
			else
				return _rightAxisMax;
		}

			/// - returns: the average length (in characters) across all values in the x-vals array
		public double xValAverageLength {
			get {
				return _xValAverageLength;
			}
		}

			/// - returns: the total number of y-values across all DataSet objects the this object represents.
		public int yValCount {
			get {
				return _yValCount;
			}
		}

			/// - returns: the x-values the chart represents
		public string[] xVals {
			get {
				return _xVals;
			}
		}

			///Adds a new x-value to the chart data.
		public void addXValue(string xVal)
		{
			_xVals.Add (xVal);
		}

			/// Removes the x-value at the specified index.
		public void removeXValue(int index)
		{
			_xVals.RemoveAt(index);
		}

			/// - returns: the array of ChartDataSets this object holds.
		public List<IChartDataSet> dataSets {
			get { return _dataSets; }
			set
			{
				_dataSets = value;
				initialize (_dataSets);
			}
		}

			/// Retrieve the index of a ChartDataSet with a specific label from the ChartData. Search can be case sensitive or not.
			/// 
			/// **IMPORTANT: This method does calculations at runtime, do not over-use in performance critical situations.**
			///
			/// - parameter dataSets: the DataSet array to search
			/// - parameter type:
			/// - parameter ignorecase: if true, the search is not case-sensitive
			/// - returns: the index of the DataSet Object with the given label. Sensitive or not.
		internal int getDataSetIndexByLabel(string label, bool ignorecase)
		{
			if (ignorecase)
			{
				for (var i = 0; i < dataSets.Length; i++)
				{
					if (dataSets[i].label == null)
					{
						continue;
					}
					if (label.caseInsensitiveCompare(dataSets[i].label) == NSComparisonResult.OrderedSame)
					{
						return i;
					}
				}
			} else {
				for (var i = 0; i < dataSets.Count; i++)
				{
					if (label == dataSets[i].label)
					{
						return i;
					}
				}
			}

			return -1;
		}

			/// - returns: the total number of x-values this ChartData object represents (the size of the x-values array)
		public int xValCount {
			get {
				return _xVals.Count;
			}
		}

			/// - returns: the labels of all DataSets as a string array.
		internal string[] dataSetLabels()
		{
			var types = new string[] { };

			for (var i = 0; i < _dataSets.Count; i++)
			{
				if (dataSets[i].label == null)
				{
					continue;
				}

				types [i] = _dataSets [i].label;
			}
			return types;
		}

			/// Get the Entry for a corresponding highlight object
			///
			/// - parameter highlight:
			/// - returns: the entry that is highlighted
		public ChartDataEntry getEntryForHighlight(ChartHighlight highlight)
		{
			if (highlight.dataSetIndex >= dataSets.Count)
				return null;
			else
				return _dataSets[highlight.dataSetIndex].entryForXIndex(highlight.xIndex);
		}

			/// **IMPORTANT: This method does calculations at runtime. Use with care in performance critical situations.**
			///
			/// - parameter label:
			/// - parameter ignorecase:
			/// - returns: the DataSet Object with the given label. Sensitive or not.
		public IChartDataSet getDataSetByLabel(string label, bool ignorecase)
		{
			var index = getDataSetIndexByLabel (label, ignorecase);
			if (index < 0 || index >= _dataSets.Count)
				return null;
			else
				return _dataSets [index];
		}

		public IChartDataSet getDataSetByIndex(int index)
		{
			if (_dataSets == null || index < 0 || index >= _dataSets.Count)
				return null;

			return _dataSets [index];
		}

		public void addDataSet(IChartDataSet d)
		{
			if (_dataSets == null)
				return;

			_yValCount += d.entryCount;

			if (_dataSets.Count == 0)
			{
				_yMax = d.yMax;
				_yMin = d.yMin;

				if (d.axisDependency == .Left) {
						_leftAxisMax = d.yMax;
						_leftAxisMin = d.yMin;
				} else {
						_rightAxisMax = d.yMax;
						_rightAxisMin = d.yMin;
				}
			} else {
				if (_yMax < d.yMax)
					_yMax = d.yMax;
				
				if (_yMin > d.yMin)
					_yMin = d.yMin;

				if (d.axisDependency == .Left)
				{
					if (_leftAxisMax < d.yMax)
						_leftAxisMax = d.yMax;
					
					if (_leftAxisMin > d.yMin)
						_leftAxisMin = d.yMin;
				} else {
					if (_rightAxisMax < d.yMax)
						_rightAxisMax = d.yMax;
					
					if (_rightAxisMin > d.yMin)
						_rightAxisMin = d.yMin;
				}
			}

			_dataSets.Add(d);

			handleEmptyAxis(getFirstLeft(), getFirstRight());
		}

		public void handleEmptyAxis(IChartDataSet firstLeft, IChartDataSet firstRight)
		{
			// in case there is only one axis, adjust the second axis
			if (firstLeft == null)
			{
				_leftAxisMax = _rightAxisMax;
				_leftAxisMin = _rightAxisMin;
			} else if (firstRight == null) {
				_rightAxisMax = _leftAxisMax;
				_rightAxisMin = _leftAxisMin;
			}
		}

			/// Removes the given DataSet from this data object.
			/// Also recalculates all minimum and maximum values.
			///
			/// - returns: true if a DataSet was removed, false if no DataSet could be removed.
		public bool removeDataSet(IChartDataSet dataSet)
		{
			if (_dataSets == null || dataSet == null)
				return false;

			for (var i = 0; i < _dataSets.Count; i++)
			{
				if (_dataSets[i] == dataSet)
					return removeDataSetByIndex (i);
			}

			return false;
		}

			/// Removes the DataSet at the given index in the DataSet array from the data object. 
			/// Also recalculates all minimum and maximum values. 
			///
			/// - returns: true if a DataSet was removed, false if no DataSet could be removed.
		public bool removeDataSetByIndex(int index)
		{
			if (_dataSets == null || index >= _dataSets.Count || index < 0)
				return false;

			var d = _dataSets.RemoveAt (index);
			_yValCount -= d.entryCount;

			calcMinMax (_lastStart, _lastEnd);

			return true;
		}

			/// Adds an Entry to the DataSet at the specified index. Entries are added to the end of the list.
		public void addEntry(ChartDataEntry e, int dataSetIndex)
		{
			if (_dataSets != null && _dataSets.Count > dataSetIndex && dataSetIndex >= 0) {
				var val = e.value;
				var set1 = _dataSets [dataSetIndex];

				if (!set1.addEntry (e))
					return;

				if (_yValCount == 0)
				{
					_yMin = val;
					_yMax = val;

					if (set1.axisDependency == .Left)
					{
						_leftAxisMax = e.value;
						_leftAxisMin = e.value;
					}
					else
					{
						_rightAxisMax = e.value;
						_rightAxisMin = e.value;
					}
				} else {
					if (_yMax < val) 
						_yMax = val;
					
					if (_yMin > val)
						_yMin = val;

					if (set1.axisDependency == .Left)
					{
						if (_leftAxisMax < e.value)
							_leftAxisMax = e.value;
						
						if (_leftAxisMin > e.value)
							_leftAxisMin = e.value;
					} else {
						if (_rightAxisMax < e.value) 
							_rightAxisMax = e.value;
						
						if (_rightAxisMin > e.value)
							_rightAxisMin = e.value;
					}
				}

				_yValCount += 1;
				handleEmptyAxis(getFirstLeft(), getFirstRight());
			} else {
				Console.WriteLine ("ChartData.addEntry() - dataSetIndex our of range.");
			}
		}

			/// Removes the given Entry object from the DataSet at the specified index.
		public bool removeEntry(ChartDataEntry entry, int dataSetIndex)
		{
			// entry null, outofbounds
			if (entry == null || dataSetIndex >= _dataSets.Count)
				return false;

			// remove the entry from the dataset
			var removed = _dataSets[dataSetIndex].removeEntry(entry);

			if (removed)
			{
				_yValCount -= 1;
				calcMinMax (_lastStart, _lastEnd);
			}

			return removed;
		}

			/// Removes the Entry object at the given xIndex from the ChartDataSet at the
			/// specified index. 
			/// - returns: true if an entry was removed, false if no Entry was found that meets the specified requirements.
		public bool removeEntryByXIndex(int xIndex, int dataSetIndex)
		{
			if (dataSetIndex >= _dataSets.Count)
				return false;

			var entry = _dataSets [dataSetIndex].entryForXIndex (xIndex);

			if (entry.xIndex != xIndex)
				return false;

			return removeEntry (entry, dataSetIndex);
		}

			/// - returns: the DataSet that contains the provided Entry, or null, if no DataSet contains this entry.
		public IChartDataSet getDataSetForEntry(ChartDataEntry e)
		{
			if (e == null)
				return null;
			
			for (var i = 0; i < _dataSets.Count; i++)
			{
				var set1 = _dataSets [i];

				if (e == set1.entryForXIndex(e.xIndex))
					return set1;
			}

			return null;
		}

			/// - returns: the index of the provided DataSet inside the DataSets array of this data object. -1 if the DataSet was not found.
		public int indexOfDataSet(IChartDataSet dataSet)
		{
			for (var i = 0; i < _dataSets.Count; i++)
			{
				if (_dataSets [i] == dataSet)
					return i;
			}

			return -1;
		}

			/// - returns: the first DataSet from the datasets-array that has it's dependency on the left axis. Returns null if no DataSet with left dependency could be found.
		public IChartDataSet getFirstLeft()
		{
			foreach (var dataSet in _dataSets) {
				if (dataSet.axisDependency == .Left)
					return dataSet;
			}

			return null;
		}

			/// - returns: the first DataSet from the datasets-array that has it's dependency on the right axis. Returns null if no DataSet with right dependency could be found.
		public IChartDataSet getFirstRight()
		{
			foreach (var dataSet in _dataSets) {
				if (dataSet.axisDependency == .Right)
					return dataSet;
			}

			return null;
		}

			/// - returns: all colors used across all DataSet objects this object represents.
		public List<UIColor> getColors()
		{
			if (_dataSets == null)
				return null;

			var clrcnt = 0;

			for (var i = 0; i < _dataSets.Count; i++)
			{
				clrcnt += _dataSets [i].colors.Length;
			}

			var colors = new List<UIColor>();

			for (var i = 0; i < _dataSets.Count; i++)
			{
				var clrs = _dataSets [i].colors;
				foreach (var clr in clrs) {
					colors.Add (clr);
				}
			}

			return colors;
		}

			/// Generates an x-values array filled with numbers in range specified by the parameters. Can be used for convenience.
		public string[] generateXVals(int from, int to)
		{
			var xvals = new List<string>();

			for (var i = from; i < to; i++)
			{
				xvals.Add (i);
			}
			return xvals;
		}

			/// Sets a custom ValueFormatter for all DataSets this data object contains.
		public void setValueFormatter(NSNumberFormatter formatter)
		{
			foreach (var item in dataSets) {
				item.valueFormatter = formatter;
			}
		}

			/// Sets the color of the value-text (color in which the value-labels are drawn) for all DataSets this data object contains.
		public void setValueTextColor(UIColor color)
		{
			foreach (var item in dataSets) {
				item.valueTextColor = color ?? item.valueTextColor;
			}
		}

			/// Sets the font for all value-labels for all DataSets this data object contains.
		public void setValueFont(UIFont font)
		{
			foreach (var item in dataSets) {
				item.valueFont = font ?? item.valueFont;
			}
		}

			/// Enables / disables drawing values (value-text) for all DataSets this data object contains.
		public void setDrawValues(bool enabled)
		{
			foreach (var item in dataSets) {
				item.drawValuesEnabled = enabled;
			}
		}

			/// Enables / disables highlighting values for all DataSets this data object contains.
			/// If set to true, this means that values can be highlighted programmatically or by touch gesture.
		public bool highlightEnabled
		{
			get
			{
				foreach (var item in dataSets) {
					if (!item.highlightEnabled)
						return false;
				}
				return true;
			}
			set
			{
				foreach (var item in dataSets) {
					item.highlightEnabled = value;
				}
			}
		}

			/// if true, value highlightning is enabled
		public bool isHighlightEnabled {
			get { 
				return highlightEnabled;
			}
		}

			/// Clears this data object from all DataSets and removes all Entries.
			/// Don't forget to invalidate the chart after this.
		public void clearValues()
		{
			dataSets.RemoveAll (false);
			notifyDataChanged ();
		}

			/// Checks if this data object contains the specified Entry. 
			/// - returns: true if so, false if not.
		public bool contains(ChartDataEntry entry)
		{
			foreach (var item in dataSets) {
				if (item.contains(entry))
					return true;
			}
			return false;
		}

			/// Checks if this data object contains the specified DataSet. 
			/// - returns: true if so, false if not.
		public bool contains(IChartDataSet dataSet)
		{
			foreach (var item in dataSets) {
				if (item == dataSet)
					return true;	
			}
			return false;
		}

			/// MARK: - ObjC compatibility

			/// - returns: the average length (in characters) across all values in the x-vals array
		public NSObject[] xValsObjc {
			get {
				return ChartUtils.bridgedObjCGetStringArray (_xVals); 
			}
		}
	}
}

