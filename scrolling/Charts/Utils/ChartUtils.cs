using System;
using Foundation;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;

namespace scrolling
{
	public class ChartUtils
	{
		public ChartUtils ()
		{
		}

		private static NSNumberFormatter _defaultValueFormatter = ChartUtils.generateDefaultValueFormatter();

		internal struct Math2
		{
			internal static nfloat FDEG2RAD = (nfloat)(Math.PI / 180.0f);
			internal static nfloat FRAD2DEG = (nfloat)(180.0f / Math.PI);
			internal static nfloat DEG2RAD = Math.PI / 180.0f;
			internal static nfloat RAD2DEG = 180.0f / Math.PI;
		}

		internal static double roundToNextSignificant(double number) 
		{
			if (double.IsInfinity (number) || double.IsNaN (number) || number == 0)
				return number;

			var d = Math.Ceiling(Math.Log10(number < 0.0 ? -number : number));
			var pw = 1 - (int)d;
			var magnitude = Math.Pow((double)(10.0), (double)(pw));
			var shifted = Math.Round(number * magnitude);
			return shifted / magnitude;
		}

		internal static int decimals(double number)
		{
			if (number == 0.0d)
				return 0;

			var i = roundToNextSignificant ((double) (number));
			return (int) (Math.Ceiling (-Math.Log10 (i))) + 2;
		}

		internal static double nextUp(double number)
		{
			if (double.IsInfinity (number) || double.IsNaN (number))
				return number;
			else
				return number + double.Epsilon;
		}

			/// - returns: the index of the DataSet that contains the closest value on the y-axis. This will return -Integer.MAX_VALUE if failure.
		internal static int closestDataSetIndex(List<ChartSelectionDetail> valsAtIndex, double value,ChartYAxis.AxisDependency axis)
		{
			var index = -int.MaxValue;
			var distance = double.MaxValue;

			for (var i = 0; i < valsAtIndex.Count; i++)
			{
				var sel = valsAtIndex [i];

				if (axis == null || sel.dataSet.axisDependency == axis)
				{
					var cdistance = Math.Abs (sel.value - value);
					if (cdistance < distance)
					{
						index = valsAtIndex [i].dataSetIndex;
						distance = cdistance;
					}
				}
			}

			return index;
		}

			/// - returns: the minimum distance from a touch-y-value (in pixels) to the closest y-value (in pixels) that is displayed in the chart.
		internal static double getMinimumDistance(List<ChartSelectionDetail> valsAtIndex, double val,ChartYAxis.AxisDependency axis)
		{
			var distance = double.MaxValue;

			for (var i = 0, count = valsAtIndex.Count; i < count; i++)
			{
				var sel = valsAtIndex [i];

				if (sel.dataSet.axisDependency == axis)
				{
					var cdistance = Math.Abs(sel.value - val);
					if (cdistance < distance)
						distance = cdistance;
				}
			}
			return distance;
		}

			/// Calculates the position around a center point, depending on the distance from the center, and the angle of the position around the center.
		internal static CGPoint getPosition(CGPoint center , nfloat dist,nfloat angle)
		{
			return CGPoint (
				center.X + dist * Math.Cos (angle * Math2.FDEG2RAD),
				center.Y + dist * Math.Sin (angle * Math2.FDEG2RAD)
			);
		}

		public static void drawText(CGContext context , string text, CGPoint point, UITextAlignment align, List<string> attrStr, UIFont font)//FIXME может быть косяк с атрибутами [String : AnyObject]?)
		{
			if (align == UITextAlignment.Center)
			{
				point.X -= text.StringSize (font).Width / 2.0f;// sizeWithAttributes(attributes).width / 2.0
			}
			else if (align == UITextAlignment.Right)
			{
				point.X -= text.StringSize (font).Width;// sizeWithAttributes(attributes).width
			}

			UIGraphics.PushContext (context);

			text.DrawString (point, font);
//			(text as NSString).drawAtPoint(point, withAttributes: attributes)

			UIGraphics.PopContext ();
		}

		public static void drawText(CGContext context , string text, CGPoint point, UIFont font, CGPoint anchor, nfloat angleRadians)
		{
			var drawOffset = new CGPoint ();

			UIGraphics.PushContext (context);

			if (angleRadians != 0.0)
			{
				var size = text.StringSize (font);

						// Move the text drawing rect in a way that it always rotates around its center
				drawOffset.X = -size.Width * 0.5f;
				drawOffset.Y = -size.Height * 0.5f;

				var translate = point;

				// Move the "outer" rect relative to the anchor, assuming its centered
				if (anchor.X != 0.5f || anchor.Y != 0.5f)
				{
					var rotatedSize = sizeOfRotatedRectangle (size, angleRadians);

					translate.X -= rotatedSize.Width * (anchor.X - 0.5f);
					translate.Y -= rotatedSize.Height * (anchor.Y - 0.5f);
				}

				context.SaveState ();
				context.TranslateCTM (translate.X, translate.Y);
				context.RotateCTM (angleRadians);
				text.DrawString (drawOffset, font);
				context.RestoreState ();
			}
			else
			{
				if (anchor.X != 0.0f || anchor.Y != 0.0f)
				{
					var size = text.StringSize (font);

					drawOffset.X = -size.Width * anchor.X;
					drawOffset.Y = -size.Height * anchor.Y;
				}

				drawOffset.X += point.X;
				drawOffset.Y += point.Y;
				text.DrawString (drawOffset, font);
			}

			UIGraphics.PopContext();
		}

