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
	public class BasicBullet : GameEntity
	{
		
		Vector2 step; //step taken each frame
		
		public BasicBullet ()
		{
			SpriteUV sprite = new SpriteUV();
			sprite.TextureInfo = new TextureInfo(Bullet.fireTexture);
			sprite.Scale = new Vector2(0.1f,0.1f);
			sprite.Color = Colors.White;
			sprite.CenterSprite(new Vector2(0.5f,0.5f));
			
			
			SpriteUV shadow = new SpriteUV(new TextureInfo(Bullet.fireTexture));
			shadow.CenterSprite(new Vector2(0.5f,0.5f));
			shadow.Color = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Black,0.5f);
			shadow.Scale = new Vector2(0.4f,0.4f);
			this.AddChild(shadow);

			this.AddChild(sprite);
		}
		
		public override void Tick (float dt)
		{
			base.Tick(dt);
			
			//move the bullet
			this.Position+=step;
		}
	}
}

