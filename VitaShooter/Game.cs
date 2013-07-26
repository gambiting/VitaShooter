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
	public class Game : BasicScene
	{
		
		public static Game Instance;
		
		
		
		

		//enemy sprite list
		public SpriteList enemySpriteList;
		
		//bullets
		public List<Bullet> bulletList;
		
		//ammo packs
		public List<AmmoItem> ammoList;
		
		//enemies
		public List<Enemy> enemyList;
		
		//QuadTree for colliding objects
		public QuadTree quadTree;
		
		public UI ui;
		
		public int score=0;
		
		public bool paused=false;
		
		public float cameraHeight = 10.0f;
		
		public float multitouchDistance = 0.0f;
		
		public static bool autoAim = true;
		
		public Game ()
		{
			

			
			MusicSystem.Instance.Play("DST-Darkseid.mp3");
		}
		
		

		
		
		
		
		
		public void initGame()
		{
			cameraHeight = 10.0f;
			
			//set view close to the scene
			this.Camera2D.SetViewFromHeightAndCenter(cameraHeight, Sce.PlayStation.HighLevel.GameEngine2D.Base.Math._00);
			
			
			
			//add all sprites loaded from the map
			foreach(SpriteList sl in MapManager.Instance.currentMap.spriteList)
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
			List<MapTile> list = MapManager.Instance.currentMap.returnTilesOfType(MapTile.Types.floor);
			
			//add a specified number of ammo packs
			for(int i=0;i<AmmoItem.noOfAmmoToGenerate;i++)
			{
				AmmoItem a = new AmmoItem(list[Support.random.Next(0,list.Count-1)].position);
				ammoList.Add(a);
				World.AddChild(a);
			}
			
			//create the quad tree
			quadTree = new QuadTree(new Vector2 (MapManager.Instance.currentMap.width/2.0f, MapManager.Instance.currentMap.height/2.0f), new Vector2 (MapManager.Instance.currentMap.width/2.0f, MapManager.Instance.currentMap.height/2.0f));
			
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
			list = MapManager.Instance.currentMap.returnTilesOfType(MapTile.Types.floor);
			
			//generate a given number of enemies
			for(int i=0;i<BasicEnemy.noOfEnemiesToGenerate;i++)
			{
				Enemy e = new BasicEnemy(list[Support.random.Next(0,list.Count-1)].position, texture);
				enemyList.Add(e);
				enemySpriteList.AddChild(((BasicEnemy)e).sprite);
				EffectsLayer.AddChild(e);
				quadTree.insert(e);
			}
			
			
			Foreground.AddChild(enemySpriteList);
			
			ui = new UI();
			Interface.AddChild(ui);
			
			
			//add an enemy spawner every second
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, (dt) => {
				
				list = MapManager.Instance.currentMap.returnTilesOfType(MapTile.Types.floor);
				EnemySpawnPoint esp = new EnemySpawnPoint(list[Support.random.Next(0,list.Count-1)].position);
				World.AddChild(esp);


			;}, 1.0f,false, -1);
			
			
		}
		
		public override void Tick (float dt)
		{
			//base.Tick(dt);
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
			
			
			//check start button(for pause menu)
			if(Input2.GamePad0.Start.Press)
			{
				this.paused = true;
				SceneManager.Instance.pushScene(PauseScene.Instance);
			}
			
			// pinch-to-zoom
			//only check for multitouch points if device supports more than one touch point
			if(Input2.Touch.MaxTouch>1)
			{
				//only proceed if the user is actually touching both points
				if(Input2.Touch.GetData(0)[0].Down && Input2.Touch.GetData(0)[1].Down)
				{
					
					
				
					//first touch point
					Vector2 touch1 = Director.Instance.CurrentScene.GetTouchPos(0);
					
					//second touch point
					Vector2 touch2 = Director.Instance.CurrentScene.GetTouchPos(1);
					
					//reset the distance measure if the screen has just been touched
					if(Input2.Touch.GetData(0)[0].Press || Input2.Touch.GetData(0)[1].Press)
					{
						multitouchDistance = touch1.Distance(touch2);
					}else
					{
						var newDistance = touch1.Distance(touch2);
						
						cameraHeight+=(multitouchDistance-newDistance);
						this.Camera2D.SetViewFromHeightAndCenter(cameraHeight, Player.Instance.Position);
					}
				}
				
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
				
				
				
				SceneManager.Instance.changeSceneTo(GameOverScene.Instance);
				//Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(Scene, gameTick);
				//Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(Scene, gameoverTick, 0.0f, false);
				
			}
		}
		
		public void setCameraPosition()
		{
			//camera.Center = new Vector2((float) System.Math.Round(player.Position.X,1),(float) System.Math.Round(player.Position.Y,1)) ;
			this.Camera2D.Center = Player.Instance.Position;
			
			//camera.Center.
			
			//camera.Center.MoveTo(player.Position,1f);
			
		}
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			score = 0;
			
			if(!paused)
			{
				this.initGame();
			}else{
				paused = false;
			}
		}
		
		public override void OnExit ()
		{
			base.OnExit ();
			
			if(!paused)
			{
				this.Background.RemoveAllChildren(true);
				this.Foreground.RemoveAllChildren(true);
				this.EffectsLayer.RemoveAllChildren(true);
				this.World.RemoveAllChildren(true);
				this.Interface.RemoveAllChildren(true);
			}
			
			
		}
		
	}
}

