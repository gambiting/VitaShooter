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
		public SpriteTile sprite;
		
		public static int noOfEnemiesToGenerate = 10;
		
		public Vector2 randomMovement;
		
		public bool isMovingRandomly=false;
		
		
		public BasicEnemy (Vector2 pos)
		{
			
			Position = pos;
			
			
			var tex1 = new TextureInfo (new Texture2D ("/Application/data/tiles/enemy_sword2.png", false)
													, new Vector2i (25,1));
			
			tex1.Texture.SetFilter(TextureFilterMode.Disabled);
			sprite = new SpriteTile ();
			sprite.TextureInfo = tex1;
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
					if(Player.Instance.Position.Distance(Position) < 4.0f)
					{
						isMovingRandomly=false;
						step = (Player.Instance.Position - Position).Normalize()/30.0f;
						
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
				
				//only move when not attacking
				if(!attacking)
				{
					Vector2 proposedChange = new Vector2(step.X,0.0f);
					if(!Collisions.checkWallsCollisions(this, Map.Instance, proposedChange))
					{
						Position+=proposedChange;
					}else if(isMovingRandomly)
					{
						randomMovement.X = -randomMovement.X;
					}
					
					proposedChange = new Vector2(0.0f,step.Y);
					if(!Collisions.checkWallsCollisions(this, Map.Instance, proposedChange))
					{
						Position+=proposedChange;
					}else if(isMovingRandomly)
						
						
					{
						randomMovement.Y = -randomMovement.Y;
					}
				}
				
				//rotate the sprite to face the direction of walking
				var angleInRadians = -FMath.Atan2 (step.X, step.Y);
				sprite.Rotation = new Vector2 (FMath.Cos (angleInRadians), FMath.Sin (angleInRadians));
				
				//correct for the fact that the sprite is rotated in the texture file
				sprite.Rotation = sprite.Rotation.Rotate(45.0f);
				
				
				
			},-1);
			
			SpriteUV shadow = new SpriteUV(new TextureInfo(Bullet.fireTexture));
			
			shadow.CenterSprite(new Vector2(0.5f,0.5f));
			shadow.Color = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Black,0.5f);
			
			
			
			this.AddChild(shadow);
			this.AddChild(sprite);
			
			bounds = new Bounds2();
			sprite.GetlContentLocalBounds(ref bounds);
			
			bounds = new Bounds2(bounds.Min*0.5f,bounds.Max*0.5f);
			
			
		}
	}
}

