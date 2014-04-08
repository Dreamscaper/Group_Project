//AG-131
//Student: Wesley Couturier

//Code not modified yet


/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameCode
{   
    class Movement
    {
        MouseState cMouse;
        MouseState pMouse;

        KeyboardState cKeyBoard;
        KeyboardState pKeyBoard;

        Vector2 FontPos;
        Vector2 lStickPos;
        Vector2 rStickPos;

        GamePadState cController1;
        GamePadState pController1;

        GamePadState cController2;
        GamePadState pController2;

        GamePadState cController3;
        GamePadState pController3;

        GamePadState cController4;
        GamePadState pController4;

        double direction;

        float pSpeed = 5f; //This is the player speed, Can be set something of your liking

        public void Load()
        { 
            FontPos = new Vector2(0, 0);
            lStickPos = new Vector2(0, 15);
            rStickPos = new Vector2(15, 30);
        }

        public void Update(GameTime gameTime, Tank player, string OP)       //Tank is be the object you wish to move, just rename "Tank", OP is "OutPut" 
        {
            /////////////////GamePad Code////////////////////////////////////////////////////////////
            cController1 = GamePad.GetState(PlayerIndex.One);
            cController2 = GamePad.GetState(PlayerIndex.Two);
            cController3 = GamePad.GetState(PlayerIndex.Three);
            cController4 = GamePad.GetState(PlayerIndex.Four);

            if (pController1 == null)
                pController1 = cController1;
            if (pController2 == null)
                pController2 = cController2;
            if (pController3 == null)
                pController3 = cController3;
            if (pController4 == null)
                pController4 = cController4;

            checkGamePad(PlayerIndex.One, player, cController1, pController1, OP);
            checkGamePad(PlayerIndex.Two, player, cController2, pController2, OP);
            checkGamePad(PlayerIndex.Three, player, cController3, pController3, OP);
            checkGamePad(PlayerIndex.Four, player, cController4, pController4, OP);

            //if (cController1 != null)           //Stops the check for Mouse and Keyboard Input if Controller is registered
            //{
            //    return;
            //}

            ////////////////Keyboard Code////////////////////////////////////////////////////////////
            cKeyBoard = Keyboard.GetState();        //Updates what is being pressed

            if (pKeyBoard == null)                  //Used to get rid of null
            {
                pKeyBoard = cKeyBoard;
            }

            keyDown(cKeyBoard, player);

            /////////////////Mouse Code//////////////////////////////////////////////////////////////           
            cMouse = Mouse.GetState();
            if (pMouse == null)
            {
                pMouse = cMouse;
            }

           // myMouse.setLocation(cMouse.X, cMouse.Y);            

            // Set every previous to current
            pMouse = cMouse;

            pController1 = cController1;
            pController2 = cController2;
            pController3 = cController3;
            pController4 = cController4;

            pKeyBoard = cKeyBoard;

        }

        public double convertDtoR(double n)
        {
            n = n * (Math.PI / 180);
            return n;
        }

        public void checkGamePad(PlayerIndex index, Tank P, GamePadState current, GamePadState previous, string output) //Tank "P" is for the player
        {
            bool active = current.IsConnected;
            float playerSpeed = pSpeed;

            if (active)
            {
                
                if (current.Buttons.A == ButtonState.Pressed)
                {
                   // output = "A";
                }

                if (current.Buttons.B == ButtonState.Pressed)
                {
                   // output = ", B";
                }

                if (current.Buttons.X == ButtonState.Pressed)
                {
                    //output = ", X";
                }

                if (current.Buttons.Y == ButtonState.Pressed)
                {
                   // output = ", Y";
                }

                if (current.Buttons.Back == ButtonState.Pressed)
                {
                    //output = ", Back";
                }

                if (current.Buttons.BigButton == ButtonState.Pressed)
                {
                    //output = ", BigButton";
                }

                if (current.Buttons.Start == ButtonState.Pressed)
                {
                    //output = ", Start";
                }

                if (current.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                   // output = ", LeftShoulder";
                }

                if (current.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    //output = ", RightShoulder";
                }
                 
                if (current.Buttons.LeftStick == ButtonState.Pressed)
                {
                    //output = ", LeftStick";
                }

                if (current.Buttons.RightStick == ButtonState.Pressed)
                {
                   // output = ", RightStick";
                }

                if (current.DPad.Down == ButtonState.Pressed)
                {
                   // output = ", Dpad Down";
                }

                if (current.DPad.Up == ButtonState.Pressed)
                {
                    //output = ", Dpad Up";
                }

                if (current.DPad.Left == ButtonState.Pressed)
                {
                    //output = ", Dpad Left";
                }

                if (current.DPad.Right == ButtonState.Pressed)
                {
                   // output = ", Dpad Right";
                }
                if (current.Triggers.Right > 0)
                {
                    //output = current.Triggers.Right.ToString();
                }

                if (current.Triggers.Left > 0)
                {
                    //output = current.Triggers.Left.ToString();
                }
                 
                if (current.ThumbSticks.Left.X >= 0 || current.ThumbSticks.Left.X < 0 ||
                           current.ThumbSticks.Left.Y > 0 || current.ThumbSticks.Left.Y < 0)
                {
                   // lStick = "Left ThumbStick: ( " + current.ThumbSticks.Left.X + ", " + current.ThumbSticks.Left.Y + " )";
                    P.Position.X += current.ThumbSticks.Left.X * pSpeed;
                    P.Position.Y -= current.ThumbSticks.Left.Y * pSpeed;
                 
                    //Console.Out.WriteLine("P.Location.Y : " + P.tankBody.Location.Y);
                }

                if (current.ThumbSticks.Left.Length() >= 0.3f)
                {
                    P.BodyFacing = (float)Math.Atan2(-current.ThumbSticks.Left.Y, current.ThumbSticks.Left.X);
                }

                if (current.ThumbSticks.Right.Length() >= 0.3f)
                {
                    //rStick = "Right ThumbStick: ( " + current.ThumbSticks.Right.X + ", " + current.ThumbSticks.Right.Y + " )";
                    P.tankBarrel.rotation = (float)Math.Atan2(-current.ThumbSticks.Right.Y, current.ThumbSticks.Right.X);
                }
            }
        }

        public void keyDown(KeyboardState cKeyBoard, Tank tankSprite)
        {
            //NOTE/////////////////////////////////
            //  degree assignments are in tune with the cartesian system starting on the 'Y'
            //  axis and revolving clockwise

            if (cKeyBoard.IsKeyDown(Keys.W))
            {
                direction = convertDtoR(270);               //Converts initial degree to Radians
                tankSprite.tankBody.rotation = (float)direction;

                tankSprite.tankBody.Location.Y -= 5;
            }

            if (cKeyBoard.IsKeyDown(Keys.A))
            {
                direction = convertDtoR(180);
                tankSprite.tankBody.rotation = (float)direction;

                tankSprite.tankBody.Location.X -= 5;
            }

            if (cKeyBoard.IsKeyDown(Keys.S))
            {
                direction = convertDtoR(90);
                tankSprite.tankBody.rotation = (float)direction;

                tankSprite.tankBody.Location.Y += 5;
            }

            if (cKeyBoard.IsKeyDown(Keys.D))
            {
                direction = convertDtoR(0);
                tankSprite.tankBody.rotation = (float)direction;

                tankSprite.tankBody.Location.X += 5;
            }

            if (cKeyBoard.IsKeyDown(Keys.W) && cKeyBoard.IsKeyDown(Keys.D))
            {
                direction = convertDtoR(315);
                tankSprite.tankBody.rotation = (float)direction;

                tankSprite.tankBody.Location.Y -= 0.25f;
                tankSprite.tankBody.Location.X += 1.75f;
            }

            if (cKeyBoard.IsKeyDown(Keys.W) && cKeyBoard.IsKeyDown(Keys.A))
            {
                direction = convertDtoR(225);
                tankSprite.tankBody.rotation = (float)direction;

                tankSprite.tankBody.Location.Y -= 0.25f;
                tankSprite.tankBody.Location.X -= 1.75f;
            }

            if (cKeyBoard.IsKeyDown(Keys.S) && cKeyBoard.IsKeyDown(Keys.A))
            {
                direction = convertDtoR(135);
                tankSprite.tankBody.rotation = (float)direction;

                tankSprite.tankBody.Location.Y -= 0.25f;
                tankSprite.tankBody.Location.X -= 1.75f;
            }

            if (cKeyBoard.IsKeyDown(Keys.S) && cKeyBoard.IsKeyDown(Keys.D))
            {
                direction = convertDtoR(45);
                tankSprite.tankBody.rotation = (float)direction;

                tankSprite.tankBody.Location.Y -= 0.25f;
                tankSprite.tankBody.Location.X -= 1.75f;
            }
        }


    }
}

*/