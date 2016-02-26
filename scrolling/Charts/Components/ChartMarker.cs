using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace scrolling
{
    class ChartMarker : ChartComponentBase
    {

        public UIImage image;

        /// Use this to return the desired offset you wish the MarkerView to have on the x-axis.
        public CGPoint offset = new CGPoint();

        /// The marker's size
        public CGSize size
        {
            get { return image.Size; }
        }

        public ChartMarker()
        {
        }

        /// Returns the offset for drawing at the specific `point`
        ///
        /// - parameter point: This is the point at which the marker wants to be drawn. You can adjust the offset conditionally based on this argument.
        /// - By default returns the self.offset property. You can return any other value to override that.
        public CGPoint offsetForDrawingAtPos(CGPoint point)
        {
            return offset;
        }

        /// Draws the ChartMarker on the given position on the given context
        public void draw(CGContext context, CGPoint point)
        {
            var offset = this.offsetForDrawingAtPos(point);
            var size = this.size;

            var rect = new CGRect(point.X + offset.X, point.Y + offset.Y, size.Width, size.Height);

            UIGraphics.PushContext(context);
            image.Draw(rect);
            UIGraphics.PopContext();
        }

        /// This method enables a custom ChartMarker to update it's content everytime the MarkerView is redrawn according to the data entry it points to.
        ///
        /// - parameter highlight: the highlight object contains information about the highlighted value such as it's dataset-index, the selected range or stack-index (only stacked bar entries).
        public void refreshContent(ChartDataEntry entry, ChartHighlight highlight)
        {
            // Do nothing here...
        }
    }
}