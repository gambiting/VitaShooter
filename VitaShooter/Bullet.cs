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
		public Vector2 step {get;set;}
		
		public static int bulletDelay;
		
		
		
		public static Texture2D fireTexture;
		
		public Bullet ()
		{
			
			
			Vector2 startingPosition = Player.Instance.Position;
			
			Vector2 offsetPosition = new Vector2(0.0f,1.0f);
			
			offsetPosition = offsetPosition.Rotate(Player.Instance.Rotation);
			
			Position = startingPosition+offsetPosition;
			
			//step the bullet takes each tick
			step = (Position - startingPosition)/2.0f;
			
			Texture2D bulletTexture = AppMain.ttCreateTexture(1,1, 0xff00ffff);
			
			SpriteUV sprite = new SpriteUV();
			sprite.TextureInfo = new TextureInfo(fireTexture);
			sprite.Scale = new Vector2(0.2f,0.2f);
			sprite.Color = new Vector4(1.0f,0.6f,0.0f,1.0f);
			
			this.AddChild(sprite);
			
			
			
			
		}
		
		public override void Tick (float dt)
		{
			
			
			if(Collisions.checkWallsCollisions(this, Map.Instance,step))
			{
				SoundSystem.Instance.Play("wallhit2.wav");
				
				float scale = 0.5f;
				
				
				Particles fire_node= new Particles( 1 );
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
				fire_node.ScheduleInterval((at) => Director.Instance.CurrentScene.RemoveChild(fire_node,true),0.5f,1);
				
				this.Parent.RemoveChild(this, true);
			}else
			{
				//move the bullet
				Position+=step;
			}
				
		}
	}
	
	
	
	
}

