using System;
using CoreGraphics;
using UIKit;
using System.Collections.Generic;
using CoreAnimation;
using Foundation;

namespace charts
{
	public enum ValueLabelPosition
	{
		Left, Right, LeftMirrored
	}

	public class LineChart : UIView
	{
		public delegate NSString LabelForIndex(int index);
		public delegate NSString LabelForValue(nfloat value);

		public LabelForIndex labelForIndex { get; set; }
		public LabelForValue labelForValue { get; set; }

		public nfloat min;
		public nfloat max;
		public CGPath initialPath;
		public CGPath newPath;
		public List<CAShapeLayer> Layers;
		public List<float> Data;

		public LineChart (CGRect frame) : base (frame)
		{
			commonInit ();	
		}

		public UIColor color { get; set; }
		public UIColor fillColor { get; set; }
		public nfloat lineWidth { get; set; }
		public int gridStep {
			set {
				verticalGridStep = value;
				horizontalGridStep = value;
			}
		}
		public int verticalGridStep { get; set; }
		public int horizontalGridStep { get; set; }
		public nfloat margin { get ; set; }
		public nfloat axisHeight { get; set; }
		public nfloat axisWidth { get; set; }
		public UIColor axisColor { get; set; }
		public UIColor innerGridColor { get; set; }
		public bool drawInnerGrid { get; set; }
		public bool bezierSmoothing { get; set; }
		public nfloat bezierSmoothingTension { get; set; }
		public nfloat innerGridLineWidth { get; set; }
		public nfloat axisLineWidth { get; set; }
		public nfloat animationDuration { get; set; }
		public bool displayDataPoint { get; set; }
		public nfloat dataPointRadius { get; set; }
		public UIColor dataPointColor { get; set; }
		public UIColor dataPointBackgroundColor { get; set; }
		public UIColor indexLabelBackgroundColor { get; set; }
		public UIColor indexLabelTextColor { get; set; }
		public UIFont indexLabelFont { get; set; }
		public UIColor valueLabelBackgroundColor { get; set; }
		public UIColor valueLabelTextColor { get; set; }
		public UIFont valueLabelFont { get; set; }
		public ValueLabelPosition valueLabelPosition { get; set; }

		public event Action Selected;
		protected virtual void OnSelected ()
		{
			var handler = Selected;
			if (handler != null)
				handler ();
		}

		void commonInit()
		{
			BackgroundColor = UIColor.White;
			Layers = new List<CAShapeLayer> ();
			setDefaultParameters ();
		}

		void setDefaultParameters() 
		{
			color = UIColor.Blue;
			fillColor = color.ColorWithAlpha (0.25f);
			verticalGridStep = 3;
			horizontalGridStep = 3;
			margin = 5.0f;
			axisWidth = Frame.Size.Width - 2 * margin;
			axisHeight = Frame.Size.Height - 2 * margin;
			axisColor = UIColor.FromWhiteAlpha (0.7f, 1f);
			innerGridColor = UIColor.FromWhiteAlpha (0.9f, 1f);
			drawInnerGrid = true;
			bezierSmoothing = true;
			bezierSmoothingTension = 0.2f;
			lineWidth = 1;
			innerGridLineWidth = 0.5f;
			axisLineWidth = 1;
			animationDuration = 0.5f;
			displayDataPoint = false;
			dataPointRadius = 1;
			dataPointColor = color;
			dataPointBackgroundColor = color;

			// Labels attributes
			indexLabelBackgroundColor = UIColor.Clear;
			indexLabelTextColor = UIColor.Gray;
			indexLabelFont = UIFont.FromName ("HelveticaNeue-Light", 10);

			valueLabelBackgroundColor = UIColor.FromWhiteAlpha (1, 0.75f);
			valueLabelTextColor = UIColor.Gray;
			valueLabelFont = UIFont.FromName ("HelveticaNeue-Light", 11);
			valueLabelPosition = ValueLabelPosition.Right;
		}

		public override void LayoutSubviews ()
		{
			axisWidth = Frame.Size.Width - 2 * margin;
			axisHeight = Frame.Height - 2 * margin;

			foreach (var view in Subviews) {
				view.RemoveFromSuperview ();
			}

			foreach (var layer in Layers) {
				layer.RemoveFromSuperLayer ();
			}

			layoutChart ();
			base.LayoutSubviews ();
		}

