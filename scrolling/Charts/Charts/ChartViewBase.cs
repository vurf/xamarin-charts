using System;
using UIKit;
using Foundation;
using CoreGraphics;
using System.Collections.Generic;

namespace scrolling
{
	public interface IChartViewDelegate 
	{
		/// Called when a value has been selected inside the chart.
		/// - parameter entry: The selected Entry.
		/// - parameter dataSetIndex: The index in the datasets array of the data object the Entrys DataSet is in.
		void chartValueSelected(ChartViewBase chartView, ChartDataEntry entry, int dataSetIndex, ChartHighlight highlight);

		// Called when nothing has been selected or an "un-select" has been made.
		void chartValueNothingSelected(ChartViewBase chartView);
	
		// Callbacks when the chart is scaled / zoomed via pinch zoom gesture.
		void chartScaled(ChartViewBase chartView, nfloat scaleX, nfloat scaleY);

		// Callbacks when the chart is moved / translated via drag gesture.
		void chartTranslated(ChartViewBase chartView, nfloat dX, nfloat dY);
	}

	public class ChartViewBase : UIView
	{

		/// the default value formatter
		public NSNumberFormatter  _defaultValueFormatter = ChartUtils.defaultValueFormatter();

			/// object that holds all data that was originally set for the chart, before it was modified or any filtering algorithms had been applied
		public ChartData _data;

			/// Flag that indicates if highlighting per tap (touch) is enabled
		private bool _highlightPerTapEnabled = true;

			/// If set to true, chart continues to scroll after touch up
		public bool dragDecelerationEnabled = true;

			/// Deceleration friction coefficient in [0 ; 1] interval, higher values indicate that speed will decrease slowly, for example if it set to 0, it will stop immediately.
			/// 1 is an invalid value, and will be converted to 0.999 automatically.
		private nfloat _dragDecelerationFrictionCoef = 0.9f;

			/// Font object used for drawing the description text (by default in the bottom right corner of the chart)
		public UIFont descriptionFont = UIFont.FromName ("HelveticaNeue", 9.0f);

			/// Text color used for drawing the description text
		public UIColor descriptionTextColor = UIColor.Black;

			/// Text align used for drawing the description text
		public UITextAlignment descriptionTextAlign = UITextAlignment.Right;

			/// Custom position for the description text in pixels on the screen.
		public CGPoint descriptionTextPosition = new CGPoint();

			/// font object for drawing the information text when there are no values in the chart
		public UIFont infoFont  = UIFont.FromName("HelveticaNeue", 12.0f);
		public UIColor infoTextColor = UIColor.FromRGB(247.0f/255.0f, 189.0f/255.0f, 51.0f/255.0f); // orange

			/// description text that appears in the bottom right corner of the chart
		public string descriptionText = "Description";

			/// if true, units are drawn next to the values in the chart
		internal bool _drawUnitInChart = false;

			/// the number of x-values the chart displays
			internal nfloat _deltaX = 1.0f;

		internal double _chartXMin = 0.0d;
		internal double _chartXMax = 0.0d;

			/// the legend object containing all data associated with the legend
		internal ChartLegend _legend;

			/// delegate to receive chart events
		public ChartViewDelegate _delegate;

			/// text that is displayed when the chart is empty
		public string noDataText = "No chart data available.";

			/// text that is displayed when the chart is empty that describes why the chart is empty
		public string noDataTextDescription;

		internal ChartLegendRenderer _legendRenderer;

			/// object responsible for rendering the data
		public ChartDataRendererBase renderer;

		public ChartHighlighter highlighter;

			/// object that manages the bounds and drawing constraints of the chart
		internal ChartViewPortHandler _viewPortHandler;

			/// object responsible for animations
		internal ChartAnimator _animator;

			/// flag that indicates if offsets calculation has already been done or not
		private bool _offsetsCalculated = false;

			/// array of Highlight objects that reference the highlighted slices in the chart
		internal List<ChartHighlight> _indicesToHighlight = new List<ChartHighlight>();

			/// if set to true, the marker is drawn when a value is clicked
		public bool drawMarkers = true;

			/// the view that represents the marker
		public ChartMarker marker;

		private bool _interceptTouchEvents = false;

