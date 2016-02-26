using System;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using Foundation;

namespace scrolling
{
    public class ChartLegend : ChartComponentBase
    {


        public enum ChartLegendPosition
        {
            RightOfChart,
            RightOfChartCenter,
            RightOfChartInside,
            LeftOfChart,
            LeftOfChartCenter,
            LeftOfChartInside,
            BelowChartLeft,
            BelowChartRight,
            BelowChartCenter,
            AboveChartLeft,
            AboveChartRight,
            AboveChartCenter,
            PiechartCenter
        }

        public enum ChartLegendForm
        {
            Square,
            Circle,
            Line
        }

        public enum ChartLegendDirection
        {
            LeftToRight,
            RightToLeft
        }

        /// the legend colors array, each color is for the form drawn at the same index
        public List<UIColor> colors = new List<UIColor>();

        // the legend text array. a nil label will start a group.
        public List<string> labels = new List<string>();

        internal List<UIColor> _extraColors = new List<UIColor>();
        internal List<string> _extraLabels = new List<string>();

        /// colors that will be appended to the end of the colors array after calculating the legend.
        public List<UIColor> extraColors
        {
            get { return _extraColors; }
        }

        /// labels that will be appended to the end of the labels array after calculating the legend. a nil label will start a group.
        public List<string> extraLabels
        {
            get { return _extraLabels; }
        }

        /// Are the legend labels/colors a custom value or auto calculated? If false, then it's auto, if true, then custom.
        /// 
        /// **default**: false (automatic legend)
        private bool _isLegendCustom = false;

        public ChartLegendPosition position = ChartLegendPosition.BelowChartLeft;
        public ChartLegendDirection direction = ChartLegendDirection.LeftToRight;

        public UIFont font = UIFont.SystemFontOfSize(10.0f);
        public UIColor textColor = UIColor.Black;

        public ChartLegendForm form = ChartLegendForm.Square;
        public nfloat formSize = 8.0f;
        public nfloat formLineWidth = 1.5f;

        public nfloat xEntrySpace = 6.0f;
        public nfloat yEntrySpace = 0.0f;
        public nfloat formToTextSpace = 5.0f;
        public nfloat stackSpace = 3.0f;

        public List<CGSize> calculatedLabelSizes = new List<CGSize>();
        public List<bool> calculatedLabelBreakPoints = new List<bool>();
        public List<CGSize> calculatedLineSizes = new List<CGSize>();

        public ChartLegend()
        {
            xOffset = 5.0f;
            yOffset = 4.0f;
        }

        public ChartLegend(List<UIColor> _colors, List<string> _labels)
        {
            colors = _colors;
            labels = _labels;
        }

        public ChartLegend(List<NSObject> colors, List<NSObject> labels)
        {
            colorsObjc = colors;
            labelsObjc = labels;
        }

        public CGSize getMaximumEntrySize(UIFont font)
        {
            var maxW = 0.0f;
            var maxH = 0.0f;

            var labels = this.labels;
            for (var i = 0; i < labels.Count; i++)
            {
                if (labels[i] == null)
                    continue;

                var size = labels[i].StringSize(font);
                if (size.Width > maxW)
                    maxW = (float) size.Width;

                if (size.Height > maxH)
                    maxH = (float) size.Height;
            }

            return new CGSize(maxW + formSize + formToTextSpace, maxH);
        }

        public string getLabel(int index)
        {
            return labels[index];
        }

        public CGSize getFullSize(UIFont labelFont)
        {
            var width = 0.0f;
            var height = 0.0f;

            var labels = this.labels;
            for (var i = 0, count = labels.Count; i < count; i++)
            {
                if (labels[i] != null)
                {
                    // make a step to the left
                    if (colors[i] != null)
                        width += formSize + formToTextSpace;

                    var size = labels[i].StringSize(labelFont);

                    width += size.Width;
                    height += size.Height;

                    if (i < count - 1)
                    {
                        width += xEntrySpace;
                        height += yEntrySpace;
                    }
                }
                else
                {
                    width += formSize + stackSpace;
                    if (i < count - 1)
                        width += stackSpace;
                }
            }

            return new CGSize(width, height);
        }

        public nfloat neededWidth = 0.0f;
        public nfloat neededHeight = 0.0f;
        public nfloat textWidthMax = 0.0f;
        public nfloat textHeightMax = 0.0f;

        /// flag that indicates if word wrapping is enabled
        /// this is currently supported only for: `BelowChartLeft`, `BelowChartRight`, `BelowChartCenter`.
        /// note that word wrapping a legend takes a toll on performance.
        /// you may want to set maxSizePercent when word wrapping, to set the point where the text wraps.
        /// 
        /// **default**: false
        public bool wordWrapEnabled = false;

        /// if this is set, then word wrapping the legend is enabled.
        public bool isWordWrapEnabled
        {
            get { return wordWrapEnabled; }
        }

