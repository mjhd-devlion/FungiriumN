﻿using System;
using System.Drawing;

using MonoTouch.SpriteKit;

namespace FungiriumN.Sprites.Fungi
{
	public class SampleFungus : IFungus
	{
		static string InternalName = "SampleFungus";

		public SampleFungus ()
		{
			this._Sprite = new SKSpriteNode ();

			this._SetTexturesFromFungusID (this._InternalName);

			this.State = State.Move;
		}

		public SKSpriteNode Sprite
		{
			get {
				return this._Sprite;
			}
		}

		public State State
		{
			get {
				return this._State;
			}
			set {
				this._SwitchAnimation (value);
				this._State = value;
			}
		}

		public void Update (double delta)
		{
			// TODO: 空腹などの管理

			// 移動
			if (this.State == State.Move) {

				const int MoveOccuringPerc = 30; // %
				var rand = new Random ();

				if (rand.Next(100) < MoveOccuringPerc) {

					this._MoveAround (1.0f);

				}

			}
		}


		protected virtual string _InternalName { get { return SampleFungus.InternalName; }}
		protected SKSpriteNode _Sprite;
		protected State _State;
		protected SKAction _MoveAnimation;
		protected SKAction _EatAnimation;
		protected SKAction _HappyAnimation;
		protected SKAction _DeadAnimation;

		protected virtual void _SetTexturesFromFungusID (string fungusId)
		{
			var textures = SKTextureAtlas.FromName ("Fungi");

			var moveTexture = new SKTexture[] {
				textures.TextureNamed (fungusId),
				textures.TextureNamed (fungusId + "_Move")
			};
			var eatTexture  = new SKTexture[] {
				textures.TextureNamed (fungusId + "_Eat1"),
				textures.TextureNamed (fungusId + "_Eat2")
			};
			var happyTexture = new SKTexture[] {
				textures.TextureNamed (fungusId + "_Happy")
			};
			var deadTexture = new SKTexture[] {
				textures.TextureNamed (fungusId + "_Dead")
			};

			var moveAnimation = SKAction.AnimateWithTextures (moveTexture, 0.5);
			var eatAnimation = SKAction.AnimateWithTextures (eatTexture, 0.5);
			var happyAnimation = SKAction.AnimateWithTextures (happyTexture, 0.5);
			var deadAnimation = SKAction.AnimateWithTextures (deadTexture, 0.5);

			this._MoveAnimation = SKAction.RepeatActionForever (moveAnimation);
			this._EatAnimation = SKAction.RepeatActionForever (eatAnimation);
			this._HappyAnimation = SKAction.Sequence (
				happyAnimation,
				SKAction.Run (() => {
					this._SwitchAnimation (State.Move);
				})
			);
			this._DeadAnimation = SKAction.Sequence (
				deadAnimation,
				SKAction.Run (() => {
					// 死後の処理
				})
			);
	
			this._Sprite.NormalTexture = moveTexture [0];
			this._Sprite.Texture = moveTexture [0];
			this._Sprite.Size = moveTexture [0].Size;
		}

		protected virtual void _SwitchAnimation (State state)
		{
			SKAction action;

			switch (state) {
			case State.Move:
				action = this._MoveAnimation;
				break;
			case State.Eat:
				action = this._EatAnimation;
				break;
			case State.Happy:
				action = this._HappyAnimation;
				break;
			case State.Dead:
				action = this._DeadAnimation;
				break;
			default:
				throw new NotImplementedException ();
			}

			this._Sprite.RunAction (action);
		}

		protected virtual bool _MoveAround (float duration)
		{
			var attemptCount = 5; // 5回まで移動の試行
			var resultPoint = this.Sprite.Position;
			var resultAngle = 0.0f;
			var rand = new Random ();

			const double MaxDistanceToMove = 30.0;

			while (attemptCount-- > 0) {

				var angleToMove = rand.NextDouble () * 2.0 * Math.PI;
				var distToMove = rand.NextDouble () * MaxDistanceToMove;

				var attemptPoint = new PointF (
					this.Sprite.Position.X +  (float)(distToMove *Math.Cos(angleToMove)),
					this.Sprite.Position.Y +  (float)(distToMove *Math.Sin(angleToMove))
				);

				// TestTubeにattemptPointが含まれているかどうか
				if (Sprites.TestTubeSprite.DoesShapeContains (attemptPoint)) {
				
					resultPoint = attemptPoint;
					resultAngle = (float)angleToMove;

					break;
				}

			}

			if (attemptCount == 0) {
				return false;
			} else {

				// 移動させる
				this._Sprite.RunAction (SKAction.Group (
					SKAction.RotateToAngle (resultAngle - (float)Math.PI/2.0f, duration, true),
					SKAction.MoveTo (resultPoint, duration)
				));

				return true;
			}

		}

	}
}
