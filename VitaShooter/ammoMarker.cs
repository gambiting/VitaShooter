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
	 * Class representing the "+ammo" sign left after collecting an ammo pack
	 */ 
	public class ammoMarker : GameEntity
	{
		SpriteUV sprite;
		public static Texture2D texture;
		
		public ammoMarker (Vector2 position)
		{
			this.Position = position+new Vector2(0.0f,0.5f);
			this.sprite = new SpriteUV(new TextureInfo(texture));
			this.sprite.Scale = new Vector2(3.0f,1.0f);
			
			
			this.AddChild(this.sprite);
		}
		
		public override void Tick (float dt)
		{
			
			this.Position+= new Vector2(0.0f,0.02f);
			this.sprite.Color = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.White,sprite.Color.W-0.05f);
			if(sprite.Color.W<=0.0f) 
			{	
				Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(this,this.Tick);
				Game.Instance.EffectsLayer.RemoveChild(this,true);
			}

			
		}
	}
}

