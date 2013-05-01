
using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace VitaShooter
{
	public class UI : Sce.PlayStation.HighLevel.GameEngine2D.Node
	{
		public Label ammoLabel = new Label();
		
		public Label healthLabel = new Label();
		
		public Label scoreLabel = new Label();
		
		public Vector2 cameraOffset;
		
		public SpriteUV bottomBar;
		
		public SpriteUV ammoIcon;
		
		public SpriteUV heartIcon;
		
		public SpriteUV coinIcon;
		
		public UI ()
		{
			cameraOffset = new Vector2(-8.5f,-4.8f);
			
			
			this.AdHocDraw += this.Draw;
			
			TextureInfo bottomBarInfo = new TextureInfo(AppMain.ttCreateTexture(1,1, 0xFF000000));
			
			bottomBar = new SpriteUV(bottomBarInfo);
			bottomBar.Scale= new Vector2(30.0f,2.0f);
			
			TextureInfo ammoIconInfo = new TextureInfo("/Application/data/tiles/ammo.png");
			ammoIcon = new SpriteUV(ammoIconInfo);
			ammoIcon.Scale = new Vector2(0.6f,0.6f);
			
			TextureInfo heartIconInfo = new TextureInfo("/Application/data/heart.png");
			heartIcon = new SpriteUV(heartIconInfo);
			heartIcon.Scale = new Vector2(0.6f,0.5f);
			
			TextureInfo coinIconInfo = new TextureInfo("/Application/data/coin.png");
			coinIcon = new SpriteUV(coinIconInfo);
			coinIcon.Scale = new Vector2(0.6f,0.6f);
			
			Font f = new Font("/Application/data/data-latin.ttf",15, FontStyle.Bold);
			FontMap fm = new FontMap(f);
			
			//ammoLabel.FontMap = fm; 
			//healthLabel.FontMap = fm;
			//scoreLabel.FontMap = fm;
			
			ammoLabel.Scale = new Vector2(0.05f,0.05f);
			healthLabel.Scale = new Vector2(0.05f,0.05f);
			scoreLabel.Scale = new Vector2(0.05f,0.05f);

			this.AddChild(bottomBar);
			this.AddChild(ammoIcon);
			this.AddChild(heartIcon);
			this.AddChild(coinIcon);
			this.AddChild(this.ammoLabel);
			this.AddChild (this.healthLabel);
			this.AddChild(this.scoreLabel);
			
		}
		
		public new void Draw()
		{
			
			bottomBar.Position = Game.Instance.camera.Center + cameraOffset + new Vector2(-1.0f,-1.2f);
			ammoIcon.Position = Game.Instance.camera.Center + cameraOffset + new Vector2(-0.1f,0.0f);
			heartIcon.Position = Game.Instance.camera.Center + cameraOffset + new Vector2(6.3f,0.0f);
			coinIcon.Position = Game.Instance.camera.Center + cameraOffset + new Vector2(11.5f,0.0f);
			
			ammoLabel.Text = "Ammo: " + Player.Instance.ammo + "/" + Player.Instance.maxAmmo;
			ammoLabel.Position = Game.Instance.camera.Center + cameraOffset + new Vector2(0.5f,0.0f);  //new Vector2(-2.0f,4.0f);
			
			healthLabel.Text = "Health: " + Player.Instance.Health;
			healthLabel.Position = Game.Instance.camera.Center + cameraOffset + new Vector2(7.0f,0.0f); //+ new Vector2(2.0f,-2.0f);  //new Vector2(-2.0f,4.0f);
			
			scoreLabel.Text = "Score: " + Game.Instance.score ;
			scoreLabel.Position = Game.Instance.camera.Center + cameraOffset + new Vector2(12f,0.0f);
			
			
			
			
		}
	}
}

