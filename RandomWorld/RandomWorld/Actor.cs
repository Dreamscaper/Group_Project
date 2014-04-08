using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//Collision needs work

namespace RandomWorld
{
    abstract class Actor
    {
        public bool isActive = false;
        public Vector2 Position = Vector2.Zero;
        public Rectangle? Collision = null;
        public Rectangle c_copy;
        private Vector2 offset = Vector2.Zero; //For textures with origin in different places
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void Load(ContentManager content);
        public abstract void Update(GameTime gameTime);

        public Actor()
        {
            Collision = new Rectangle?();
            c_copy = new Rectangle();
        }
        public virtual bool CheckCollisionWith(Actor other)
        {

            other.SetCollision(other.c_copy);
            return CheckCollisionWith(other.c_copy, offset );
        }
        public virtual bool CheckCollisionWith(Rectangle other, Vector2 offset)
        {
             if (c_copy.X < other.X + offset.X && (c_copy.X + c_copy.Width) > other.X - offset.X
                 && c_copy.Y < other.Y + offset.Y && (c_copy.Y + c_copy.Height) > other.Y - offset.Y)
                {
                    return true;
                }
      
            return false;
        }
        public virtual void SetCollision(Rectangle collision)
        {
 
        }


        public Vector2 getOffset(int amountX, int amountY)
        {
            offset.X = amountX;
            offset.Y = amountY;

            return offset;
        }
    }
}
