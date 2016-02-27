using System;
using System.Collections.Generic;
using System.Linq;

namespace scrolling
{
    public class ChartDataSet : ChartBaseDataSet
    {
        public ChartDataSet() : base()
        {
            _yVals = new List<ChartDataEntry>();
        }


        public ChartDataSet(string label) : base(label)
        {
            _yVals = new List<ChartDataEntry>();
        }

        public ChartDataSet(List<ChartDataEntry> yVals, string label) : base(label)
        {
            _yVals = yVals ?? new List<ChartDataEntry>();
            calcMinMax(_lastStart, _lastEnd);
        }

        public ChartDataSet(List<ChartDataEntry> yVals) : this(yVals, "DataSet")
        {
        }

        // MARK: - Data functions and accessors

        internal List<ChartDataEntry> _yVals;
        internal double _yMax = 0.0;
        internal double _yMin = 0.0;

        /// the last start value used for calcMinMax
        internal int _lastStart = 0;

        /// the last end value used for calcMinMax
        internal int _lastEnd = 0;

        public List<ChartDataEntry> yVals
        {
            get { return _yVals; }
        }

        /// Use this method to tell the data set that the underlying data has changed
        public override void notifyDataSetChanged()
        {
            calcMinMax(_lastStart, _lastEnd);
        }

        public override void calcMinMax(int start, int end)
        {
            var yValCount = _yVals.Count;

            if (yValCount == 0)
            {
                return;
            }

            int endValue = 0;

            if (end == 0 || end >= yValCount)
            {
                endValue = yValCount - 1;
            }
            else
            {
                endValue = end;
            }

            _lastStart = start;
            _lastEnd = endValue;

            _yMin = double.MaxValue;
            _yMax = -double.MaxValue;

            for (var i = start; i <= endValue; i++)
            {
                var e = _yVals[i];

                if (!double.IsNaN(e.value))
                {
                    if (e.value < _yMin)
                    {
                        _yMin = e.value;
                    }
                    if (e.value > _yMax)
                    {
                        _yMax = e.value;
                    }
                }
            }

            if (Math.Abs(_yMin - double.MaxValue) < 0.0001)
            {
                _yMin = 0.0;
                _yMax = 0.0;
            }
        }

        /// - returns: the minimum y-value this DataSet holds
        public override double yMin
        {
            get { return _yMin; }
        }

        /// - returns: the maximum y-value this DataSet holds
        public override double yMax
        {
            get { return _yMax; }
        }

        /// - returns: the number of y-values this DataSet represents
        public override int entryCount
        {
            get
            {
                if (_yVals == null)
                {
                    return 0;
                }
                return _yVals.Count;
            }
        }

        /// - returns: the value of the Entry object at the given xIndex. Returns NaN if no value is at the given x-index.
        public override double yValForXIndex(int x)
        {
            var e = entryForXIndex(x);

            if (e != null && e.xIndex == x)
            {
                return e.value;
            }
            else
            {
                return double.NaN;
            }
        }

        /// - returns: the entry object found at the given index (not x-index!)
        /// - throws: out of bounds
        /// if `i` is out of bounds, it may throw an out-of-bounds exception
        public override ChartDataEntry entryForIndex(int i)
        {
            return _yVals[i];
        }

        /// - returns: the first Entry object found at the given xIndex with binary search.
        /// If the no Entry at the specifed x-index is found, this method returns the Entry at the closest x-index.
        /// nil if no Entry object at that index.
        public override ChartDataEntry entryForXIndex(int x)
        {
            var index = this.entryIndex(x);
            if (index > -1)
            {
                return _yVals[index];
            }
            return null;
        }

