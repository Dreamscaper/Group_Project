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

namespace SpriteSpace
{
    public class Sprite
    {
        public string AssetName;
        public Texture2D Texture;
        public Vector2 Position = Vector2.Zero;
        public Color Tint = Color.White;

        public Single Rotation = 0.0f;
        public Vector2 Origin = Vector2.Zero;
        public Single Scale = 1.0f;
        public SpriteEffects Effects = SpriteEffects.None;
        public Single Layer = 1.0f;
        public Rectangle? SourceRectangle = null;

        public Boolean HasCollision = false;
        public Rectangle collision;

        public Sprite()
        {
        }
        public Sprite(string assetNameN, ContentManager content)
        {
            AssetName = assetNameN;
            LoadTexture(content);
        }

        public void LoadTexture(ContentManager content)
        {
            Texture = content.Load<Texture2D>(AssetName);
        }
        public void DrawMethod(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Tint);
        }
        public void DrawMethod(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Position, SourceRectangle, Tint, MathHelper.ToRadians(Rotation), Origin, Scale, Effects, Layer);
        }
        public bool isCollidingWith(Sprite other)
        {
            if (this.isCollidingWith(other))
            {
                return true;
            }
            else 
            { 
                return false;
            }
        }
        public bool isCollidingWith(Rectangle other)
        {
            if (this.isCollidingWith(other))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