        /// The maximum relative size out of the whole chart view in percent.
        /// If the legend is to the right/left of the chart, then this affects the width of the legend.
        /// If the legend is to the top/bottom of the chart, then this affects the height of the legend.
        /// If the legend is the center of the piechart, then this defines the size of the rectangular bounds out of the size of the "hole".
        /// 
        /// **default**: 0.95 (95%)
        public nfloat maxSizePercent = 0.95f;

        public void calculateDimensions(UIFont labelFont, ChartViewPortHandler viewPortHandler)
        {
            if (position == ChartLegendPosition.RightOfChart
                || position == ChartLegendPosition.RightOfChartCenter
                || position == ChartLegendPosition.LeftOfChart
                || position == ChartLegendPosition.LeftOfChartCenter
                || position == ChartLegendPosition.PiechartCenter)
            {
                var maxEntrySize = getMaximumEntrySize(labelFont);
                var fullSize = getFullSize(labelFont);

                neededWidth = maxEntrySize.Width;
                neededHeight = fullSize.Height;
                textWidthMax = maxEntrySize.Width;
                textHeightMax = maxEntrySize.Height;
            }
            else if (position == ChartLegendPosition.BelowChartLeft
                     || position == ChartLegendPosition.BelowChartRight
                     || position == ChartLegendPosition.BelowChartCenter
                     || position == ChartLegendPosition.AboveChartLeft
                     || position == ChartLegendPosition.AboveChartRight
                     || position == ChartLegendPosition.AboveChartCenter)
            {
                var labels = this.labels;
                var colors = this.colors;
                var labelCount = labels.Count;

                var labelLineHeight = labelFont.LineHeight;
                var formSize = this.formSize;
                var formToTextSpace = this.formToTextSpace;
                var xEntrySpace = this.xEntrySpace;
                var stackSpace = this.stackSpace;
                var wordWrapEnabled = this.wordWrapEnabled;

                var contentWidth = viewPortHandler.ContentWidth;

                // Prepare arrays for calculated layout
                if (calculatedLabelSizes.Count != labelCount)
                {
                    calculatedLabelSizes = new List<CGSize>(labelCount);
                }

                if (calculatedLabelBreakPoints.Count != labelCount)
                {
                    calculatedLabelBreakPoints = new List<bool>(labelCount);
                }

                calculatedLineSizes.Clear();

                // Start calculating layout
                //FIXME hz
                var labelAttrs = labelFont;
                nfloat maxLineWidth = 0.0f;
                nfloat currentLineWidth = 0.0f;
                nfloat requiredWidth = 0.0f;
                var stackedStartIndex = -1;

                for (var i = 0; i < labelCount; i++)
                {
                    var drawingForm = colors[i] != null;

                    calculatedLabelBreakPoints[i] = false;

                    if (stackedStartIndex == -1)
                    {
                        // we are not stacking, so required width is for this label only
                        requiredWidth = 0.0f;
                    }
                    else
                    {
                        // add the spacing appropriate for stacked labels/forms
                        requiredWidth += stackSpace;
                    }

                    // grouped forms have null labels
                    if (labels[i] != null)
                    {
                        calculatedLabelSizes[i] = labels[i].StringSize(labelAttrs);

                        requiredWidth += drawingForm ? formToTextSpace + formSize : 0.0f;
                        requiredWidth += calculatedLabelSizes[i].Width;
                    }
                    else
                    {
                        calculatedLabelSizes[i] = new CGSize();
                        requiredWidth += drawingForm ? formSize : 0.0f;

                        if (stackedStartIndex == -1)
                        {
                            // mark this index as we might want to break here later
                            stackedStartIndex = i;
                        }
                    }

                    if (labels[i] != null || i == labelCount - 1)
                    {
                        var requiredSpacing = currentLineWidth == 0.0f ? 0.0f : xEntrySpace;

                        if (!wordWrapEnabled || // No word wrapping, it must fit.
                            currentLineWidth == 0.0f || // The line is empty, it must fit.
                            (contentWidth - currentLineWidth >= requiredSpacing + requiredWidth)) // It simply fits
                        {
                            // Expand current line
                            currentLineWidth += requiredSpacing + requiredWidth;
                        }
                        else
                        {
                            // It doesn't fit, we need to wrap a line

                            // Add current line size to array
                            calculatedLineSizes.Add(new CGSize(currentLineWidth, labelLineHeight));
                            maxLineWidth = (nfloat) Math.Max(maxLineWidth, currentLineWidth);

                            // Start a new line
                            calculatedLabelBreakPoints[stackedStartIndex > -1 ? stackedStartIndex : i] = true;
                            currentLineWidth = requiredWidth;
                        }

                        if (i == labelCount - 1)
                        {
                            // Add last line size to array
                            calculatedLineSizes.Add(new CGSize(currentLineWidth, labelLineHeight));
                            maxLineWidth = (nfloat) Math.Max(maxLineWidth, currentLineWidth);
                        }
                    }

                    stackedStartIndex = labels[i] != null ? -1 : stackedStartIndex;
                }

                var maxEntrySize = getMaximumEntrySize(labelFont);

                textWidthMax = maxEntrySize.Width;
                textHeightMax = maxEntrySize.Height;
                neededWidth = maxLineWidth;
                neededHeight = labelLineHeight*calculatedLineSizes.Count +
                               yEntrySpace*
                               (calculatedLineSizes.Count == 0 ? 0 : (calculatedLineSizes.Count - 1));
            }
            else
            {
                var maxEntrySize = getMaximumEntrySize(labelFont);
                var fullSize = getFullSize(labelFont);

                /* RightOfChartInside, LeftOfChartInside */
                neededWidth = fullSize.Width;
                neededHeight = maxEntrySize.Height;
                textWidthMax = maxEntrySize.Width;
                textHeightMax = maxEntrySize.Height;
            }
        }

