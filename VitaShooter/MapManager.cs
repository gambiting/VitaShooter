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
	public class MapManager
	{
		public static MapManager Instance;
		
		public Map currentMap;
		
		public List<Map> predefinedMaps;
		
		public MapManager ()
		{
			predefinedMaps = new List<Map>();
			
			LoadMaps();
			
			if(predefinedMaps.Count>0)
			{
				currentMap = predefinedMaps[1];
			}else{
				System.Console.WriteLine("Error - no maps loaded!");
			}
		}
		
		//load all the maps in the maps directory
		public void LoadMaps()
		{
			foreach(String path in Directory.GetFiles("/Application/data/maps/"))
			{
				predefinedMaps.Add(new Map(path));
			}
		}
	}
}