        public List<ChartDataEntry> entriesForXIndex(int x)
        {
            var entries = new List<ChartDataEntry>();

            var low = 0;
            var high = _yVals.Count - 1;

            while (low <= high)
            {
                var m = (int) ((high + low)/2);
                var entry = _yVals[m];

                if (x == entry.xIndex)
                {
                    while (m > 0 && _yVals[m - 1].xIndex == x)
                    {
                        m--;
                    }

                    high = _yVals.Count;
                    for (; m < high; m++)
                    {
                        entry = _yVals[m];
                        if (entry.xIndex == x)
                        {
                            entries.Add(entry);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (x > _yVals[m].xIndex)
                {
                    low = m + 1;
                }
                else
                {
                    high = m - 1;
                }
            }

            return entries;
        }

        /// - returns: the array-index of the specified entry
        ///
        /// - parameter x: x-index of the entry to search for
        public override int entryIndex(int x)
        {
            var low = 0;
            var high = _yVals.Count - 1;
            var closest = -1;

            while (low <= high)
            {
                var m = (high + low)/2;
                var entry = _yVals[m];

                if (x == entry.xIndex)
                {
                    while (m > 0 && _yVals[m - 1].xIndex == x)
                    {
                        m--;
                    }

                    return m;
                }

                if (x > entry.xIndex)
                {
                    low = m + 1;
                }
                else
                {
                    high = m - 1;
                }

                closest = m;
            }

            return closest;
        }

        /// - returns: the array-index of the specified entry
        ///
        /// - parameter e: the entry to search for
        public override int entryIndex(ChartDataEntry e)
        {
            for (var i = 0; i < _yVals.Count; i++)
            {
                if (_yVals[i] == e)
                {
                    return i;
                }
            }

            return -1;
        }

        /// Adds an Entry to the DataSet dynamically.
        /// Entries are added to the end of the list.
        /// This will also recalculate the current minimum and maximum values of the DataSet and the value-sum.
        /// - parameter e: the entry to add
        /// - returns: true
        public override bool addEntry(ChartDataEntry e)
        {
            var val = e.value;

            if (_yVals == null)
            {
                _yVals = new List<ChartDataEntry>();
            }

            if (_yVals.Count == 0)
            {
                _yMax = val;
                _yMin = val;
            }
            else
            {
                if (_yMax < val)
                {
                    _yMax = val;
                }
                if (_yMin > val)
                {
                    _yMin = val;
                }
            }

            _yVals.Add(e);

            return true;
        }

        /// Adds an Entry to the DataSet dynamically.
        /// Entries are added to their appropriate index respective to it's x-index.
        /// This will also recalculate the current minimum and maximum values of the DataSet and the value-sum.
        /// - parameter e: the entry to add
        /// - returns: true
        public override bool addEntryOrdered(ChartDataEntry e)
        {
            var val = e.value;

            if (_yVals == null)
            {
                _yVals = new List<ChartDataEntry>();
            }

            if (_yVals.Count == 0)
            {
                _yMax = val;
                _yMin = val;
            }
            else
            {
                if (_yMax < val)
                {
                    _yMax = val;
                }
                if (_yMin > val)
                {
                    _yMin = val;
                }
            }

            if (_yVals[_yVals.Count-1].xIndex > e.xIndex)
            {
                var closestIndex = entryIndex(e.xIndex);
                if (_yVals[closestIndex].xIndex < e.xIndex)
                {
                    closestIndex++;
                }
                _yVals.Insert(closestIndex, e);

                return true;
            }

            _yVals.Add(e);

            return true;
        }

        /// Removes an Entry from the DataSet dynamically.
        /// This will also recalculate the current minimum and maximum values of the DataSet and the value-sum.
        /// - parameter entry: the entry to remove
        /// - returns: true if the entry was removed successfully, else if the entry does not exist
        public override bool removeEntry(ChartDataEntry entry)
        {
            var removed = false;

            for (var i = 0; i < _yVals.Count; i++)
            {
                if (_yVals[i] == entry)
                {
                    _yVals.RemoveAt(i);
                    removed = true;
                    break;
                }
            }

            if (removed)
            {
                calcMinMax(_lastStart, _lastEnd);
            }

            return removed;
        }

        /// Removes the first Entry (at index 0) of this DataSet from the entries array.
        ///
        /// - returns: true if successful, false if not.
        public override bool removeFirst()
        {//FIXME show in xcode
            ChartDataEntry entry = !_yVals.Any() ? null : _yVals.removeFirst();

            var removed = entry != null;

            if (removed)
            {
                calcMinMax(_lastStart, end: _lastEnd);
            }

            return removed;
        }

        /// Removes the last Entry (at index size-1) of this DataSet from the entries array.
        ///
        /// - returns: true if successful, false if not.
        public override bool removeLast()
        {//FIXME show in xcode project
            ChartDataEntry entry = !_yVals.Any() ? null : _yVals.removeLast();

            var removed = entry != null;

            if (removed)
            {
                calcMinMax(_lastStart, _lastEnd);
            }

            return removed;
        }

        /// Checks if this DataSet contains the specified Entry.
        /// - returns: true if contains the entry, false if not.
        public override bool contains(ChartDataEntry e)
        {
            foreach (var entry in _yVals)
            {
                if (entry.IsEqual(e))
                {
                    return true;
                }
            }

            return false;
        }

        /// Removes all values from this DataSet and recalculates min and max value.
        public override void clear()
        {
            _yVals.Clear();
            _lastStart = 0;
            _lastEnd = 0;
            notifyDataSetChanged();
        }



        // MARK: - Data functions and accessors

        /// - returns: the number of entries this DataSet holds.
        public int valueCount
        {
            get { return _yVals.Count; }
        }
    }
}