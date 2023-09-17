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
using System.Threading;
using System.Security.AccessControl;

namespace MainProject
{
    internal class Level
    {
        //keeps track of what frame the level is on (60 fps)
        private int timer;

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

        //keyboard stuff
        private KeyboardState kBState;
        private KeyboardState prevKBState;
        private bool normalTube;

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
            normalTube = true;

            //start timer at 0
            timer = 0;

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
                                bgAssets["wall"], bgAssets["wall"], true, "surface", 0, 0, "none");
                        }
                        //floor
                        else if (data[j] == "F")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["floor"], bgAssets["floor"], true,"surface", 0, 0, "none");
                        }
                        //left platform
                        else if (data[j] == "L")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["leftPlat"], bgAssets["leftPlat"], true, "surface", 0, 0, "none");
                        }
                        //center platform
                        else if (data[j] == "C")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["centerPlat"], bgAssets["centerPlat"], true, "surface", 0, 0, "none");
                        }
                        //right platform
                        else if (data[j] == "R")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["rightPlat"], bgAssets["rightPlat"], true, "surface", 0, 0, "none");
                        }
                        //ice
                        else if (data[j] == "I")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["ice"], bgAssets["ice"], true, "ice", 0, 0, "none");
                        }
                        //background
                        else if (data[j] == "B")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["background"], bgAssets["background"], false, "none", 0, 0, "none");
                        }
                        //spawn
                        else if (data[j] == "S")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["background"], bgAssets["background"], false, "none", 0, 0, "none");
                            playerPosX = j * 100;
                            playerPosY = i * 100;
                        }
                        //spawn
                        else if (data[j] == "E")
                        {
                            bgLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                bgAssets["spawn"], bgAssets["spawn"], true, "end", 0, 0, "none");
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
                                intAssets["null"], intAssets["null"], false, "none", 0, 0, "none");
                        }
                        //left spring
                        else if (data[j] == "LS")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                intAssets["leftSpring"], intAssets["leftSpring"], true, "leftSpring", 5, 4, "none");
                        }
                        //right spring
                        else if (data[j] == "RS")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                intAssets["rightSpring"], intAssets["rightSpring"], true, "rightSpring", 5, 4, "none");
                        }
                        //up spring
                        else if (data[j] == "US")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 100),
                                intAssets["upSpring"], intAssets["upSpring"], true, "upSpring", 5, 4, "none");
                        }
                        //left tube
                        else if (data[j] == "LT")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100 - 100, 100, 300),
                                intAssets["leftTube"], intAssets["rightTube"], true, "leftTube", 5, 3, "none");
                        }
                        //right tube
                        else if (data[j] == "RT")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100 - 100, 100, 300),
                                intAssets["rightTube"], intAssets["leftTube"], true, "rightTube", 5, 3, "none");
                        }
                        //up tube
                        else if (data[j] == "UT")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100 - 100, i * 100, 300, 100),
                                intAssets["upTube"], intAssets["downTube"], true, "upTube", 5, 3, "none");
                        }
                        //down tube
                        else if (data[j] == "DT")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100 - 100, i * 100, 300, 100),
                                intAssets["downTube"], intAssets["upTube"], true, "downTube", 5, 3, "none");
                        }
                        //down spike
                        else if (data[j] == "DH")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100 + 50, 100, 50),
                                intAssets["spikes"], intAssets["spikes"], true, "spikes", 5, 3, "down");
                        }
                        //up spike
                        else if (data[j] == "UH")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 100, 50),
                                intAssets["spikes"], intAssets["spikes"], true, "spikes", 5, 3, "up");
                        }
                        //left spike
                        else if (data[j] == "LH")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100, i * 100, 50, 100),
                                intAssets["spikes"], intAssets["spikes"], true, "spikes", 5, 3, "left");
                        }
                        //right spike
                        else if (data[j] == "RH")
                        {
                            intLevelBlueprint[i, j] = new Room(new Rectangle(j * 100 + 50, i * 100, 50, 100),
                                intAssets["spikes"], intAssets["spikes"], true, "spikes", 5, 3, "right");
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
        /// updates the position and animation frame of the level tiles
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, double xVelocity, double yVelocity)
        {
            //increment timer
            timer++;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    //updates the position of each level tile
                    intLevelBlueprint[i, j].RectX += (int)xVelocity;
                    intLevelBlueprint[i, j].RectY += (int)yVelocity;
                    bgLevelBlueprint[i, j].RectX += (int)xVelocity;
                    bgLevelBlueprint[i, j].RectY += (int)yVelocity;

                    //as long as the timer has a speed
                    if (intLevelBlueprint[i, j].AnimationSpeed != 0)
                    {
                        //update the tile's frame.  different depending on the individual tile's speed
                        if (timer % intLevelBlueprint[i, j].AnimationSpeed == 0)
                        {
                            //repeats the animation if it is at its end
                            if (intLevelBlueprint[i, j].CurrentFrame == intLevelBlueprint[i, j].NumberOfFrames)
                            {
                                intLevelBlueprint[i, j].CurrentFrame = 0;
                            }
                            //otherwise, go to next frame
                            else
                            {
                                intLevelBlueprint[i, j].CurrentFrame++;
                            }

                        }
                    }
                }
            }
            KeyboardState kbState = Keyboard.GetState();

            //pressing the E key reverses every tube's direction
            if (kbState.IsKeyDown(Keys.E) && prevKBState.IsKeyUp(Keys.E))
            {
                normalTube = !normalTube;
            }
            prevKBState = kbState;
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
                    if (normalTube)
                    {
                        bgLevelBlueprint[i,j].Draw(sb, true);
                    }
                    else
                    {
                        bgLevelBlueprint[i, j].Draw(sb, false);
                    }
                }
            }
            //draws interactables
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (normalTube)
                    {
                        intLevelBlueprint[i, j].Draw(sb, true);
                    }
                    else
                    {
                        intLevelBlueprint[i, j].Draw(sb, false);
                    }
                }
            }
        }
    }
}