        /// MARK: - Custom legend

        /// colors and labels that will be appended to the end of the auto calculated colors and labels after calculating the legend.
        /// (if the legend has already been calculated, you will need to call notifyDataSetChanged() to let the changes take effect)
        public void setExtra(List<UIColor> colors, List<string> labels)
        {
            this._extraLabels = labels;
            this._extraColors = colors;
        }

        /// Sets a custom legend's labels and colors arrays.
        /// The colors count should match the labels count.
        /// * Each color is for the form drawn at the same index.
        /// * A nil label will start a group.
        /// * A nil color will avoid drawing a form, and a clearColor will leave a space for the form.
        /// This will disable the feature that automatically calculates the legend labels and colors from the datasets.
        /// Call `resetCustom(...)` to re-enable automatic calculation (and then `notifyDataSetChanged()` is needed).
        public void setCustom(List<UIColor> colors, List<string> labels)
        {
            this.labels = labels;
            this.colors = colors;
            _isLegendCustom = true;
        }

        /// Calling this will disable the custom legend labels (set by `setLegend(...)`). Instead, the labels will again be calculated automatically (after `notifyDataSetChanged()` is called).
        public void resetCustom()
        {
            _isLegendCustom = false;
        }

        /// **default**: false (automatic legend)
        /// - returns: true if a custom legend labels and colors has been set
        public bool isLegendCustom
        {
            get { return _isLegendCustom; }
        }

        /// MARK: - ObjC compatibility

        /// colors that will be appended to the end of the colors array after calculating the legend.
        public List<NSObject> extraColorsObjc
        {
            get { return ChartUtils.bridgedObjCGetUIColorArray(_extraColors); }
        }

        /// labels that will be appended to the end of the labels array after calculating the legend. a nil label will start a group.
        public List<NSObject> extraLabelsObjc
        {
            get { return ChartUtils.bridgedObjCGetStringArray(_extraLabels); }
        }

        /// the legend colors array, each color is for the form drawn at the same index
        /// (ObjC bridging functions, as Swift 1.2 does not bridge optionals in array to `NSNull`s)
        public List<NSObject> colorsObjc
        {
            get { return ChartUtils.bridgedObjCGetUIColorArray(colors); }
            set { colors = ChartUtils.bridgedObjCGetUIColorArray(value); }
        }

        // the legend text array. a nil label will start a group.
        /// (ObjC bridging functions, as Swift 1.2 does not bridge optionals in array to `NSNull`s)
        public List<NSObject> labelsObjc
        {
            get { return ChartUtils.bridgedObjCGetStringArray(labels); }
            set { labels = ChartUtils.bridgedObjCGetStringArray(value); }
        }

        /// colors and labels that will be appended to the end of the auto calculated colors and labels after calculating the legend.
        /// (if the legend has already been calculated, you will need to call `notifyDataSetChanged()` to let the changes take effect)
        public void setExtra(List<NSObject> colors, List<NSObject> labels)
        {
            if (colors.Count != labels.Count)
            {
                Console.WriteLine("ChartLegend:setExtra() - colors array and labels array need to be of same size");
            }

            _extraLabels = ChartUtils.bridgedObjCGetStringArray(labels);
            _extraColors = ChartUtils.bridgedObjCGetUIColorArray(colors);
        }

        /// Sets a custom legend's labels and colors arrays.
        /// The colors count should match the labels count.
        /// * Each color is for the form drawn at the same index.
        /// * A nil label will start a group.
        /// * A nil color will avoid drawing a form, and a clearColor will leave a space for the form.
        /// This will disable the feature that automatically calculates the legend labels and colors from the datasets.
        /// Call `resetLegendToAuto(...)` to re-enable automatic calculation, and then if needed - call `notifyDataSetChanged()` on the chart to make it refresh the data.
        public void setCustom(List<NSObject> colors, List<NSObject> labels)
        {
            if (colors.Count != labels.Count)
            {
                Console.WriteLine("ChartLegend:setCustom() - colors array and labels array need to be of same size");
            }

            labelsObjc = labels;
            colorsObjc = colors;
            _isLegendCustom = true;
        }
    }
}

