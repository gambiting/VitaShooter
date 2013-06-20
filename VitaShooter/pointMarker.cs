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
	public class pointMarker : GameEntity
	{
		SpriteUV sprite;
		public static Texture2D texture;
		public pointMarker (Vector2 position)
		{
			this.Position = position;
			this.sprite = new SpriteUV(new TextureInfo(texture));
			this.sprite.Scale = new Vector2(0.75f,0.5f);
			
			
			this.AddChild(this.sprite);
		}
		
		public override void Tick (float dt)
		{
			
			this.Position+= new Vector2(0.0f,0.05f);
			this.sprite.Color = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.White,sprite.Color.W-0.05f);
			if(sprite.Color.W<=0.0f) 
			{	
				Game.Instance.EffectsLayer.RemoveChild(this,true);
				this.Die();
			}

			
		}
	}
}

