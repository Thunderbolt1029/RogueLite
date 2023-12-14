using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueLight___The_attack_of_the_upholstery
{
	class Door
	{
		public static Texture2D Texture;
		public DoorType doorType;
		public Point Centre
		{
			get
			{
				switch (doorType)
				{
					case DoorType.Top:
						return new Point(Globals.GameWindowRectangle.Center.X, (int)(Globals.GameWindowRectangle.Top + Texture.Height * Globals.WindowScale / 2));

					case DoorType.Right:
						return new Point((int)(Globals.GameWindowRectangle.Right - Texture.Height * Globals.WindowScale / 2), Globals.GameWindowRectangle.Center.Y);

					case DoorType.Bottom:
						return new Point(Globals.GameWindowRectangle.Center.X, (int)(Globals.GameWindowRectangle.Bottom - Texture.Height * Globals.WindowScale / 2));

					case DoorType.Left:
						return new Point((int)(Globals.GameWindowRectangle.Left + Texture.Height * Globals.WindowScale / 2), Globals.GameWindowRectangle.Center.Y);
				}

				return new Point();
			}
		}
		public Point Position
		{
			get
			{
				if (doorType == DoorType.Top || doorType == DoorType.Bottom)
					return Centre - new Point((int)Math.Ceiling(Texture.Width * Globals.WindowScale / 2), (int)Math.Ceiling(Texture.Height * Globals.WindowScale / 2));
				if (doorType == DoorType.Left || doorType == DoorType.Right)
					return Centre - new Point((int)Math.Ceiling(Texture.Height * Globals.WindowScale / 2), (int)Math.Ceiling(Texture.Width * Globals.WindowScale / 2));

				return new Point();
			}
		}
		Point LandscapeSize
		{
			get => new Point((int)Math.Ceiling(Texture.Width * Globals.WindowScale), (int)Math.Ceiling(Texture.Height * Globals.WindowScale));
		}
		Point PortraitSize
		{
			get => new Point(LandscapeSize.Y, LandscapeSize.X);
		}

		public Rectangle CollisionBox
		{
			get
			{
				if (doorType == DoorType.Top || doorType == DoorType.Bottom)
					return new Rectangle(Position, LandscapeSize);

				if (doorType == DoorType.Right || doorType == DoorType.Left)
					return new Rectangle(Position, PortraitSize);

				return new Rectangle();
			}
				
		} 

		public Door(DoorType doorType)
		{
			this.doorType = doorType;
		}

		public void Update(GameTime gameTime)
		{

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, new Rectangle(Centre, LandscapeSize), null, Color.White, MathHelper.PiOver2 * (int)doorType, Texture.Bounds.Center.ToVector2(), SpriteEffects.None, 1f);
		}
	}
}
