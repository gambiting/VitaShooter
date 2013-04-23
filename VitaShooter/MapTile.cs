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
		public String type {get; set;}
		public float x {get; set;}
		public float y {get; set;}
		
		public Vector2 position {get; set;}
		public SpriteTile sprite;
		
		public MapTile ()
		{
		}
		
		public MapTile(String t)
		{
			type = t;
		}
	}
}

