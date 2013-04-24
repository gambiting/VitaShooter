using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Imaging;
// Font
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace VitaShooter
{
	public class Player : GameEntity
	{
		public SpriteTile playerBodySprite;
		
		public int animationFrame=0;
		
		public static Player Instance;
		
		public Player ()
		{
			//load the player's sprite
			var tex1 = new TextureInfo (new Texture2D ("/Application/data/tiles/machinegun_fire.png", false)
													, new Vector2i (14,1));
			
			tex1.Texture.SetFilter(TextureFilterMode.Disabled);
			playerBodySprite = new SpriteTile ();
			playerBodySprite.TextureInfo = tex1;
			playerBodySprite.TileIndex2D = new Vector2i (0, 0);
			
			
			//set up scale,position ect
			
			playerBodySprite.CenterSprite(new Vector2(0.3f,0.1f));
			
			playerBodySprite.Scale = new Vector2 (0.75f,1.5f);
			
			
			this.AddChild (playerBodySprite);
			
			Position = new Vector2 (0.0f, 0.0f);
			
			//get the local bounds of the sprite
			bounds = new Bounds2();
			playerBodySprite.GetlContentLocalBounds(ref bounds);
			bounds = new Bounds2(bounds.Min*0.5f,bounds.Max*0.5f);
		}
		
		public override void Tick (float dt)
		{
			base.Tick (dt);
			
			GamePadData data = GamePad.GetData (0);
			
			float analogX=0.0f;
			float analogY=0.0f;
			
			
			//d-pad movement,emulating analog movement of the sticks
			if(Input2.GamePad0.Right.Down)
			{
				analogX = 1.0f;
			}else if(Input2.GamePad0.Left.Down)
			{
				analogX = -1.0f;
			}
			if(Input2.GamePad0.Up.Down)
			{
				analogY = -1.0f;
			}else if(Input2.GamePad0.Down.Down)
			{
				analogY = 1.0f;
			}
			
			
			//if the left stick is moving,then use values read from the stick
			if (data.AnalogLeftX > 0.2f || data.AnalogLeftX < -0.2f || data.AnalogLeftY > 0.2f || data.AnalogLeftY < -0.2f) {

				analogX = data.AnalogLeftX;
				analogY = data.AnalogLeftY;
			}
			
			
			//calculate the position
			
			/*if(analogX>0)
			{
				Position = new Vector2 (Position.X + 0.1f, Position.Y);
			}else if(analogX<0)
			{
				Position = new Vector2 (Position.X - 0.1f, Position.Y);
			}
			
			if(analogY>0)
			{
				Position = new Vector2 (Position.X + Position.Y- 0.1f);
			}else if(analogY<0)
			{
				Position = new Vector2 (Position.X, Position.Y + 0.1f);
			}*/
			Vector2 proposedChange = new Vector2(analogX/15f,0.0f);
			if(!Collisions.checkWallsCollisions(this,Map.Instance,proposedChange))
			{
				Position+=proposedChange;
			}
			
			proposedChange = new Vector2(0.0f,-analogY/15f);
			if(!Collisions.checkWallsCollisions(this,Map.Instance,proposedChange))
			{
				Position+=proposedChange;
			}
			
				
			
			//rotate according to the right analog stick, or if it's not moving, then according the the left stick
			// so basically if you are not pointing the player in any direction with the right stick he is going to point in the walking direction
			//or if both sticks are not moving,then use the analogX and analogY values(d-pad movement)
			if (data.AnalogRightX > 0.2f || data.AnalogRightX < -0.2f || data.AnalogRightY > 0.2f || data.AnalogRightY < -0.2f) {
				var angleInRadians = FMath.Atan2 (-data.AnalogRightX, -data.AnalogRightY);
				Rotation = new Vector2 (FMath.Cos (angleInRadians), FMath.Sin (angleInRadians));
			} else if (data.AnalogLeftX > 0.2f || data.AnalogLeftX < -0.2f || data.AnalogLeftY > 0.2f || data.AnalogLeftY < -0.2f) {
				var angleInRadians = FMath.Atan2 (-data.AnalogLeftX, -data.AnalogLeftY);
				Rotation = new Vector2 (FMath.Cos (angleInRadians), FMath.Sin (angleInRadians));
			} else if(analogX != 0.0f || analogY != 0.0f)
			{
				var angleInRadians = FMath.Atan2 (-analogX, -analogY);
				Rotation = new Vector2 (FMath.Cos (angleInRadians), FMath.Sin (angleInRadians));
			}
				
		}
		
		
	}
}