			/// An extra offset to be appended to the viewport's top
		public nfloat extraTopOffset  = 0.0f;

			/// An extra offset to be appended to the viewport's right
		public nfloat extraRightOffset  = 0.0f;

			/// An extra offset to be appended to the viewport's bottom
		public nfloat extraBottomOffset  = 0.0f;

			/// An extra offset to be appended to the viewport's left
		public nfloat extraLeftOffset  = 0.0f;

		public void setExtraOffsets(nfloat left,nfloat top, nfloat right, nfloat bottom)
		{
			extraLeftOffset = left;
			extraTopOffset = top;
			extraRightOffset = right;
			extraBottomOffset = bottom;
		}

			// MARK: - Initializers
		public ChartViewBase(CGRect frame) : base (frame)
		{
			BackgroundColor = UIColor.Clear;
			initialize ();
		}

		public ChartViewBase(IntPtr handle) : base(handle) {
			initialize ();
		}


		internal void initialize()
		{
			_animator = new ChartAnimator ();
			_animator.Delegate = this;

			_viewPortHandler = new ChartViewPortHandler();
			_viewPortHandler.setChartDimens(bounds.size.width, bounds.size.height);

			_legend = new ChartLegend();
			_legendRenderer = ChartLegendRenderer(_viewPortHandler, _legend);

			AddObserver (this, "bounds", NSKeyValueObservingOptions.New, null);
			AddObserver (this, "frame", NSKeyValueObservingOptions.New, null);
		}

			// MARK: - ChartViewBase

			/// The data for the chart
		public ChartData data
		{
			get { return _data; }
			set
			{
				if (value == null)
				{
					Console.WriteLine ("Charts: data argument is nil on setData()" + terminator);
					return null; 
				}

				_offsetsCalculated = false;
				_data = value;

					// calculate how many digits are needed
				calculateFormatter(_data.getYMin(), _data.getYMax());
				notifyDataSetChanged ();
			}
		}

			/// Clears the chart from all data (sets it to null) and refreshes it (by calling setNeedsDisplay()).
		public void clear()
		{
			_data = null;
			_indicesToHighlight.removeAll ();
			SetNeedsDisplay ();
		}

			/// Removes all DataSets (and thereby Entries) from the chart. Does not remove the x-values. Also refreshes the chart by calling setNeedsDisplay().
		public void clearValues()
		{
			if (_data != null)
				_data.clearValues ();
			SetNeedsDisplay ();
		}

			/// - returns: true if the chart is empty (meaning it's data object is either null or contains no entries).
		public Bool isEmpty()
		{
			if (_data == null)
				return true;
			else
				return _data.yValCount <= 0;
		}

			/// Lets the chart know its underlying data has changed and should perform all necessary recalculations.
			/// It is crucial that this method is called everytime data is changed dynamically. Not calling this method can lead to crashes or unexpected behaviour.
		public void notifyDataSetChanged()
		{
			fatalError("notifyDataSetChanged() cannot be called on ChartViewBase")
		}

			/// calculates the offsets of the chart to the border depending on the position of an eventual legend or depending on the length of the y-axis and x-axis labels and their position
		internal void calculateOffsets()
		{
			fatalError("calculateOffsets() cannot be called on ChartViewBase")
		}

			/// calcualtes the y-min and y-max value and the y-delta and x-delta value
		internal void calcMinMax()
		{
			fatalError("calcMinMax() cannot be called on ChartViewBase")
		}

			/// calculates the required number of digits for the values that might be drawn in the chart (if enabled), and creates the default value formatter
		internal void calculateFormatter(double min, double max)
		{
			// check if a custom formatter is set or not
			var reference = 0d;
			if (_data == null || _data.xValCount < 2)
			{
				var absMin = Math.Abs (min);
				var absMax = Math.Abs (max);
				reference = absMin > absMax ? absMin : absMax;
			}
			else
			{
				reference = Math.Abs (max - min);
			}

			var digits = ChartUtils.decimals (reference);

			_defaultValueFormatter.maximumFractionDigits = digits;
			_defaultValueFormatter.minimumFractionDigits = digits;
		}

