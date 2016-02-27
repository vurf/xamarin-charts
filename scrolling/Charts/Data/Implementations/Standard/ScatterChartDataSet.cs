using System;
using CoreGraphics;
using UIKit;

namespace scrolling
{
    public class ScatterChartDataSet : LineScatterCandleRadarChartDataSet, IScatterChartDataSet
    {
        private nfloat _scatterShapeSize = 10.0f;
        private ScatterShape _scatterShape = ScatterShape.Square;
        private nfloat _scatterShapeHoleRadius = 0.0f;

        public enum ScatterShape
        {
            Square,
            Circle,
            Triangle,
            Cross,
            X,
            Custom
        }

        public nfloat scatterShapeSize
        {
            get { return _scatterShapeSize; }
            set { _scatterShapeSize = value; }
        }

        public ScatterShape scatterShape
        {
            get { return _scatterShape; }
            set { _scatterShape = value; }
        }

        public nfloat scatterShapeHoleRadius
        {
            get { return _scatterShapeHoleRadius; }
            set { _scatterShapeHoleRadius = value; }
        }

        public UIColor scatterShapeHoleColor { get; set; }
        public CGPath customScatterShape { get; set; }


    }
}