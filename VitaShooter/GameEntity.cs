using System;

using System.Collections.Generic;

using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace VitaShooter
{
	public class GameEntity :  Sce.PlayStation.HighLevel.GameEngine2D.Node
	{
		
		public float Health { get; set; }
		public int FrameCount { get; set; }
		
		public GameEntity ()
		{
			
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, Tick, 0.0f, false);
		}
		
		public virtual void Tick(float dt)
		{
			FrameCount += 1;
		} 
	}
}