		public override void Draw (CGRect rect)
		{

			using (var c = UIGraphics.GetCurrentContext ()) {
				var frame = Bounds;

				if (_data == null) {
					c.SaveState ();
					// if no data, inform the user
					ChartUtils.drawText(c, noDataText, 
						new CGPoint(frame.Width / 2.0, frame.Height / 2.0),  
						UITextAlignment.Center, [NSFontAttributeName: infoFont, NSForegroundColorAttributeName: infoTextColor]);

					if (!string.IsNullOrEmpty (noDataTextDescription))
					{   
						var textOffset = infoFont.lineHeight;
						ChartUtils.drawText(context, noDataTextDescription, 
							new CGPoint(frame.Width / 2.0, frame.Height / 2.0 + textOffset), 
							UITextAlignment.Center, [NSFontAttributeName: infoFont, NSForegroundColorAttributeName: infoTextColor]);
					}
					return;
				}

				if (!_offsetsCalculated)
				{
					calculateOffsets();
					_offsetsCalculated = true;
				}
			};

				
		}



			/// draws the description text in the bottom right corner of the chart
		internal void drawDescription(CGContext context)
		{
			if (descriptionText.lengthOfBytesUsingEncoding (NSUTF16StringEncoding) == 0)
				return;

			var frame = Bounds;

			var attrs = new string[] { };[String : AnyObject]()

				var font = descriptionFont

				if (font == nil)
				{
					#if os(tvOS)
					// 23 is the smallest recommened font size on the TV
					font = UIFont.systemFontOfSize(23, weight: UIFontWeightMedium)
					#else
					font = UIFont.systemFontOfSize(UIFont.systemFontSize())
					#endif
				}

					attrs[NSFontAttributeName] = font
					attrs[NSForegroundColorAttributeName] = descriptionTextColor

				if descriptionTextPosition == nil
				{
					ChartUtils.drawText(
						context: context,
						text: descriptionText,
						point: CGPoint(
							x: frame.width - _viewPortHandler.offsetRight - 10.0,
							y: frame.height - _viewPortHandler.offsetBottom - 10.0 - (font?.lineHeight ?? 0.0)),
						align: descriptionTextAlign,
						attributes: attrs)
				}
				else
				{
					ChartUtils.drawText(
						context: context,
						text: descriptionText,
						point: descriptionTextPosition!,
						align: descriptionTextAlign,
						attributes: attrs)
				}
				}

			// MARK: - Highlighting

			/// - returns: the array of currently highlighted values. This might an empty if nothing is highlighted.
			public var highlighted: [ChartHighlight]
		{
			return _indicesToHighlight
			}

			/// Set this to false to prevent values from being highlighted by tap gesture.
			/// Values can still be highlighted via drag or programmatically.
			/// - default: true
			public var highlightPerTapEnabled: Bool
		{
			get { return _highlightPerTapEnabled }
			set { _highlightPerTapEnabled = newValue }
		}

			/// Returns true if values can be highlighted via tap gesture, false if not.
			public var isHighLightPerTapEnabled: Bool
		{
			return highlightPerTapEnabled
			}

			/// Checks if the highlight array is null, has a length of zero or if the first object is null.
			/// - returns: true if there are values to highlight, false if there are no values to highlight.
			public func valuesToHighlight() -> Bool
		{
			return _indicesToHighlight.count > 0
			}

			/// Highlights the values at the given indices in the given DataSets. Provide
			/// null or an empty array to undo all highlighting. 
			/// This should be used to programmatically highlight values. 
			/// This DOES NOT generate a callback to the delegate.
			public func highlightValues(highs: [ChartHighlight]?)
		{
			// set the indices to highlight
			_indicesToHighlight = highs ?? [ChartHighlight]()

				if (_indicesToHighlight.isEmpty)
				{
					self.lastHighlighted = nil
				}
				else
				{
					self.lastHighlighted = _indicesToHighlight[0];
				}

					// redraw the chart
					setNeedsDisplay()
				}


			/// Highlights the values represented by the provided Highlight object
			/// This DOES NOT generate a callback to the delegate.
			/// - parameter highlight: contains information about which entry should be highlighted
			public func highlightValue(highlight: ChartHighlight?)
		{
			highlightValue(highlight: highlight, callDelegate: false)
		}

