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
	 * Class representing the ammo packs that can be picked up by the player
	 * 
	 */
	public class AmmoItem : GameEntity
	{
		//initial number of ammo packs to generate
		// TODO: Move somewhere else, prefferably an external file
		public static int noOfAmmoToGenerate = 5;
		
		public static Texture2D ammoTexture;
		
		public AmmoItem (Vector2 pos)
		{
			//assign the position of the item
			Position = pos;
			
			//create a new texture if one doesn't exist yet
			if(AmmoItem.ammoTexture == null) ammoTexture = new Texture2D( "/Application/data/tiles/ammo.png", false );
			
			//create a sprite for this specific ammo item
			SpriteUV sprite = new SpriteUV();
			sprite.TextureInfo = new TextureInfo(AmmoItem.ammoTexture);
			sprite.Scale = new Vector2(0.5f,0.5f);
			sprite.Pivot = new Vector2(0.5f,0.5f);
			
			//rotate by a random angle
			sprite.Rotation = sprite.Rotation.Rotate(Support.random.Next(-180,180));
			
			//schedule this sprite to rotate every frame by a fraction of an angle
			sprite.Schedule((dt) => {
				sprite.Rotate(0.01f);
			},-1);
			
			//create an underlying shadow for the item
			SpriteUV shadow = new SpriteUV(new TextureInfo(Bullet.fireTexture));
			shadow.CenterSprite(new Vector2(0.25f,0.25f));
			shadow.Color = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Yellow,0.5f);
			shadow.Scale = new Vector2(2.0f,2.0f);
			
			//add a shadow and a sprite to this GameEntity
			this.AddChild(shadow);
			this.AddChild(sprite);
			
			//calculate the bounds of this AmmoItem
			bounds = new Bounds2();
			sprite.GetlContentLocalBounds(ref bounds);
			
			
		}
	}
}

