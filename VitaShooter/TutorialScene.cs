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
	public class TutorialScene : BasicScene
	{
		//tutorial sprites
		SpriteUV tut1;
		
		public int tutorialProgress=1;
		
		public static TutorialScene Instance;
		
		public TutorialScene ()
		{
			
			this.Camera2D.SetViewFromViewport();
			
			tut1 = new SpriteUV(new TextureInfo("/Application/data/tut1.png"));
			tut1.Scale = tut1.TextureInfo.TextureSizef;
			Foreground.AddChild(tut1);
		}
		
		public override void Tick (float dt)
		{
			base.Tick (dt);
			if(Input2.GamePad0.Cross.Press)
			{
				switch(tutorialProgress)
				{
				case 1:
					tut1.TextureInfo = new TextureInfo("/Application/data/tut2.png");
					break;
				case 2:
					tut1.TextureInfo = new TextureInfo("/Application/data/tut3.png");
					break;
				case 3:
					tut1.TextureInfo = new TextureInfo("/Application/data/tut4.png");
					break;
				case 4:
					tut1.TextureInfo = new TextureInfo("/Application/data/tut5.png");
					break;
				case 5:
					tutorialProgress=1;
					SceneManager.Instance.changeSceneTo(MainMenu.Instance);
					break;
					
				}
				tutorialProgress++;	
			}
		}
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			
			tut1.TextureInfo = new TextureInfo("/Application/data/tut2.png");
		}
		
	}
}

