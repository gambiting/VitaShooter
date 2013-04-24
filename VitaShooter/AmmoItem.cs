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
	public class AmmoItem : GameEntity
	{
		
		public static int noOfAmmoToGenerate = 5;
		
		public AmmoItem (Vector2 pos)
		{
			
			Position = pos;
			
			
			Texture2D ammoTexture = new Texture2D( "/Application/data/tiles/ammo.png", false );
			
			SpriteUV sprite = new SpriteUV();
			sprite.TextureInfo = new TextureInfo(ammoTexture);
			sprite.Scale = new Vector2(0.5f,0.5f);
			sprite.CenterSprite(new Vector2(0.5f,0.5f));
			
			sprite.Rotation = sprite.Rotation.Rotate(AppMain.random.Next(-180,180));
			
			sprite.Schedule((dt) => {
				sprite.Rotate(0.01f);
			},-1);
			
			SpriteUV shadow = new SpriteUV(new TextureInfo(Bullet.fireTexture));
			shadow.CenterSprite(new Vector2(0.5f,0.5f));
			shadow.Color = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Yellow,0.5f);
			shadow.Scale = new Vector2(2.0f,2.0f);
			this.AddChild(shadow);
			
			this.AddChild(sprite);
			
			bounds = new Bounds2();
			sprite.GetlContentLocalBounds(ref bounds);
			
			
		}
	}
}

