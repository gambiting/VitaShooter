using System;
using System.IO;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Imaging;	// Font
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace VitaShooter
{
	/*
	 * Class representing enemies
	 * there should be no instances of this class - any class representing actual enemies should inherit from this class
	 * */
	public class Enemy: GameEntity
	{
		public static Texture2D healthBar;
		
		public SpriteTile sprite;
		
		//basic health
		//TODO - improve or put somewhere else
		public int health = 100;
		
		public bool attacking = false;
		public int animationFrame=0;
		public Vector2 step= new Vector2(0f,0f);
		
		public Vector2 randomMovement;
		
		public bool isMovingRandomly=false;
		
		public SpriteUV healthBarSprite;
		
		
		
		public Enemy ()
		{
			
			//creates a texture for the health bar
			healthBar = AppMain.ttCreateTexture(16,1,0xff00ff00);
			
			TextureInfo tex = new TextureInfo(healthBar);
			
			healthBarSprite = new SpriteUV(tex);
			
			healthBarSprite.Quad.T = new Vector2(-0.5f,0.5f);
			healthBarSprite.Quad.S = new Vector2(1.0f,0.1f);
			this.AddChild(healthBarSprite);
			
		}
		
		public override void Tick (float dt)
		{
			
			//make the health bar proportionally long
			healthBarSprite.Quad.S = new Vector2(health/100f,0.1f);
			
			//move the sprite to the position of this enemy
			sprite.Position = this.Position;
			
			//if health <0 then remove this enemy
			if(health<=0)
			{
				this.RemoveChild(healthBarSprite,true);
				Game.Instance.enemyList.Remove(this);
				Game.Instance.quadTree.removeEntity(this);
				Game.Instance.enemySpriteList.RemoveChild(this.sprite, true);
				Game.Instance.EffectsLayer.AddChild(new pointMarker(this.Position));
				Game.Instance.score+=100;
				this.Die();
				
				
				drop();
			}
			
		}
		
		public void drop()
		{
			if(AppMain.random.Next(0,10)<2)
			{

				AmmoItem a = new AmmoItem(this.Position);
				Game.Instance.ammoList.Add(a);
				Game.Instance.World.AddChild(a);

			}
		}
	}
}

