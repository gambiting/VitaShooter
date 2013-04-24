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
        public Layer Curtains { get; set; }
        public Layer Interface { get; set; }
		
		//bullets
		public List<Bullet> bulletList;
		
		//ammo packs
		public List<AmmoItem> ammoList;
		
		//enemies
		public List<Enemy> enemyList;
		
		//camera object
		Camera2D camera;
		
		//map - dungeon 1
		Map dungeon1;
		
		
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
            Scene.AddChild(EffectsLayer);
            Scene.AddChild(Foreground);
            Scene.AddChild(Interface);
			

			//Vector2 ideal_screen_size = new Vector2(960.0f, 544.0f);
			camera = Scene.Camera as Camera2D;
			//camera.SetViewFromWidthAndCenter( 40.0f, Math._00 );
			
			camera.SetViewFromHeightAndCenter(10.0f, Sce.PlayStation.HighLevel.GameEngine2D.Base.Math._00);
			//camera.SetViewFromViewport();
			
			
			//load the map
			Map.Instance =  new Map();
			
			foreach(SpriteList sl in Map.Instance.spriteList)
			{
				Background.AddChild(sl);
			}
			
			//load the fire texture for the bullet
			Bullet.fireTexture = new Texture2D( "/Application/data/tiles/fire.png", false );
			
			
			Player.Instance = new Player();
			Foreground.AddChild(Player.Instance);
			
			//create the list for bullets
			bulletList = new List<Bullet>();
			
			
			//create ammo packs
			ammoList = new List<AmmoItem>();
			List<MapTile> list = Map.Instance.returnTilesOfType(MapTile.Types.floor);
			
			for(int i=0;i<AmmoItem.noOfAmmoToGenerate;i++)
			{
				AmmoItem a = new AmmoItem(list[AppMain.random.Next(0,list.Count-1)].position);
				ammoList.Add(a);
				World.AddChild(a);
			}
			
			
			//create enemies
			enemyList = new List<Enemy>();
			list = Map.Instance.returnTilesOfType(MapTile.Types.floor);
			
			for(int i=0;i<AmmoItem.noOfAmmoToGenerate;i++)
			{
				Enemy e = new BasicEnemy(list[AppMain.random.Next(0,list.Count-1)].position);
				enemyList.Add(e);
				Foreground.AddChild(e);
			}
			
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, gameTick, 0.0f, false);
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
				if(Bullet.bulletDelay==0)
				{
					SoundSystem.Instance.Play("shot.wav");
					Bullet bullet = new Bullet();
					bulletList.Add(bullet);
					World.AddChild(bullet);
					Bullet.bulletDelay=5;
					
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
				SoundSystem.Instance.Play("ammoclip.wav");
				World.RemoveChild(ammoItemToRemove,true);
				ammoList.Remove(ammoItemToRemove);
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
			/*Collider.Collide();
			
			foreach (GameEntity e in RemoveQueue)
				World.RemoveChild(e,true);
			foreach (GameEntity e in AddQueue)
				World.AddChild(e);
				
			RemoveQueue.Clear();
			AddQueue.Clear();
			
			// is player dead?
			if (PlayerDead)
			{
				if (PlayerInput.AnyButton())
				{
					// ui will transition to title mode
					World.RemoveAllChildren(true);
					Collider.Clear();
					PlayerDead = false;
					
					// hide UI and then null player to swap back to title
					UI.HangDownTarget = -1.0f;
					UI.HangDownSpeed = 0.175f;
					var sequence = new Sequence();
					sequence.Add(new DelayTime() { Duration = 0.4f });
					sequence.Add(new CallFunc(() => this.Player = null));
					World.RunAction(sequence);
				}
			}*/
		}
	}
}