			/// Highlights the value at the given x-index in the given DataSet.
			/// Provide -1 as the x-index to undo all highlighting.
			public func highlightValue(xIndex xIndex: Int, dataSetIndex: Int, callDelegate: Bool)
		{
			if (xIndex < 0 || dataSetIndex < 0 || xIndex >= _data.xValCount || dataSetIndex >= _data.dataSetCount)
			{
				highlightValue(highlight: nil, callDelegate: callDelegate)
			}
			else
			{
				highlightValue(highlight: ChartHighlight(xIndex: xIndex, dataSetIndex: dataSetIndex), callDelegate: callDelegate)
			}
		}

			/// Highlights the value selected by touch gesture.
			public func highlightValue(highlight highlight: ChartHighlight?, callDelegate: Bool)
		{
			var entry: ChartDataEntry?
			var h = highlight

				if (h == nil)
				{
					_indicesToHighlight.removeAll(keepCapacity: false)
				}
				else
				{
					// set the indices to highlight
					entry = _data.getEntryForHighlight(h!)
						if (entry === nil || entry!.xIndex != h?.xIndex)
						{
							h = nil
								entry = nil
								_indicesToHighlight.removeAll(keepCapacity: false)
						}
						else
						{
							_indicesToHighlight = [h!]
						}
						}

					if (callDelegate && delegate != nil)
					{
						if (h == nil)
						{
							delegate!.chartValueNothingSelected?(self)
						}
						else
						{
							// notify the listener
							delegate!.chartValueSelected?(self, entry: entry!, dataSetIndex: h!.dataSetIndex, highlight: h!)
						}
					}

						// redraw the chart
						setNeedsDisplay()
					}

			/// The last value that was highlighted via touch.
			public var lastHighlighted: ChartHighlight?

			// MARK: - Markers

			/// draws all MarkerViews on the highlighted positions
			internal func drawMarkers(context context: CGContext)
		{
			// if there is no marker view or drawing marker is disabled
			if (marker === nil || !drawMarkers || !valuesToHighlight())
			{
				return
				}

			for (var i = 0, count = _indicesToHighlight.count; i < count; i++)
			{
				let highlight = _indicesToHighlight[i]
					let xIndex = highlight.xIndex

					if (xIndex <= Int(_deltaX) && xIndex <= Int(_deltaX * _animator.phaseX))
					{
						let e = _data.getEntryForHighlight(highlight)
							if (e === nil || e!.xIndex != highlight.xIndex)
							{
								continue
							}

								let pos = getMarkerPosition(entry: e!, highlight: highlight)

								// check bounds
							if (!_viewPortHandler.isInBounds(x: pos.x, y: pos.y))
							{
								continue
							}

								// callbacks to update the content
								marker!.refreshContent(entry: e!, highlight: highlight)

								let markerSize = marker!.size
							if (pos.y - markerSize.height <= 0.0)
							{
								let y = markerSize.height - pos.y
									marker!.draw(context: context, point: CGPoint(x: pos.x, y: pos.y + y))
							}
							else
							{
								marker!.draw(context: context, point: pos)
							}
							}
					}
		}

			/// - returns: the actual position in pixels of the MarkerView for the given Entry in the given DataSet.
			public func getMarkerPosition(entry entry: ChartDataEntry, highlight: ChartHighlight) -> CGPoint
		{
			fatalError("getMarkerPosition() cannot be called on ChartViewBase")
		}

			// MARK: - Animation

			/// - returns: the animator responsible for animating chart values.
			public var animator: ChartAnimator!
		{
			return _animator
			}


			public func animate(xAxisDuration xAxisDuration: NSTimeInterval, yAxisDuration: NSTimeInterval, easingX: ChartEasingFunctionBlock?, easingY: ChartEasingFunctionBlock?)
		{
			_animator.animate(xAxisDuration: xAxisDuration, yAxisDuration: yAxisDuration, easingX: easingX, easingY: easingY)
		}

