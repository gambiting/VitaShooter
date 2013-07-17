using System.Collections.Generic;
using System.Xml;
using System.Threading;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Imaging;
// Font
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace VitaShooter
{
	public class SceneManager
	{
		public static SceneManager Instance;
		static FontMap UIFontMap;
		static FontMap LargeFontMap;
		System.Diagnostics.Stopwatch timer;
		FrameBuffer offscreenBuffer;
		SpriteUV offscreenSprite;
		
		public SceneManager ()
		{
			
		}
		
		public void changeSceneTo(Scene scene)
		{
			//set the director to run with the scene within the game object
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.PushScene (scene);
		}
		
		public void Initialize()
		{
			//initialize the director and set the clear colour to black
			Director.Initialize ();
			Director.Instance.GL.Context.SetClearColor (Colors.Black);
			
			//loading fonts - currently not used
			UIFontMap = new FontMap (new Font (FontAlias.System, 20, FontStyle.Bold));
			LargeFontMap = new FontMap (new Font (FontAlias.System, 48, FontStyle.Bold));
			
			//make a new Game object
			Game.Instance = new Game ();
			var game = Game.Instance;
			
			//make a new MainMenu object
			MainMenu.Instance = new MainMenu();
			
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.RunWithScene(MainMenu.Instance,false);
			
			//timer object to measure amount of time it takes to render a frame
			timer = new System.Diagnostics.Stopwatch ();
			
			//additional framebuffer
			offscreenBuffer = new FrameBuffer ();
			
			//set the size of the framebuffer to be exactly the same as the vita's screen 
			Texture2D tex2d = new Texture2D (960, 544, false, PixelFormat.Rgba, PixelBufferOption.Renderable);
			offscreenBuffer.SetColorTarget (tex2d, 0);
			
			//offscreenSprite to use as a back framebuffer
			offscreenSprite = new SpriteUV (new TextureInfo (tex2d));
			
			/*
			 * For unknown reasons, the framebuffer needs to be scaled 2x and flipped to display correctly
			 * the issue has been noted on Sony's forums but nobody knows the answer
			 * 
			 * */
			offscreenSprite.Quad.S = new Vector2 (2.0f, 2.0f);
			offscreenSprite.Quad.T = new Vector2 (-1.0f, -1.0f);
			offscreenSprite.FlipV = true;
			
			Support.LoadGameParameters();
		}
		
		public void Update ()
		{
			//start the timer to measure the frame rendering time
			timer.Start ();
				
			//get the system events(suspend, shut down etc)
			SystemEvents.CheckEvents ();
				
			//set up GL instance
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.SetBlendMode (BlendMode.Normal);
			//update the Director
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.Update ();

			//set the additional framebuffer to render to
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetFrameBuffer (offscreenBuffer);
				
			//set the viewport to the size of the offscreen framebuffer
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetViewport (0, 0, 960, 544);
			//render the scene to the framebuffer
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.CurrentScene.render ();
				
			//switch to the default framebuffer again
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetFrameBuffer (null);
				
			//set the viewport to the size of the device again
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetViewport (0, 0, GraphicsContext.ScreenSizes [0].Width, GraphicsContext.ScreenSizes [0].Height);
				
			//draw the offscreen framebuffer on the actual screen
			offscreenSprite.Draw ();
				
			//stop the timer,calculate the time per frame
			timer.Stop ();
			long ms = timer.ElapsedMilliseconds; 
				
				
			//sleep to limit the FPS to 30
			if (ms < 33) {
				Thread.Sleep ((int)(33 - ms));
				ms += (33 - ms);
			}
				
			//calculate the number of FPS and send to console
			int fps = (int)(1000 / ms);
			System.Console.WriteLine ("fps: {0}", fps);
			System.Console.WriteLine ("Memory in use in KBytes: " + System.GC.GetTotalMemory (true) / 1024);
				
			//if (Game.Instance != null && Game.Instance.ammoList != null)
			//	System.Console.WriteLine ("Number of enemies in the list: {0} ", Game.Instance.bulletList.Count);
				
			//reset the timer again
			timer.Reset ();
				
			//swap the buffers and cleanup afterwards
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SwapBuffers ();
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.PostSwap ();
		}
	}
}

