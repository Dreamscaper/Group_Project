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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level Level;
        Player Player;
        bool change = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Level = new Level();
            Player = new Player();
        }

        protected override void Initialize()
        {
 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Level.Load(Content);
            Player.Load(Content, Level);
        }

        protected override void UnloadContent()
        {
 
        }

        protected override void Update(GameTime gameTime)
        {
         
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            //Following lines used for testing - temporary
            //Click to reload the room
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && change == false)
            {
                Level.reLoad();
                Level.Load(Content);
                Player.Load(Content, Level);
                change = true;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                change = false;
            }
            Player.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            Level.Draw(spriteBatch, gameTime);
            Player.Draw(spriteBatch, gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
