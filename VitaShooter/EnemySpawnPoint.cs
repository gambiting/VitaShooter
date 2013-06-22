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
	public class EnemySpawnPoint : GameEntity
	{
		SpriteUV spawnSprite;
		
		bool inserted = false;
		
		public EnemySpawnPoint (Vector2 position)
		{
			this.Position = position;
			
			TextureInfo spawnInfo = new TextureInfo(Bullet.fireTexture);
			
			spawnSprite = new SpriteUV(spawnInfo);
			
			spawnSprite.Color = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Red,0.0f);
			
			this.AddChild(spawnSprite);
			
			
			
		}
		
		public override void Tick (float dt)
		{
			//if the enemy for this spawn point has not been inserted yet, increase the alpha value
			if(!this.inserted) spawnSprite.Color = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Red,spawnSprite.Color.W+0.01f);
			
			//once the alpha value reaches 1.0, insert an enemy, set inserted to true
			if(spawnSprite.Color.W>=1.0f)
			{
				
				Enemy e = new BasicEnemy(this.Position, Game.Instance.enemySpriteList.TextureInfo);
				Game.Instance.enemyList.Add(e);
				Game.Instance.enemySpriteList.AddChild(((BasicEnemy)e).sprite);
				
				Game.Instance.EffectsLayer.AddChild(e);
				
				Game.Instance.quadTree.insert(e);
				
				this.inserted=true;
			}
			
			//once the enemy has been inserted, slowly disappear
			if(this.inserted)
			{
				spawnSprite.Color = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Red,spawnSprite.Color.W-0.05f);
				
				if(spawnSprite.Color.W==0.0f)
				{
					Game.Instance.World.RemoveChild(this,true);
					this.Die ();
				}
			}
			
		}
		
	}
}

