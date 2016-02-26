namespace scrolling
{
    public interface ILineScatterCandleRadarChartDataSet : IBarLineScatterCandleBubbleChartDataSet
    {
        // MARK: - Data functions and accessors

        // MARK: - Styling functions and accessors

        /// Enables / disables the horizontal highlight-indicator. If disabled, the indicator is not drawn.
        bool drawHorizontalHighlightIndicatorEnabled { get; set; }

        /// Enables / disables the vertical highlight-indicator. If disabled, the indicator is not drawn.
        bool drawVerticalHighlightIndicatorEnabled { get; set; }

        /// - returns: true if horizontal highlight indicator lines are enabled (drawn)
        bool isHorizontalHighlightIndicatorEnabled { get; }

        /// - returns: true if vertical highlight indicator lines are enabled (drawn)
        bool isVerticalHighlightIndicatorEnabled { get; }

        /// Enables / disables both vertical and horizontal highlight-indicators.
        /// :param: enabled
        void setDrawHighlightIndicators(bool enabled);
    }
}