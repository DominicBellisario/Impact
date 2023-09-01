using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{
    internal class Enemy
    {
        //how fast the enemy moves
        private int speed;

        //the aggro radius for the enemy
        private int aggroRadius;

        //wether or not the enemy is walking
        private bool isWalking;

        //enemy hitbox
        private Rectangle hitbox;

        //rectangles that will be made with coords above
        private Rectangle leftRect;
        private Rectangle rightRect;

        //player does not move on screen, coords are constant
        private const int playerXPos = 1920;
        private const int playerYPos = 1080;

        //walking sprite sheet
        private Texture2D walkingSpriteSheet;

        //shooting sprite sheet
        private Texture2D shootingSpriteSheet;

        //animation
        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame
        const int WalkFrameCount = 3;       // The number of frames in the animation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="aggroRadius"></param>
        /// <param name="hitbox"></param>
        /// <param name="leftX"></param>
        /// <param name="leftY"></param>
        /// <param name="rightX"></param>
        /// <param name="rightY"></param>
        public Enemy(int speed, int aggroRadius, int xPos, int yPos, int leftX, int leftY, 
            int rightX, int rightY, Texture2D walkingSpriteSheet, Texture2D shootingSpriteSheet)
        {
            this.speed = speed;
            this.aggroRadius = aggroRadius;
            this.walkingSpriteSheet = walkingSpriteSheet;
            this.shootingSpriteSheet = shootingSpriteSheet;

            //enemy begins by walking
            isWalking = true;

            //creates edge rectangles
            leftRect = new Rectangle(leftX, leftY, 100, 100);
            rightRect = new Rectangle(rightX, rightY, 100, 100);

            //creates hitbox
            hitbox = new Rectangle(xPos, yPos, 300, 300);

            //3 animation frames/second
            fps = 3.0;
            timePerFrame = 1.0 / fps;
        }

        /// <summary>
        /// updates enemy position
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="xVelocity"></param>
        /// <param name="yVelocity"></param>
        public void Update(GameTime gameTime, int xVelocity, int yVelocity)
        {
            //actions that take place when the enemy is walking
            if (isWalking)
            {
                //updates enemy position and edge position when player moves
                hitbox.X += xVelocity;
                hitbox.Y += yVelocity;
                leftRect.X += xVelocity;
                leftRect.Y += yVelocity;
                rightRect.X = xVelocity;
                rightRect.Y = yVelocity;

                //enemy walks the other way if they hit an edge
                if (CollidingWithEdge())
                {
                    speed = -speed;
                }

                //move the enemy
                hitbox.X += speed;

                //switch to shooting mode if player is in radius
                if (PlayerInAggroRange())
                {
                    isWalking = false;
                }

                //update animations
                UpdateAnimation(gameTime);
            }

            //actions that take place while enemy is shooting
            else
            {
                //updates enemy position and edge position when player moves
                hitbox.X += xVelocity;
                hitbox.Y += yVelocity;
                leftRect.X += xVelocity;
                leftRect.Y += yVelocity;
                rightRect.X = xVelocity;
                rightRect.Y = yVelocity;

                //begin walking again if the player leaves radius
                if (!PlayerInAggroRange())
                {
                    isWalking = true;
                }
            }
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            // Handle animation timing
            // - Add to the time counter
            // - Check if we have enough "time" to advance the frame

            // How much time has passed?  
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // If enough time has passed:
            if (timeCounter >= timePerFrame)
            {
                frame += 1;                     // Adjust the frame to the next image

                if (frame > WalkFrameCount)     // Check the bounds - have we reached the end of walk cycle?
                    frame = 1;                  // Back to 1 (since 0 is the "standing" frame)

                timeCounter -= timePerFrame;    // Remove the time we "used" - don't reset to 0
                                                // This keeps the time passed 
            }
        }

        public void Draw(SpriteBatch sb)
        {
            //draws walking sprites when walking
            if (isWalking || !isWalking)
            {
                //enemy is waling towards the right
                if (speed > 0)
                {
                    sb.Draw(
                        walkingSpriteSheet,
                        new Vector2(hitbox.X, hitbox.Y),
                        new Rectangle(
                            frame * hitbox.Width,
                            0,
                            hitbox.Width,
                            hitbox.Height),
                        Color.White,
                        0,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        0);
                }
                //walking towards the left
                else
                {
                    sb.Draw(
                        walkingSpriteSheet,
                        new Vector2(hitbox.X, hitbox.Y),
                        new Rectangle(
                            frame * hitbox.Width,
                            0,
                            hitbox.Width,
                            hitbox.Height),
                        Color.White,
                        0,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.FlipHorizontally,
                        0);
                }

            }
        }

        /// <summary>
        /// checks to see if the enemy is collding with the left or right path edge
        /// </summary>
        /// <returns></returns>
        private bool CollidingWithEdge()
        {
            if (hitbox.Intersects(leftRect) || hitbox.Intersects(rightRect))
            {
                return true;
            }

            return false;
        }

        private bool PlayerInAggroRange()
        {
            //calculates x and y distances between player and enemy
            int xDistance = Math.Abs(playerXPos - hitbox.Center.X);
            int yDistance = Math.Abs(playerYPos - hitbox.Center.Y);

            //finds total distance using pythag
            int distance = (int)Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));

            //enemy is aggro if the player is inside radius
            if (distance <= aggroRadius)
            {
                return true;
            }

            return false;
        }
    }
}
