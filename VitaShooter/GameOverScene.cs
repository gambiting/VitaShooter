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
	public class GameOverScene : BasicScene
	{
		public static GameOverScene Instance;
		
		//sprite for gameover screen
		SpriteUV gameoverSprite;
		
		Label gameoverLabel = new Label();
		
		public GameOverScene ()
		{
		}
		
		public void initGameOver()
		{
			this.Camera2D.SetViewFromViewport();
			gameoverSprite = new SpriteUV(new TextureInfo("/Application/data/gameover.png"));
			gameoverSprite.Scale = gameoverSprite.TextureInfo.TextureSizef;
			gameoverSprite.Position = new Vector2(300f,544.0f/2.0f- gameoverSprite.TextureInfo.TextureSizef.Y+ 80f);
			
			gameoverLabel.Text = "Your final score was: " + Game.Instance.score + "\nPress X to try again\nO to return to menu";
			gameoverLabel.Position = new Vector2(330f, 200.0f);
			
			Font f = new Font("/Application/data/data-latin.ttf",25, FontStyle.Bold);
			FontMap fm = new FontMap(f);
			gameoverLabel.FontMap = fm;
			
			Foreground.AddChild(gameoverSprite);
			Foreground.AddChild(gameoverLabel);
			
			
		}
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			initGameOver();
		}
		
		public override void OnExit ()
		{
			base.OnExit ();
			Foreground.RemoveAllChildren(true);
		}
		
		public override void Tick (float dt)
		{
			base.Tick (dt);
			
			if(Input2.GamePad0.Cross.Press)
			{

				
				SceneManager.Instance.changeSceneTo(Game.Instance);

			}
			if(Input2.GamePad0.Circle.Press)
			{
				
				SceneManager.Instance.changeSceneTo(MainMenu.Instance);
			}
		}
	}
}

