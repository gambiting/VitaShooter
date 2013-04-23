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
			foreach(MapTile mt in map.tiles)
			{
				if(mt.type.Equals("wall"))
				{
					if(mt.sprite!= null && mt.sprite.IsWorldPointInsideContentLocalBounds(ge.Position + proposedChange))
					{
						return true;
					}
						
					
				}
			}
			
			
			
			//Bounds2 box = new Bounds2();
			//ge.Children[0].GetContentWorldBounds(ref box);
			
			//System.Console.WriteLine(box.ToString());
			
			
			return false;
		}
	}
}

