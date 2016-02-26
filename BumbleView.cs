using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace charts
{
	public class BumbleView : UIView
	{
		public BumbleView ()
		{
		}

		public override void Draw (CGRect rect)
		{
			//// General Declarations
			var context = UIGraphics.GetCurrentContext();

			//// Rectangle Drawing
			var rectanglePath = UIBezierPath.FromRoundedRect(new CGRect(0.0f, 0.0f, 70.0f, 25.0f), 5.0f);
			UIColor.Black.SetFill();
			rectanglePath.Fill();


			//// Bezier Drawing
			UIBezierPath bezierPath = new UIBezierPath();
			bezierPath.MoveTo(new CGPoint(17.5f, 24.5f));
			bezierPath.AddLineTo(new CGPoint(29.5f, 26.5f));
			bezierPath.AddLineTo(new CGPoint(32.5f, 31.5f));
			bezierPath.AddLineTo(new CGPoint(34.5f, 26.5f));
			bezierPath.AddLineTo(new CGPoint(47.5f, 24.5f));
			UIColor.Black.SetFill();
			bezierPath.Fill();
			UIColor.Black.SetStroke();
			bezierPath.LineWidth = 1.0f;
			bezierPath.Stroke();


			//// Text Drawing
			CGRect textRect = new CGRect(7.0f, 8.0f, 56.0f, 11.0f);
			{
				var textContent = "15 кг * 10 п";
				UIColor.White.SetFill();
				var textStyle = new NSMutableParagraphStyle ();
				textStyle.Alignment = UITextAlignment.Left;

				var textFontAttributes = new UIStringAttributes () {Font = UIFont.SystemFontOfSize(10.0f), ForegroundColor = UIColor.White, ParagraphStyle = textStyle};
				var textTextHeight = new NSString(textContent).GetBoundingRect(new CGSize(textRect.Width, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, textFontAttributes, null).Height;
				context.SaveState();
				context.ClipToRect(textRect);
				new NSString(textContent).DrawString(new CGRect(textRect.GetMinX(), textRect.GetMinY() + (textRect.Height - textTextHeight) / 2.0f, textRect.Width, textTextHeight), UIFont.SystemFontOfSize(10.0f), UILineBreakMode.WordWrap, UITextAlignment.Left);
				context.RestoreState();
			}

		}
	}
}

