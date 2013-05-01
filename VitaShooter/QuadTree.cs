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
	public struct AABB
	{
	  public Vector2 center;
	  public Vector2 halfDimension;

	}
		
	public class QuadTree
	{
		public static int nodes=0;
		// Axis-aligned bounding box stored as a center with half-dimensions
		  // to represent the boundaries of this quad tree
		  AABB boundary;
		
		  // Points in this quad tree node
		  GameEntity entity;
		
		  // Children
		  QuadTree northWest;
		  QuadTree northEast;
		  QuadTree southWest;
		  QuadTree southEast;
		
		int entitiesWithin = 0;
		
		
		public static bool containsPoint(AABB bound,Vector2 position)
		{
			if((bound.center.X+bound.halfDimension.X)>position.X &&
			   (bound.center.X-bound.halfDimension.X)<=position.X &&
			   (bound.center.Y+bound.halfDimension.Y)>position.Y &&
			   (bound.center.Y-bound.halfDimension.Y)<=position.Y)
				return true;
			
			return false;
		}
		
		public static bool intersectsAABB(AABB a, AABB b)
		{
			if((a.center.X-a.halfDimension.X)<= (b.center.X+b.halfDimension.X) &&
			   (b.center.X-b.halfDimension.X)<= (a.center.X+a.halfDimension.X) &&
			   (a.center.Y-a.halfDimension.Y)<= (b.center.Y+b.halfDimension.Y) &&
			   (b.center.Y-b.halfDimension.Y)<= (a.center.Y+a.halfDimension.Y))
				return true;
			
			return false;
		}
		
		
		
		public QuadTree (Vector2 Center, Vector2 halfDimension)
		{
			this.boundary.center = Center;
			this.boundary.halfDimension = halfDimension;
			//nodes++;
			//System.Console.WriteLine(nodes.ToString());
		}
		
		
		public bool insert(GameEntity entity)
		{
			if (!containsPoint(this.boundary,entity.Position))
		    	return false; // object cannot be added
		
		    // If there is space in this quad tree, add the object here
			
			this.entitiesWithin++;
		    if(this.entity == null)
			{
				this.entity = entity;
				return true;
			}
		
		    // Otherwise, we need to subdivide then add the point to whichever node will accept it
		    if (northWest == null)
			{
				this.northWest = new QuadTree(this.boundary.center + this.boundary.halfDimension/(new Vector2(-2.0f,2.0f)), this.boundary.halfDimension/2.0f);
			}
		    if (northWest.insert(entity)) return true;
			
			
			if(northEast==null)
			{
				this.northEast = new QuadTree(this.boundary.center + this.boundary.halfDimension/(new Vector2(2.0f,2.0f)), this.boundary.halfDimension/2.0f); 
			}
		    if (northEast.insert(entity)) return true;
			
			if(southWest==null)
			{
				this.southWest = new QuadTree(this.boundary.center + this.boundary.halfDimension/(new Vector2(-2.0f,-2.0f)), this.boundary.halfDimension/2.0f);
			}
		    if (southWest.insert(entity)) return true;
			
			if(southEast==null)
			{
				this.southEast = new QuadTree(this.boundary.center + this.boundary.halfDimension/(new Vector2(2.0f,-2.0f)), this.boundary.halfDimension/2.0f);
			}
		    if (southEast.insert(entity)) return true;
		
		    // Otherwise, the point cannot be inserted for some unknown reason (which should never happen)
		    return false;
		}
		
		
		/*public List<GameEntity> queryRange(AABB range)
		{
			List<GameEntity> tempList = new List<GameEntity>();
			
			if (!intersectsAABB(this.boundary,range))
		      return tempList; // empty list
		
			
		    // Check objects at this quad level
		    if (this.entity != null && containsPoint(range,this.entity.Position))
		        tempList.Add(this.entity);

		
		    // Terminate here, if there are no children
		    if (northWest == null)
		      return tempList;
		
		    // Otherwise, add the points from the children
		    tempList.AddRange(northWest.queryRange(range));
		    tempList.AddRange(northEast.queryRange(range));
		    tempList.AddRange(southWest.queryRange(range));
		    tempList.AddRange(southEast.queryRange(range));
		
		    return tempList;
		}*/
		
		
		public GameEntity queryRange(AABB range)
		{
			if(this.entitiesWithin==0)
				return null;
			
			if (!intersectsAABB(this.boundary,range))
		      return null; // empty object
			
			if (this.entity != null && containsPoint(range,this.entity.Position))
		        return this.entity;
			
			
			if(northWest!=null) 
			{
				GameEntity temp = northWest.queryRange(range);
				if(temp!=null) return temp;
			}
			if(northEast!=null) 
			{
				GameEntity temp = northEast.queryRange(range);
				if(temp!=null) return temp;
			}
			if(southEast!=null) 
			{
				GameEntity temp = southEast.queryRange(range);
				if(temp!=null) return temp;
			}
			if(southWest!=null) 
			{
				GameEntity temp = southWest.queryRange(range);
				if(temp!=null) return temp;
			}
			
			return null;
		}
		
		public List<GameEntity> queryRangeList(AABB range)
		{
			List<GameEntity> list = new List<GameEntity>();
			
			if(!intersectsAABB(this.boundary, range))
			   return list;
			   
			   
			if(this.entity!=null && containsPoint(range,entity.Position))
			{
				list.Add(this.entity);
			}
			
			
			if(northWest!=null) list.AddRange(northWest.queryRangeList(range));
			if(northEast!=null) list.AddRange(northEast.queryRangeList(range));
			if(southWest!=null) list.AddRange(southWest.queryRangeList(range));
			if(southEast!=null) list.AddRange(southEast.queryRangeList(range));
			
			return list;
		}
		
		
		public bool removeEntity(GameEntity e)
		{
			//correction for the floating point error
			AABB tempBoundary;
			tempBoundary.center= this.boundary.center;
			tempBoundary.halfDimension = this.boundary.halfDimension + new Vector2(0.1f,0.1f);
			
			if(!containsPoint(tempBoundary,e.Position))
			{
				return false;
			}
			
			

			if(entity != null && this.entity.Equals(e))
			{
				this.entitiesWithin--;
				entity = null;
				return true;
			}
			
			if(northWest != null)
			{
				if(northWest.removeEntity(e))
				{
					this.entitiesWithin--;
					if(northWest.entitiesWithin==0) northWest=null;
					
					return true;
				}
			}
			if(northEast != null)
			{
				if(northEast.removeEntity(e))
				{
					this.entitiesWithin--;
					if(northEast.entitiesWithin==0) northEast=null;
					
					return true;
				}
			}
			if(southWest != null)
			{
				if(southWest.removeEntity(e))
				{
					this.entitiesWithin--;
					if(southWest.entitiesWithin==0) southWest=null;
					
					return true;
				}
			}
			if(southEast != null)
			{
				if(southEast.removeEntity(e))
				{
					this.entitiesWithin--;
					if(southEast.entitiesWithin==0) southEast=null;
					
					return true;
				}
			}
			
			return false;
		}
	}
}

