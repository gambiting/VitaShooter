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
	public class Collisions
	{
		public Collisions ()
		{
		}
		
		
		
		public static bool checkWallsCollisions(GameEntity ge, Map map, Vector2 proposedChange)
		{
			//construct bounds for the coliding object
			Bounds2 tempBounds2 = new Bounds2(ge.Position + ge.bounds.Min + proposedChange, ge.Position + ge.bounds.Max + proposedChange);
			
			
			foreach(MapTile mt in map.wallTiles)
			{

				//construct bounds for the wall
				Bounds2 tempBounds1 = new Bounds2(mt.position + mt.bounds.Min ,mt.position + mt.bounds.Max);
			
				
				
				//check if overlap exists, if yes,return true
				if(tempBounds1.Overlaps(tempBounds2))
				{
					return true;
				}

			}
			
			
			
			//Bounds2 box = new Bounds2();
			//ge.Children[0].GetContentWorldBounds(ref box);
			
			//System.Console.WriteLine(box.ToString());
			
			
			return false;
		}
		
		public static bool checkAmmoPackCollisions(GameEntity ge, List<AmmoItem> list, out AmmoItem ai)
		{
			//temp bounds
			Bounds2 tempBounds1 = new Bounds2(ge.Position + ge.bounds.Min, ge.Position + ge.bounds.Max);
			
			
			
			foreach(AmmoItem a in list)
			{
				//for the ammo pack
				Bounds2 tempBounds2 = new Bounds2(a.Position + a.bounds.Min, a.Position + a.bounds.Max);
				
				if(tempBounds1.Overlaps(tempBounds2))
				{
					ai = a;
					return true;
				}
			}
			
			
			ai = null;
			return false;
		}
		
		public static bool checkEnemiesCollisions(GameEntity ge, List<Enemy> list, out Enemy oe)
		{
			//temp bounds
			Bounds2 tempBounds1 = new Bounds2(ge.Position + ge.bounds.Min, ge.Position + ge.bounds.Max);
			foreach(Enemy e in list)
			{
				//also making the enemy bound box bigger
				Bounds2 tempBounds2 = new Bounds2(e.Position + e.bounds.Min*2.0f, e.Position + e.bounds.Max*2.0f);
				
				if(tempBounds1.Overlaps(tempBounds2))
				{
					oe = e;
					return true;
				}
			}
			
			oe = null;
			return false;
		}
		
		
		public static bool checkCollisionBetweenEntities(GameEntity ge1, GameEntity ge2)
		{
			Bounds2 tempBounds1 = new Bounds2(ge1.Position + ge1.bounds.Min, ge1.Position + ge1.bounds.Max);
			Bounds2 tempBounds2 = new Bounds2(ge2.Position + ge2.bounds.Min, ge2.Position + ge2.bounds.Max);
			
			
			return tempBounds1.Overlaps(tempBounds2);
		}
	}
}

