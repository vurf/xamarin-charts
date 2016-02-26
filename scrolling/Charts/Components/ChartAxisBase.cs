using System;
using UIKit;
using System.Collections.Generic;

namespace scrolling
{
	public class ChartAxisBase : ChartComponentBase
	{
		public ChartAxisBase ()
		{
		}

		public UIFont labelFont = UIFont.SystemFontOfSize(10.0);
		public UIColor labelTextColor = UIColor.Black;

		public UIColor axisLineColor = UIColor.Gray;
		public nfloat axisLineWidth = 0.5f;
		public nfloat axisLineDashPhase = 0.0f;
		public List<nfloat> axisLineDashLengths = new List<nfloat>();

		public UIColor gridColor = UIColor.Gray.ColorWithAlpha (0.9f);
		public nfloat gridLineWidth = 0.5f;
		public nfloat gridLineDashPhase = 0.0f;
		public List<nfloat> gridLineDashLengths = new List<nfloat>();

		public bool drawGridLinesEnabled = true;
		public bool drawAxisLineEnabled = true;

			/// flag that indicates of the labels of this axis should be drawn or not
		public bool drawLabelsEnabled = true;

			/// array of limitlines that can be set for the axis
		private List<ChartLimitLine> _limitLines = new List<ChartLimitLine>();

			/// Are the LimitLines drawn behind the data or in front of the data?
			/// 
			/// **default**: false
		public bool drawLimitLinesBehindDataEnabled = false;

			/// the flag can be used to turn off the antialias for grid lines
		public bool gridAntialiasEnabled = true;



		public virtual string getLongestLabel()
		{
			Console.WriteLine ("getLongestLabel() cannot be called on ChartAxisBase");
		}

		public bool isDrawGridLinesEnabled {
			get {
				return drawGridLinesEnabled;
			}
		}

		public bool isDrawAxisLineEnabled {
			get { 
				return drawAxisLineEnabled;
			}
		}

		public bool isDrawLabelsEnabled {
			get { 
				return drawLabelsEnabled; 
			}
		}

			/// Are the LimitLines drawn behind the data or in front of the data?
			/// 
			/// **default**: false
		public bool isDrawLimitLinesBehindDataEnabled {
			get {
				return drawLimitLinesBehindDataEnabled;
			}
		}

			/// Adds a new ChartLimitLine to this axis.
		public void addLimitLine(ChartLimitLine line)
		{
			_limitLines.Add (line);
		}

			/// Removes the specified ChartLimitLine from the axis.
		public void removeLimitLine(ChartLimitLine line) 
		{
			for (var i = 0; i < _limitLines.Count; i++)
			{
				if (_limitLines[i] == line)
				{
					_limitLines.RemoveAt (i);
					return;
				}
			}
		}

			/// Removes all LimitLines from the axis.
		public void removeAllLimitLines()
		{
			_limitLines.RemoveAll ();
		}

			/// - returns: the LimitLines of this axis.
		public List<ChartLimitLine> limitLines {
			get {
				return _limitLines;
			}
		}


	}
}

