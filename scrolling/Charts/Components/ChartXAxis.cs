using System;
using System.Collections.Generic;
using Foundation;

namespace scrolling
{
	public class ChartXAxis : ChartAxisBase
	{

		public enum XAxisLabelPosition {
			Top, Bottom, BothSided, TopInside, BottomInside
		}

		public List<string> values = new List<string>();

			/// width of the x-axis labels in pixels - this is automatically calculated by the computeAxis() methods in the renderers
		public nfloat labelWidth = 1.0f;

			/// height of the x-axis labels in pixels - this is automatically calculated by the computeAxis() methods in the renderers
		public nfloat labelHeight = 1.0f;

			/// width of the (rotated) x-axis labels in pixels - this is automatically calculated by the computeAxis() methods in the renderers
		public nfloat labelRotatedWidth = 1.0f;

			/// height of the (rotated) x-axis labels in pixels - this is automatically calculated by the computeAxis() methods in the renderers
		public nfloat labelRotatedHeight = 1.0f;

			/// This is the angle for drawing the X axis labels (in degrees)
		public nfloat labelRotationAngle = 0.0f;

			/// the space that should be left out (in characters) between the x-axis labels
			/// This only applies if the number of labels that will be skipped in between drawn axis labels is not custom set.
			/// 
			/// **default**: 4
		public int spaceBetweenLabels = 4;

			/// the modulus that indicates if a value at a specified index in an array(list) for the x-axis-labels is drawn or not. Draw when `(index % modulus) == 0`.
		public int axisLabelModulus = 1;

			/// Is axisLabelModulus a custom value or auto calculated? If false, then it's auto, if true, then custom.
			/// 
			/// **default**: false (automatic modulus)
		private bool _isAxisModulusCustom = false;

			/// the modulus that indicates if a value at a specified index in an array(list) for the y-axis-labels is drawn or not. Draw when `(index % modulus) == 0`.
			/// Used only for Horizontal BarChart
		public int yAxisLabelModulus = 1;

			/// if set to true, the chart will avoid that the first and last label entry in the chart "clip" off the edge of the chart
		public bool avoidFirstLastClippingEnabled = false;

			/// Custom formatter for adjusting x-value strings
		private ChartXAxisValueFormatter _xAxisValueFormatter = new ChartDefaultXAxisValueFormatter();

			/// Custom XValueFormatter for the data object that allows custom-formatting of all x-values before rendering them.
			/// Provide null to reset back to the default formatting.
		public ChartXAxisValueFormatter valueFormatter
		{
			get { return _xAxisValueFormatter; }
			set { _xAxisValueFormatter = value ?? ChartDefaultXAxisValueFormatter (); }
		}

			/// the position of the x-labels relative to the chart
		public XAxisLabelPosition labelPosition = XAxisLabelPosition.Top;

			/// if set to true, word wrapping the labels will be enabled.
			/// word wrapping is done using `(value width * labelRotatedWidth)`
			///
			/// *Note: currently supports all charts except pie/radar/horizontal-bar*
		public bool wordWrapEnabled = false;

			/// - returns: true if word wrapping the labels is enabled
		public bool isWordWrapEnabled { 
			get {
				return wordWrapEnabled;
			}
		}

			/// the width for wrapping the labels, as percentage out of one value width.
			/// used only when isWordWrapEnabled = true.
			/// 
			/// **default**: 1.0
		public nfloat wordWrapWidthPercent = 1.0f;

		public ChartXAxis ()
		{
			yOffset = 4.0f;
		}


		public override string getLongestLabel()
		{
			var longest = "";

			for (var i = 0; i < values.Count; i++)
			{
				var text = values [i];

				if (text != null && longest.Length < text.Length) {
					longest = text;
				}
			}

			return longest;
		}

		public bool isAvoidFirstLastClippingEnabled {
			get {
				return avoidFirstLastClippingEnabled;
			}
		}

			/// Sets the number of labels that should be skipped on the axis before the next label is drawn. 
			/// This will disable the feature that automatically calculates an adequate space between the axis labels and set the number of labels to be skipped to the fixed number provided by this method. 
			/// Call `resetLabelsToSkip(...)` to re-enable automatic calculation.
		public void setLabelsToSkip(int count)
		{
			_isAxisModulusCustom = true;

			axisLabelModulus = count < 0 ? 1 : count + 1;
		}

			/// Calling this will disable a custom number of labels to be skipped (set by `setLabelsToSkip(...)`) while drawing the x-axis. Instead, the number of values to skip will again be calculated automatically.
		public void resetLabelsToSkip()
		{
			_isAxisModulusCustom = false;
		}

			/// - returns: true if a custom axis-modulus has been set that determines the number of labels to skip when drawing.
		public bool isAxisModulusCustom {
			get {
				return _isAxisModulusCustom;
			}
		}

		public List<NSObject> valuesObjc
		{
			get { return ChartUtils.bridgedObjCGetStringArray(values); }
			set { values = ChartUtils.bridgedObjCGetStringArray(value); }
		}
			
	}
}