		internal static void drawMultilineText(CGContext context, string text,CGSize knownTextSize,CGPoint point, UIFont font,CGSize constrainedToSize,CGPoint anchor,nfloat angleRadians)
		{
			var rect = new CGRect (new CGPoint (), knownTextSize);

			UIGraphics.PushContext (context);

			if (angleRadians != 0.0f)
			{
					// Move the text drawing rect in a way that it always rotates around its center
				rect.X = -knownTextSize.Width * 0.5f;
				rect.Y = -knownTextSize.Height * 0.5f;

				var translate = point;

				// Move the "outer" rect relative to the anchor, assuming its centered
				if (anchor.X != 0.5f || anchor.Y != 0.5f)
				{
					var rotatedSize = sizeOfRotatedRectangle (knownTextSize, angleRadians);

					translate.X -= rotatedSize.Width * (anchor.X - 0.5f);
					translate.Y -= rotatedSize.Height * (anchor.Y - 0.5f);
				}

				context.SaveState ();
				context.TranslateCTM (translate.X, translate.Y);
				context.RotateCTM (angleRadians);
				text.DrawString (rect, font);
//				(text as NSString).drawWithRect(rect, options: .UsesLineFragmentOrigin, attributes: attributes, context: nil)
				context.RestoreState ();
			}
			else
			{
				if (anchor.X != 0.0f || anchor.Y != 0.0f)
				{
					rect.X = -knownTextSize.Width * anchor.X;
					rect.Y = -knownTextSize.Height * anchor.Y;
				}

				rect.X += point.X;
				rect.Y += point.Y;

				text.DrawString (rect, font);
			}

			UIGraphics.PopContext();
		}
			
		internal static void drawMultilineText(CGContext context, string text, CGPoint point, UIFont font, CGSize constrainedToSize,CGPoint anchor, nfloat angleRadians)
		{
			var rect = text.StringSize (font, constrainedToSize);//  boundingRectWithSize(constrainedToSize, options: .UsesLineFragmentOrigin, attributes: attributes, context: nil)
			drawMultilineText(context, text, rect, point, font, constrainedToSize, anchor, angleRadians);
		}

			/// - returns: an angle between 0.0 < 360.0 (not less than zero, less than 360)
		internal static nfloat normalizedAngleFromAngle(nfloat angle) 
		{
			while (angle < 0.0f)
			{
				angle += 360.0f;
			}

			return angle % 360.0f
		}

		private static NSNumberFormatter generateDefaultValueFormatter()
		{
			var formatter = new NSNumberFormatter();
			formatter.MinimumIntegerDigits = 1;
			formatter.MaximumFractionDigits = 1;
			formatter.MinimumFractionDigits = 1;
			formatter.UsesGroupingSeparator = true;
			return formatter;
		}

			/// - returns: the default value formatter used for all chart components that needs a default
		internal static NSNumberFormatter defaultValueFormatter()
		{
			return _defaultValueFormatter;
		}

		internal static CGSize sizeOfRotatedRectangle(CGSize rectangleSize, nfloat degrees) 
		{
			var radians = degrees * Math2.FDEG2RAD;
			return sizeOfRotatedRectangle(rectangleSize.Width, rectangleSize.Height, radians);
		}

		internal static CGSize sizeOfRotatedRectangle(CGSize rectangleSize, nfloat radians)
		{
			return sizeOfRotatedRectangle(rectangleSize.Width, rectangleSize.Height, radians);
		}

		internal static CGSize sizeOfRotatedRectangle(nfloat rectangleWidth, nfloat rectangleHeight, nfloat degrees)
		{
			var radians = degrees * Math2.FDEG2RAD;
			return sizeOfRotatedRectangle(rectangleWidth, rectangleHeight, radians);
		}

		internal static CGSize sizeOfRotatedRectangle(nfloat rectangleWidth, nfloat rectangleHeight, nfloat radians)
		{
			return CGSize( 
				Math.Abs(rectangleWidth * Math.Cos(radians)) + Math.Abs(rectangleHeight * Math.Sin(radians)),
				Math.Abs(rectangleWidth * Math.Sin(radians)) + Math.Abs(rectangleHeight * Math.Cos(radians))
			);
		}

			/// MARK: - Bridging functions

		internal static List<NSObject> bridgedObjCGetUIColorArray (List<UIColor> array)
		{
			var newArray = new List<NSObject>();
			foreach (var item in array) {
				newArray.Add(item);
			}
			return newArray;
		}

		internal static List<UIColor> bridgedObjCGetUIColorArray (List<NSObject> array)
		{
			var newArray = new List<UIColor>();

			foreach (var obj in array) {
				newArray.Add(obj as UIColor);
			}
			return newArray;
		}

		internal static List<NSObject> bridgedObjCGetStringArray (List<string> array)
		{
			var newArray = new List<NSObject>();

			foreach (var val in array) {
				newArray.Add(val);
			}
			return newArray;
		}

		internal static List<string> bridgedObjCGetStringArray (List<NSObject> array)
		{
			var newArray = new List<string>();

			foreach (var item in array) {
				newArray.Add(item as string);
			}
			return newArray;
		}
			
	}
}

