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
	public class Crosshair : GameEntity
	{
		public static Crosshair Instance;
		
		public static Texture2D crosshairTexture;
		
		public Crosshair ()
		{
			crosshairTexture = AppMain.ttCreateTexture(1,1,0xff000000);
			SpriteUV sprite = new SpriteUV();
			sprite.TextureInfo = new TextureInfo(crosshairTexture);
			sprite.Scale = new Vector2(0.1f,0.1f);
			//sprite.Color = Colors.White;
			//sprite.CenterSprite(new Vector2(0.5f,0.5f));
			
			this.AddChild(sprite);
		}
	}
}

