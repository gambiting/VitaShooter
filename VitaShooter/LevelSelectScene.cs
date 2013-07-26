using System;
using System.IO;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Imaging;

// Font
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace VitaShooter
{
	public class LevelSelectScene : BasicScene
	{
		public static LevelSelectScene Instance;
		public int levelSelection = 0;
		public static float thumbnailSize = 350.0f;
		public static float thumbnailSpacing = 50.0f;
		public static float transitionDuration = 0.5f;
		public Vector2 originalTouch;
		public List<Support.CustomLabel> labels;
		
		public LevelSelectScene ()
		{
		}
		
		public void initLevelSelect ()
		{
			this.levelSelection = 0;
			
			this.Camera2D.SetViewFromViewport ();
			
			//labels
			labels = new List<Support.CustomLabel> ();
			
			
			//mini background discofloor
			
			var tex = new Texture2D ("/Application/data/tiles/simple5.png", false);
			var texture = new TextureInfo (tex, new Vector2i (1, 13));
			
			var menuBackground = new SpriteList (texture);
			
			for (int x=0; x<30; x++) {
				for (int y=0; y<17; y++) {
					SpriteTile bgTile = new SpriteTile (texture);
					bgTile.TileIndex1D = Support.random.Next (4, 13);
					bgTile.Position = new Vector2 ((float)x * 32.0f, (float)y * 32.0f);
					bgTile.Scale = bgTile.TextureInfo.TileSizeInPixelsf * 2.0f;
					bgTile.ScheduleInterval ((dt) => {
						bgTile.TileIndex1D = Support.random.Next (4, 13); }, 0.2f, -1);
					menuBackground.AddChild (bgTile);
				}
			}
			
			Background.AddChild (menuBackground);
			
			
			
			for (int i=0; i<MapManager.Instance.predefinedMaps.Count; i++) {
				
				//add thumbnail
				float newX = 960.0f / 2.0f + i * (thumbnailSize + thumbnailSpacing);
				float scaleFactor = FMath.Clamp (thumbnailSize - FMath.Abs ((960.0f / 2.0f) - newX) / 4.0f, 0.0f, thumbnailSize);
				MapManager.Instance.predefinedMaps [i].thumbnailSprite.Scale = new Vector2 (scaleFactor, scaleFactor);
				MapManager.Instance.predefinedMaps [i].thumbnailSprite.Position = new Vector2 (newX, 544.0f / 2.0f);
				
				Foreground.AddChild (MapManager.Instance.predefinedMaps [i].thumbnailSprite);
				
				//add label:
				var tempLabel = new Support.CustomLabel (new Vector2 (newX, 544.0f / 2.0f - thumbnailSize * 0.6f), "Level " + (i + 1) + "\nHighscore: 0", SceneManager.UIFontMap);
				labels.Add (tempLabel);
				
				Foreground.AddChild (tempLabel);
				
			}
		}
		
		public override void Tick (float dt)
		{
			base.Tick (dt);
			
			if (Input2.GamePad0.Left.Press) {
				
				if (levelSelection > 0) {
					levelSelection -= 1;
		
					adjustPositions ();
				}
			}
			
			if (Input2.GamePad0.Right.Press) {
				
				if (levelSelection + 1 < MapManager.Instance.predefinedMaps.Count) {
					levelSelection += 1;
					adjustPositions ();
					
				}
			}
			
			if (Input2.GamePad0.Circle.Press) {
				SceneManager.Instance.changeSceneTo (MainMenu.Instance);
			}
			
			if (Input2.GamePad0.Cross.Press) {
				MapManager.Instance.currentMap = MapManager.Instance.predefinedMaps [levelSelection];
				
				SceneManager.Instance.changeSceneTo (Game.Instance);
			}
			
			if (Input2.Touch00.Down) {
				if (Input2.Touch00.Press) {
					originalTouch = GetTouchPos ();
				}
				
				
				for (int i=0; i<MapManager.Instance.predefinedMaps.Count; i++) {
						
					float newX = 960.0f / 2.0f + i * (thumbnailSize + thumbnailSpacing) - levelSelection * (thumbnailSize + thumbnailSpacing) - (originalTouch.X - GetTouchPos ().X) * 2.0f;
						
					MapManager.Instance.predefinedMaps [i].thumbnailSprite.RunAction (
						new MoveTo (
						new Vector2 (
						newX,
						MapManager.Instance.predefinedMaps [i].thumbnailSprite.Position.Y),
						transitionDuration));
						
					float scaleFactor = FMath.Clamp (thumbnailSize - FMath.Abs ((960.0f / 2.0f) - newX) / 4.0f, 0.0f, thumbnailSize);
						
					MapManager.Instance.predefinedMaps [i].thumbnailSprite.RunAction (
							new ScaleTo (new Vector2 (scaleFactor, scaleFactor), 0.5f));
					
					
					//adjust labels
				
					float labelScaleFactor = FMath.Clamp (1.0f - FMath.Abs ((960.0f / 2.0f) - newX) / 1000.0f, 0.0f, 1.0f);
					labels [i].RunAction (
						new MoveTo (
						new Vector2 (
						newX,
						MapManager.Instance.predefinedMaps [i].thumbnailSprite.Position.Y - thumbnailSize * 0.65f * labelScaleFactor), transitionDuration));
					labels [i].RunAction (new ScaleTo (new Vector2 (labelScaleFactor, labelScaleFactor), 0.5f));
				}
				
				
			}
			
			
			if (Input2.Touch00.Release) {
				levelSelection += (int)((originalTouch.X - GetTouchPos ().X) * 2.0f / (thumbnailSize * 0.75f));
				
				levelSelection = (int)FMath.Clamp (levelSelection, 0, MapManager.Instance.predefinedMaps.Count - 1);
				adjustPositions ();
			}
			
			
		}
		
		public void adjustPositions ()
		{
			for (int i=0; i<MapManager.Instance.predefinedMaps.Count; i++) {
						
				float newX = 960.0f / 2.0f + i * (thumbnailSize + thumbnailSpacing) - levelSelection * (thumbnailSize + thumbnailSpacing);
						
				MapManager.Instance.predefinedMaps [i].thumbnailSprite.RunAction (
						new MoveTo (
						new Vector2 (
						newX,
						MapManager.Instance.predefinedMaps [i].thumbnailSprite.Position.Y),
						transitionDuration));
						
				float scaleFactor = FMath.Clamp (thumbnailSize - FMath.Abs ((960.0f / 2.0f) - newX) / 4.0f, 0.0f, thumbnailSize);
						
				MapManager.Instance.predefinedMaps [i].thumbnailSprite.RunAction (
							new ScaleTo (new Vector2 (scaleFactor, scaleFactor), 0.5f));
				
				
				//adjust labels
				
				float labelScaleFactor = FMath.Clamp (1.0f - FMath.Abs ((960.0f / 2.0f) - newX) / 1000.0f, 0.0f, 1.0f);
				labels [i].RunAction (
						new MoveTo (
						new Vector2 (
						newX,
						MapManager.Instance.predefinedMaps [i].thumbnailSprite.Position.Y - thumbnailSize * 0.65f * labelScaleFactor), transitionDuration));
				labels [i].RunAction (new ScaleTo (new Vector2 (labelScaleFactor, labelScaleFactor), 0.5f));
				
				
				
			}
		}
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			
			this.initLevelSelect ();
			
		}
		
		public override void OnExit ()
		{
			base.OnExit ();
			
			this.Foreground.RemoveAllChildren (false);
			this.Background.RemoveAllChildren (false);
		}
		
		
	}
}

