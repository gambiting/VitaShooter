using System.Collections.Generic;
using System.Xml;
using System.Threading;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Imaging;	// Font
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

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
		public static Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator rand = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator();
		
		public static void Main(string[] args)
		{
			Director.Initialize();

			Director.Instance.GL.Context.SetClearColor( Colors.Black );
			//Director.Instance.DebugFlags |= DebugFlags.DrawGrid;
			
			

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
				
				if(ms<33)
				{
					Thread.Sleep((int)(33-ms));
					ms+=(33-ms);
				}
				
				
				
				int fps = (int)(1000/ms);
				System.Console.WriteLine("fps: {0}" , fps);
				System.Console.WriteLine("Memory in use in KBytes: " + System.GC.GetTotalMemory(false)/1024);
				
            	timer.Reset();

                Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SwapBuffers();
                Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.PostSwap();
            }
			
		}
		
		
		/*
		 * taken from sony forums:
		 * http://community.eu.playstation.com/t5/GameEngine2D/Creating-a-white-Texture2D-programmatically/td-p/18781830
		 * */
		public static Texture2D ttCreateTexture( int w, int h, uint color )
		{
			Texture2D newTex = new Texture2D( w, h, false, PixelFormat.Rgba );
			uint[] pix = new uint[w*h];
				
			for( int i=0; i<pix.Length; i++ )
				pix[i] = color;
				
			newTex.SetPixels( 0, pix );
		
			return( newTex );
		}

	}
	
	
	public class SoundSystem
		{
			public static SoundSystem Instance = new SoundSystem("/Application/data/sounds/");

			public string AssetsPrefix;
			public Dictionary<string, SoundPlayer> SoundDatabase;

			public SoundSystem(string assets_prefix)
			{
				AssetsPrefix = assets_prefix;
				SoundDatabase = new Dictionary<string, SoundPlayer>();
			}

			public void CheckCache(string name)
			{
				if (SoundDatabase.ContainsKey(name))
					return;

				var sound = new Sound(AssetsPrefix + name);
				var player = sound.CreatePlayer();
				SoundDatabase[name] = player;
			}

			public void Play(string name)
			{
				CheckCache(name);

				// replace any playing instance
				//SoundDatabase[name].Stop();
				SoundDatabase[name].Play();
				SoundDatabase[name].Volume = 0.5f;
			}
		
			public void Stop(string name)
			{
				CheckCache(name);
				SoundDatabase[name].Stop();
			}
		
			public void PlayNoClobber(string name)
			{
				CheckCache(name);
				if (SoundDatabase[name].Status == SoundStatus.Playing)
					return;
				SoundDatabase[name].Play();
			}
		}
	
	public class MusicSystem
		{
			public static MusicSystem Instance = new MusicSystem("/Application/data/sounds/");

			public string AssetsPrefix;
			public Dictionary<string, BgmPlayer> MusicDatabase;

			public MusicSystem(string assets_prefix)
			{
				AssetsPrefix = assets_prefix;
				MusicDatabase = new Dictionary<string, BgmPlayer>();
			}

			public void StopAll()
			{
				foreach (KeyValuePair<string, BgmPlayer> kv in MusicDatabase)
				{
					kv.Value.Stop();
					kv.Value.Dispose();
				}

				MusicDatabase.Clear();
			}

			public void Play(string name)
			{
				StopAll();

				var music = new Bgm(AssetsPrefix + name);
				var player = music.CreatePlayer();
				MusicDatabase[name] = player;

				MusicDatabase[name].Play();
				MusicDatabase[name].Loop = true;
				MusicDatabase[name].Volume = 0.5f;
			}
		
			public void Stop(string name)
			{
				StopAll();
			}
		
			public void PlayNoClobber(string name)
			{
				if (MusicDatabase.ContainsKey(name))
				{
					if (MusicDatabase[name].Status == BgmStatus.Playing)
						return;
				}

				Play(name);
			}
		}
}
