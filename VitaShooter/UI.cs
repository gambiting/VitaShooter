
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
			//camera which is independent from the scene camera - means that all changes done to the underlying scene will not
			//change the perspective of the UI and that it will stay in place
			//the default view for the UI camera is ViewFromViewport - so the operational area is as big as the screen - currently 960x540
			this.Camera = (ICamera) SceneManager.Instance.UICamera;
			
			this.AdHocDraw += this.Update;
			
			TextureInfo bottomBarInfo = new TextureInfo(AppMain.ttCreateTexture(1,1, 0xFF000000));
			
			bottomBar = new SpriteUV(bottomBarInfo);
			//texture is 1x1 so we need to scale it to size we want to occupy on the screen
			bottomBar.Scale= new Vector2(960.0f,50.0f);
			
			TextureInfo ammoIconInfo = new TextureInfo("/Application/data/tiles/ammo.png");
			ammoIcon = new SpriteUV(ammoIconInfo);
			ammoIcon.Scale = new Vector2(20,20);
			
			TextureInfo heartIconInfo = new TextureInfo("/Application/data/heart.png");
			heartIcon = new SpriteUV(heartIconInfo);
			heartIcon.Scale = new Vector2(20,20);
			
			TextureInfo coinIconInfo = new TextureInfo("/Application/data/coin.png");
			coinIcon = new SpriteUV(coinIconInfo);
			coinIcon.Scale = new Vector2(20,20);
			
			//experimental fontmap
			Font f = new Font("/Application/data/data-latin.ttf",15, FontStyle.Bold);
			FontMap fm = new FontMap(f);
			
			bottomBar.Position = new Vector2(0.0f,0.0f);
			
			coinIcon.Position =  new Vector2(176.0f,15.0f);			
			heartIcon.Position =  new Vector2(424.0f,15.0f);
			ammoIcon.Position =  new Vector2(676.0f,15.0f);			

			scoreLabel.Position =  new Vector2(200f,20.0f);
			healthLabel.Position =  new Vector2(450.0f,20.0f);			
			ammoLabel.Position =  new Vector2(700.0f,20.0f);

			this.AddChild(bottomBar);
			this.AddChild(ammoIcon);
			this.AddChild(heartIcon);
			this.AddChild(coinIcon);
			this.AddChild(this.ammoLabel);
			this.AddChild (this.healthLabel);
			this.AddChild(this.scoreLabel);
			
			this.Position = new Vector2(0.0f,0.0f);
		}
		
		public void Update()
		{
			
			ammoLabel.Text = "Ammo: " + Player.Instance.ammo + "/" + Player.Instance.maxAmmo;
			
			healthLabel.Text = "Health: " + Player.Instance.Health;
			
			scoreLabel.Text = "Score: " + Game.Instance.score ;
			
			
		}
	}
}

