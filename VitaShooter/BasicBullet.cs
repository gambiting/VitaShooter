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
	public class BasicBullet : GameEntity
	{
		
		Vector2 step; //step taken each frame
		public static int bulletDamage = 20;
		
		public BasicBullet ()
		{
			SpriteUV sprite = new SpriteUV();
			sprite.TextureInfo = new TextureInfo(Bullet.fireTexture);
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
			base.Tick(dt);
			
			Enemy tempEnemy;
			
			
			//check the collision with the walls
			if(Collisions.checkWallsCollisions(this, MapManager.Instance.currentMap,ref step))
			{
				
				//play a sound
				SoundSystem.Instance.Play("wallhit2.wav");
				float scale = 0.5f;
				
				
				//try a texture in place of the bullethole
				SpriteUV bulletHole = new SpriteUV();
				bulletHole.TextureInfo = new TextureInfo(Bullet.fireTexture);
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
				
				//Game.Instance.bulletList.Remove(this);
				
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
				
				//Game.Instance.bulletList.Remove(this);
				
				this.Die();
			}else
			{
				//no collisions with anything,move the bullet
				Position+=step;
			}
			
			
		}
	}
}