		void layoutChart() 
		{
			if(Data == null) {
				return;
			}

			computeBounds();

			// No data
			if(float.IsNaN ((float)max)) {
				max = 1;
			}

			strokeChart();

			if(displayDataPoint) {
				strokeDataPoints();
			}

			if(labelForValue!=null) {
				for(int i=0;i<verticalGridStep;i++) {
					var label = createLabelForValue(i);

					if(label != null) {
						AddSubview(label);
					}
				}
			}

			if(labelForIndex != null) {
				for(int i=0;i<horizontalGridStep + 1;i++) {
					var label = createLabelForIndex(i);

					if(label != null) {
						AddSubview(label);
					}
				}
			}

			SetNeedsDisplay();
		}

		public void setChartData(List<float> chartData)
		{
			if (chartData == null || chartData.Count == 0) {
				return;
			}
			Data = new List<float> ();
			Data.Clear ();
			Data.AddRange (chartData);
			layoutChart ();
		}

		UILabel createLabelForValue(int index)
		{
			var minBound = minVerticalBound;
			var maxBound = maxVerticalBound;

			CGPoint p = new CGPoint(margin + (valueLabelPosition == ValueLabelPosition.Right ? axisWidth : 0), axisHeight + margin - (index + 1) * axisHeight / verticalGridStep);

			NSString text = labelForValue(minBound + (maxBound - minBound) / verticalGridStep * (index + 1));

			if(string.IsNullOrEmpty (text))
			{
				return null;
			}

			CGRect rect = new CGRect(margin, p.Y + 2, Frame.Width - margin * 2 - 4.0f, 14);

			nfloat width = text.GetBoundingRect (
				rect.Size, 
				NSStringDrawingOptions.UsesLineFragmentOrigin, 
				new UIStringAttributes (new NSDictionary (UIStringAttributeKey.Font, valueLabelFont)), 
				null).Width;

			nfloat xPadding = 6;
			nfloat xOffset = width + xPadding;

			if (valueLabelPosition == ValueLabelPosition.LeftMirrored) {
				xOffset = -xPadding;
			}

			UILabel label = new UILabel (new CGRect (p.X - xOffset, p.Y + 2, width + 2, 14)); 
			label.Text = text;
			label.Font = valueLabelFont;
			label.TextColor = valueLabelTextColor;
			label.TextAlignment = UITextAlignment.Center;
			label.BackgroundColor = valueLabelBackgroundColor;

			return label;
		}

		UILabel createLabelForIndex(int index)
		{
			var scale = horizontalScale;
			int q = Data.Count / horizontalGridStep;
			int itemIndex = q * index;

			if(itemIndex >= Data.Count)
			{
				itemIndex = Data.Count - 1;
			}

			NSString text = labelForIndex(itemIndex);

			if(string.IsNullOrEmpty (text))
			{
				return null;
			}

			CGPoint p = new CGPoint(margin + index * (axisWidth / horizontalGridStep) * scale, axisHeight + margin);

			CGRect rect = new CGRect(margin, p.Y + 2, Frame.Width - margin * 2 - 4.0f, 14);

			nfloat width = text.GetBoundingRect (
				rect.Size, 
				NSStringDrawingOptions.UsesLineFragmentOrigin, 
				new UIStringAttributes (new NSDictionary (UIStringAttributeKey.Font, indexLabelFont)), 
				null).Width;

			var label = new UILabel (new CGRect (p.X - 4f, p.Y + 2, width + 2, 14));
			label.Text = text;
			label.Font = indexLabelFont;
			label.TextColor = indexLabelTextColor;
			label.BackgroundColor = indexLabelBackgroundColor;

			return label;
		}

		public override void Draw (CGRect rect)
		{
			if (Data.Count > 0) {
				drawGrid ();
			}
		}

