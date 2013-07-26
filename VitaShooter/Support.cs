using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Imaging;	// Font

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace VitaShooter
{
	public class Support
	{
		
		public class CustomLabel : SpriteUV
		{
			public Label label;
			
			public CustomLabel(Vector2 position, String text, FontMap fontmap)
			{
				label = new Label(text, fontmap);

				this.BlendMode = BlendMode.None;
				this.TextureInfo = Director.Instance.GL.WhiteTextureInfo;
				this.Color = new Vector4(0.0f,0.0f,0.0f,1.0f);
				this.Position = position;
				
				this.Quad = new TRS( label.GetlContentLocalBounds() );
				this.CenterSprite(new Vector2(0.5f,0.5f));
				
				label.Position = new Vector2(-(label.GetlContentLocalBounds().Max/2.0f).X,0.0f);
				this.AddChild(label);
			
			}
		}
		
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

