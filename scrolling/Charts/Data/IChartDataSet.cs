using System;
using UIKit;
using Foundation;
using System.Collections.Generic;

namespace scrolling
{
	public interface IChartDataSet
	{
		// MARK: - Data functions and accessors

		/// Use this method to tell the data set that the underlying data has changed
		void notifyDataSetChanged();

		/// This is an opportunity to calculate the minimum and maximum y value in the specified range.
		/// If your data is in an array, you might loop over them to find the values.
		/// If your data is in a database, you might query for the min/max and put them in variables.
		/// - parameter start: the index of the first y entry to calculate
		/// - parameter end: the index of the last y entry to calculate
		void calcMinMax(int start, int end);

		/// - returns: the minimum y-value this DataSet holds
		double yMin { get; }

		/// - returns: the maximum y-value this DataSet holds
		double yMax { get; }

		/// - returns: the number of y-values this DataSet represents
		int entryCount { get; }

		/// - returns: the value of the Entry object at the given xIndex. Returns NaN if no value is at the given x-index.
		double yValForXIndex(int x);

		/// - returns: the entry object found at the given index (not x-index!)
		/// - throws: out of bounds
		/// if `i` is out of bounds, it may throw an out-of-bounds exception
		ChartDataEntry entryForIndex(int i);

		/// - returns: the first Entry object found at the given xIndex with binary search.
		/// If the no Entry at the specifed x-index is found, this method returns the Entry at the closest x-index.
		/// nil if no Entry object at that index.
		ChartDataEntry entryForXIndex(int x);

		/// - returns: the array-index of the specified entry
		///
		/// - parameter x: x-index of the entry to search for
		int entryIndex(int x);

		/// - returns: the array-index of the specified entry
		///
		/// - parameter e: the entry to search for
		int entryIndex(ChartDataEntry e);

		/// Adds an Entry to the DataSet dynamically.
		///
		/// *optional feature, can return false or throw*
		///
		/// Entries are added to the end of the list.
		/// - parameter e: the entry to add
		/// - returns: true if the entry was added successfully, else if this feature is not supported
		bool addEntry(ChartDataEntry e);

		/// Removes an Entry from the DataSet dynamically.
		///
		/// *optional feature, can return false or throw*
		///
		/// - parameter entry: the entry to remove
		/// - returns: true if the entry was removed successfully, else if the entry does not exist or if this feature is not supported
		bool removeEntry(ChartDataEntry entry);

		/// Checks if this DataSet contains the specified Entry.
		/// - returns: true if contains the entry, false if not.
		bool contains(ChartDataEntry e);

		// MARK: - Styling functions and accessors

		/// The label string that describes the DataSet.
		string label { get; }

		/// The axis this DataSet should be plotted against.
		ChartYAxis.AxisDependency axisDependency  { get; private set; }

		/// All the colors that are set for this DataSet
		List<UIColor> colors { get; }

		/// - returns: the color at the given index of the DataSet's color array.
		/// This prevents out-of-bounds by performing a modulus on the color index, so colours will repeat themselves.
		UIColor colorAt(int index);

		void resetColors();

		void addColor(UIColor color);

		void setColor(UIColor color);

		/// if true, value highlighting is enabled
		bool highlightEnabled { get; set; }

		/// - returns: true if value highlighting is enabled for this dataset
		bool isHighlightEnabled { get; }

		/// The formatter used to customly format the values
		NSNumberFormatter valueFormatter { get; set; }

		/// the color used for the value-text
		UIColor valueTextColor { get; set; }

		/// the font for the value-text labels
		UIFont valueFont { get; set; }

		/// Set this to true to draw y-values on the chart
		bool drawValuesEnabled { get; set; }

		/// Returns true if y-value drawing is enabled, false if not
		bool isDrawValuesEnabled { get; }

		/// Set the visibility of this DataSet. If not visible, the DataSet will not be drawn to the chart upon refreshing it.
		bool visible { get; set; }

		/// Returns true if this DataSet is visible inside the chart, or false if it is currently hidden.
		bool isVisible { get; }
	}
}

