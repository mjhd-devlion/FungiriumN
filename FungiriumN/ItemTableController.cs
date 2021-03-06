using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FungiriumN
{
	partial class ItemTableController : UITableViewController
	{
		public ItemTableController (IntPtr handle) : base (handle)
		{
			this.TableView.SeparatorColor = UIColor.Clear;
		}

		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return Items.Inventory.Instance.AvailableCount;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var inventory = Items.Inventory.Instance;
			var stat = inventory.GetAvailableAt (indexPath.Item);
			var item = stat.Instance;

			var cell = tableView.DequeueReusableCell (ItemTableCell.Key) as ItemTableCell;

			cell.NameLabel.Text = item.GetMetadata ().Name;
			cell.DetailLabel.Text = item.GetMetadata ().Description;
			cell.CountLabel.Text = stat.Count.ToString () + "個";

			cell.PushedUse += (sender, e) => {

				var itemStat = Items.Inventory.Instance.GetAvailableAt (indexPath.Item);

				if (itemStat.Count <= 0) return;

				this.NavigationController.PopToRootViewController (true);

				itemStat.Instance.UseToTestTube ();
				itemStat.Count --;
			};

			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			this._selectedIndex = indexPath.Item;

			tableView.BeginUpdates ();
			tableView.EndUpdates ();
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Item == this._selectedIndex) {
				return 150.0f;
			}
			return 50.0f;
		}

		private int _selectedIndex = 0;
	}
}
