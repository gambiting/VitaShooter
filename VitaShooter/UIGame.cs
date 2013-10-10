
using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace VitaShooter
{

	public class UIGame : Node
	{
		public Label label = new Label();
		
		public UIGame ()
		{
			label.Text = "hello world";
			label.Color = new Vector4(1.0f,0.0f,0.0f,1.0f);
			
			this.Position = new Vector2(100.0f,100.0f);
			
			this.AddChild(label);
			
			this.AdHocDraw += this.test ;
			
			this.Scale = new Vector2(100.0f,100.0f);
			
		}
		
		public void test()
		{
			Console.WriteLine("hello");
		}
	}
	
	
}

