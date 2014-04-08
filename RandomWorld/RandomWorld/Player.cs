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
    class Player : Actor
    {
        public Sprite _Icon;
        public float Rotation;

        public Player()
        {
            _Icon = new Sprite();
            _Icon.AssetName = "temp";
            _Icon.Position.X = Position.X;
            _Icon.Position.Y = Position.Y;
            _Icon.Origin.X = _Icon.Origin.Y = 20;
            _Icon.Scale = .6f;
            _Icon.SourceRectangle = new Rectangle(0,0,40,40);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _Icon.DrawMethod(spriteBatch, gameTime);
        }
        public override void Load(ContentManager content)
        {
            _Icon.LoadTexture(content);
            PlayerMovement.Load();
        }
        public override void Update(GameTime gameTime)
        {
            PlayerMovement.Update(gameTime,this,"");
            _Icon.Position.X = Position.X;
            _Icon.Position.Y = Position.Y;
            _Icon.Rotation = Rotation;
        }

    }
}