		void drawGrid() 
		{
			var ctx = UIGraphics.GetCurrentContext ();
			UIGraphics.PushContext (ctx);
			ctx.SetLineWidth (axisLineWidth);
			ctx.SetStrokeColor (axisColor.CGColor);

			ctx.MoveTo (margin, margin);
			ctx.AddLineToPoint (margin, axisHeight + margin + 3);
			ctx.StrokePath ();

			var scale = horizontalScale;
			var minBound = minVerticalBound;
			var maxBound = maxVerticalBound;

			if (drawInnerGrid) {
				for (int i = 0; i < horizontalGridStep; i++) {
					ctx.SetStrokeColor (innerGridColor.CGColor);
					ctx.SetLineWidth (innerGridLineWidth);

					var point = new CGPoint ((1 + i) * axisWidth / horizontalGridStep * scale + margin, margin);

					ctx.MoveTo (point.X, point.Y);
					ctx.AddLineToPoint (point.X, axisHeight + margin);
					ctx.StrokePath ();

					ctx.SetStrokeColor (axisColor.CGColor);
					ctx.SetLineWidth (axisLineWidth);
					ctx.MoveTo (point.X - 0.5f, axisHeight + margin);
					ctx.AddLineToPoint (point.X - 0.5f, axisHeight + margin + 3);
					ctx.StrokePath ();
				}

				for (int i = 0; i < verticalGridStep; i++) {
					float v = maxBound - (maxBound - minBound) / verticalGridStep * i;

					if(v == 0f) {
						ctx.SetLineWidth(axisLineWidth);
						ctx.SetStrokeColor(axisColor.CGColor);
					} else {
						ctx.SetStrokeColor(innerGridColor.CGColor);
						ctx.SetLineWidth(innerGridLineWidth);
					}

					CGPoint point = new CGPoint(margin, (i) * axisHeight / verticalGridStep + margin);

					ctx.MoveTo(point.X, point.Y);
					ctx.AddLineToPoint(axisWidth + margin, point.Y);
					ctx.StrokePath ();
				}
			} 
		}

		void clearChartData() 
		{
			foreach (var layer in Layers) {
				layer.RemoveFromSuperLayer ();
			}
			Layers.Clear ();
		}

		void strokeChart()
		{
			var minBound = minVerticalBound;
			var maxBound = maxVerticalBound;
			nfloat spread = maxBound - minBound;
			nfloat scale = 0f;

			if (spread != 0) {
				scale = axisHeight / spread;
			}

			var noPath = getLinePath(0, bezierSmoothing, false);
			var path = getLinePath(scale, bezierSmoothing, false);

			var noFill = getLinePath (0, bezierSmoothing, true);
			var fill = getLinePath (scale, bezierSmoothing, true);

			if(fillColor != null) {
				var fillLayer = new CAShapeLayer ();
				fillLayer.Frame = new CGRect(Bounds.X, Bounds.Y + minBound * scale, Bounds.Width, Bounds.Height);
				fillLayer.Bounds = Bounds;
				fillLayer.Path = fill.CGPath;
				fillLayer.StrokeColor = null;
				fillLayer.FillColor = fillColor.CGColor;
				fillLayer.LineWidth = 0;
				fillLayer.LineJoin = CAShapeLayer.JoinRound;

				Layer.AddSublayer(fillLayer);
				Layers.Add (fillLayer);

				var fillAnimation = CABasicAnimation.FromKeyPath ("path");
				fillAnimation.Duration = animationDuration;
				fillAnimation.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseInEaseOut);
				fillAnimation.FillMode = CAFillMode.Forwards;
				fillAnimation.From = NSObject.FromObject (noFill.CGPath);
				fillAnimation.To = NSObject.FromObject (fill.CGPath);
				fillLayer.AddAnimation (fillAnimation, "path");
			}

			var pathLayer = new CAShapeLayer ();
			pathLayer.Frame = new CGRect(Bounds.X, Bounds.Y + minBound * scale, Bounds.Width, Bounds.Height);
			pathLayer.Bounds = Bounds;
			pathLayer.Path = path.CGPath;
			pathLayer.StrokeColor = color.CGColor;
			pathLayer.FillColor = null;
			pathLayer.LineWidth = lineWidth;
			pathLayer.LineJoin = CAShapeLayer.JoinRound;

			Layer.AddSublayer(pathLayer);
			Layers.Add(pathLayer);

