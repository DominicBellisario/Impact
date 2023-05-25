using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{
    internal class Player
    {
        //position of the player
        private double xPos;
        private double yPos;

        //player hitbox
        private Rectangle rect;

        //current speed of the player
        private double xVelocity;
        private double yVelocity;

        //booleans that check if the player is touching or being blocked by a surface
        private bool isGrounded;
        private bool touchingLeftWall;
        private bool touchingRightWall;

        //simulates gravity
        private const double gravity = -1;

        //very fast horizontal acceleration when player begins to walk
        private const double walkAccel = 2;

        //less fast horizonal accelertaion while player is in the air
        private const double airAccel = 1;

        //max speeds in the x and y directions
        private const int maxXSpeed = 10;
        private const int maxYSpeed = -30;

        //player asset
        private Texture2D asset;

        //keyboard stuff
        private KeyboardState prevKBState;

        //debug
        private SpriteFont debugFont;
        private string debugText;

        public double XVelocity
        {
            get { return xVelocity; }
        }

        public double YVelocity
        {
            get { return yVelocity; }
        }

        public Texture2D Asset
        {
            get { return asset; }
        }

        public Player(double xPos, double yPos, Texture2D asset, SpriteFont debugFont)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.asset = asset;
            this.debugFont = debugFont;
            xVelocity = 0;
            yVelocity = 0;
            isGrounded = false;
            touchingLeftWall = false;
            touchingRightWall = false;
            rect = new Rectangle((int)xPos - 100, (int)yPos - 100, 200, 200);
        }

        /// <summary>
        /// controls player physics
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            //Y VELOCITY
            //player accelerates downward if not touching the ground and not at max speed
            if (!isGrounded && yVelocity > maxYSpeed)
            {
                yVelocity += gravity;
            }
            //player reaches terminal velocity, no acceleration
            else if (!isGrounded && yVelocity <= maxYSpeed)
            {
                yVelocity = maxYSpeed;
            }
            //player is touching the ground
            else
            {
                //player jumps up if the space key is pressed
                if (kbState.IsKeyDown(Keys.Space) && prevKBState.IsKeyUp(Keys.Space))
                {
                    yVelocity = 45;
                }
                //otherwise, player rests on the ground
                else
                {
                    yVelocity = 0;
                }
                
            }

            //X VELOCITY
            //player moves left if a is pressed, d is not pressed,
            //the player is not blocked, and they are not at max speed
            if (kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D) && 
                !touchingLeftWall && Math.Abs(xVelocity) < maxXSpeed)
            {
                xVelocity += walkAccel;
            }
            //player moves right if d is pressed, a is not pressed,
            //the player is not blocked, and they are not at max speed
            if (kbState.IsKeyDown(Keys.D) && kbState.IsKeyUp(Keys.A) &&
                !touchingRightWall && Math.Abs(xVelocity) < maxXSpeed)
            {
                xVelocity -= walkAccel;
            }

            //if none or both "a" and "d" are pressed, decelerate the player to 0
            if ((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) ||
                (kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D)))
            {
                if (xVelocity > 0)
                {
                    xVelocity -= walkAccel;
                }
                else if (xVelocity < 0)
                {
                    xVelocity += walkAccel;
                }
            }

            //updates prev keyboard state
            prevKBState = kbState;
        }

        /// <summary>
        /// checks tiles to see if they collide with the player
        /// </summary>
        /// <param name="level"></param>
        public void Collisions(Room[,] level, int rows, int columns)
        {
            Rectangle collisionRect;
            //reset all collisions
            isGrounded = false;
            touchingLeftWall = false;
            touchingRightWall = false;
            debugText = "0, 0";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    //makes sure only near blocks that have collision are taken into account
                    if (Math.Abs(level[i, j].Rect.X - rect.X) < 400 &&
                        Math.Abs(level[i, j].Rect.Y - rect.Y) < 400 && level[i, j].CanCollide)
                    {
                        //creates a rectangle of the overlaping area
                        collisionRect = Rectangle.Intersect(level[i, j].Rect, rect);

                        //player is hitting the top or bottom of a tile
                        if (collisionRect.Width > collisionRect.Height)
                        {
                            //player is landing on a tile
                            if (rect.Y <= level[i, j].Rect.Y)
                            {
                                //player is on the ground
                                isGrounded = true;
                                //player is not stuck in the tile
                                AdjustPosition(level, -collisionRect.Height, false, rows, columns);
                            }
                            //player is hitting the bottom of a tile with their head
                            else
                            {
                                //player has a light bounce off of the tile
                                yVelocity = -1;
                                //player is not stuck in the tile
                                AdjustPosition(level, collisionRect.Height, false, rows, columns);
                            }
                            
                        }
                        //player is hitting the side of a tile
                        if (Math.Abs(collisionRect.Height) > Math.Abs(collisionRect.Width))
                        {
                            //player is on the right side of the tile, cannot move left
                            if (rect.X + 100 > level[i, j].Rect.X)
                            {
                                touchingLeftWall = true;
                                //player is not stuck in the tile
                                AdjustPosition(level, collisionRect.Width + 1, true, rows, columns);
                            }
                            //player is on the left side of the tile, cannot move right
                            else if (rect.X + 100 <= level[i, j].Rect.X)
                            {
                                touchingRightWall = true;
                                //player is not stuck in the tile
                                AdjustPosition(level, -collisionRect.Width - 1, true, rows, columns);
                            }
                        }
                        debugText = collisionRect.Width + ", " + collisionRect.Height;
                    }
                }
            }
        }

        private void AdjustPosition(Room[,] level, int distance, bool isHorizontal, int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (isHorizontal)
                    {
                        level[i, j].RectX -= distance;
                    }
                    else
                    {
                        level[i, j].RectY -= distance + 1;
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Asset, rect, Color.White);
            sb.DrawString(debugFont, isGrounded + ", " + touchingLeftWall + ", " + touchingRightWall + 
                ", "  + debugText, 
                new Vector2(100, 100), Color.Red);
        }

    }
}
