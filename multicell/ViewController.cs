using System;

using UIKit;
using CoreGraphics;

namespace multicell
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		UITableView table;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			table = new UITableView (CGRect.Empty);

		}

		public override void ViewDidLayoutSubviews ()
		{
			table.Frame = View.Frame;
		}

	}
}

