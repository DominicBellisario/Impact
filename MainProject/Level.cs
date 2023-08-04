﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace MainProject
{
    internal class Level
    {
        //holds background level assets
        private Dictionary<string, Texture2D> bgAssets;

        //holds interactable level assets
        private Dictionary<string, Texture2D> intAssets;

        //width and height of screen
        private int width;
        private int height;

        private Room[,] bgLevelBlueprint;
        private Room[,] intLevelBlueprint;

        //x and y pos of player
        private double playerPosX;
        private double playerPosY;

        //rows and columns in the 2d array
        private int rows;
        private int columns;

        public double PlayerPosX
        {
            get { return playerPosX; }
        }
        public double PlayerPosY
        {
            get { return playerPosY; }
        }

        public Room[,] BgLevelBlueprint
        {
            get { return bgLevelBlueprint; }
        }

        public Room[,] IntLevelBlueprint
        {
            get { return intLevelBlueprint; }
        }

        public int Rows
        {
            get { return rows; }
        }

        public int Columns
        {
            get { return columns; }
        }

        //paramaterized constructor
        public Level(Dictionary<string, Texture2D> bgAssets, Dictionary<string, Texture2D> intAssets, 
            int width, int height, string bgFilename, string intFilename)
        {
            this.bgAssets = bgAssets;
            this.intAssets = intAssets;
            this.width = width;
            this.height = height;

            //player starts in the middle of the screen
            playerPosX = width / 2;
            playerPosY = height / 2;

            //create 2D array of tiles for the background
            LoadBgLevel(bgFilename);
            //create one for the interactables
            LoadIntLevel(intFilename);
        }

        /// <summary>
        /// Loads the level's background layout from a text file and creates a 2D array that represents it
        /// </summary>
        /// <param name="filename"></param>
        public void LoadBgLevel(string filename)
        {
            StreamReader input;
            string currentLine;
            List<string> lines = new List<string>();
            string[] data;
            string[] tileData;


            try
            {
                input = new StreamReader("..\\..\\..\\" + filename); //may need 2 or 0 double slashes

                //reads through each line of the text file and stores it in a list
                while ((currentLine = input.ReadLine()) != null)
                {
                    lines.Add(currentLine);
                }

                //initialize levelBlueprint
                rows = lines.Count;
                columns = lines[0].Split(',').Length;
                bgLevelBlueprint = new Room[rows, columns];

                //break up lines of data into individual letter tiles
                for (int i = 0; i < rows; i++)
                {
                    //gets one line of data
                    data = lines[i].Split(",");
                    for (int j = 0; j < columns; j++)
                    {
                        #region tileSorting
                        //determines what type of tile it is
                        //wall
                        if (data[j] == "W")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["wall"], true, "surface");
                        }
                        //floor
                        else if (data[j] == "F")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["floor"], true,"surface");
                        }
                        //left platform
                        else if (data[j] == "L")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["leftPlat"], true, "surface");
                        }
                        //center platform
                        else if (data[j] == "C")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["centerPlat"], true, "surface");
                        }
                        //right platform
                        else if (data[j] == "R")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["rightPlat"], true, "surface");
                        }
                        //background
                        else if (data[j] == "B")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["background"], false, "none");
                        }
                        //spawn
                        else if (data[j] == "S")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["spawn"], false, "none");
                            playerPosX = j * 100;
                            playerPosY = i * 100;
                        }
                        #endregion
                    }
                }
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        bgLevelBlueprint[i, j].RectX -= playerPosX - width / 2;
                        bgLevelBlueprint[i, j].RectY -= playerPosY - height / 2;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            //if file was opened close it
            if (input != null)
            {
                input.Close();
            }
        }

        /// <summary>
        /// Loads the level's background layout from a text file and creates a 2D array that represents it
        /// </summary>
        /// <param name="filename"></param>
        public void LoadIntLevel(string filename)
        {
            StreamReader input;
            string currentLine;
            List<string> lines = new List<string>();
            string[] data;
            string[] tileData;


            try
            {
                input = new StreamReader("..\\..\\..\\" + filename); //may need 2 or 0 double slashes

                //reads through each line of the text file and stores it in a list
                while ((currentLine = input.ReadLine()) != null)
                {
                    lines.Add(currentLine);
                }

                //initialize levelBlueprint
                rows = lines.Count;
                columns = lines[0].Split(',').Length;
                intLevelBlueprint = new Room[rows, columns];

                //break up lines of data into individual letter tiles
                for (int i = 0; i < rows; i++)
                {
                    //gets one line of data
                    data = lines[i].Split(",");
                    for (int j = 0; j < columns; j++)
                    {
                        #region tileSorting
                        //determines what type of tile it is
                        //null
                        if (data[j] == "N")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                intAssets["null"], false, "none");
                        }
                        //left spring
                        else if (data[j] == "LS")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                intAssets["leftSpring"], true, "leftSpring");
                        }
                        //right spring
                        else if (data[j] == "RS")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                intAssets["rightSpring"], true, "rightSpring");
                        }
                        //up spring
                        else if (data[j] == "US")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                intAssets["upSpring"], true, "upSpring");
                        }
                        //left tube
                        else if (data[j] == "LT")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100 - 100, 100, 300),
                                intAssets["leftTube"], true, "leftTube");
                        }
                        //right tube
                        else if (data[j] == "RT")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100 - 100, 100, 300),
                                intAssets["rightTube"], true, "rightTube");
                        }
                        //up tube
                        else if (data[j] == "UT")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100 - 100, i * 100, 300, 100),
                                intAssets["upTube"], true, "upTube");
                        }
                        //down tube
                        else if (data[j] == "DT")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100 - 100, i * 100, 300, 100),
                                intAssets["downTube"], true, "downTube");
                        }
                        #endregion
                    }
                }
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        intLevelBlueprint[i, j].RectX -= playerPosX - width / 2;
                        intLevelBlueprint[i, j].RectY -= playerPosY - height / 2;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            //if file was opened close it
            if (input != null)
            {
                input.Close();
            }
        }

        /// <summary>
        /// updates the position of the level using player velocity
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, double xVelocity, double yVelocity)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    intLevelBlueprint[i, j].RectX += (int)xVelocity;
                    intLevelBlueprint[i, j].RectY += (int)yVelocity;
                    bgLevelBlueprint[i, j].RectX += (int)xVelocity;
                    bgLevelBlueprint[i, j].RectY += (int)yVelocity;
                    
                }
            }
        }

        /// <summary>
        /// draws the level using the 2d array
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            //draws background
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    bgLevelBlueprint[i,j].Draw(sb);
                }
            }
            //draws interactables
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    intLevelBlueprint[i, j].Draw(sb);
                }
            }
        }
    }
}
