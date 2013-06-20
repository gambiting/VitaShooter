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
	public class Bullet :  GameEntity
	{
		public Vector2 step;
		
		public static int bulletDelay;
		
		public static int bulletDamage = 20;
		
		public static Texture2D fireTexture;
		
		public static Texture2D bulletTexture;
		
		public Bullet ()
		{
			
			
			Vector2 startingPosition = Player.Instance.Position + (new Vector2(0.2f,0.0f).Rotate(Player.Instance.playerBodySprite.Rotation));
			
			Vector2 offsetPosition = new Vector2(0.0f,0.5f);
			
			offsetPosition = offsetPosition.Rotate(Player.Instance.playerBodySprite.Rotation);
			Position = startingPosition+offsetPosition;
			
			//step the bullet takes each tick
			step = (Position - startingPosition)*1.5f;
			
			if(Bullet.bulletTexture == null)
				Bullet.bulletTexture = AppMain.ttCreateTexture(1,1, 0xff00ffff);
			
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
			//Vector2 temp = new Vector2(step.X,step.Y);
			if(Collisions.checkWallsCollisions(this, Map.Instance,ref step))
			{
				SoundSystem.Instance.Play("wallhit2.wav");
				
				float scale = 0.5f;
				
				
				/*Particles fire_node= new Particles( 1 );
				ParticleSystem fire = fire_node.ParticleSystem;
				
				fire.TextureInfo = new TextureInfo( fireTexture );
				fire.Emit.Velocity = new Vector2( 0.0f, 0.0f );
				fire.Emit.VelocityVar = new Vector2( 0.0f, 0.0f );
				fire.Emit.ForwardMomentum = 0.0f;
				fire.Emit.AngularMomentun = 0.0f;
				fire.Emit.LifeSpan = 0.5f;
				fire.Emit.WaitTime = 0.0f;
				float s = 0.0f;
				fire_node.Position = Position+step;
				fire.Emit.PositionVar = new Vector2(s,s);
				fire.Emit.ColorStart = Colors.Black;
				fire.Emit.ColorStartVar = new Vector4(0.0f,0.0f,0.0f,0.0f);
				fire.Emit.ColorEnd = Colors.White;
				fire.Emit.ColorEndVar = new Vector4(0.2f,0.0f,0.0f,0.0f);
				fire.Emit.ScaleStart = 0.3f * scale;
				fire.Emit.ScaleStartRelVar = 0.2f;
				fire.Emit.ScaleEnd = 0.6f * scale;
				fire.Emit.ScaleEndRelVar = 0.2f;
				fire.Simulation.Fade = 0.5f;
				
				fire.Emit.Transform = fire_node.GetWorldTransform();        
				fire.Emit.TransformForVelocityEstimate = fire_node.GetWorldTransform();
				fire.RenderTransform = Director.Instance.CurrentScene.GetWorldTransform(); // most probably identity
		
				Director.Instance.CurrentScene.AddChild(fire_node);
				
				Director.Instance.CurrentScene.RegisterDisposeOnExit(fire);
				
				//remove the particle emmiter after 0.5 seconds
				fire_node.ScheduleInterval((at) => Director.Instance.CurrentScene.RemoveChild(fire_node,true),0.5f,1);*/
				
				//try a texture in place of the bullethole
				SpriteUV bulletHole = new SpriteUV();
				bulletHole.TextureInfo = new TextureInfo(fireTexture);
				bulletHole.Scale = new Vector2(0.15f,0.15f);
				bulletHole.Color = Colors.Black;
				bulletHole.CenterSprite(new Vector2(0.5f,0.5f));
				bulletHole.Position = Position+step;
				
				bulletHole.ScheduleInterval((at) => bulletHole.Parent.RemoveChild(bulletHole,true),0.5f,-1);
				
				Game.Instance.Background.AddChild(bulletHole);
				
				this.Parent.RemoveChild(this, true);
				
				this.Die();
			}else if(Collisions.checkEnemiesCollisions(this, Game.Instance.enemyList, step,  out tempEnemy))
			{
				tempEnemy.health-= bulletDamage;
				
				float scale = 0.5f;
				//particles!
				Particles fire_node= new Particles( 10 );
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
				
				this.Parent.RemoveChild(this,true);
				
				this.Die();
			}else
			{
				//move the bullet
				Position+=step;
			}
				
		}
	}
	
	
	
	
}

