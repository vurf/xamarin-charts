using System;
using Foundation;
using UIKit;
using System.Collections.Generic;

namespace scrolling
{
	public class ChartBaseDataSet : NSObject, IChartDataSet
	{
		public ChartBaseDataSet ()
		{
		    colors.Add(UIColor.FromRGB(140.0f/255.0f, 234.0f/255.0f, 255.0f/255.0f));
		    valueColors.Add(UIColor.Black);
		}

	    public ChartBaseDataSet(string _label)
	    {
	        colors.Add(UIColor.FromRGB(140.0f/255.0f, 234.0f/255.0f, 255.0f/255.0f));
		    valueColors.Add(UIColor.Black);

	        label = _label;
	    }

		#region IChartDataSet implementation

		public virtual void notifyDataSetChanged ()
		{
			calcMinMax (0, entryCount - 1);
		}

		public virtual void calcMinMax (int start, int end)
		{
            Console.WriteLine("calcMinMax is not implemented in ChartBaseDataSet");
		}

		public virtual double yValForXIndex (int x)
		{
            Console.WriteLine("yValForXIndex is not implemented in ChartBaseDataSet");
		}

		public virtual ChartDataEntry entryForIndex (int i)
		{
            Console.WriteLine("entryForIndex is not implemented in ChartBaseDataSet");
		}

		public virtual ChartDataEntry entryForXIndex (int x)
		{
            Console.WriteLine("entryForXIndex is not implemented in ChartBaseDataSet");
		}

		public virtual int entryIndex (int x)
		{
            Console.WriteLine("entryIndex is not implemented in ChartBaseDataSet");
		}

		public virtual int entryIndex (ChartDataEntry e)
		{
		    Console.WriteLine("entryIndex is not implemented in ChartBaseDataSet");
		}

		public virtual bool addEntry (ChartDataEntry e)
		{
            Console.WriteLine("addEntry is not implemented in ChartBaseDataSet");
		}


         public virtual bool addEntryOrdered(ChartDataEntry e)
         {
             Console.WriteLine("addEntryOrdered is not implemented in ChartBaseDataSet");
         }

		public virtual bool removeEntry (ChartDataEntry entry)
		{
            Console.WriteLine("removeEntry is not implemented in ChartBaseDataSet");
		}

        public virtual bool removeEntry(int xIndex)
        {
            var entry = entryForXIndex(xIndex);
            if (entry != null)
            {
                return removeEntry(entry);
            }
            return false;
        }

	    public virtual bool removeFirst()
	    {
	        var entry = entryForIndex(0);

	        if (entry != null)
	        {
	            return removeEntry(entry);
	        }
	        return false;
	    }

	    public virtual bool removeLast()
	    {
	        var entry = entryForIndex(entryCount - 1);

	        if (entry != null)
	        {
	            return removeEntry(entry);
	        }
	        return false;
	    }

	    public virtual bool contains (ChartDataEntry e)
		{
            Console.WriteLine("removeEntry is not implemented in ChartBaseDataSet");
	        return true;
		}

        public virtual void clear()
        {
            Console.WriteLine("clear is not implemented in ChartBaseDataSet");
        }

	    public virtual UIColor colorAt(int index)
	    {
	        if (index < 0)
	        {
	            index = 0;
	        }
	        return colors[index%colors.Count];
	    }

	    public virtual void resetColors ()
		{
			colors.Clear();
		}

		public void addColor (UIColor color)
		{
			colors.Add(color);
		}

		public void setColor (UIKit.UIColor color)
		{
			colors.Clear();
		    colors.Add(color);
		}

        public void setColor(UIColor color,nfloat alpha)
        {
            setColor(color.ColorWithAlpha(alpha));
        }

        public void setColors(List<UIColor> _colors, nfloat alpha)
        {
            var colorsWithAlpha = _colors;

            for (int i = 0; i < colorsWithAlpha.Count; i++)
            {
                colorsWithAlpha[i] = colorsWithAlpha[i].ColorWithAlpha(alpha);
            }
            colors.Clear();
            colors.AddRange(colorsWithAlpha);
        }

		public virtual double yMin {
			get
			{
			    Console.WriteLine("yMin is not implemented in ChartBaseDataSet");
			    return 0d;
			}
		}

		public virtual double yMax {
			get {
                Console.WriteLine("yMax is not implemented in ChartBaseDataSet");
			    return 0d;
			}
		}

		public virtual int entryCount {
			get {
                Console.WriteLine("entryCount is not implemented in ChartBaseDataSet");
			    return 0;
			}
		}

	    string __label = "DataSet";
		public virtual string label {
			get { return __label; }
            set { __label = value; }
		}

		public virtual ChartYAxis.AxisDependency axisDependency {
			get { return ChartYAxis.AxisDependency.Left; }
		}

		List<UIColor> _colors;
	    List<UIColor> _valueColors;

	    public List<UIColor> colors {
			get { return _colors ?? (_colors = new List<UIColor> ()); }
		}

	    public List<UIColor> valueColors
	    {
	        get { return _valueColors ?? (_valueColors = new List<UIColor>()); }
	    }

	    bool _hightlightEnabled = true;
	    public bool highlightEnabled {
			get { return _hightlightEnabled; }
			set { _hightlightEnabled = value; }
		}


		public bool isHighlightEnabled {
			get { return highlightEnabled; }
		}

	    NSNumberFormatter _valueFormatter = ChartUtils.defaultValueFormatter();

	    public NSNumberFormatter valueFormatter
	    {
	        get { return _valueFormatter; }
	        set
	        {
	            if (value == null)
	            {
	                _valueFormatter = ChartUtils.defaultValueFormatter();
	            }
	            else
	            {
	                _valueFormatter = value;
	            }
	        }
	    }



	    public UIColor valueTextColor {
			get { return valueColors[0]; }
			set {
				valueColors.Clear();
			    valueColors.Add(value);
			}
		}

	    public UIColor valueTextColorAt(int index)
	    {
	        if (index < 0)
	        {
	            index = 0;
	        }
	        return valueColors[index%valueColors.Count];
	    }

	    UIFont _valueFont = UIFont.SystemFontOfSize(7.0f);
	    public UIFont valueFont {
			get { return _valueFont; }
			set { _valueFont = value; }
		}

	    bool _drawValuesEnabled = true;
		public bool drawValuesEnabled {
			get { return _drawValuesEnabled; }
			set { _drawValuesEnabled = value; }
		}

		public bool isDrawValuesEnabled {
			get { return drawValuesEnabled; }
		}

	    bool _visible = true;
		public bool visible {
			get { return _visible; }
			set { _visible = value; }
		}

		public bool isVisible {
			get { return visible; }
		}

		#endregion


	}
}

