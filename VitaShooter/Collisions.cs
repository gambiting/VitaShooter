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
		
		
		
		public static bool checkWallsCollisions(GameEntity ge, Map map, ref Vector2 proposedChange)
		{
			
			//alternative,experimental method
			int position = ((int)System.Math.Floor(ge.Position.Y+proposedChange.Y)*map.width)+(int)System.Math.Floor(ge.Position.X+proposedChange.X);
			
			if(map.tiles[position].type == MapTile.Types.wall)
			{
				return true;
			}

			return false;
		}
		
		
		public static bool checkWallsCollisionsSimple(Vector2 location, Map map)
		{
			int position = ((int)System.Math.Floor(location.Y)*map.width)+(int)System.Math.Floor(location.X);
			
			if(map.tiles[position].type == MapTile.Types.wall)
			{
				return true;
			}

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
		
		public static bool efficientCollisionsCheck(GameEntity ge, Vector2 proposedChange, out GameEntity oge)
		{
			
			AABB tempRange;
			
			tempRange.center = ge.Position + proposedChange.Normalize();
			tempRange.halfDimension = new Vector2(0.4f,0.4f);
			GameEntity e = Game.Instance.quadTree.queryRange(tempRange);
			
			
			oge = null;
			
			if(e == null || e.Equals(ge)  ) return false;
			else return true;
		}
		
		public static bool checkEnemiesCollisions(GameEntity ge, List<Enemy> list, Vector2 proposedChange, out Enemy oe)
		{
			//temp bounds
			Bounds2 tempBounds1 = new Bounds2(ge.Position + proposedChange + ge.bounds.Min, ge.Position + proposedChange + ge.bounds.Max);
			foreach(Enemy e in list)
			{
				if(ge.GetType() == typeof(BasicEnemy) && e.Equals(ge))
				{
					continue;
				}
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
		
		public static bool findNearestEnemy(GameEntity ge, float range, out GameEntity oge)
		{
			
			
			AABB tempRange;
			
			tempRange.center = ge.Position;
			tempRange.halfDimension = new Vector2(range,range);
			
			
			List<GameEntity> tempList = Game.Instance.quadTree.queryRangeList(tempRange);
			
			GameEntity temp;
			
			if(tempList.Count>0)
			{
				temp = tempList[0];
			}else
			{
				oge = null;
				return false;
			}
			
			foreach(GameEntity g in tempList)
			{
				if(g.Position.DistanceSquared(ge.Position) < temp.Position.DistanceSquared(ge.Position))
				{
					temp = g;
				}
			}
			
			oge = temp;
			return true;
		}
		
		
		public static bool checkCollisionBetweenEntities(GameEntity ge1, GameEntity ge2)
		{
			Bounds2 tempBounds1 = new Bounds2(ge1.Position + ge1.bounds.Min, ge1.Position + ge1.bounds.Max);
			Bounds2 tempBounds2 = new Bounds2(ge2.Position + ge2.bounds.Min, ge2.Position + ge2.bounds.Max);
			
			
			return tempBounds1.Overlaps(tempBounds2);
		}
	}
}

