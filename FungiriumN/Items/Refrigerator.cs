﻿using System;
using System.Collections.Generic;
using System.Collections;

namespace FungiriumN.Items
{
	public class Refrigerator : IEnumerable<Statistics>
	{
		#region シングルトン

		private static Refrigerator _instance = null;
		public static Refrigerator Instance
		{
			get {
				if (_instance == null) {
					_instance = new Refrigerator ();
				}

				return _instance;
			}
		}

		#endregion


		public Refrigerator ()
		{
			this._statistics = new Statistics[this.ItemType.GetLength (0)];

			foreach (var type in this.ItemType) {
				this [type] = new Statistics (new RefrigeratedFungus(type)) {
					Count = 0,
				};
			}

		}

		public void Increment (Type type)
		{
			for (int i = 0; i < this.ItemType.GetLength(0); i++)
			{
				if (type == this.ItemType[i]) {
					this [type].Count++;
					return;
				}
			}

			throw new KeyNotFoundException ();
		}

		public void Decrement (Type type)
		{
			for (int i = 0; i < this.ItemType.GetLength(0); i++)
			{
				if (type == this.ItemType[i]) {
					this [type].Count--;
					return;
				}
			}

			throw new KeyNotFoundException ();
		}

		public bool Contains (Type type)
		{
			foreach (var t in this.ItemType)
			{
				if (t == type)
					return true;
			}

			return false;
		}

		public bool Reset (Type type)
		{
			foreach (var t in this.ItemType)
			{
				if (t == type) {

					this [t].Count = 0;

					// TODO: インスタンスも初期化すべき?

					return true;
				}
			}

			return false;
		}

		public void ResetAll ()
		{
			foreach (var t in this.ItemType)
			{
				this [t].Count = 0;
			}
		}

		public Statistics this [Type type]
		{
			set {
				this.SetValue (type, value);
			}
			get {
				return this.GetValue(type);
			}
		}

		public void SetValue (Type type, Statistics value)
		{
			for (int i = 0; i < this.ItemType.GetLength (0); i++)
			{
				if (this.ItemType[i] == type) {
					this._statistics [i] = value;
					return;
				}
			}

			throw new KeyNotFoundException ("指定されたアイテムが見つかりませんでした。");
		}

		public Statistics GetValue(Type type)
		{
			for (int i = 0; i < this.ItemType.GetLength(0); i++ )
			{
				if (this.ItemType[i] == type)
				{
					return this._statistics [i];
				}
			}

			throw new KeyNotFoundException ("指定されたアイテムが見つかりませんでした。");
		}

		public Statistics GetValueAt(int i)
		{
			if ((i < 0) || (i >= this.ItemType.GetLength (0)))
				throw new IndexOutOfRangeException ("指定されたアイテムが見つかりませんでした。");

			return this._statistics [i];
		}

		public Statistics GetAvailableAt (int i)
		{
			var count = 0;
			Statistics result = null;

			foreach (var stat in this)
			{
				if (stat.Count > 0)
					count++;

				if (count-1 == i) {
					result = stat;
					break;
				}
			}

			if (result == null)
				throw new IndexOutOfRangeException ("指定されたアイテムが見つかりませんでした。");

			return result;
		}

		public Statistics GetRevealedAt (int i)
		{
			var count = 0;
			Statistics result = null;

			foreach (var stat in this)
			{
				if (stat.IsRevealed)
					count++;

				if (count-1 == i) {
					result = stat;
					break;
				}
			}

			if (result == null)
				throw new IndexOutOfRangeException ("指定されたアイテムが見つかりませんでした。");

			return result;
		}

		public int Count
		{
			get {
				return this.ItemType.GetLength (0);
			}
		}


		public int AvailableCount
		{
			get {
				var count = 0;
				foreach (var stat in this)
				{
					if (stat.Count > 0)
						count++;
				}

				return count;
			}
		}

		public int RevealedCount
		{
			get {
				var count = 0;
				foreach (var stat in this)
				{
					if (stat.IsRevealed)
						count++;
				}

				return count;
			}
		}


		protected virtual Type[] ItemType
		{
			get { return Sprites.Fungi.Population.FungusType; }
		}


		private Statistics[] _statistics;

		#region IEnumerable implementation

		public IEnumerator<Statistics> GetEnumerator ()
		{
			foreach (var t in this.ItemType)
			{
				yield return this [t];
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return this.GetEnumerator ();
		}

		#endregion

	}
}

