// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.CodeDom.Compiler;

namespace FungiriumN
{
	[Register ("ItemTableCell")]
	partial class ItemTableCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel _CountLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel _DetailLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView _ItemIcon { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel _NameLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton _UseButton { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (_CountLabel != null) {
				_CountLabel.Dispose ();
				_CountLabel = null;
			}
			if (_DetailLabel != null) {
				_DetailLabel.Dispose ();
				_DetailLabel = null;
			}
			if (_ItemIcon != null) {
				_ItemIcon.Dispose ();
				_ItemIcon = null;
			}
			if (_NameLabel != null) {
				_NameLabel.Dispose ();
				_NameLabel = null;
			}
			if (_UseButton != null) {
				_UseButton.Dispose ();
				_UseButton = null;
			}
		}
	}
}
