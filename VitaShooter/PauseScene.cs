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
	public class PauseScene : BasicScene
	{
		public static PauseScene Instance;
		
		SpriteUV sprite_button_returntomenu;
		SpriteUV sprite_button_returntogame;
		
		int menuSelection = 0;
		
		public PauseScene ()
		{
			
			
		}
		
		public void initPause()
		{
			this.Camera2D.SetViewFromViewport();
			
			if(sprite_button_returntogame == null)
				sprite_button_returntogame = new SpriteUV(new TextureInfo("/Application/data/button_returntogame.png"));
			
			sprite_button_returntogame.CenterSprite(new Vector2(0.5f,0.5f));
			sprite_button_returntogame.Scale = sprite_button_returntogame.TextureInfo.TextureSizef*1.5f;
			sprite_button_returntogame.Position = new Vector2(960.0f/2.0f,544.0f/2.0f+100.0f);
			
			if(sprite_button_returntomenu == null)
				sprite_button_returntomenu = new SpriteUV(new TextureInfo("/Application/data/button_returntomenu.png"));
			
			sprite_button_returntomenu.CenterSprite(new Vector2(0.5f,0.5f));
			sprite_button_returntomenu.Scale = sprite_button_returntomenu.TextureInfo.TextureSizef*1.5f;
			sprite_button_returntomenu.Position = new Vector2(960.0f/2.0f,544.0f/2.0f-100.0f);
			
			Foreground.AddChild(sprite_button_returntogame);
			Foreground.AddChild(sprite_button_returntomenu);
		}
		
		public override void Tick (float dt)
		{
			base.Tick (dt);
			
			if (Input2.GamePad0.Up.Press) {
				menuSelection = (menuSelection+1)%2;
			}
			if (Input2.GamePad0.Down.Press) {
				menuSelection = (int)FMath.Abs((menuSelection-1)%2);
			}
			
			if (Input2.GamePad0.Cross.Press) {
				switch(menuSelection)
				{
				case 0:
					SceneManager.Instance.popScene();
					break;
				case 1:
					SceneManager.Instance.popScene();
					
					Game.Instance.paused=false;
					Game.Instance.OnExit();
					SceneManager.Instance.changeSceneTo(MainMenu.Instance);
					break;
				}
			}
			
			if(Input2.Touch00.Down && sprite_button_returntogame.IsWorldPointInsideContentLocalBounds(GetTouchPos()))
			{
				menuSelection=0;
			}else if(Input2.Touch00.Down && sprite_button_returntomenu.IsWorldPointInsideContentLocalBounds(GetTouchPos()))
			{
				menuSelection=1;
			}
			
			
			switch(menuSelection)
			{
			case 0:
				sprite_button_returntogame.Scale = sprite_button_returntogame.TextureInfo.TextureSizef*1.7f;
				sprite_button_returntogame.Color = new Vector4(1.0f,1.0f,0.0f,1.0f);
				sprite_button_returntomenu.Scale = sprite_button_returntomenu.TextureInfo.TextureSizef*1.5f;
				sprite_button_returntomenu.Color = new Vector4(1.0f,1.0f,1.0f,1.0f);
				break;
			case 1:
				sprite_button_returntogame.Scale = sprite_button_returntogame.TextureInfo.TextureSizef*1.5f;
				sprite_button_returntogame.Color = new Vector4(1.0f,1.0f,1.0f,1.0f);
				sprite_button_returntomenu.Scale = sprite_button_returntomenu.TextureInfo.TextureSizef*1.7f;
				sprite_button_returntomenu.Color = new Vector4(1.0f,1.0f,0.0f,1.0f);
				break;
			}
			
			if(Input2.Touch00.Release && sprite_button_returntogame.IsWorldPointInsideContentLocalBounds(GetTouchPos()))
			{
				SceneManager.Instance.popScene();
			}else if(Input2.Touch00.Release && sprite_button_returntomenu.IsWorldPointInsideContentLocalBounds(GetTouchPos()))
			{
				Game.Instance.paused=false;
				Game.Instance.OnExit();
				SceneManager.Instance.changeSceneTo(MainMenu.Instance);
			}
			
		}
	
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			initPause ();
			menuSelection = 0;
		}
		
		public override void OnExit ()
		{
			base.OnExit ();
			
			Foreground.RemoveAllChildren(true);
			Background.RemoveAllChildren(true);
		}
	}
}