			if(fillColor != null) {
				var pathAnimation = CABasicAnimation.FromKeyPath ("path");
				pathAnimation.Duration = animationDuration;
				pathAnimation.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseInEaseOut);
				pathAnimation.From = NSObject.FromObject (noPath.CGPath);
				pathAnimation.To = NSObject.FromObject(path.CGPath);
				pathLayer.AddAnimation (pathAnimation, "path");
			} else {
				var pathAnimation = CABasicAnimation.FromKeyPath ("strokeEnd");
				pathAnimation.Duration = animationDuration;
				pathAnimation.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseInEaseOut);
				pathAnimation.From = NSNumber.FromFloat (0.0f);
				pathAnimation.To = NSNumber.FromFloat (1.0f);
				pathLayer.AddAnimation (pathAnimation, "path");
			}

		}

		public List<UIView> Views { get; set; }

		void strokeDataPoints() 
		{
			Views = new List<UIView> ();
			var minBound = minVerticalBound;
			var maxBound = maxVerticalBound;
			nfloat spread = maxBound - minBound;
			nfloat scale = 0f;

			if (spread != 0) {
				scale = axisHeight / spread;
			}

			for (int i = 0; i < Data.Count; i++) {
				var p = getPointForIndex(i, scale);
				p.Y +=  minBound * scale;

				var circle = UIBezierPath.FromOval (new CGRect(10 - dataPointRadius, 10 - dataPointRadius, dataPointRadius * 2, dataPointRadius * 2));

				var view = new UIView (new CGRect (p.X-10, p.Y-10, 20, 20));
//				view.BackgroundColor = UIColor.Cyan;

				var fillLayer = new CAShapeLayer ();
				fillLayer.Frame = new CGRect(10, 10, dataPointRadius, dataPointRadius);
				fillLayer.Bounds = new CGRect(10, 10, dataPointRadius, dataPointRadius);
				fillLayer.Path = circle.CGPath;
				fillLayer.StrokeColor = dataPointColor.CGColor;
				fillLayer.FillColor = dataPointBackgroundColor.CGColor;
				fillLayer.LineWidth = 1;
				fillLayer.LineJoin = CAShapeLayer.JoinRound;

				AddSubview (view);
				Views.Add (view);
				view.Layer.AddSublayer (fillLayer);

//				Layer.AddSublayer (fillLayer);
				Layers.Add (fillLayer);
			}

			ConfigAction ();
		}

		static UIView selectedView;

		void ConfigAction() {
			foreach (var view in Views) {
				view.AddGestureRecognizer (new UITapGestureRecognizer(()=>{
					if (selectedView == null) {
						selectedView = view;
					}
					deselectSelectedView ();
					var bumble = new BumbleView ();
					bumble.BackgroundColor = UIColor.Clear;
					bumble.Frame = new CGRect(12 - 35, 10 - 32, 70, 32);
					bumble.Alpha = 0;
					view.AddSubview (bumble);
					UIView.Animate (0.3f, ()=>{ bumble.Alpha = 1; });

					var circle = UIBezierPath.FromOval (new CGRect(10 - dataPointRadius*2, 10 - dataPointRadius*2, dataPointRadius * 4, dataPointRadius * 4));
					var fill =  view.Layer.Sublayers[0] as CAShapeLayer;
					fill.Path = circle.CGPath;
					fill.LineJoin = CAShapeLayer.JoinRound;

					selectedView = view;
				}));
			}
		}

		void deselectSelectedView() {
			if (selectedView.Subviews.Length > 0) { 
				var v = selectedView.Subviews [0] as BumbleView;
				UIView.AnimateNotify (0.3f, ()=>{ 
					var circle = UIBezierPath.FromOval (new CGRect(10 - dataPointRadius, 10 - dataPointRadius, dataPointRadius * 2, dataPointRadius * 2));
					var shape = selectedView.Layer.Sublayers[0] as CAShapeLayer;
					shape.Path = circle.CGPath;
					shape.LineJoin = CAShapeLayer.JoinRound;
					v.Alpha = 0; 
				}, (bool fini)=>{ 
					if (fini) v.RemoveFromSuperview ();
				});

			}
		}

		float horizontalScale {
			get {
				var scale = 1f;
				var q = Data.Count / horizontalGridStep;

				if (Data.Count > 1) {
					scale = (q * horizontalGridStep) / (Data.Count - 1);
				}
				return scale;
			}
		}

		float minVerticalBound { 
			get { return (float)Math.Min (min, 0); }
		}

		float maxVerticalBound {
			get { return (float)Math.Max (max, 0); }
		}

		void computeBounds() 
		{
			min = float.MaxValue;
			max = -float.MaxValue;

			for (int i = 0; i < Data.Count; i++) {
				var number = Data [i];
				if (number < min) {
					min = number;
				}
				if (number > max) {
					max = number;
				}
			}

			max = getUpperRoundNumber (max, verticalGridStep);

			if (min < 0) {
				nfloat step;

				if (verticalGridStep > 3) {
					step = (nfloat)(Math.Abs (max - min) / (nfloat)(verticalGridStep - 1));
				} else {
					step = (nfloat)Math.Max (Math.Abs (max - min) / 2, Math.Max (Math.Abs (min), Math.Abs (max)));
				}

				step = getUpperRoundNumber (step, verticalGridStep);

				nfloat newMin, newMax;

				if (Math.Abs (min) > Math.Abs (max)) {
					int m = (int)Math.Ceiling (Math.Abs (min) / step);
					newMin = step * m * (min > 0 ? 1 : -1);
					newMax = step * (verticalGridStep - m) * (max > 0 ? 1 : -1);
				} else {
					int m = (int)Math.Ceiling (Math.Abs (max) / step);
					newMax = step * m * (max > 0 ? 1 : -1);
					newMin = step * (verticalGridStep - m) * (min > 0 ? 1 : -1);
				}


				if(min < newMin) {
					newMin -= step;
					newMax -=  step;
				}

				if(max > newMax + step) {
					newMin += step;
					newMax +=  step;
				}

				min = newMin;
				max = newMax;

				if(max < min) {
					nfloat tmp = max;
					max = min;
					min = tmp;
				}
			}
		}

		nfloat getUpperRoundNumber(nfloat value, int gridStep)
		{
			if (value <= 0) {
				return 0;
			}

			nfloat logValue = (nfloat)Math.Log10 (value);
			nfloat scale = (nfloat)Math.Pow (10, Math.Floor (logValue));
			nfloat n = (nfloat)Math.Ceiling (value / scale * 4);

			int tmp = (int)(n % gridStep);
			if (tmp != 0) {
				n += gridStep - tmp;
			}
			return n * scale / 4f;
		}

		CGPoint getPointForIndex(int idx, nfloat scale)
		{
			if (idx >= Data.Count) {
				return CGPoint.Empty;
			}
			var number = Data [Math.Abs (idx)];
			if (Data.Count < 2) {
				return new CGPoint (margin, axisHeight + margin - number * scale);
			} else {
				return new CGPoint (margin + idx * (axisWidth / (Data.Count - 1)), axisHeight + margin - number * scale);
			}
		}

		UIBezierPath getLinePath(nfloat scale, bool smoothed, bool closed) 
		{
			var path = UIBezierPath.Create ();

			if (smoothed) {
				for (int i = 0; i < Data.Count-1; i++) {
					var controlPoint = new CGPoint[2];
					var p = getPointForIndex (i, scale);

					if (i==0) {
						path.MoveTo (p);
					}

					CGPoint nextPoint, previousPoint, m;

					nextPoint = getPointForIndex (i + 1, scale);
					previousPoint = getPointForIndex (i - 1, scale);
					m = CGPoint.Empty;

					if (i>0) {
						m.X = (nextPoint.X - previousPoint.X) / 2;
						m.Y = (nextPoint.Y - previousPoint.Y) / 2;
					} else {
						m.X = (nextPoint.X - p.X) / 2;
						m.Y = (nextPoint.Y - p.Y) / 2;
					}

					controlPoint[0].X = p.X + m.X * bezierSmoothingTension;
					controlPoint[0].Y = p.Y + m.Y * bezierSmoothingTension;

					// Second control point
					nextPoint = getPointForIndex(i + 2, scale);
					previousPoint = getPointForIndex(i, scale);
					p = getPointForIndex(i + 1, scale);
					m = CGPoint.Empty;

					if(i < Data.Count - 2) {
						m.X = (nextPoint.X - previousPoint.X) / 2;
						m.Y = (nextPoint.Y - previousPoint.Y) / 2;
					} else {
						m.X = (p.X - previousPoint.X) / 2;
						m.Y = (p.Y - previousPoint.Y) / 2;
					}

					controlPoint[1].X = p.X - m.X * bezierSmoothingTension;
					controlPoint[1].Y = p.Y - m.Y * bezierSmoothingTension;

					path.AddCurveToPoint (p, controlPoint[0], controlPoint[1]);
				}
			} else {
				for (int i = 0; i < Data.Count; i++) {
					if (i>0) {
						path.AddLineTo (getPointForIndex (i, scale));
					} else {
						path.MoveTo (getPointForIndex (i, scale));
					}
				}
			}

			if (closed) {
				path.AddLineTo (getPointForIndex (Data.Count - 1, scale));
				path.AddLineTo (getPointForIndex (Data.Count - 1, 0));
				path.AddLineTo (getPointForIndex (0, 0));
				path.AddLineTo (getPointForIndex (0, scale));
			}
			return path;
		}
	}
}