			/// Animates the drawing / rendering of the chart on both x- and y-axis with the specified animation time.
			/// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
			/// - parameter xAxisDuration: duration for animating the x axis
			/// - parameter yAxisDuration: duration for animating the y axis
			/// - parameter easingOptionX: the easing function for the animation on the x axis
			/// - parameter easingOptionY: the easing function for the animation on the y axis
			public func animate(xAxisDuration xAxisDuration: NSTimeInterval, yAxisDuration: NSTimeInterval, easingOptionX: ChartEasingOption, easingOptionY: ChartEasingOption)
		{
			_animator.animate(xAxisDuration: xAxisDuration, yAxisDuration: yAxisDuration, easingOptionX: easingOptionX, easingOptionY: easingOptionY)
		}

			/// Animates the drawing / rendering of the chart on both x- and y-axis with the specified animation time.
			/// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
			/// - parameter xAxisDuration: duration for animating the x axis
			/// - parameter yAxisDuration: duration for animating the y axis
			/// - parameter easing: an easing function for the animation
			public func animate(xAxisDuration xAxisDuration: NSTimeInterval, yAxisDuration: NSTimeInterval, easing: ChartEasingFunctionBlock?)
		{
			_animator.animate(xAxisDuration: xAxisDuration, yAxisDuration: yAxisDuration, easing: easing)
		}

			/// Animates the drawing / rendering of the chart on both x- and y-axis with the specified animation time.
			/// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
			/// - parameter xAxisDuration: duration for animating the x axis
			/// - parameter yAxisDuration: duration for animating the y axis
			/// - parameter easingOption: the easing function for the animation
			public func animate(xAxisDuration xAxisDuration: NSTimeInterval, yAxisDuration: NSTimeInterval, easingOption: ChartEasingOption)
		{
			_animator.animate(xAxisDuration: xAxisDuration, yAxisDuration: yAxisDuration, easingOption: easingOption)
		}

			/// Animates the drawing / rendering of the chart on both x- and y-axis with the specified animation time.
			/// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
			/// - parameter xAxisDuration: duration for animating the x axis
			/// - parameter yAxisDuration: duration for animating the y axis
			public func animate(xAxisDuration xAxisDuration: NSTimeInterval, yAxisDuration: NSTimeInterval)
		{
			_animator.animate(xAxisDuration: xAxisDuration, yAxisDuration: yAxisDuration)
		}

			/// Animates the drawing / rendering of the chart the x-axis with the specified animation time.
			/// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
			/// - parameter xAxisDuration: duration for animating the x axis
			/// - parameter easing: an easing function for the animation
			public func animate(xAxisDuration xAxisDuration: NSTimeInterval, easing: ChartEasingFunctionBlock?)
		{
			_animator.animate(xAxisDuration: xAxisDuration, easing: easing)
		}

			/// Animates the drawing / rendering of the chart the x-axis with the specified animation time.
			/// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
			/// - parameter xAxisDuration: duration for animating the x axis
			/// - parameter easingOption: the easing function for the animation
			public func animate(xAxisDuration xAxisDuration: NSTimeInterval, easingOption: ChartEasingOption)
		{
			_animator.animate(xAxisDuration: xAxisDuration, easingOption: easingOption)
		}

			/// Animates the drawing / rendering of the chart the x-axis with the specified animation time.
			/// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
			/// - parameter xAxisDuration: duration for animating the x axis
			public func animate(xAxisDuration xAxisDuration: NSTimeInterval)
		{
			_animator.animate(xAxisDuration: xAxisDuration)
		}

			/// Animates the drawing / rendering of the chart the y-axis with the specified animation time.
			/// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
			/// - parameter yAxisDuration: duration for animating the y axis
			/// - parameter easing: an easing function for the animation
			public func animate(yAxisDuration yAxisDuration: NSTimeInterval, easing: ChartEasingFunctionBlock?)
		{
			_animator.animate(yAxisDuration: yAxisDuration, easing: easing)
		}

			/// Animates the drawing / rendering of the chart the y-axis with the specified animation time.
			/// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
			/// - parameter yAxisDuration: duration for animating the y axis
			/// - parameter easingOption: the easing function for the animation
			public func animate(yAxisDuration yAxisDuration: NSTimeInterval, easingOption: ChartEasingOption)
		{
			_animator.animate(yAxisDuration: yAxisDuration, easingOption: easingOption)
		}

