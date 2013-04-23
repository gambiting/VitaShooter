using System;
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
	public class MapTile
	{
		public enum Types {wall=1,floor=2,empty=3};
		public Types type;
		public float x {get; set;}
		public float y {get; set;}
		
		public Vector2 position {get; set;}
		public SpriteTile sprite;
		
		public Bounds2 bounds;
		
		public MapTile ()
		{
			bounds = new Bounds2();
		}
		
		public MapTile(Types t)
		{
			type = t;
			bounds = new Bounds2();
		}
	}
}

