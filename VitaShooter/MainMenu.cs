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
	public class MainMenu : BasicScene
	{
		
		public static MainMenu Instance;
		
		//buttons sprites
		SpriteUV logo;
		SpriteUV sprite_button_newgame;
		SpriteUV sprite_button_tutorial;
		SpriteUV sprite_button_autoaim;
		
		//menu background
		SpriteList menuBackground;
		
		public int menuSelection = 0;
		
		public MainMenu ()
		{
			initTitle();
		}
		
		public void initTitle()
		{
			this.Camera2D.SetViewFromViewport();
			
			logo = new SpriteUV(new TextureInfo("/Application/data/logo.png"));
			logo.Scale = logo.TextureInfo.TextureSizef*1.8f;
			logo.Pivot = new Vector2(0.5f,0.5f);
			logo.Position = new Vector2((960.0f/2.0f), (540.0f/2.0f)+80f);
			
			sprite_button_newgame = new SpriteUV(new TextureInfo("/Application/data/button_newgame.png"));
			sprite_button_newgame.Scale = sprite_button_newgame.TextureInfo.TextureSizef*1.2f;
			sprite_button_newgame.Position = new Vector2((960.0f/5.0f),(540.0f/4.0f)-sprite_button_newgame.TextureInfo.TextureSizef.Y/2.0f);
			
			sprite_button_tutorial = new SpriteUV(new TextureInfo("/Application/data/button_tutorial.png"));
			sprite_button_tutorial.Scale = sprite_button_newgame.TextureInfo.TextureSizef*1.2f;
			sprite_button_tutorial.Position = new Vector2((960.0f/2.0f),(540.0f/4.0f)-sprite_button_newgame.TextureInfo.TextureSizef.Y/2.0f);
			
			sprite_button_autoaim = new SpriteUV(new TextureInfo("/Application/data/button_autoaimon.png"));
			sprite_button_autoaim.Scale = sprite_button_newgame.TextureInfo.TextureSizef*1.2f;
			sprite_button_autoaim.Position = new Vector2((960.0f/2.0f)+(960.0f/3.5f),(540.0f/4.0f)-sprite_button_newgame.TextureInfo.TextureSizef.Y/2.0f);
			
			sprite_button_newgame.Pivot = new Vector2(0.5f,0.5f);
			sprite_button_autoaim.Pivot = new Vector2(0.5f,0.5f);
			sprite_button_tutorial.Pivot = new Vector2(0.5f,0.5f);
			
			Foreground.AddChild(sprite_button_newgame);
			Foreground.AddChild(sprite_button_tutorial);
			Foreground.AddChild(sprite_button_autoaim);
			Foreground.AddChild(logo);
			
			
			var tex = new Texture2D ("/Application/data/tiles/simple5.png", false);
			var texture = new TextureInfo ( tex,  new Vector2i (1, 13));
			
			menuBackground = new SpriteList(texture);
			
			int menuBackgroundWidth=100;
			int menuBackgroundHeight=50;
			
			
			//mini background discofloor
			for(int x=0;x<30;x++)
			{
				for(int y=0;y<17;y++)
				{
					SpriteTile bgTile = new SpriteTile(texture);
					bgTile.TileIndex1D = Support.random.Next(4,13);
					bgTile.Position = new Vector2((float)x*32.0f,(float)y*32.0f);
					bgTile.Scale = bgTile.TextureInfo.TileSizeInPixelsf*2.0f;
					bgTile.ScheduleInterval((dt) => { bgTile.TileIndex1D = Support.random.Next(4,13); }, 0.2f,-1);
					menuBackground.AddChild(bgTile);
				}
			}
			
			Background.AddChild(menuBackground);
			
		}
		
		
		public override void Tick (float dt)
		{
			base.Tick(dt);
			
			
			if(Input2.GamePad0.Cross.Press)
			{
				acceptChoice();
			}
			
			
			
			if(Input2.GamePad0.Left.Press)
			{
				if(menuSelection==0) menuSelection = 2;
				else menuSelection--;
			}
			
			if(Input2.GamePad0.Right.Press)
			{
				if(menuSelection==2) menuSelection = 0;
				else menuSelection++;
			}
			
			if(Input2.Touch00.Down)
			{
				if(sprite_button_newgame.IsWorldPointInsideContentLocalBounds(GetTouchPos()))
				{
					menuSelection=0;
					
				}else if(sprite_button_tutorial.IsWorldPointInsideContentLocalBounds(GetTouchPos()))
				{
					menuSelection=1;

				}else if(sprite_button_autoaim.IsWorldPointInsideContentLocalBounds(GetTouchPos()))
				{
					menuSelection=2;
				}
			}
			
			if(Input2.Touch00.Release) 
			{
				if(sprite_button_newgame.IsWorldPointInsideContentLocalBounds(GetTouchPos())
				   || sprite_button_tutorial.IsWorldPointInsideContentLocalBounds(GetTouchPos())
				   || sprite_button_autoaim.IsWorldPointInsideContentLocalBounds(GetTouchPos()))
				{
					acceptChoice();
				}
			}
			
			
			
			switch(menuSelection)
			{
			case 0:
				sprite_button_newgame.Color = Colors.Orange;
				sprite_button_autoaim.Color = Colors.White;
				sprite_button_tutorial.Color = Colors.White;
				sprite_button_newgame.Scale = sprite_button_newgame.TextureInfo.TextureSizef*1.3f;
				sprite_button_autoaim.Scale = sprite_button_autoaim.TextureInfo.TextureSizef*1.2f;
				sprite_button_tutorial.Scale = sprite_button_tutorial.TextureInfo.TextureSizef*1.2f;
				break;
			case 1:
				sprite_button_newgame.Color = Colors.White;
				sprite_button_autoaim.Color = Colors.White;
				sprite_button_tutorial.Color = Colors.Orange;
				sprite_button_newgame.Scale = sprite_button_newgame.TextureInfo.TextureSizef*1.2f;
				sprite_button_autoaim.Scale = sprite_button_autoaim.TextureInfo.TextureSizef*1.2f;
				sprite_button_tutorial.Scale = sprite_button_tutorial.TextureInfo.TextureSizef*1.3f;
				break;
			case 2:
				sprite_button_newgame.Color = Colors.White;
				sprite_button_autoaim.Color = Colors.Orange;
				sprite_button_tutorial.Color = Colors.White;
				sprite_button_newgame.Scale = sprite_button_newgame.TextureInfo.TextureSizef*1.2f;
				sprite_button_autoaim.Scale = sprite_button_autoaim.TextureInfo.TextureSizef*1.3f;
				sprite_button_tutorial.Scale = sprite_button_tutorial.TextureInfo.TextureSizef*1.2f;
				break;
			}
			
			
		}
		
		private void acceptChoice()
		{
			if(menuSelection==0)
				{
					
					SceneManager.Instance.changeSceneTo(LevelSelectScene.Instance);
				}else if(menuSelection==1)
				{
					SceneManager.Instance.changeSceneTo(TutorialScene.Instance);
					
				}else if(menuSelection==2)
				{
					if(Game.autoAim)
					{
						Game.autoAim=false;
						sprite_button_autoaim.TextureInfo = new TextureInfo("/Application/data/button_autoaimoff.png");
					}else
					{
						Game.autoAim=true;
						sprite_button_autoaim.TextureInfo = new TextureInfo("/Application/data/button_autoaimon.png");
					}
				}
		}
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			this.initTitle();
			menuSelection = 0;
		}
		
		public override void OnExit ()
		{
			base.OnExit ();
			
			this.Background.RemoveAllChildren(true);
			this.Foreground.RemoveAllChildren(true);
		}
	}
}

