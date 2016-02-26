using System;

using UIKit;
using CoreGraphics;
using Foundation;
using System.Collections.Generic;

namespace charts
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public LineChart chart { get; set; }
		public LineChart chartWithDates { get; set; }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			chart = new LineChart (new CGRect(0, 0, 320, 200));
			chartWithDates = new LineChart(new CGRect(20,300,300,200));
			View.AddSubview (chart);
			View.AddSubviews (chartWithDates);

			chart.Selected += () => {
				
			};

			chartWithDates.Selected += () => {
				Console.WriteLine ("asd1");
			};

			loadSimpleChart ();
			loadChartWithDates ();
		}

		void loadSimpleChart() 
		{
			var chartData = new List<float> () {
				1f, 5f, 20f, 4f, 20, 1, 3, 5
			};


			// Setting up the line chart
			chart.verticalGridStep = 5;
			chart.horizontalGridStep = DateTime.DaysInMonth (DateTime.Now.Year, DateTime.Now.Month);

			chart.labelForIndex = (int index) => {
				return new NSString (string.Format ("{0}", index));
			};

			chart.labelForValue = (nfloat value) => {
				return new NSString (string.Format ("{0}", value));
			};

			chart.setChartData (chartData);
		}

		void loadChartWithDates() 
		{
			var chartData = new List<float> () {
				1f, 5f, 20f, 25f, 20, 10, 60, 5
			};


//			var months = new NSString[] { 
//				new NSString("January"), 
//				new NSString("February"), 
//				new NSString("March"), 
//				new NSString("April"), 
//				new NSString("May"), 
//				new NSString("June"), 
//				new NSString("July") 
//			};

			// Setting up the line chart
			chartWithDates.verticalGridStep = 5;
			chartWithDates.horizontalGridStep = DateTime.DaysInMonth (DateTime.Now.Year, DateTime.Now.Month);
			chartWithDates.fillColor = UIColor.Orange.ColorWithAlpha (0.3f);
			chartWithDates.displayDataPoint = true;
			chartWithDates.dataPointColor = UIColor.Orange;
			chartWithDates.dataPointBackgroundColor = UIColor.Orange;
			chartWithDates.dataPointRadius = 2;
			chartWithDates.color = chartWithDates.dataPointColor.ColorWithAlpha (0.3f);
			chartWithDates.valueLabelPosition = ValueLabelPosition.Left;

			chartWithDates.labelForIndex = (int index) => {
//				return months[index];
				var str = string.Format ("{0}", index);
				return new NSString (str);
			};

			chartWithDates.labelForValue = (nfloat value) => {
				return new NSString (string.Format ("{0}", value));
			};

			chartWithDates.setChartData(chartData);
		}
	}
}

