using System;
using System.Collections.Generic;
using UIKit;

namespace scrolling
{
    public class LineChartDataSet : LineRadarChartDataSet, ILineChartDataSet
    {
        private nfloat _cubicIntensity = 0.2f;
        private nfloat _circleRadius = 8.0f;
        private List<UIColor> _circleColors = new List<UIColor>();
        private bool _drawCirclesEnabled = true;
        private UIColor _circleHoleColor = UIColor.White;
        private bool _drawCircleHoleEnabled = true;
        private ChartFillFormatter _fillFormatter = new ChartDefaultFillFormatter();

        private void initialize()
        {
            // default color
            circleColors.Add(UIColor.FromRGB(140.0f/255.0f, 234.0f/255.0f, 255.0f/255.0f));
        }

        public LineChartDataSet()
        {
            initialize();
        }

        public LineChartDataSet(List<ChartDataEntry> yVals, string label) : base(yVals, label)
        {
            initialize();
        }


        public nfloat cubicIntensity
        {
            get { return _cubicIntensity; }
            set
            {
                _cubicIntensity = value;
                if (_cubicIntensity > 1.0f)
                {
                    _cubicIntensity = 1.0f;
                }
                if (_cubicIntensity < 0.05f)
                {
                    _cubicIntensity = 0.05f;
                }
            }
        }

        public bool drawCubicEnabled { get; set; }

        public bool isDrawCubicEnabled
        {
            get { return drawCubicEnabled; }
        }

        public nfloat circleRadius
        {
            get { return _circleRadius; }
            set { _circleRadius = value; }
        }

        public List<UIColor> circleColors
        {
            get { return _circleColors; }
            set { _circleColors = value; }
        }

        public UIColor getCircleColor(int index)
        {
            var size = circleColors.Count;
            index = index%size;
            if (index >= size)
            {
                return null;
            }
            return circleColors[index];
        }

        public void setCircleColor(UIColor color)
        {
            circleColors.Clear();
            circleColors.Add(color);
        }

        public void resetCircleColors(int index)
        {
            circleColors.Clear();
        }

        public bool drawCirclesEnabled
        {
            get { return _drawCirclesEnabled; }
            set { _drawCirclesEnabled = value; }
        }

        public bool isDrawCirclesEnabled
        {
            get { return drawCirclesEnabled; }
        }

        public UIColor circleHoleColor
        {
            get { return _circleHoleColor; }
            set { _circleHoleColor = value; }
        }

        public bool drawCircleHoleEnabled
        {
            get { return _drawCircleHoleEnabled; }
            set { _drawCircleHoleEnabled = value; }
        }

        public bool isDrawCircleHoleEnabled
        {
            get { return drawCircleHoleEnabled; }
        }

        public nfloat lineDashPhase
        {
            get { return 0f; }
        }

        public List<nfloat> lineDashLengths { get; set; }

        public ChartFillFormatter fillFormatter
        {
            get { return _fillFormatter; }
            set {
                _fillFormatter = value ?? new ChartDefaultFillFormatter();
            }
        }



    }
}