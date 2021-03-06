﻿using System;
using System.Drawing;

using MonoTouch.SpriteKit;
using MonoTouch.CoreGraphics;

namespace FungiriumN.Sprites
{
	public class TestTubeSprite : SKSpriteNode
	{

		#region PATHの設定

		const float GapForShape = 255.0f;

		private static PointF[] _Shape = {
			new PointF(67.140f *2.0f, 475.325f *2.0f - GapForShape),
			new PointF(67.140f *2.0f, -427.81f *2.0f - GapForShape),
			new PointF(0.016f  *2.0f, -475.22f *2.0f - GapForShape),
			new PointF(-67.140f*2.0f, -427.81f *2.0f - GapForShape),
			new PointF(-67.140f*2.0f, 475.325f *2.0f - GapForShape)
		};

		private static CGPath _Path = null;
		public static CGPath Path {
			get {
				if (_Path == null) {
					_Path = new CGPath ();

					_Path.AddLines (_Shape);

					_Path.CloseSubpath ();
				}

				return _Path;
			}
		}

		public static bool DoesShapeContains (PointF p)
		{
			return Path.ContainsPoint (p, false);
		}

		#endregion

		private static TestTubeSprite _testTubeSprite = null;
		public static TestTubeSprite Instance
		{
			get {
				if (_testTubeSprite == null) {
					_testTubeSprite = new TestTubeSprite();
				}

				return _testTubeSprite;
			}
		}

		public static uint TestTubeCategory = 1 << 13;

		public Fungi.Fungi Fungi;

		public TestTubeSprite ()
			: base()
		{
			var testTube = new SKSpriteNode ("TestTube.png");

			this.Size = testTube.Size;

			// 基準点を中心に設定
			this.AnchorPoint = new PointF (
				this.Size.Width / 2.0f,
				this.Size.Height / 2.0f
			);

			// 試験管を追加
			testTube.ZPosition = 2.0f;
			this.AddChild (testTube);

			// 溶液を追加	
			var solution = new TestTubeSolutionSprite () {
				Position = new PointF(0.0f, -GapForShape),
				ZPosition = 0.0f,
			};
			this.AddChild (solution);

			// PhysicsBodyの設定
			var body = SKPhysicsBody.CreateEdgeChain (Path);
			body.CategoryBitMask = TestTubeCategory;
			body.ContactTestBitMask = 0;
			body.CollisionBitMask = Sprites.Fungi.Metadata.FungusCategory;
			body.AffectedByGravity = false;
			this.PhysicsBody = body;

			// Fungiコレクションの初期化
			this.Fungi = new Fungi.Fungi ();
		}

		// Fungus用のAddChildを定義
		public void AddChild (Fungi.Fungus fungus)
		{
			fungus.ZPosition = 1.0f;

			base.AddChild (fungus);
		}

		public void Update (double time)
		{
			this.Fungi.Update (time);
		}

		public void DidContactBegin (SKPhysicsContact contact)
		{

			var a = (Fungi.Fungus)contact.BodyA.Node;
			var b = (Fungi.Fungus)contact.BodyB.Node;

			if (a.GetMetadata ().Power > b.GetMetadata ().Power) {
				this.Fungi.Eat (a, b);
			} else 
				if (a.GetMetadata ().Power < b.GetMetadata ().Power){
				this.Fungi.Eat (b, a);
			}

		}

	}
}

