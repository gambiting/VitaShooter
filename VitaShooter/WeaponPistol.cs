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


/*
 * Basic pistol - produces a single basic bullet
 * 
 * */

namespace VitaShooter
{
	public class WeaponPistol : Weapon
	{
		public WeaponPistol ()
		{
			
		}
		
		public override List<BasicBullet> fire(Vector2 startingPosition, Vector2 aimingPosition)
		{
			List<BasicBullet> bullets = new List<BasicBullet>();
			
			BasicBullet tempBullet = new BasicBullet();
			
			tempBullet.Position = aimingPosition;
			
			
			
			return bullets;
			
		}
		
	}
}

