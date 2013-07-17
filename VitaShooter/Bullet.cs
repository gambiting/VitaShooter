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
	
	/*
	 * Class representing a bullet in game
	 * 
	 * */
	public class Bullet :  GameEntity
	{
		public Vector2 step;
		
		
		//bullet damage
		//TODO - move somewhere else
		public static int bulletDamage = 20;
		public static int bulletDelay;
		
		//texture for the splash effect
		//bear in mind that it's used in several other places too
		public static Texture2D fireTexture;
		
		//texture for the actual bullet
		public static Texture2D bulletTexture;
		
		public Bullet ()
		{
			
			//starting position - calculated from the position of the player
			Vector2 startingPosition = Player.Instance.Position + (new Vector2(0.2f,0.0f).Rotate(Player.Instance.playerBodySprite.Rotation));
			
			//offset,so the bullet would appear at the end of the riffle - could use improvement
			Vector2 offsetPosition = new Vector2(0.0f,0.5f);
			
			//rotate the offset by the same angle the player is rotated by
			offsetPosition = offsetPosition.Rotate(Player.Instance.playerBodySprite.Rotation);
			
			//calculate final position from the starting position and the offset
			Position = startingPosition+offsetPosition;
			
			//step the bullet takes each tick
			step = (Position - startingPosition)*1.5f;
			
			//if the bullet texture has not been created yet, create it
			if(Bullet.bulletTexture == null)
				Bullet.bulletTexture = AppMain.ttCreateTexture(1,1, 0xff00ffff);
			
			//the bullet itself is made from two fireTexture textures
			//one white in the middle
			//and second one black as a shadow
		
			//TODO - make sure that bulletTexture is not used
			SpriteUV sprite = new SpriteUV();
			sprite.TextureInfo = new TextureInfo(fireTexture);
			sprite.Scale = new Vector2(0.1f,0.1f);
			sprite.Color = Colors.White;
			sprite.CenterSprite(new Vector2(0.5f,0.5f));
			
			
			SpriteUV shadow = new SpriteUV(new TextureInfo(Bullet.fireTexture));
			shadow.CenterSprite(new Vector2(0.5f,0.5f));
			shadow.Color = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Black,0.5f);
			shadow.Scale = new Vector2(0.4f,0.4f);
			this.AddChild(shadow);
			
			
			this.AddChild(sprite);
			
			
			
		}
		
		public override void Tick (float dt)
		{
			Enemy tempEnemy;
			
			
			//check the collision with the walls
			if(Collisions.checkWallsCollisions(this, Map.Instance,ref step))
			{
				
				//play a sound
				SoundSystem.Instance.Play("wallhit2.wav");
				float scale = 0.5f;
				
				
				//try a texture in place of the bullethole
				SpriteUV bulletHole = new SpriteUV();
				bulletHole.TextureInfo = new TextureInfo(fireTexture);
				bulletHole.Scale = new Vector2(0.15f,0.15f);
				bulletHole.Color = Colors.Black;
				bulletHole.CenterSprite(new Vector2(0.5f,0.5f));
				bulletHole.Position = Position+step;
				
				//remove the bulletHole after half a second
				bulletHole.ScheduleInterval((at) => bulletHole.Parent.RemoveChild(bulletHole,true),0.5f,-1);
				
				//add the bullethole to the background
				Game.Instance.Background.AddChild(bulletHole);
				
				//remove this bullet
				this.Parent.RemoveChild(this, true);
				
				Game.Instance.bulletList.Remove(this);
				
				//die
				this.Die();
			}else if(Collisions.checkEnemiesCollisions(this, Game.Instance.enemyList, step,  out tempEnemy))
			{
				//reduce the health of the enemy the bullet hit
				tempEnemy.health-= bulletDamage;
				
				float scale = 0.5f;
				//particles!
				
				//create and start the particle emmiter
				/*Particles fire_node= new Particles( 10 );
				ParticleSystem fire = fire_node.ParticleSystem;
				
				fire.TextureInfo = new TextureInfo( fireTexture );
				fire.Emit.Velocity = step;
				fire.Emit.VelocityVar = new Vector2( 2.0f, 2.0f );
				fire.Emit.ForwardMomentum = 0.0f;
				fire.Emit.AngularMomentun = 0.0f;
				fire.Emit.LifeSpan = 0.5f;
				fire.Emit.WaitTime = 0.0f;
				float s = 0.0f;
				fire_node.Position = Position+step;
				fire.Emit.PositionVar = new Vector2(s,s);
				fire.Emit.ColorStart = Colors.Red;
				fire.Emit.ColorStartVar = new Vector4(0.0f,0.0f,0.0f,0.0f);
				fire.Emit.ColorEnd = Colors.Red;
				fire.Emit.ColorEndVar = new Vector4(0.2f,0.0f,0.0f,0.0f);
				fire.Emit.ScaleStart = 0.3f * scale;
				fire.Emit.ScaleStartRelVar = 0.2f;
				fire.Emit.ScaleEnd = 0.6f * scale;
				fire.Emit.ScaleEndRelVar = 0.2f;
				fire.Simulation.Fade = 0.5f;
				fire.Simulation.Gravity = 0.0f;
				
				fire.Emit.Transform = fire_node.GetWorldTransform();        
				fire.Emit.TransformForVelocityEstimate = fire_node.GetWorldTransform();
				fire.RenderTransform = Director.Instance.CurrentScene.GetWorldTransform(); // most probably identity
		
				Director.Instance.CurrentScene.AddChild(fire_node);
				
				Director.Instance.CurrentScene.RegisterDisposeOnExit(fire);
				
				//remove the particle emmiter after 0.5 seconds
				fire_node.ScheduleInterval((at) => Director.Instance.CurrentScene.RemoveChild(fire_node,true),0.5f,1);
				*/
				this.Parent.RemoveChild(this,true);
				
				Game.Instance.bulletList.Remove(this);
				
				this.Die();
			}else
			{
				//no collisions with anything,move the bullet
				Position+=step;
			}
				
		}
	}
	
	
	
	
}

