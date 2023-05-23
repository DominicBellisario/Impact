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
        private int playerPosX;
        private int playerPosY;

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
        }

        /// <summary>
        /// Loads the level's layout from a text file and creates a 2D array that represents the level
        /// </summary>
        /// <param name="filename"></param>
        public void LoadLevel(string filename)
        {
            StreamReader input = null;
            string currentLine;
            int rows;
            int columns;
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
                    for (int j = 0; j < columns - 1; j++)
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
                        }
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
    }
}
