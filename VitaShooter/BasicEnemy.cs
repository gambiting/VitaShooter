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
	public class BasicEnemy : Enemy
	{
		public SpriteTile sprite;
		
		public static int noOfEnemiesToGenerate = 10;
		
		
		
		
		public BasicEnemy (Vector2 pos)
		{
			
			Position = pos;
			
			
			var tex1 = new TextureInfo (new Texture2D ("/Application/data/tiles/enemy_sword2.png", false)
													, new Vector2i (25,1));
			
			tex1.Texture.SetFilter(TextureFilterMode.Disabled);
			sprite = new SpriteTile ();
			sprite.TextureInfo = tex1;
			sprite.TileIndex2D = new Vector2i (0, 0);
			sprite.CenterSprite(new Vector2(0.5f,0.35f));
			sprite.Rotation = sprite.Rotation.Rotate(AppMain.random.Next(-180,180));
			sprite.Color = Colors.Red;
			sprite.Scale = new Vector2(2.0f,2.0f);
			
			sprite.Schedule((dt) => {
				if(FrameCount%5==0)
				{
					if(attacking)
					{
						animationFrame = (animationFrame+1) % 25;
					}else
					{
						animationFrame = (animationFrame+1) % 4;
						
					}
					sprite.TileIndex1D = animationFrame;
				}
			},-1);
			
			bounds = new Bounds2();
			sprite.GetlContentLocalBounds(ref bounds);
			
			this.AddChild(sprite);
			
		}
	}
}

