using System.Collections.Generic;
using System.Xml;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Imaging;	// Font
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace VitaShooter
{
	public class Layer
    : Node
    {
    }
	
	public class AppMain
	{
		
		
		
		static FontMap UIFontMap;
		static FontMap LargeFontMap;
		
		
		public static System.Random random = new System.Random();
		
		public static void Main(string[] args)
		{
			Director.Initialize();

			Director.Instance.GL.Context.SetClearColor( Colors.Black );
			Director.Instance.DebugFlags |= DebugFlags.DrawGrid;
			
			

			//UICamera = new Camera2D( Director.Instance.GL, Director.Instance.DrawHelpers );
			//UICamera.SetViewFromWidthAndCenter( 16.0f, Math._00 );

			UIFontMap = new FontMap( new Font( FontAlias.System, 20, FontStyle.Bold ) );
			LargeFontMap = new FontMap( new Font( FontAlias.System, 48, FontStyle.Bold ) );
			
			//make a new Game object
			Game.Instance = new Game();
			var game = Game.Instance;
			
			
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.RunWithScene(game.Scene,true);
			
			System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
			
			FrameBuffer offscreenBuffer = new FrameBuffer();
			Texture2D tex2d = new Texture2D(960, 544,false,PixelFormat.Rgba,PixelBufferOption.Renderable);
			//tex2d.SetFilter(TextureFilterMode.Disabled);
			offscreenBuffer.SetColorTarget(tex2d,0);
			
			//TextureInfo offscreenTextureInfo = new TextureInfo(tex2d);
			SpriteUV offscreenSprite = new SpriteUV(new TextureInfo(tex2d));
			
			offscreenSprite.Quad.S = new Vector2(2.0f,2.0f);
			offscreenSprite.Quad.T = new Vector2(-1.0f,-1.0f);
			offscreenSprite.FlipV = true;
			
			
            while (true)
            {
            	timer.Start();
                SystemEvents.CheckEvents();


                Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.SetBlendMode(BlendMode.Normal);
                Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.Update();
				game.FrameUpdate();
				
				Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetFrameBuffer(offscreenBuffer);
				
				Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetViewport(0,0,960,544);
				Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.CurrentScene.render();
				//Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.Render();
				
				Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetFrameBuffer(null);
				
				
				Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SetViewport(0,0,GraphicsContext.ScreenSizes[0].Width,GraphicsContext.ScreenSizes[0].Height);
				
				offscreenSprite.Draw();
				
				
               
				
               
                
            	timer.Stop();
                long ms = timer.ElapsedMilliseconds;
                //Console.WriteLine("ms: {0}", (int)ms);
            	timer.Reset();

                Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SwapBuffers();
                Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.PostSwap();
            }
			
		}
		

	}
}
