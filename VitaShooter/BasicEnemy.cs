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
	public class BasicEnemy : Enemy
	{
		
		
		public static int noOfEnemiesToGenerate = 20;
		
		
		public float speedModifier=5.0f;
		
		public BasicEnemy (Vector2 pos, TextureInfo tex)
		{
			
			Position = pos;

			sprite = new SpriteTile ();
			sprite.TextureInfo = tex;
			sprite.Position = pos;
			sprite.TileIndex2D = new Vector2i (0, 0);
			sprite.CenterSprite(new Vector2(0.4f,0.5f));
			//sprite.Pivot = new Vector2(0.5f,0.5f);
			//sprite.Rotation = sprite.Rotation.Rotate(AppMain.random.Next(-180,180));
			//sprite.Color = Colors.Red;
			sprite.Scale = new Vector2(2.0f,2.0f);
			//set up the random vector
			
			randomMovement = AppMain.rand.NextVector2(-0.01f,0.01f);
			
			//update the animation frames
			sprite.Schedule((dt) => {
				if(FrameCount%2==0)
				{
					//if attacking,use all animation frames
					if(attacking)
					{
						animationFrame = (animationFrame+1) % 25;
					}else
					{
						//if not,then use first three animation frames
						animationFrame = (animationFrame+1) % 4;
						
					}
					//assign the correct tileindex
					sprite.TileIndex1D = animationFrame;
					
					//if close to the player,then follow,otherwise move randomly
					if(Player.Instance.Position.Distance(sprite.Position) < 6.0f && isViewClear())
					{
						isMovingRandomly=false;
						step = (Player.Instance.Position - sprite.Position).Normalize()/30.0f;
						
						//check if should be attacking
						if(Collisions.checkCollisionBetweenEntities(this,Player.Instance))
						{
							if(attacking==false)
							{
								attacking=true;
								//manualy skip the frames
								animationFrame=4;
							}
							
						}else
						{
							attacking=false;
						}
					}else{
						isMovingRandomly=true;
						step = randomMovement;
					}
					
					
					
				}
				sprite.Position+=step;
				
				

				//only move when not attacking
				if(!attacking)
				{
					Vector2 proposedChange = new Vector2(step.X,0.0f)*speedModifier;
					GameEntity tempEntity;
					if(!Collisions.checkWallsCollisions(this, Map.Instance, ref proposedChange) && !Collisions.efficientCollisionsCheck(this,proposedChange,out tempEntity))
					{
						Position+=proposedChange/speedModifier;
					}else if(isMovingRandomly)
					{
						randomMovement.X = -randomMovement.X;
					}
					
					proposedChange = new Vector2(0.0f,step.Y)*speedModifier;
					if(!Collisions.checkWallsCollisions(this, Map.Instance, ref proposedChange) && !Collisions.efficientCollisionsCheck(this,proposedChange,out tempEntity))
					{
						Position+=proposedChange/speedModifier;
					}else if(isMovingRandomly)	
					{
						randomMovement.Y = -randomMovement.Y;
					}
					
					
					Game.Instance.quadTree.removeEntity(this);
					Game.Instance.quadTree.insert(this);
				}
				
				//test collision detection, done differently
				
				//rotate the sprite to face the direction of walking
				var angleInRadians = -FMath.Atan2 (step.X, step.Y);
				sprite.Rotation = new Vector2 (FMath.Cos (angleInRadians), FMath.Sin (angleInRadians));
				
				//correct for the fact that the sprite is rotated in the texture file
				sprite.Rotation = sprite.Rotation.Rotate(90.0f);
				
				
				
			},-1);

			
			SpriteUV shadow = new SpriteUV(new TextureInfo(Bullet.fireTexture));
			
			shadow.CenterSprite(new Vector2(0.5f,0.5f));
			shadow.Color = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Black,0.5f);
			
			
			
			//this.AddChild(shadow);
			//this.AddChild(sprite);
			
			bounds = new Bounds2();
			sprite.GetlContentLocalBounds(ref bounds);
			
			bounds = new Bounds2(bounds.Min*0.5f,bounds.Max*0.5f);
			
			
		}
		
		public bool isViewClear()
		{
			Vector2 imaginaryBulletPosition  = this.Position;
			
			
			Vector2 iStep = (Player.Instance.Position - sprite.Position)/10.0f;
			
			for(int i=0;i<10;i++)
			{
				if(Collisions.checkWallsCollisionsSimple(imaginaryBulletPosition, Map.Instance))
				{
					return false;
				}
				  
				imaginaryBulletPosition+= iStep;
			}
			
			
			return true;
			
		}
		
	}
}

