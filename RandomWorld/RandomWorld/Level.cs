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
    class Level : Structure
    {
        public const int MAX_TILES_X = 20; //The maximum amount of blocks on the X axis
        public const int MAX_TILES_Y = 12; //The maximum amount of blocks on the X axis
        public const int SPRITE_SIZE = 40; //The size of each block

        public struct Hall
        {
            public int start_X;
            public int start_Y;

            public bool isValid;

        }

        public struct Room
        {
            public int[,] _room;
            public int Size_X;
            public int Size_Y;

            public int start_X;
            public int start_Y;

            public int X_Mid;
            public int Y_Mid;

            public Hall Hall1;
            public Hall Hall2;
            public Hall Hall3;
            public Hall Hall4;

            public bool isNULL;
        }
      
        Room newRoom;
        Room room_NULL = new Room();
        Random rand = new Random();
        Sprite[,] GameBoard = new Sprite[MAX_TILES_X, MAX_TILES_Y];
        int numofRooms;
        int finishedRooms = 0;

        Room[] roomList;
        Sprite tiles;
        
        bool isValid;

        public int new_Size_X;
        public int new_Size_Y;
        public int new_start_X;
        public int new_start_Y;
        public int loadingTimer = 0;
        public int[,] newArray = new int[MAX_TILES_X, MAX_TILES_Y];
        public int[,] hallArray = new int[MAX_TILES_X, MAX_TILES_Y];

        public Level()
        {
            room_NULL.isNULL = true;
            newRoom = new Room();
            newRoom._room = new int[MAX_TILES_X, MAX_TILES_Y];
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
            for (int b = 0; b < MAX_TILES_X; b++)
            {
                for (int c = 0; c < MAX_TILES_Y; c++)
                {

                    if (GameBoard[b, c] != null)
                    {
                        GameBoard[b, c].DrawMethod(spriteBatch, gameTime);
                    }
                }
                
            }
        }
        public override void Load(ContentManager content)
        {

            numofRooms = rand.Next(4, 6);
            roomList = new Room[numofRooms];
            ClearArray(roomList);
            ClearArray(hallArray);
            finishedRooms = 0;

            while (finishedRooms < numofRooms)
            {
                ClearArray(newArray);
                ClearArray(roomList);
                new_start_X = rand.Next(0, MAX_TILES_X);
                new_start_Y = rand.Next(0, MAX_TILES_Y);
                new_Size_X = rand.Next(3, 6);
                new_Size_Y = rand.Next(3, 6);

                isValid = CheckLocation(new_Size_X, new_Size_Y, new_start_X, new_start_Y, newArray, GameBoard);
                if (isValid)
                {
                    ClearArray(newRoom._room);
                    for(int i = 0; i < MAX_TILES_X; i++)
                    {
                        for (int j = 0; j < MAX_TILES_Y; j++)
                        {
                            newRoom._room[i,j] = newArray[i,j];
                        }
                    }
                    
                    newRoom.start_X = new_start_X;
                    newRoom.start_Y = new_start_Y;
                    newRoom.Size_X = new_Size_X;
                    newRoom.Size_Y = new_Size_Y;
                    newRoom.X_Mid = new_start_X + (newRoom.Size_X / 2);
                    newRoom.Y_Mid = new_start_Y + (newRoom.Size_Y / 2);
                    newRoom.isNULL = false;

                    for (int i = 0; i < numofRooms; i++)
                    {
                        Room room = roomList[i];

                        if (room.isNULL)
                        {
                            room = newRoom;
                        }

                            if (!room.isNULL)
                            {

                                room.Hall1.start_X = room.X_Mid;
                                room.Hall1.start_Y = room.start_Y;

                                room.Hall2.start_X = room.X_Mid;
                                room.Hall2.start_Y = room.start_Y + room.Size_Y;

                                room.Hall3.start_X = room.start_X;
                                room.Hall3.start_Y = room.Y_Mid;

                                room.Hall4.start_X = room.start_X + room.Size_X;
                                room.Hall4.start_Y = room.Y_Mid;

                                for (int k = 0; k < MAX_TILES_X; k++)
                                {
                                    for (int m = 0; m < MAX_TILES_Y + 1; m++)
                                    {
                                        if (room.Hall1.start_Y - m >= 0)
                                        {
                                            hallArray[room.Hall1.start_X, room.Hall1.start_Y - m] = 1;
                                        }
                                        else
                                        {
                                            //If hallArray != 0, stop because it intersected another thing
                                        }

                                        if (room.Hall2.start_Y + m < MAX_TILES_Y)
                                        {
                                            hallArray[room.Hall2.start_X, room.Hall2.start_Y + m] = 1;
                                        }
                                        else
                                        {
                                            //Intersection
                                        }

                                        if (room.Hall3.start_X - k >= 0)
                                        {
                                            hallArray[room.Hall3.start_X - k, room.Hall3.start_Y] = 1;
                                        }
                                        else
                                        {
                                            //If hallArray != 0, stop because it intersected another thing
                                        }

                                        if (room.Hall4.start_X + k < MAX_TILES_X)
                                        {
                                            hallArray[room.Hall4.start_X + k, room.Hall4.start_Y] = 1;
                                        }
                                        else
                                        {
                                            //Intersection
                                        }

                                    }
                                }

                            }

                        
                    }
                    SetGameBoard(newRoom, GameBoard, content);
                    finishedRooms++;
                }
                else
                {
                    loadingTimer++;
                    if (loadingTimer >= 10)
                    {
                        reLoad();
                        finishedRooms = 0;
                        loadingTimer = 0;
                    }
                }
            }

            SetHalls(content);
        }
        public void reLoad()
        {
            ClearArray(GameBoard);
            ClearArray(newRoom._room);
            ClearArray(roomList);
            ClearArray(hallArray);
        }
        public override void Update(GameTime gameTime)
        {

        }
 

        public override bool CheckLocation(int size_x, int size_y, int start_pos_X, int start_pos_Y, int[,] array, Sprite[,] gameboard)
        {
            if (start_pos_X > 0 && start_pos_X + size_x < MAX_TILES_X && start_pos_Y > 0 && start_pos_Y + size_y < MAX_TILES_Y)
            {
                for (int x = start_pos_X; x <= start_pos_X + size_x; x++)
                {
                    for (int y = start_pos_Y; y <= start_pos_Y + size_y; y++)
                    {
                        if (gameboard[x, y] != null) //Prevent Overlapping
                        {
                            Console.WriteLine("Returned false : GameBoard Occupied");
                            return false;
                        }
                        else if (array[x, y] != 0)
                        {
                            return false;
                        } 
                        else
                        {
                            array[x, y] = 1;
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private void ClearArray(Room[] array)
        {
            for (int i = 0; i < numofRooms; i++)
            {
                array[i] = room_NULL;
            }
        }
        private void ClearArray(int[,] array)
        {
            for (int i = 0; i < MAX_TILES_X; i++)
            {
                for (int j = 0; j < MAX_TILES_Y; j++)
                {
                    array[i, j] = 0;
                }
            }
        }
        private void ClearArray(Sprite[,] array)
        {
            for (int i = 0; i < MAX_TILES_X; i++)
            {
                for (int j = 0; j < MAX_TILES_Y; j++)
                {
                    array[i, j] = null;
                }
            }
        }
        public void SetGameBoard(Room room, Sprite[,] gameBoard, ContentManager content)
        {
            for (int x = room.start_X; x < (room.Size_X + room.start_X); x++)
            {
                for (int y = room.start_Y; y < (room.Size_Y + room.start_Y); y++)
                {
                    if (room._room[x,y] != 0)
                    {
                        tiles = new Sprite("FloorTiles", content);
                        GameBoard[x, y] = tiles;
                        Console.WriteLine("Room at " + x + "," + y);
                        GameBoard[x, y].Position.X = (SPRITE_SIZE * x);
                        GameBoard[x, y].Position.Y = (SPRITE_SIZE * y);
                    }
                }
            }

          
           
        }

        private void SetHalls (ContentManager content)
        {
            for (int j = 0; j < MAX_TILES_X; j++)
            {
                for (int k = 0; k < MAX_TILES_Y; k++)
                {
                    if (GameBoard[j, k] == null)
                    {
                        if (hallArray[j, k] != 0)
                        {
                            tiles = new Sprite("FloorTiles", content);
                            GameBoard[j, k] = tiles;
                            Console.WriteLine("Hall at " + j + "," + k);
                            GameBoard[j, k].Position.X = (SPRITE_SIZE * j);
                            GameBoard[j, k].Position.Y = (SPRITE_SIZE * k);
                        }
                    }
                }
            }
        }
 
    }
}
