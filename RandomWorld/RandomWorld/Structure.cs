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
using SpriteSpace;

namespace RandomWorld
{
    abstract class Structure : Sprite
    {
        //public bool hasCollision;
        //public Rectangle Collision;
        //public Sprite Image;

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime); //Draw, Load, And Update functions
        public abstract void Load(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract bool CheckLocation(int size_x, int size_y, int start_pos_X, int start_pos_Y, int[,] array, Sprite[,] gameboard); //See if the location of the structure is okay to place
        public virtual bool CheckCollisionWith(Structure other) //Default to false for no collision detection.
        {
            return false;
        }
    }
}