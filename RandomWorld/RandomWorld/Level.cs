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
    class Level
    {
        public const int MAX_TILES_X = 20; //The maximum amount of blocks on the X axis
        public const int MAX_TILES_Y = 12; //The maximum amount of blocks on the X axis
        public const int SPRITE_SIZE = 40; //The size of each block

        public struct Hall
        {
            public int start_X;
            public int start_Y;

            public bool stopGoing;
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

        private List<int> Mid_List_X = new List<int>();
        private List<int> Mid_List_Y = new List<int>();

        public Level()
        {
            room_NULL.isNULL = true;
            newRoom = new Room();
            newRoom._room = new int[MAX_TILES_X, MAX_TILES_Y];
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
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
        public void Load(ContentManager content)
        {

            numofRooms = rand.Next(4, 6);
            roomList = new Room[numofRooms];
            ClearArray(roomList);
            ClearArray(hallArray);
            finishedRooms = 0;

            while (finishedRooms < numofRooms)
            {
                ClearArray(newArray);
                new_start_X = rand.Next(0, MAX_TILES_X);
                new_start_Y = rand.Next(0, MAX_TILES_Y);
                new_Size_X = rand.Next(3, 6);
                new_Size_Y = rand.Next(3, 6);

                isValid = CheckLocation(new_Size_X, new_Size_Y, new_start_X, new_start_Y, newArray, GameBoard);
                if (isValid)
                {
                    ClearArray(newRoom._room);
                    for (int i = 0; i < MAX_TILES_X; i++)
                    {
                        for (int j = 0; j < MAX_TILES_Y; j++)
                        {
                            newRoom._room[i, j] = newArray[i, j];
                        }
                    }

                    newRoom.start_X = new_start_X;
                    newRoom.start_Y = new_start_Y;
                    newRoom.Size_X = new_Size_X;
                    newRoom.Size_Y = new_Size_Y;
                    newRoom.X_Mid = new_start_X + (newRoom.Size_X / 2);
                    newRoom.Y_Mid = new_start_Y + (newRoom.Size_Y / 2);
                    newRoom.isNULL = false;

                    Mid_List_X.Add(newRoom.X_Mid);
                    Mid_List_Y.Add(newRoom.Y_Mid);


                    Room room = roomList[finishedRooms];

                    if (room.isNULL)
                    {
                        roomList[finishedRooms] = newRoom;
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
                for (int i = 0; i < numofRooms; i++)
                {
                    Room room = roomList[i];
                    Console.WriteLine("I got here");
                    room.Hall1.start_X = room.X_Mid;
                    room.Hall1.start_Y = room.start_Y;
                    room.Hall1.stopGoing = false;

                    room.Hall2.start_X = room.X_Mid;
                    room.Hall2.start_Y = room.start_Y + room.Size_Y;
                    room.Hall2.stopGoing = false;

                    room.Hall3.start_X = room.start_X;
                    room.Hall3.start_Y = room.Y_Mid;
                    room.Hall3.stopGoing = false;

                    room.Hall4.start_X = room.start_X + room.Size_X;
                    room.Hall4.start_Y = room.Y_Mid;
                    room.Hall4.stopGoing = false;

                    for (int k = 0; k < MAX_TILES_X; k++)
                    {
                        for (int m = 0; m < MAX_TILES_Y; m++)
                        {
                            if (room.Hall1.start_Y - m >= 0 && !room.Hall1.stopGoing)
                            {
                                if (GameBoard[room.Hall1.start_X, room.Hall1.start_Y - m] != null)
                                {
                                    room.Hall1.stopGoing = true;
                                }
                                hallArray[room.Hall1.start_X, room.Hall1.start_Y - m] = 1;
                                SetHalls(content);
                            }

                            if (room.Hall2.start_Y + m < MAX_TILES_Y && !room.Hall2.stopGoing)
                            {
                                if (GameBoard[room.Hall2.start_X, room.Hall2.start_Y + m] != null)
                                {
                                    room.Hall2.stopGoing = true;
                                }
                                hallArray[room.Hall2.start_X, room.Hall2.start_Y + m] = 1;
                                SetHalls(content);
                            }
                        }
                            if (room.Hall3.start_X - k >= 0 && !room.Hall3.stopGoing)
                            {
                                if (GameBoard[room.Hall3.start_X - k, room.Hall3.start_Y] != null)
                                {
                                    room.Hall3.stopGoing = true;
                                } 
                                hallArray[room.Hall3.start_X - k, room.Hall3.start_Y] = 1;
                                SetHalls(content);
                            }

                            if (room.Hall4.start_X + k < MAX_TILES_X && !room.Hall4.stopGoing)
                            {
                                if (GameBoard[room.Hall4.start_X + k, room.Hall4.start_Y] != null)
                                {
                                   room.Hall4.stopGoing = true;
                                } 
                                hallArray[room.Hall4.start_X + k, room.Hall4.start_Y] = 1;
                                SetHalls(content);
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
        public void Update(GameTime gameTime)
        {

        }
 

        public bool CheckLocation(int size_x, int size_y, int start_pos_X, int start_pos_Y, int[,] array, Sprite[,] gameboard)
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
        private void SetGameBoard(Room room, Sprite[,] gameBoard, ContentManager content)
        {
            for (int x = room.start_X; x < (room.Size_X + room.start_X); x++)
            {
                for (int y = room.start_Y; y < (room.Size_Y + room.start_Y); y++)
                {
                    if (room._room[x,y] != 0)
                    {
                        tiles = new Sprite("FloorTiles", content);
                        GameBoard[x, y] = tiles;
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
                            GameBoard[j, k].Position.X = (SPRITE_SIZE * j);
                            GameBoard[j, k].Position.Y = (SPRITE_SIZE * k);
                        }
                    }
                }
            }
        }
 
    }
}
