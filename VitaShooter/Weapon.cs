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
	public class Weapon
	{
		public int firingDelay;		//delay between shots
		public Timer lastShotTimer;	//timer which stores the time of the last shot
		public bool repeatable;		//sets whatever the weapon can be fired by holding the fire button
		
		public Weapon ()
		{
		}
		
		public virtual List<Bullet> fire(Vector2 startingPosition, Vector2 aimingPosition);
	}
}

