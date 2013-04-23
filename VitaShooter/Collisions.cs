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
			foreach(MapTile mt in map.wallTiles)
			{

				//construct bounds for the wall
				Bounds2 tempBounds1 = new Bounds2(mt.position + mt.bounds.Min ,mt.position + mt.bounds.Max);
			
				//construct bounds for the coliding object
				Bounds2 tempBounds2 = new Bounds2(ge.Position + ge.bounds.Min + proposedChange, ge.Position + ge.bounds.Max + proposedChange);
				
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
	}
}

