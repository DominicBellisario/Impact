using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        //wether or not the player is standing on a surface
        private bool isGrounded;

        //simulates gravity
        private const double gravity = -.2;

        //very fast horizontal acceleration when player begins to walk
        private const double walkAccel = 20;

        //less fast horizonal accelertaion while player is in the air
        private const double airAccel = 5;

        //max speeds in the x and y directions
        private const int maxXSpeed = 10;
        private const int maxYSpeed = -30;

        //player asset
        private Texture2D asset;

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

        public Player(double xPos, double yPos, Texture2D asset)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.asset = asset;
            xVelocity = 0;
            yVelocity = 0;
            isGrounded = false;
            rect = new Rectangle((int)xPos - 100, (int)yPos - 100, 200, 200);
        }

        /// <summary>
        /// controls player physics
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
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
            //player is touching the ground, velocity is 0
            else
            {
                yVelocity = 0;
            }
        }

        /// <summary>
        /// checks tiles to see if they collide with the player
        /// </summary>
        /// <param name="level"></param>
        public void Collisions(Room[,] level, int rows, int columns)
        {
            Rectangle collisionRect;
            isGrounded = false;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    //makes sure only near blocks that have collision are taken into account
                    if(Math.Abs(level[i, j].Rect.X - rect.X) < 400 &&
                        Math.Abs(level[i, j].Rect.Y - rect.Y) < 400 && level[i, j].CanCollide)
                    {
                        //creates a rectangle of the overlaping area
                        collisionRect = Rectangle.Intersect(level[i, j].Rect, rect);

                        //player is not rubbing against a wall
                        if(collisionRect.X > collisionRect.Y)
                        {
                            //player is not hitting the bottom of a tile
                            if(rect.Y < level[i, j].Rect.Y)
                            {
                                isGrounded = true;
                            }
                            else
                            {
                                yVelocity = 0;
                            }
                            
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Asset, rect, Color.White);
        }

    }
}
