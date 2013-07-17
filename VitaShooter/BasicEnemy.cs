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
	 * Class used to represent the basic enemies in the game, extends the Enemy class
	 * 
	 * */
	public class BasicEnemy : Enemy
	{
		//parameters for the basic enemy
		//TODO - move somewhere else, prefferably to an external file
		
		public static int noOfEnemiesToGenerate = 20;
		
		public float speedModifier=5.0f;
		//deal damage every X frames(shouldn't be needed?)
		public float damageDelay=10;
		public int damage=1;
		
		public BasicEnemy (Vector2 pos, TextureInfo tex)
		{
			
			Position = pos;

			sprite = new SpriteTile ();
			sprite.TextureInfo = tex;
			sprite.Position = pos;
			sprite.TileIndex2D = new Vector2i (0, 0);
			sprite.CenterSprite(new Vector2(0.4f,0.5f));
			sprite.Scale = new Vector2(2.0f,2.0f);
			
			//set up the random vector
			randomMovement = Support.rand.NextVector2(-0.01f,0.01f);
			
			//update the animation frames
			sprite.Schedule((dt) => {
				if(FrameCount%2==0)
				{
					//if attacking,use all animation frames
					if(attacking)
					{
						//deal damage to the player 
						if(FrameCount%damageDelay==0) Player.Instance.Health-=damage;

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
						step = (Player.Instance.Position - sprite.Position).Normalize()/15.0f;
						
						//check if should be attacking
						if(Collisions.checkCollisionBetweenEntities(this,Player.Instance))
						{
							//check if is not attacking already
							if(attacking==false)
							{
								attacking=true;
								//manualy skip the frames
								animationFrame=4;
							}
							
						}else
						{
							//make sure that attacking is set to false
							attacking=false;
						}
					}else{
						//player is not within range, move randomly
						isMovingRandomly=true;
						step = randomMovement;
					}
				}
				
				//advance the position by the given Vector2 step
				sprite.Position+=step;
				
				//only move when not attacking
				if(!attacking)
				{
					//collision detection on the X axis
					
					//create a vector 2 containing a proposed change to the position
					Vector2 proposedChange = new Vector2(step.X,0.0f)*speedModifier;
					//temporary game entity to contain the entity the enemy might have collided with
					GameEntity tempEntity;
					
					//check wall collisions and then collisions with other enemies
					if(!Collisions.checkWallsCollisions(this, Map.Instance, ref proposedChange) && !Collisions.efficientCollisionsCheck(this,proposedChange,out tempEntity))
					{
						//no collision, so we can change the position
						Position+=proposedChange/speedModifier;
					}else if(isMovingRandomly)
					{
						//collided with something,but if the enemy should be moving randomly, then let it move
						randomMovement.X = -randomMovement.X;
					}
					
					
					//collision detection on the Y axis
					
					proposedChange = new Vector2(0.0f,step.Y)*speedModifier;
					if(!Collisions.checkWallsCollisions(this, Map.Instance, ref proposedChange) && !Collisions.efficientCollisionsCheck(this,proposedChange,out tempEntity))
					{
						Position+=proposedChange/speedModifier;
					}else if(isMovingRandomly)	
					{
						randomMovement.Y = -randomMovement.Y;
					}
					
					
					//position changed, so we need to remove the entity from the QuadTree and add again
					//this is because the entity might be in the wrong place in the QuadTree and removing and re-adding is the only way to fix that
					Game.Instance.quadTree.removeEntity(this);
					Game.Instance.quadTree.insert(this);
				}
				
				
				
				//rotate the sprite to face the direction of walking
				var angleInRadians = -FMath.Atan2 (step.X, step.Y);
				sprite.Rotation = new Vector2 (FMath.Cos (angleInRadians), FMath.Sin (angleInRadians));
				
				//correct for the fact that the sprite is rotated in the texture file
				sprite.Rotation = sprite.Rotation.Rotate(90.0f);
				
				
				
			},-1);

			//create a shadow texture - temporarily disabled
			//SpriteUV shadow = new SpriteUV(new TextureInfo(Bullet.fireTexture));
			//shadow.CenterSprite(new Vector2(0.5f,0.5f));
			//shadow.Color = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Black,0.5f);
			
			
			//calculate the bounds for the entire sprite
			bounds = new Bounds2();
			sprite.GetlContentLocalBounds(ref bounds);
			
			bounds = new Bounds2(bounds.Min*0.5f,bounds.Max*0.5f);
			
			
		}
		
		/*
		 * Checks if the view from the enemy to the Player is clear or not
		 * 
		 * */
		public bool isViewClear()
		{
			
			//create a vector2 with a position of an "imaginary bullet" - a bullet on its path from the entity
			//to the position of the player
			Vector2 imaginaryBulletPosition  = this.Position;
			
			//divide the distance into 10 steps - not super precise,but enough for quick detection
			Vector2 iStep = (Player.Instance.Position - sprite.Position)/10.0f;
			
			//do the collision check for each step
			for(int i=0;i<10;i++)
			{
				if(Collisions.checkWallsCollisionsSimple(imaginaryBulletPosition, Map.Instance))
				{
					return false;
				}
				  
				imaginaryBulletPosition+= iStep;
			}
			
			//return true if there was no collision along the way
			return true;
			
		}
		
	}
}

