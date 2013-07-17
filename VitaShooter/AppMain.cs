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
	public class Layer
    : Node
	{
	}
	
	public class AppMain
	{
		public static void Main (string[] args)
		{
			
			
			SceneManager.Instance = new SceneManager();
			
			SceneManager.Instance.Initialize();
			
			
			//main game loop
			while (true) {
				SceneManager.Instance.Update();
			}
			
		}
		
		
		/*
		 * taken from sony forums:
		 * http://community.eu.playstation.com/t5/GameEngine2D/Creating-a-white-Texture2D-programmatically/td-p/18781830
		 * 
		 * creates a texture with a given weight, height and a colour
		 * */
		public static Texture2D ttCreateTexture (int w, int h, uint color)
		{
			Texture2D newTex = new Texture2D (w, h, false, PixelFormat.Rgba);
			uint[] pix = new uint[w * h];
				
			for (int i=0; i<pix.Length; i++)
				pix [i] = color;
				
			newTex.SetPixels (0, pix);
		
			return(newTex);
		}

	}
	
	public class SoundSystem
	{
		public static SoundSystem Instance = new SoundSystem ("/Application/data/sounds/");
		public string AssetsPrefix;
		public Dictionary<string, SoundPlayer> SoundDatabase;

		public SoundSystem (string assets_prefix)
		{
			AssetsPrefix = assets_prefix;
			SoundDatabase = new Dictionary<string, SoundPlayer> ();
		}

		public void CheckCache (string name)
		{
			if (SoundDatabase.ContainsKey (name))
				return;

			var sound = new Sound (AssetsPrefix + name);
			var player = sound.CreatePlayer ();
			SoundDatabase [name] = player;
		}

		public void Play (string name)
		{
			CheckCache (name);

			// replace any playing instance
			//SoundDatabase[name].Stop();
			SoundDatabase [name].Play ();
			SoundDatabase [name].Volume = 0.5f;
		}
		
		public void Stop (string name)
		{
			CheckCache (name);
			SoundDatabase [name].Stop ();
		}
		
		public void PlayNoClobber (string name)
		{
			CheckCache (name);
			if (SoundDatabase [name].Status == SoundStatus.Playing)
				return;
			SoundDatabase [name].Play ();
		}
	}
	
	public class MusicSystem
	{
		public static MusicSystem Instance = new MusicSystem ("/Application/data/sounds/");
		public string AssetsPrefix;
		public Dictionary<string, BgmPlayer> MusicDatabase;

		public MusicSystem (string assets_prefix)
		{
			AssetsPrefix = assets_prefix;
			MusicDatabase = new Dictionary<string, BgmPlayer> ();
		}

		public void StopAll ()
		{
			foreach (KeyValuePair<string, BgmPlayer> kv in MusicDatabase) {
				kv.Value.Stop ();
				kv.Value.Dispose ();
			}

			MusicDatabase.Clear ();
		}

		public void Play (string name)
		{
			StopAll ();

			var music = new Bgm (AssetsPrefix + name);
			var player = music.CreatePlayer ();
			MusicDatabase [name] = player;

			MusicDatabase [name].Play ();
			MusicDatabase [name].Loop = true;
			MusicDatabase [name].Volume = 0.5f;
		}
		
		public void Stop (string name)
		{
			StopAll ();
		}
		
		public void PlayNoClobber (string name)
		{
			if (MusicDatabase.ContainsKey (name)) {
				if (MusicDatabase [name].Status == BgmStatus.Playing)
					return;
			}

			Play (name);
		}
	}
}
