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

        //Currently the limit the algorithm can handle is around 200 blocks in each direction without serious performance issues
        public struct Hall
        {
            public int start_X;
            public int start_Y;

            public bool stopGoing; //Used for hallway intersections
        }
        
        public struct Room
        {
            public int[,] _room; //An array to keep track of each block's location. 0 for empty and 1 for full
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

            public bool isNULL; //A value to make the rooms pseudo-nullable
        }
      
        Room newRoom;
        Room room_NULL = new Room();
        Random rand = new Random();

        //Each index in GameBoard can be thought of as a 40x40 pixel area on the screen. 
        //This array will be populated based on the location of 1's in each room's array.
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
        public int[,] newArray = new int[MAX_TILES_X, MAX_TILES_Y]; //These will be populated based upon the individual room's locations
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
            //Only the final GameBoard is used in the draw function, for efficiency
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
            //This function will generate random places for rooms and halls until everything is in a valid location

            numofRooms = rand.Next(4, 6);
            roomList = new Room[numofRooms];
            ClearArray(roomList);
            ClearArray(hallArray);
            ClearArray(newArray);
            finishedRooms = 0;
            Room room; //A reusable temporary variable to iterate through the list of rooms

            while (finishedRooms < numofRooms)
            {
                
                new_start_X = rand.Next(0, MAX_TILES_X); 
                new_start_Y = rand.Next(0, MAX_TILES_Y);
                new_Size_X = rand.Next(3, 6);
                new_Size_Y = rand.Next(3, 6);

                isValid = CheckLocation(new_Size_X, new_Size_Y, new_start_X, new_start_Y, newArray, GameBoard);//Essentially making sure rooms don't overlap each other

                if (isValid)
                {
                    ClearArray(newRoom._room); 
                    //We only need to use one room over and over again, we just have to set it's values to different things and store those values in the gameboard when it's ready.
                    //The purpose of this is to avoid dynamically allocating memory when we don't need to. Such a process is a huge performance drain especially inside a loop of this nature.

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

                    room = roomList[finishedRooms];

                    if (room.isNULL)
                    {
                        roomList[finishedRooms] = newRoom;
                    }
                    SetGameBoard(newRoom, GameBoard, content);
                    finishedRooms++;
                }
                else
                {
                    //This resets the gameboard and starts over. This will occur whenever a room is placed in an invalid location.
                    //This has the potential to be optimized. The timer is to fix a glitch that caused the screen to flash upon reloading.
                    //Some lag will occur if this number is less than 10
                    //If it's impossible for the rooms to fit in the boundaries this will be infinite and cause the game to crash
                    //As the boundaries get bigger in proportion to the number of rooms and their size, this will be called less, as there's a creater chance to fit everything on the screen the first time. 
                    loadingTimer++;
                    if (loadingTimer >= 5)
                    {
                        reLoad();
                        finishedRooms = 0;
                        loadingTimer = 0;
                    }
                }
            } //End of while loop - ROOMS GENERATED

                for (int i = 0; i < numofRooms; i++)//For each room, set the hallways
                {

                    //As of now, each room has four hallways. These hallways are placed in the middle of the four edges of each room.

                    room = roomList[i];
                    room.Hall1.start_X = room.X_Mid;
                    room.Hall1.start_Y = room.start_Y - 1;
                    room.Hall1.stopGoing = false;

                    room.Hall2.start_X = room.X_Mid;
                    room.Hall2.start_Y = room.start_Y + room.Size_Y;
                    room.Hall2.stopGoing = false;

                    room.Hall3.start_X = room.start_X - 1;
                    room.Hall3.start_Y = room.Y_Mid;
                    room.Hall3.stopGoing = false;

                    room.Hall4.start_X = room.start_X + room.Size_X;
                    room.Hall4.start_Y = room.Y_Mid;
                    room.Hall4.stopGoing = false;
                    //These loops are to add the halls to the gameboard and make ure that they stop at intersections and upon touching rooms.
                    //This is implemented by checking if gameboard is null in a potential location. If it is, place the block. If not, still place the block but don't continue after.
                    //Since each hall is set after the other, the gameboard must be set after each block is placed for the halls to intersect properly. 
                    //It's still imperfect, occasionally a hall will keep extending past a room.
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
                Console.WriteLine("DONE LOADING");
        }
        public void reLoad()
        {
            ClearArray(GameBoard);
            ClearArray(newRoom._room);
            ClearArray(roomList);
            ClearArray(hallArray);
            ClearArray(newArray);
        }
        public void Update(GameTime gameTime)
        {
            //So far, we don't need to update this class.
        }

        private bool CheckLocation(int size_x, int size_y, int start_pos_X, int start_pos_Y, int[,] array, Sprite[,] gameboard)
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

        private void SetHalls(ContentManager content)
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
