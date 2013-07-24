using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VitaShooter
{
	public class Support
	{
		public static Dictionary<string,string> GameParameters = new Dictionary<string, string>();
		
		public static float screenWidth,screenHeight;
		
		//two different types of random generators, for different purposes
		public static System.Random random = new System.Random ();
		public static Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator rand = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator ();
		
		public Support ()
		{
		}
		
		
		/*
		 * Loads common game parameters
		 * */
		public static void LoadGameParameters()
		{
			
			foreach(String line in File.ReadAllLines("/Application/data/configuration/GameConfig.txt"))
			{
				var parameters = line.Split('=');
				GameParameters.Add(parameters[0],parameters[1]);
			}

			
		}
	}
}

