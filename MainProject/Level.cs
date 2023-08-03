using System;
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

        //width and height of screen
        private int width;
        private int height;

        private Room[,] bgLevelBlueprint;

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

        public int Rows
        {
            get { return rows; }
        }

        public int Columns
        {
            get { return columns; }
        }

        //paramaterized constructor
        public Level(Dictionary<string, Texture2D> assets, int width, int height, string bgFilename)
        {
            this.bgAssets = assets;
            this.width = width;
            this.height = height;

            //player starts in the middle of the screen
            playerPosX = width / 2;
            playerPosY = height / 2;

            //create 2D array of tiles for the background
            LoadLevel(bgFilename);
        }

        /// <summary>
        /// Loads the level's layout from a text file and creates a 2D array that represents the level
        /// </summary>
        /// <param name="filename"></param>
        public void LoadLevel(string filename)
        {
            StreamReader input = null;
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
                        //left spring
                        else if (data[j] == "LS")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["leftSpring"], true, "leftSpring");
                        }
                        //right spring
                        else if (data[j] == "RS")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["rightSpring"], true, "rightSpring");
                        }
                        //up spring
                        else if (data[j] == "US")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["upSpring"], true, "upSpring");
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
        /// updates the position of the level using player velocity
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, double xVelocity, double yVelocity)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
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
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    bgLevelBlueprint[i,j].Draw(sb);
                }
            }
        }
    }
}
