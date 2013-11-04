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
	 * Class used to parse and represent a map loaded from a file
	 * 
	 * */
	public class Map
	{
		public static Map Instance;
		public List<MapTile> tiles;
		
		public String filePath;
		
		public List<MapTile> wallTiles;
		public int width = 0, height = 0;
		public List<SpriteList> spriteList { get; set;}
		
		public Random random;
		
		public Dictionary<MapTile.Types, Vector2i> tileLocations; 
		
		
		public SpriteUV thumbnailSprite;
		
		public Map (string filePath)
		{
			this.filePath = filePath;
			random = new Random();
			
			tileLocations = new Dictionary<MapTile.Types, Vector2i>();
			
			setupLocations();
			
			tiles = new List<MapTile> ();
			wallTiles = new List<MapTile> ();
			
			
			ParseFile (this, filePath, tiles);
			
			tiles.Reverse();
			
			prepareDescriptions(this, tiles);
			
			spriteList = prepareTiles (this, tiles);
			
			
			Console.WriteLine ("map: " + filePath);
			Console.WriteLine ("width: " + width);
			Console.WriteLine ("heigh: " + height);
			
			//prepare thumbnail
			
			//create new scene for the thumbnail
			ThumbnailScene ts = new ThumbnailScene();
			
			//set up the camera so the entire level is visible
			ts.Camera2D.SetViewFromWidthAndCenter(FMath.Max(width,height), new Vector2(width/2.0f,height/2.0f));
			
			
			
			//create a new framebuffer for the thumbnail
			FrameBuffer thumbnailBuffer = new FrameBuffer ();
			//create an associated texture
			Texture2D tex2d = new Texture2D (256,256, false, PixelFormat.Rgba, PixelBufferOption.Renderable);
			thumbnailBuffer.SetColorTarget (tex2d, 0);
			
			//sprite to contain the thumbnail
			thumbnailSprite = new SpriteUV (new TextureInfo (tex2d));
			
			
			//render the thumbnail:
			if(Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.CurrentScene==null)
			{
				Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.RunWithScene(ts, true);
			}else{
				Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.ReplaceScene(ts);
			}
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.Update ();
			
			//save the current framebuffer and viewport
			FrameBuffer oldBuffer = Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.GetFrameBuffer();
			ImageRect oldViewport = Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.GetViewport();
			Vector4 oldClearColour = Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.GetClearColor();
			
			//set the new framebuffer for the thumbnail
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetFrameBuffer (thumbnailBuffer);
			//set the correct viewport
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetViewport (0, 0, 256,256);
			
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetClearColor(new Vector4(0.0f,0.0f,0.0f,1.0f));
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.Clear();
			
			//add the spritelists to the scene
			foreach(SpriteList sl in spriteList)
			{
				
				ts.Background.AddChild(sl);
			}
			
			//render the thumbnail scene
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.CurrentScene.render();
			
			//switch back to old framebuffer and viewport:
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetFrameBuffer (oldBuffer);
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetViewport(oldViewport);
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetClearColor(oldClearColour);
			
			
			//get rid of the scene
			//remove all children,but WITHOUT THE CLEANUP(otherwise they won't work in the main Game scene)
			ts.Background.RemoveAllChildren(false);
			ts = null;
			
			//Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SwapBuffers ();
			//Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.PostSwap ();
			
			//make the thumbnail larger
			//thumbnailSprite.Position = new Vector2(100.0f,100.0f);
			thumbnailSprite.FlipV = true;
			thumbnailSprite.CenterSprite(new Vector2(0.5f,0.5f));
			thumbnailSprite.Scale = new Vector2(400.0f,400.0f);
			
		}
		
		
		/*
		 * Sets up the location of tiles in the map file so they can be easily searched
		 * */
		public void setupLocations()
		{
			/*tileLocations.Add("wall_corner_top_left", new Vector2i(0,0));
			tileLocations.Add("wall_corner_top_right", new Vector2i(1,0));
			tileLocations.Add("wall_corner_bottom_left", new Vector2i(2,0));
			tileLocations.Add("wall_corner_bottom_right", new Vector2i(3,0));;
			tileLocations.Add("wall_outer_bottom_right", new Vector2i(0,1));
			tileLocations.Add("wall_outer_bottom_left", new Vector2i(1,1));
			tileLocations.Add("wall_outer_top_right", new Vector2i(2,1));
			tileLocations.Add("wall_outer_top_left", new Vector2i(3,1));;
			tileLocations.Add("floor", new Vector2i(1,2));
			tileLocations.Add("floor_scratched", new Vector2i(0,2));
			tileLocations.Add("floor_bloody", new Vector2i(2,2));
			tileLocations.Add("empty", new Vector2i(3,2));
			tileLocations.Add("door", new Vector2i(3,2));  //door matched to the empty tile for now
			tileLocations.Add("wall_top", new Vector2i(0,3));
			tileLocations.Add("wall_bottom", new Vector2i(1,3));
			tileLocations.Add("wall_left", new Vector2i(3,3));
			tileLocations.Add("wall_right", new Vector2i(2,3));*/
			
			//simple setup:
			
			tileLocations.Add(MapTile.Types.wall, new Vector2i(0,2));
			tileLocations.Add (MapTile.Types.floor, new Vector2i(0,1));
			tileLocations.Add (MapTile.Types.empty,new Vector2i(0,0));
		}
		
		/*
		 * parses descriptions adding walls since they are normally not in the map
		 * */
		public void prepareDescriptions(Map m, List<MapTile> tiles)
		{

			/*
			 * FIRST PASS
			 * 
			 * set all tiles that have a floor next to them to be of type "wall"
			 * */
			for(int x=1;x<width-1;x++)
			{
				for(int y=1;y<height-1;y++)
				{
					int position = y*m.width+x;
					if(tiles[position].type == MapTile.Types.empty)
					{
						if(tiles[position+m.width].type == MapTile.Types.floor ||	//above
						   tiles[position-m.width].type == MapTile.Types.floor ||	//below
						   tiles[position+1].type == MapTile.Types.floor ||		//right
						   tiles[position-1].type == MapTile.Types.floor)			//left
						{
							tiles[position].type = MapTile.Types.wall;
						}
					}
				}
			}
			
			
			
			
			//frame
			for(int x=0;x<m.width;x++)
			{
				tiles[x].type= MapTile.Types.wall;
				tiles[(m.width)*(m.height-2) +x].type = MapTile.Types.wall;
			}
			
			for(int x=0;x<m.height-1;x++)
			{
				tiles[x*m.width].type= MapTile.Types.wall;
				tiles[x*m.width+m.width-1].type = MapTile.Types.wall;
			}
		
			
			foreach(MapTile mt in tiles)
			{
				if(mt.type == MapTile.Types.wall)
				{
					wallTiles.Add(mt);
				}
			}
			
			
		}
		
		
		/*
		 * Returnes a list with tiles of specified type
		 * */
		public List<MapTile> returnTilesOfType(MapTile.Types t)
		{
			List<MapTile> list = new List<MapTile>();
			
			foreach(MapTile mt in tiles)
			{
				if(mt.type == t)
				{
					list.Add(mt);
				}
			}
			
			return list;
		}
		
		/*
		 * prepares the tiles, x and y are set as a ratio of their position to the size of the map
		 * returns the sprite list which can then be used as a background
		 * */
		public List<SpriteList> prepareTiles (Map m, List<MapTile> tiles)
		{
			
			List<SpriteList> mapSpriteLists = new List<SpriteList>();
			
			//calculate x and y
			/*foreach(MapTile mt in tiles)
			{
				mt.x = (tiles.IndexOf(mt)%m.width)/(float)m.width;
				mt.y = ((int)(tiles.IndexOf(mt)/m.width))/(float)m.height;
				
				Console.WriteLine(mt.x + " , " + mt.y); 
			}*/
			
			var tex = new Texture2D ("/Application/data/tiles/simple5.png", false);
			
			tex.SetFilter(TextureFilterMode.Disabled);
			tex.SetWrap(TextureWrapMode.ClampToEdge);
			
			//texture map used for tiles is specified here(its dimensions)
			var texture = new TextureInfo ( tex,  new Vector2i (1, 13));
			
			
			//debug
			//System.Console.WriteLine(texture.TileSizeInPixelsf.ToString());
			
			//spritelist for the entire map
			SpriteList spriteList = new SpriteList( texture)
			{ 
				BlendMode = BlendMode.Normal
			};
			spriteList.EnableLocalTransform = true;
			
			Vector2i numCells = new Vector2i (m.width,m.height);
			
			for (int y=0; y<numCells.Y-1; y++) 
			{
				for (int x=0; x<numCells.X; x++)
				{
					
					int position = (y*m.width)+x;
					
					//why is this here? was in the feature catalog sample but I am not sure of its purpose
					//changing uv coordinates "should" move the sprite,but it does not
					//changing sprite.position achieves this goal instead
					//Vector2 uv = new Vector2 ((float)x, (float)y) / numCells.Vector2 ();
					var sprite = returnSpriteFromTile(tiles[position].type,texture);
					
					Vector2 p = new Vector2(x,y) ;//- (new Vector2(m.width,m.height))/2.0f ;
					
					//DEBUG
					//System.Console.WriteLine(p.ToString());
					
					//save the tile position in its object
					tiles[position].position = p;
					
					//set that position to the sprite
					sprite.Position = p;
					
					//add a random disco effect to the floor tiles
					/*if(tiles[position].type == MapTile.Types.floor)
					{
						sprite.Schedule( (dt) => { 

							//change the floor texture to a random one from the same pack
							if(Common.FrameCount%10==0)
							{
								var a = Support.random.Next(4,12);
								
								sprite.TileIndex2D = new Vector2i(0,a);
								
							}
							
						
						} );
					}*/
					
					tiles[position].sprite = sprite;
					
					//get the local bounds
					sprite.GetlContentLocalBounds(ref tiles[position].bounds);
					spriteList.AddChild(tiles[position].sprite);
					
						
				}
					
			}
			mapSpriteLists.Add(spriteList);
			
			return mapSpriteLists;
			
		}
		
		/*
		 * returns a sprite constructed from the given tile name
		 * tile needs to be part of the given texture
		 * */
		public SpriteTile returnSpriteFromTile(MapTile.Types tileName,TextureInfo texture)
		{
			//get the location from the dictionary
			Vector2i textureLocation;
			if(!tileLocations.TryGetValue(tileName, out textureLocation))
			{
				//if there was no such tile in the dictionary then set the location to 0,0 as a failsafe
				textureLocation = new Vector2i(1,0);
			}
			
			var rand = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator();
			
			//construct the sprite using given texture and knowing the location of the tile in the tilemap
			var sprite = new SpriteTile ()
			{
					TextureInfo = texture
					, TileIndex2D = textureLocation
					
			};
			
			return sprite;
		}
		
		/*
		 * parses the map file, figures out the width and height of the map
		 * creates basic three types of tiles - empty tiles, floor tiles and doors
		 * only these three types are included by the generator.
		 * */
		public void ParseFile (Map m, string filePath, List<MapTile> tiles)
		{
			FileStream fileStream = new FileStream (filePath, FileMode.Open, FileAccess.Read);
			StreamReader sr = new StreamReader (fileStream);
			
			int tempWidth = 0;
			
			try {
				while (!sr.EndOfStream) {
					char character = (char)sr.Read ();
					
					if (character == '\t') {
						tiles.Add (new MapTile (MapTile.Types.empty));
					} else if (character == 'D') {
						tiles.Add (new MapTile (MapTile.Types.floor)); //temporary change from the door
						//doors normaly are symolized by two characters,so we need to get rid of the other one
						sr.Read ();
						//consume the tab at the end
						sr.Read ();
						     
					} else if (character == 'F') {
						tiles.Add (new MapTile (MapTile.Types.floor));
						//consume the tab at the end
						sr.Read ();
					}
					
					
					if(character == '\n') {
						tiles.Add (new MapTile (MapTile.Types.empty));
						m.height++;
						
						if (m.width == 0)
							tempWidth++;
						
						m.width = tempWidth;
						
					} else if (character == '\r')
					{
					}else if (m.width == 0) {
						tempWidth++;
					}
				}
				
			} finally {
				m.height++;
				fileStream.Close ();
			}
		}
	}
	
	
	
	
}