			/// Animates the drawing / rendering of the chart the y-axis with the specified animation time.
			/// If `animate(...)` is called, no further calling of `invalidate()` is necessary to refresh the chart.
			/// - parameter yAxisDuration: duration for animating the y axis
			public func animate(yAxisDuration yAxisDuration: NSTimeInterval)
		{
			_animator.animate(yAxisDuration: yAxisDuration)
		}

			// MARK: - Accessors

			/// - returns: the current y-max value across all DataSets
			public var chartYMax: Double
		{
			return _data.yMax
			}

			/// - returns: the current y-min value across all DataSets
			public var chartYMin: Double
		{
			return _data.yMin
			}

			public var chartXMax: Double
		{
			return _chartXMax
			}

			public var chartXMin: Double
		{
			return _chartXMin
			}

			public var xValCount: Int
		{
			return _data.xValCount
			}

			/// - returns: the total number of (y) values the chart holds (across all DataSets)
			public var valueCount: Int
		{
			return _data.yValCount
			}

			/// *Note: (Equivalent of getCenter() in MPAndroidChart, as center is already a standard in iOS that returns the center point relative to superview, and MPAndroidChart returns relative to self)*
			/// - returns: the center point of the chart (the whole View) in pixels.
			public var midPoint: CGPoint
		{
			let bounds = self.bounds
				return CGPoint(x: bounds.origin.x + bounds.size.width / 2.0, y: bounds.origin.y + bounds.size.height / 2.0)
				}

			public func setDescriptionTextPosition(x x: CGFloat, y: CGFloat)
		{
			descriptionTextPosition = CGPoint(x: x, y: y)
		}

			/// - returns: the center of the chart taking offsets under consideration. (returns the center of the content rectangle)
			public var centerOffsets: CGPoint
		{
			return _viewPortHandler.contentCenter
			}

			/// - returns: the Legend object of the chart. This method can be used to get an instance of the legend in order to customize the automatically generated Legend.
			public var legend: ChartLegend
		{
			return _legend
			}

			/// - returns: the renderer object responsible for rendering / drawing the Legend.
			public var legendRenderer: ChartLegendRenderer!
		{
			return _legendRenderer
			}

			/// - returns: the rectangle that defines the borders of the chart-value surface (into which the actual values are drawn).
			public var contentRect: CGRect
		{
			return _viewPortHandler.contentRect
			}

			/// - returns: the x-value at the given index
			public func getXValue(index: Int) -> String!
		{
			if (_data == nil || _data.xValCount <= index)
			{
				return nil
				}
			else
			{
				return _data.xVals[index]
				}
		}

			/// Get all Entry objects at the given index across all DataSets.
			public func getEntriesAtIndex(xIndex: Int) -> [ChartDataEntry]
		{
			var vals = [ChartDataEntry]()

				for (var i = 0, count = _data.dataSetCount; i < count; i++)
				{
					let set = _data.getDataSetByIndex(i)
						let e = set.entryForXIndex(xIndex)
						if (e !== nil)
						{
							vals.append(e!)
						}
						}

					return vals
					}

			/// - returns: the ViewPortHandler of the chart that is responsible for the
			/// content area of the chart and its offsets and dimensions.
			public var viewPortHandler: ChartViewPortHandler!
		{
			return _viewPortHandler
			}

			/// - returns: the bitmap that represents the chart.
			public func getChartImage(transparent transparent: Bool) -> UIImage
		{
			UIGraphicsBeginImageContextWithOptions(bounds.size, opaque || !transparent, UIScreen.mainScreen().scale)

			let context = UIGraphicsGetCurrentContext()
				let rect = CGRect(origin: CGPoint(x: 0, y: 0), size: bounds.size)

				if (opaque || !transparent)
				{
					// Background color may be partially transparent, we must fill with white if we want to output an opaque image
					CGContextSetFillColorWithColor(context, UIColor.whiteColor().CGColor)
					CGContextFillRect(context, rect)

					if (self.backgroundColor !== nil)
					{
						CGContextSetFillColorWithColor(context, self.backgroundColor?.CGColor)
						CGContextFillRect(context, rect)
					}
				}

				if let context = context
				{
					layer.renderInContext(context)
				}

					let image = UIGraphicsGetImageFromCurrentImageContext()

					UIGraphicsEndImageContext()

					return image
					}

			public enum ImageFormat
		{
			case JPEG
			case PNG
		}

