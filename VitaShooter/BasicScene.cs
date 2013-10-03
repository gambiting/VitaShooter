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
	public class BasicScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		public static Scene Instance;
		
		public Layer Background { get; set; }
        public Layer World { get; set; }
        public Layer EffectsLayer { get; set; }
        public Layer Foreground { get; set; }
        public Layer Interface { get; set; }
		
		public UI ui;
		
		public BasicScene ()
		{
			//create layers for everyting
            Background = new Layer();
            World = new Layer();
            EffectsLayer = new Layer();
            Foreground = new Layer();
            Interface = new Layer();
			
			//add layers to the scene
			this.AddChild(Background);
            this.AddChild(World);
            this.AddChild(Foreground);
			this.AddChild(EffectsLayer);
            this.AddChild(Interface);
		}
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, Tick, 0.0f, false);
		}
		
		public override void OnExit ()
		{
			base.OnExit ();
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(this,Tick);
		}
		
		public virtual void Tick(float dt)
		{
			
		}
	}
}

