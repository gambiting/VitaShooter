using System;

using System.Collections.Generic;

using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace VitaShooter
{
	/*
	 * This class is used to represent a basic game entity, Game class extends this
	 * 
	 * */
	public class GameEntity :  Sce.PlayStation.HighLevel.GameEngine2D.Node
	{
		
		public int Health = 100;
		public int FrameCount { get; set; }
		
		public Bounds2 bounds;
		
		public GameEntity ()
		{
			
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, Tick, 0.0f, false);
		}
		
		public virtual void Tick(float dt)
		{
			FrameCount += 1;
		} 
		
		public void Die()
		{
			Sce.PlayStation.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(this,this.Tick);
		}
	}
}