			/// Saves the current chart state with the given name to the given path on
			/// the sdcard leaving the path empty "" will put the saved file directly on
			/// the SD card chart is saved as a PNG image, example:
			/// saveToPath("myfilename", "foldername1/foldername2")
			///
			/// - parameter filePath: path to the image to save
			/// - parameter format: the format to save
			/// - parameter compressionQuality: compression quality for lossless formats (JPEG)
			///
			/// - returns: true if the image was saved successfully
			public func saveToPath(path: String, format: ImageFormat, compressionQuality: Double) -> Bool
		{
			let image = getChartImage(transparent: format != .JPEG)

				var imageData: NSData!
				switch (format)
			{
			case .PNG:
				imageData = UIImagePNGRepresentation(image)
					break

			case .JPEG:
				imageData = UIImageJPEGRepresentation(image, CGFloat(compressionQuality))
					break
			}

				return imageData.writeToFile(path, atomically: true)
				}

			#if !os(tvOS)
			/// Saves the current state of the chart to the camera roll
			public func saveToCameraRoll()
			{
			UIImageWriteToSavedPhotosAlbum(getChartImage(transparent: false), nil, nil, nil)
			}
			#endif

			internal typealias VoidClosureType = () -> ()
			internal var _sizeChangeEventActions = [VoidClosureType]()

			public override func observeValueForKeyPath(keyPath: String?, ofObject object: AnyObject?, change: [String : AnyObject]?, context: UnsafeMutablePointer<Void>)
		{
			if (keyPath == "bounds" || keyPath == "frame")
			{
				let bounds = self.bounds

					if (_viewPortHandler !== nil &&
						(bounds.size.width != _viewPortHandler.chartWidth ||
							bounds.size.height != _viewPortHandler.chartHeight))
					{
						_viewPortHandler.setChartDimens(width: bounds.size.width, height: bounds.size.height)

						// Finish any pending viewport changes
						while (!_sizeChangeEventActions.isEmpty)
						{
							_sizeChangeEventActions.removeAtIndex(0)()
						}

						notifyDataSetChanged()
					}
					}
		}

			public func clearPendingViewPortChanges()
		{
			_sizeChangeEventActions.removeAll(keepCapacity: false)
		}

			/// **default**: true
			/// - returns: true if chart continues to scroll after touch up, false if not.
			public var isDragDecelerationEnabled: Bool
		{
			return dragDecelerationEnabled
			}

			/// Deceleration friction coefficient in [0 ; 1] interval, higher values indicate that speed will decrease slowly, for example if it set to 0, it will stop immediately.
			/// 1 is an invalid value, and will be converted to 0.999 automatically.
			/// 
			/// **default**: true
			public var dragDecelerationFrictionCoef: CGFloat
		{
			get
			{
				return _dragDecelerationFrictionCoef
				}
			set
			{
				var val = newValue
					if (val < 0.0)
					{
						val = 0.0
					}
					if (val >= 1.0)
					{
						val = 0.999
					}

						_dragDecelerationFrictionCoef = val
					}
		}

			// MARK: - ChartAnimatorDelegate

			public func chartAnimatorUpdated(chartAnimator: ChartAnimator)
		{
			setNeedsDisplay()
		}

			public func chartAnimatorStopped(chartAnimator: ChartAnimator)
		{

		}

			// MARK: - Touches

			public override func touchesBegan(touches: Set<UITouch>, withEvent event: UIEvent?)
		{
			if (!_interceptTouchEvents)
			{
				super.touchesBegan(touches, withEvent: event)
			}
		}

			public override func touchesMoved(touches: Set<UITouch>, withEvent event: UIEvent?)
		{
			if (!_interceptTouchEvents)
			{
				super.touchesMoved(touches, withEvent: event)
			}
		}

			public override func touchesEnded(touches: Set<UITouch>, withEvent event: UIEvent?)
		{
			if (!_interceptTouchEvents)
			{
				super.touchesEnded(touches, withEvent: event)
			}
		}

			public override func touchesCancelled(touches: Set<UITouch>?, withEvent event: UIEvent?)
		{
			if (!_interceptTouchEvents)
			{
				super.touchesCancelled(touches, withEvent: event)
			}
		}

	}
}

