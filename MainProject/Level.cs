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
        //holds all level assets
        private Dictionary<string, Texture2D> assets;

        //width and height of screen
        private int width;
        private int height;

        //name of file used to create the level
        private string filename;
        private Room[,] levelBlueprint;

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

        public Room[,] LevelBlueprint
        {
            get { return levelBlueprint; }
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
        public Level(Dictionary<string, Texture2D> assets, int width, int height, string filename)
        {
            this.assets = assets;
            this.width = width;
            this.height = height;
            this.filename = filename;

            //player starts in the middle of the screen
            playerPosX = width / 2;
            playerPosY = height / 2;

            //create 2D array of tiles
            LoadLevel(filename);
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
                levelBlueprint = new Room[rows, columns];

                //break up lines of data into individual letter tiles
                for (int i = 0; i < rows; i++)
                {
                    //gets one line of data
                    data = lines[i].Split(",");
                    for (int j = 0; j < columns; j++)
                    {
                        //determines what type of tile it is
                        //wall
                        if (data[j] == "W")
                        {
                            levelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100), assets["wall"], true);
                        }
                        //floor
                        else if (data[j] == "F")
                        {
                            levelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100), assets["floor"], true);
                        }
                        //left platform
                        else if (data[j] == "L")
                        {
                            levelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100), assets["leftPlat"], true);
                        }
                        //center platform
                        else if (data[j] == "C")
                        {
                            levelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100), assets["centerPlat"], true);
                        }
                        //right platform
                        else if (data[j] == "R")
                        {
                            levelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100), assets["rightPlat"], true);
                        }
                        //background
                        else if (data[j] == "B")
                        {
                            levelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100), assets["background"], false);
                        }
                        //spawn
                        else if (data[j] == "S")
                        {
                            levelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100), assets["spawn"], false);
                            playerPosX = j * 100;
                            playerPosY = i * 100;
                        }
                    }
                }
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        levelBlueprint[i, j].RectX -= playerPosX - width / 2;
                        levelBlueprint[i, j].RectY -= playerPosY - height / 2;
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
                    levelBlueprint[i, j].RectX += xVelocity;
                    levelBlueprint[i, j].RectY += yVelocity;
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
                    levelBlueprint[i,j].Draw(sb);
                }
            }
        }
    }
}
