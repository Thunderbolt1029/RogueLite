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
    abstract class Entity
    {
        public List<Texture2D> Textures;
        public abstract Texture2D ActiveTexture { get; }
        
        public float Scale = 1f;

        public Vector2 Position;
        public Vector2 Centre
        {
            get
            {
                return HitBox.Center.ToVector2();
            }
            set
            {
                Position = value - HitBox.Size.ToVector2() / 2;
            }
        }
        public float Rotation = 0f;

        Matrix WorldMatrixTransform => Matrix.CreateTranslation(new Vector3(-ActiveTexture.Bounds.Size.ToVector2() / 2, 0)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(new Vector3(ActiveTexture.Bounds.Size.ToVector2() / 2, 0)) * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(new Vector3(Position, 0));

        public virtual Rectangle HitBox
        {
            get
            {
                if (Rotation == 0)
                    return new Rectangle(Position.ToPoint(), (ActiveTexture.Bounds.Size.ToVector2() * Scale).ToPoint());

                Vector2[] Corners = new Vector2[4];

                Corners[0] = new Vector2(0);
                Corners[2] = ActiveTexture.Bounds.Size.ToVector2();

                Corners[1] = new Vector2(Corners[0].X, Corners[2].Y);
                Corners[3] = new Vector2(Corners[2].X, Corners[0].Y);

                Vector2[] TransformedCorners = new Vector2[4];

                for (int i = 0; i < 4; i++)
                {
                    TransformedCorners[i] = Vector2.Transform(Corners[i], WorldMatrixTransform);
                }

                Rectangle hitBox = Rectangle.Empty;

                hitBox.X = (int)TransformedCorners.Min(x => x.X);
                hitBox.Y = (int)TransformedCorners.Min(x => x.Y);

                hitBox.Width = (int)TransformedCorners.Max(x => x.X) - hitBox.X;
                hitBox.Height = (int)TransformedCorners.Max(x => x.Y) - hitBox.Y;

                return hitBox;
            }
        }

        public virtual float Speed { get; set; }
        public virtual float LinearVelocity
        {
            get => Speed * Globals.WindowScale;
        }

        public bool ShouldCollideWithPlayer;

        bool removed;
        public bool isRemoved
        {
            get
            {
                if (removed && !(this is Player))
                    return true;
                return false;
            }

            set
            {
                removed = value;
            }
        }

        public virtual bool Intersects(Entity entity)
        {
            if (entity == null)
                return false;

            if (!HitBox.Intersects(entity.HitBox))
                return false;
            return true;
        }
        public virtual bool Intersects(Rectangle rectangle)
        {
            if (!HitBox.Intersects(rectangle))
                return false;
            return true;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ActiveTexture, Position, null, Color.White, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0);
        }

        protected void Move(Vector2 MovementVector, out Vector2 NewPosition)
        {
            NewPosition = Position + MovementVector;
            Rectangle NewHitBox = new Rectangle(NewPosition.ToPoint(), HitBox.Size);

            if (isCollidingWithEnvironment(out List<Rectangle> OverlappingBoundaries))
            {
                foreach (Rectangle OverlappingBoundary in OverlappingBoundaries)
                {
                    int xDisplacement, yDisplacement;

                    int RightOverlap = OverlappingBoundary.Left - NewHitBox.Right;
                    int LeftOverlap = OverlappingBoundary.Right - NewHitBox.Left;
                    if (Math.Abs(RightOverlap) < Math.Abs(LeftOverlap))
                        xDisplacement = RightOverlap;
                    else
                        xDisplacement = LeftOverlap;

                    int TopOverlap = OverlappingBoundary.Bottom - NewHitBox.Top;
                    int BottomOverlap = OverlappingBoundary.Top - NewHitBox.Bottom;
                    if (Math.Abs(TopOverlap) < Math.Abs(BottomOverlap))
                        yDisplacement = TopOverlap;
                    else
                        yDisplacement = BottomOverlap;

                    if (Math.Abs(xDisplacement) < Math.Abs(yDisplacement))
                        NewPosition.X += xDisplacement;
                    else
                        NewPosition.Y += yDisplacement;
                }
            }
        }

        bool isCollidingWithEnvironment(out List<Rectangle> OverlappingBoundaries)
        {
            OverlappingBoundaries = new List<Rectangle>();

            foreach (Rectangle RoomCollisionBox in Globals.RoomCollisionBoxes)
                if (Intersects(RoomCollisionBox))
                    OverlappingBoundaries.Add(RoomCollisionBox);

            foreach (Door door in Globals.ActiveDoors)
                if (Intersects(door.CollisionBox))
                    OverlappingBoundaries.Add(door.CollisionBox);

            return OverlappingBoundaries.Count != 0;
        }
        internal static bool RectangleIsCollidingWithEnvironment(Rectangle Hitbox, out List<Rectangle> OverlappingBoundaries)
        {
            OverlappingBoundaries = new List<Rectangle>();

            foreach (Rectangle RoomCollisionBox in Globals.RoomCollisionBoxes)
                if (Hitbox.Intersects(RoomCollisionBox))
                    OverlappingBoundaries.Add(RoomCollisionBox);

            foreach (Door door in Globals.ActiveDoors)
                if (Hitbox.Intersects(door.CollisionBox))
                    OverlappingBoundaries.Add(door.CollisionBox);

            return OverlappingBoundaries.Count != 0;
        }

        protected static float VectorToAngle(Vector2 vector) => (float)Math.Atan2(vector.Y, vector.X);
        protected static Vector2 AngleToVector(float angle) => Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
    }
}
