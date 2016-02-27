using System.Collections.Generic;

namespace scrolling
{
    public class BarChartDataEntry : ChartDataEntry
    {
        /// the values the stacked barchart holds
        private List<double> _values;

        /// the sum of all negative values this entry (if stacked) contains
        private double _negativeSum = 0.0;

        /// the sum of all positive values this entry (if stacked) contains
        private double _positiveSum = 0.0;

        public BarChartDataEntry() : base()
        {

        }

        public BarChartDataEntry(List<double> val, int xIndex) : base(BarChartDataEntry.calcSum(val), xIndex)
        {
            values = val; // self.values = values
            calcPosNegSum();
        }


        public BarChartDataEntry(double _value1, int _xIndex1) : base(_value1, _xIndex1)
        {

        }

        public BarChartDataEntry(List<double> _values1, int _xIndex, string label)
            : base(BarChartDataEntry.calcSum(_values1), _xIndex, label)
        {
            this.values = _values1;
        }

        /// Constructor for normal bars (not stacked).
        public BarChartDataEntry(double value1,int xIndex1 , object data1): base(value1, xIndex1, data1)
        {
        }

        public double getBelowSum(int stackIndex)
        {
            if (values == null)
            {
                return 0;
            }

            double remainder = 0.0;
            var index = values.Count - 1;

            while (index > stackIndex && index >= 0)
            {
                remainder += values[index];
                index--;
            }

            return remainder;
        }

        /// - returns: the sum of all negative values this entry (if stacked) contains. (this is a positive number)
        public double negativeSum
        {
            get { return _negativeSum; }
        }

        /// - returns: the sum of all positive values this entry (if stacked) contains.
        public double positiveSum
        {
            get { return _positiveSum; }
        }

        public void calcPosNegSum()
        {
            if (_values == null)
            {
                _positiveSum = 0.0;
                _negativeSum = 0.0;
                return;
            }

            double sumNeg = 0.0;
            double sumPos = 0.0;

            foreach (var d in _values)
            {
                if (d < 0.0)
                {
                    sumNeg += d;
                }
                else
                {
                    sumPos += d;
                }
            }


            _negativeSum = sumNeg;
            _positiveSum = sumPos;
        }

        // MARK: Accessors

        /// the values the stacked barchart holds
        public bool isStacked
        {
            get { return _values != null; }
        }

        /// the values the stacked barchart holds
        public List<double> values
        {
            get { return _values; }
            set
            {
                this.Value = BarChartDataEntry.calcSum(value);
                _values = value;
                calcPosNegSum();
            }
        }



        /// Calculates the sum across all values of the given stack.
        ///
        /// - parameter vals:
        /// - returns:
        private static double calcSum(List<double> vals)
        {
            if (vals == null)
            {
                return 0.0;
            }

            var sum = 0.0;
            foreach (var f in vals)
            {
                sum += f;
            }
            return sum;
        }
    }
}