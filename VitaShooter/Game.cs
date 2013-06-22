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
	public class Game
	{
		
		public static Game Instance;
		public Sce.PlayStation.HighLevel.GameEngine2D.Scene Scene { get; set; }
		
		
        public Layer Background { get; set; }
        public Layer World { get; set; }
        public Layer EffectsLayer { get; set; }
        public Layer Foreground { get; set; }
        public Layer Interface { get; set; }
		
		
		//buttons sprites
		SpriteUV logo;
		SpriteUV sprite_button_newgame;
		SpriteUV sprite_button_tutorial;
		SpriteUV sprite_button_autoaim;
		
		
		//tutorial sprites
		SpriteUV tut1;
		
		//sprite for gameover screen
		SpriteUV gameoverSprite;
		
		Label gameoverLabel = new Label();
		
		//menu background
		SpriteList menuBackground;
		
		
		public int menuSelection = 0;
		
		//enemy sprite list
		public SpriteList enemySpriteList;
		
		//bullets
		public List<Bullet> bulletList;
		
		//ammo packs
		public List<AmmoItem> ammoList;
		
		//enemies
		public List<Enemy> enemyList;
		
		//camera object
		public Camera2D camera;
		
		//QuadTree for colliding objects
		public QuadTree quadTree;
		
		public UI ui;
		
		public int score=0;
		
		public int tutorialProgress=0;
		
		public static bool autoAim = true;
		
		public Game ()
		{
			
			//create a new scene
			Scene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			
			//create layers for everyting
            Background = new Layer();
            World = new Layer();
            EffectsLayer = new Layer();
            Foreground = new Layer();
            Interface = new Layer();
			
			//add layers to the scene
			Scene.AddChild(Background);
            Scene.AddChild(World);
            Scene.AddChild(Foreground);
			Scene.AddChild(EffectsLayer);
            Scene.AddChild(Interface);
			
			camera = Scene.Camera as Camera2D;
			
			camera.SetViewFromHeightAndCenter(10.0f, Sce.PlayStation.HighLevel.GameEngine2D.Base.Math._00);

			initTitle();
			
			MusicSystem.Instance.Play("DST-Darkseid.mp3");
			
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, titleTick, 0.0f, false);
		}
		
		public void initTutorial()
		{
			camera.SetViewFromViewport();
			
			tut1 = new SpriteUV(new TextureInfo("/Application/data/tut1.png"));
			tut1.Scale = tut1.TextureInfo.TextureSizef;
			Foreground.AddChild(tut1);
		}
		
		public void tutorialTick(float dt)
		{
			
			
			
			if(Input2.GamePad0.Cross.Press)
			{
				switch(tutorialProgress)
				{
				case 1:
					tut1.TextureInfo = new TextureInfo("/Application/data/tut2.png");
					break;
				case 2:
					tut1.TextureInfo = new TextureInfo("/Application/data/tut3.png");
					break;
				case 3:
					tut1.TextureInfo = new TextureInfo("/Application/data/tut4.png");
					break;
				case 4:
					tut1.TextureInfo = new TextureInfo("/Application/data/tut5.png");
					break;
				case 5:
					Foreground.RemoveAllChildren(true);
					tutorialProgress=-1;
					initTitle();
					Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(Scene, tutorialTick);
					Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, titleTick, 0.0f, false);
					break;
					
				}
				tutorialProgress++;	
			}
			
		}
		
		public void initGameOver()
		{
			camera.SetViewFromViewport();
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
			
			score = 0;
		}
		
		public void gameoverTick(float dt)
		{
			if(Input2.GamePad0.Cross.Press)
			{
				Foreground.RemoveAllChildren(true);
				
				initGame();
				Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(Scene, gameoverTick);
				Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, gameTick, 0.0f, false);
			}
			if(Input2.GamePad0.Circle.Press)
			{
				Foreground.RemoveAllChildren(true);
				
				initTitle();
				Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(Scene, gameoverTick);
				Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, titleTick, 0.0f, false);
			}
		}
		
		public void initTitle()
		{
			camera.SetViewFromViewport();
			
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
					bgTile.TileIndex1D = AppMain.random.Next(4,13);
					bgTile.Position = new Vector2((float)x*32.0f,(float)y*32.0f);
					bgTile.Scale = bgTile.TextureInfo.TileSizeInPixelsf*2.0f;
					bgTile.ScheduleInterval((dt) => { bgTile.TileIndex1D = AppMain.random.Next(4,13); }, 0.2f,-1);
					menuBackground.AddChild(bgTile);
				}
			}
			
			Background.AddChild(menuBackground);
		}
		
		public void titleTick(float dt)
		{
			if(Input2.GamePad0.Cross.Press)
			{
				if(menuSelection==0)
				{
					Foreground.RemoveAllChildren(true);
					Background.RemoveAllChildren(true);
					initGame();
					Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(Scene, titleTick);
					Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, gameTick, 0.0f, false);
				}else if(menuSelection==1)
				{
					Foreground.RemoveAllChildren(true);
					Background.RemoveAllChildren(true);
					initTutorial();
					Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(Scene, titleTick);
					Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, tutorialTick, 0.0f, false);
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
		
		
		public void initGame()
		{
			//empty the scene
			Scene.RemoveAllChildren(true);
			
			//unschedule everything
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.UnscheduleAll();
			
			//create layers for everyting
            Background = new Layer();
            World = new Layer();
            EffectsLayer = new Layer();
            Foreground = new Layer();
            Interface = new Layer();
			
			//add layers to the scene
			Scene.AddChild(Background);
            Scene.AddChild(World);
            Scene.AddChild(Foreground);
			Scene.AddChild(EffectsLayer);
            Scene.AddChild(Interface);
			
			//set view close to the scene
			camera.SetViewFromHeightAndCenter(10.0f, Sce.PlayStation.HighLevel.GameEngine2D.Base.Math._00);
			
			//create a new map
			Map.Instance =  new Map();
			
			//add all sprites loaded from the map
			foreach(SpriteList sl in Map.Instance.spriteList)
			{
				Background.AddChild(sl);
			}
			
			//load the fire texture for the bullet
			Bullet.fireTexture = new Texture2D( "/Application/data/tiles/fire.png", false );
			
			//texture for the points marker
			pointMarker.texture = new Texture2D("/Application/data/points100.png", false);
			
			//texture for the ammo marker
			ammoMarker.texture = new Texture2D("/Application/data/plusammo.png", false);
			
			Player.Instance = new Player();
			Foreground.AddChild(Player.Instance);
			
			//create the list for bullets
			bulletList = new List<Bullet>();
			
			//create ammo packs
			ammoList = new List<AmmoItem>();
			List<MapTile> list = Map.Instance.returnTilesOfType(MapTile.Types.floor);
			
			//add a specified number of ammo packs
			for(int i=0;i<AmmoItem.noOfAmmoToGenerate;i++)
			{
				AmmoItem a = new AmmoItem(list[AppMain.random.Next(0,list.Count-1)].position);
				ammoList.Add(a);
				World.AddChild(a);
			}
			
			//create the quad tree
			quadTree = new QuadTree(new Vector2 (Map.Instance.width/2.0f, Map.Instance.height/2.0f), new Vector2 (Map.Instance.width/2.0f, Map.Instance.height/2.0f));
			
			//create enemies
			var tex = new Texture2D ("/Application/data/tiles/enemy_sword2.png", false);
			tex.SetFilter(TextureFilterMode.Disabled);
			tex.SetWrap(TextureWrapMode.ClampToEdge);
			var texture = new TextureInfo ( tex,  new Vector2i (25, 1));
			
			//spritelist for the enemies
			enemySpriteList = new SpriteList( texture)
			{ 
				BlendMode = BlendMode.Normal
			};
			//spriteList.EnableLocalTransform = true;
			
			
			enemyList = new List<Enemy>();
			list = Map.Instance.returnTilesOfType(MapTile.Types.floor);
			
			//generate a given number of enemies
			for(int i=0;i<BasicEnemy.noOfEnemiesToGenerate;i++)
			{
				Enemy e = new BasicEnemy(list[AppMain.random.Next(0,list.Count-1)].position, texture);
				enemyList.Add(e);
				enemySpriteList.AddChild(((BasicEnemy)e).sprite);
				EffectsLayer.AddChild(e);
				quadTree.insert(e);
			}
			
			
			Foreground.AddChild(enemySpriteList);
			
			ui = new UI();
			Interface.AddChild(ui);
			
			
			//add an enemy spawner every second
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, (dt) => {
				
				list = Map.Instance.returnTilesOfType(MapTile.Types.floor);
				EnemySpawnPoint esp = new EnemySpawnPoint(list[AppMain.random.Next(0,list.Count-1)].position);
				World.AddChild(esp);


			;}, 1.0f,false, -1);
			
		}
		
		public void gameTick(float dt)
		{
			if(Player.Instance == null)
			{
				
				
				
				
			}else
			{
				setCameraPosition();
			}
			
			//check bullet delay
			if(Bullet.bulletDelay>0)
				Bullet.bulletDelay--;
			
			//check buttons
			if(Input2.GamePad0.Cross.Down || Input2.GamePad0.R.Down)
			{
				if(Bullet.bulletDelay==0 && Player.Instance.ammo>0)
				{
					SoundSystem.Instance.Play("shot.wav");
					Bullet bullet = new Bullet();
					bulletList.Add(bullet);
					World.AddChild(bullet);
					Bullet.bulletDelay=2;
					Player.Instance.ammo--;
					
					//update player's sprite
					Player.Instance.playerBodySprite.TileIndex1D = Player.Instance.animationFrame;
					Player.Instance.animationFrame = (Player.Instance.animationFrame+1)% Player.Instance.playerBodySprite.TextureInfo.NumTiles.X;
				}
			}
			
			//check buttons
			if(Input2.GamePad0.Cross.Release || Input2.GamePad0.R.Release)
			{

				//update player's sprite
				Player.Instance.playerBodySprite.TileIndex1D = 0;

			}
			
			
			
			//check if the player has collected any ammo packs
			AmmoItem ammoItemToRemove;
			if(Collisions.checkAmmoPackCollisions(Player.Instance,ammoList, out ammoItemToRemove))
			{
				Game.Instance.EffectsLayer.AddChild(new ammoMarker(ammoItemToRemove.Position));
				SoundSystem.Instance.Play("ammoclip.wav");
				World.RemoveChild(ammoItemToRemove,true);
				ammoList.Remove(ammoItemToRemove);
				
				Player.Instance.ammo = (int)FMath.Clamp((Player.Instance.ammo+50),100,100);
				
				ammoItemToRemove.Die();
			}
			
			
			
			
			if(Player.Instance.Health<=0)
			{
				Background.RemoveAllChildren(true);
				Foreground.RemoveAllChildren(true);
				EffectsLayer.RemoveAllChildren(true);
				World.RemoveAllChildren(true);
				Interface.RemoveAllChildren(true);
				
				
				
				initGameOver();
				
				Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(Scene, gameTick);
				Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, gameoverTick, 0.0f, false);
				
			}
		}
		
		public void setCameraPosition()
		{
			//camera.Center = new Vector2((float) System.Math.Round(player.Position.X,1),(float) System.Math.Round(player.Position.Y,1)) ;
			camera.Center = Player.Instance.Position;
			
			//camera.Center.
			
			//camera.Center.MoveTo(player.Position,1f);
			
		}
		
		
		// NOTE: no delta time, frame specific
		public void FrameUpdate()
		{

		}
	}
}

