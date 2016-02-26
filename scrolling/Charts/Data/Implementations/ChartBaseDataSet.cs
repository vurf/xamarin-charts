using System;
using Foundation;
using UIKit;
using System.Collections.Generic;

namespace scrolling
{
	public class ChartBaseDataSet : NSObject, IChartDataSet
	{
		public ChartBaseDataSet ()
		{
		}

		#region IChartDataSet implementation

		public void notifyDataSetChanged ()
		{
			calcMinMax (0, entryCount - 1);
		}

		public void calcMinMax (int start, int end)
		{
			throw new NotImplementedException ();
		}

		public double yValForXIndex (int x)
		{
			throw new NotImplementedException ();
		}

		public ChartDataEntry entryForIndex (int i)
		{
			throw new NotImplementedException ();
		}

		public ChartDataEntry entryForXIndex (int x)
		{
			throw new NotImplementedException ();
		}

		public int entryIndex (int x)
		{
			throw new NotImplementedException ();
		}

		public int entryIndex (ChartDataEntry e)
		{
			throw new NotImplementedException ();
		}

		public bool addEntry (ChartDataEntry e)
		{
			throw new NotImplementedException ();
		}

		public bool removeEntry (ChartDataEntry entry)
		{
			throw new NotImplementedException ();
		}

		public bool contains (ChartDataEntry e)
		{
			throw new NotImplementedException ();
		}

		public UIKit.UIColor colorAt (int index)
		{
			throw new NotImplementedException ();
		}

		public void resetColors ()
		{
			throw new NotImplementedException ();
		}

		public void addColor (UIKit.UIColor color)
		{
			throw new NotImplementedException ();
		}

		public void setColor (UIKit.UIColor color)
		{
			throw new NotImplementedException ();
		}

		public double yMin {
			get {
				throw new NotImplementedException ();
			}
			private set {
				throw new NotImplementedException ();
			}
		}

		public double yMax {
			get {
				throw new NotImplementedException ();
			}
			private set {
				throw new NotImplementedException ();
			}
		}

		public int entryCount {
			get {
				throw new NotImplementedException ();
			}
			private set {
				throw new NotImplementedException ();
			}
		}

		public string label {
			get {
				throw new NotImplementedException ();
			}
			private set {
				throw new NotImplementedException ();
			}
		}

		public AxisDependency axisDependency {
			get {
				return "DataSet";
			}
			private set {
				
			}
		}

		List<UIColor> _colors;
		public List<UIColor> colors {
			get {
				return _colors ?? (_colors = new List<UIColor> ());
			}
		}

		public bool highlightEnabled {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public bool isHighlightEnabled {
			get {
				throw new NotImplementedException ();
			}
			private set {
				throw new NotImplementedException ();
			}
		}

		public NSNumberFormatter valueFormatter {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public UIKit.UIColor valueTextColor {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public UIKit.UIFont valueFont {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public bool drawValuesEnabled {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public bool isDrawValuesEnabled {
			get {
				throw new NotImplementedException ();
			}
			private set {
				throw new NotImplementedException ();
			}
		}

		public bool visible {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public bool isVisible {
			get {
				throw new NotImplementedException ();
			}
			private set {
				throw new NotImplementedException ();
			}
		}

		#endregion
	}
}

