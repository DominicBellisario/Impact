using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainProject
{
    internal class Player
    {
        //counts to 1000, then repeats
        private int timer;

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
        private bool currentlyJumping;
        private bool canDoubleJump;

        #region general speeds and accelerations
        //initial y jump speed
        private const int jumpSpeedY = 30;

        //initial x jump speed
        private const int jumpSpeedX = 20;

        //simulates gravity
        private const double gravity = -1.5;

        //very fast horizontal acceleration when player begins to walk
        private const double walkAccel = 2;

        //less fast horizonal accelertaion while player is in the air
        private const double airAccel = 1;

        //max speeds in the x and y directions
        private const int maxXGroundSpeed = 20;
        private const int maxXAirSpeed = 30;
        private const int maxYSpeed = -50;

        //deceleration when space bar is released during a jump
        private const int jumpDecceleration = 3;
        #endregion

        #region spring variables
        //left or right velocity when hitting a horizontal spring
        private const int xSpringXVelocity = 30;

        //up velocity when hitting a horizontal spring
        private const int xSpringYVelocity = 20;

        //up velocity when hitting a vertical spring
        private const int ySpringYVelocity = 50;
        #endregion

        #region tube variables
        //helps the tubes accelerate the player slower
        private int tubeAccelerationChance;

        //turns off gravity, horizontal controls, and ground pound when in a horizontal tube
        private bool inHTube;

        //turns off ground pound and double jump when in a verical tube
        private bool inVTube;

        //timer that disables a beam to allow the player to jump while in it
        private int tubeDisableTimer;

        //when true, player is attempting to leave the beam and the center attraction stops
        private bool playerWantsOut;

        //max speed for beams
        private const int maxBeamSpeed = 70;

        //keeps track of the direction that the tubes take the player
        private bool normalTube;

        #endregion 

        private bool onIce;

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

        public bool NormalTube
        {
            get { return normalTube; }
        }

        public Player(double xPos, double yPos, Texture2D asset, SpriteFont debugFont)
        {
            timer = 0;
            this.xPos = xPos;
            this.yPos = yPos;
            this.asset = asset;
            this.debugFont = debugFont;
            xVelocity = 0;
            yVelocity = 0;
            isGrounded = false;
            touchingLeftWall = false;
            touchingRightWall = false;
            currentlyJumping = false;
            canDoubleJump = false;
            rect = new Rectangle((int)xPos - 100, (int)yPos - 100, 200, 200);
            tubeAccelerationChance = 0;
            inHTube = false;
            inVTube = false;
            //60 frames before tube is reactivated
            tubeDisableTimer = 60;
            playerWantsOut = false;
            normalTube = true;
            onIce = false;
        }

        /// <summary>
        /// controls player physics
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //timer counting
            timer++;
            if (timer == 1000)
            {
                timer = 0;
            }

            KeyboardState kbState = Keyboard.GetState();

            //pressing the E key reverses every tube's direction
            if (kbState.IsKeyDown(Keys.E) && prevKBState.IsKeyUp(Keys.E))
            {
                normalTube = !normalTube;
            }

            //---------------------- motion in the Y direction -----------------------

                //DOUBLE JUMP
                //player jumps while airborne without jumping previously
            if (canDoubleJump && kbState.IsKeyDown(Keys.Space) && prevKBState.IsKeyUp(Keys.Space) && !isGrounded)
            {
                //player can not jump again
                canDoubleJump = false;
                //player jumps right
                if (kbState.IsKeyDown(Keys.D) && kbState.IsKeyUp(Keys.A) && !inHTube && !inVTube)
                {
                    xVelocity = -jumpSpeedX;
                    yVelocity = jumpSpeedY;
                }
                //player jumps left
                else if (kbState.IsKeyUp(Keys.D) && kbState.IsKeyDown(Keys.A) && !inHTube && !inVTube)
                {
                    xVelocity = jumpSpeedX;
                    yVelocity = jumpSpeedY;
                }
                //player jumps straight up
                else if ((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) ||
                (kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D)) && !inVTube)
                {
                    //starts the timer to disable the tube
                    if (inHTube)
                    {
                        tubeDisableTimer = 0;
                    }
                    yVelocity = jumpSpeedY;
                }

            }

            //player accelerates downward if not touching the ground, not at max speed, and not in a tube
            if (!isGrounded && yVelocity > maxYSpeed && !inHTube && !inVTube)
            {
                yVelocity += gravity;
            }
            //player reaches terminal velocity, no acceleration
            else if (!isGrounded && yVelocity <= maxYSpeed && !inHTube && !inVTube)
            {
                yVelocity = maxYSpeed;
            }
            //player is touching the ground
            else
            {
                //player jumps up if the space key is pressed
                if (kbState.IsKeyDown(Keys.Space) && prevKBState.IsKeyUp(Keys.Space) &&!inHTube && !inVTube)
                {
                    yVelocity = jumpSpeedY + 5;
                    currentlyJumping = true;
                }
                //otherwise, player rests on the ground
                else
                {
                    currentlyJumping = false;
                    canDoubleJump = true;
                }
            }
            //the player can release the space button in the middle of a jump to jump less
            if (currentlyJumping && kbState.IsKeyUp(Keys.Space) && yVelocity >= 0)
            {
                yVelocity = yVelocity / jumpDecceleration;
                //jump ends
                currentlyJumping = false;
            }

            //-----------------------------motion in the X direction ---------------------------
            //player moves left if a is pressed, d is not pressed,
            //the player is not blocked, and they are not at max speed
            if (kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D) && 
                !touchingLeftWall && xVelocity <= maxXGroundSpeed)
            {
                if (!inHTube)
                {
                    //player accelerates slightly faster on the ground than in the air
                    if (isGrounded)
                    {
                        xVelocity += walkAccel;
                    }
                    else
                    {
                        xVelocity += airAccel;
                    }
                }
                if (inVTube)
                {
                    //turns off beam center pull so player can escape
                    playerWantsOut = true;
                }
            }
            //player moves right if d is pressed, a is not pressed,
            //the player is not blocked, and they are not at max speed
            if (kbState.IsKeyDown(Keys.D) && kbState.IsKeyUp(Keys.A) &&
                !touchingRightWall && xVelocity >= -maxXGroundSpeed)
            {
                if (!inHTube)
                {
                    //player accelerates slightly faster on the ground than in the air
                    if (isGrounded)
                    {
                        xVelocity -= walkAccel;
                    }
                    else
                    {
                        xVelocity -= airAccel;
                    }
                    
                }
                if (inVTube)
                {
                    //turns off beam center pull so player can escape
                    playerWantsOut = true;
                }
            }

            //if none or both "a" and "d" are pressed, and the player is not on ice, decelerate the player to 0
            if (((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) ||
                (kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D))) && isGrounded && !onIce)
            {
                if (!inHTube && !inVTube)
                {
                    if (xVelocity > 2)
                    {
                        xVelocity -= walkAccel;
                    }
                    else if (xVelocity < -2)
                    {
                        xVelocity += walkAccel;
                    }
                    else
                    {
                        xVelocity = 0;
                    }
                }
                //turns back on beam center pull
                playerWantsOut = false;
            }

            //if in the air, deceleration is slower
            else if (((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) ||
                (kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D))) && !isGrounded)
            {
                /* reactivate if natural air deceleration is needed
                if (!inHTube && !inVTube && timer % 100 == 0)
                {
                    if (xVelocity > 0)
                    {
                        xVelocity -= airAccel;
                    }
                    else if (xVelocity < 0)
                    {
                        xVelocity += airAccel;
                    }
                }
                */
                //turns back on beam center pull
                playerWantsOut = false;
            }

            //if speed exceeds max speed, such as with a spring, slow down faster than normal (except for tubes)
            else if (xVelocity > maxXGroundSpeed && !inHTube && !inVTube && isGrounded && !onIce)
            {
                xVelocity -= airAccel;
            }
            else if (xVelocity < -maxXGroundSpeed && !inHTube && !inVTube && isGrounded && !onIce)
            {
                xVelocity += airAccel;
            }
            else if (xVelocity > maxXAirSpeed && !inHTube && !inVTube && !isGrounded && timer % 3 == 0)
            {
                xVelocity -= airAccel;
            }
            else if (xVelocity < -maxXAirSpeed && !inHTube && !inVTube && !isGrounded && timer % 3 == 0)
            {
                xVelocity += airAccel;
            }
            //updates prev keyboard state
            prevKBState = kbState;
            //resets ice trigger
            onIce = false;
            
        }

        /// <summary>
        /// checks tiles to see if they collide with the player
        /// </summary>
        /// <param name="level"></param>
        public void Collisions(Room[,] bgLevel, Room[,] intLevel, int rows, int columns, 
            List<Enemy> enemies)
        {
            bool isColliding;
            bool collidingWithSpring;
            Rectangle collisionRect;
            bool hitHTube;
            bool hitVTube;
            //reset all collisions
            isGrounded = false;
            touchingLeftWall = false;
            touchingRightWall = false;
            collidingWithSpring = false;
            hitHTube = false;
            hitVTube = false;


            debugText = "0, 0";

            //----------collisions with background layer----------

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    //makes sure only near blocks that have collision are taken into account
                    if (Math.Abs(bgLevel[i, j].Rect.X - rect.X) < 400 &&
                        Math.Abs(bgLevel[i, j].Rect.Y - rect.Y) < 400 && bgLevel[i, j].CanCollide)
                    {
                        //creates a rectangle of the overlaping area
                        collisionRect = Rectangle.Intersect(bgLevel[i, j].Rect, rect);
                        
                        //player is hitting the top or bottom of a tile while not hitting a spring
                        if (collisionRect.Width > collisionRect.Height && (bgLevel[i, j].TypeOfCollision == "surface" 
                            || bgLevel[i, j].TypeOfCollision == "ice"))
                        {
                            //player is landing on a tile
                            if (rect.Y <= bgLevel[i, j].Rect.Y)
                            {
                                //player is on the ground
                                isGrounded = true;
                                //player is not stuck in the tile
                                AdjustEnemyPosition(-collisionRect.Height, false, enemies);
                                AdjustPosition(bgLevel, -collisionRect.Height, false, rows, columns);
                                AdjustPosition(intLevel, -collisionRect.Height, false, rows, columns);
                                
                            }
                            //player is hitting the bottom of a tile with their head
                            else
                            {
                                //player has a light bounce off of the tile
                                yVelocity = -1;
                                //player is not stuck in the tile
                                AdjustEnemyPosition(collisionRect.Height, false, enemies);
                                AdjustPosition(bgLevel, collisionRect.Height, false, rows, columns);
                                AdjustPosition(intLevel, collisionRect.Height, false, rows, columns);
                                
                            }

                            //triggers if the player is hitting ice
                            if (bgLevel[i, j].TypeOfCollision == "ice")
                            {
                                onIce = true;
                            }
                        }

                        //player is hitting the side of a tile and not touching a spring
                        else if (Math.Abs(collisionRect.Height) > Math.Abs(collisionRect.Width) 
                            && (bgLevel[i, j].TypeOfCollision == "surface"/* || 
                            bgLevel[i, j].TypeOfCollision == "ice"*/) && !collidingWithSpring)
                        {
                            //player is on the right side of the tile, cannot move left
                            if (rect.X + 100 > bgLevel[i, j].Rect.X)
                            {
                                touchingLeftWall = true;
                                //player is not stuck in the tile
                                AdjustEnemyPosition(collisionRect.Width, true, enemies);
                                AdjustPosition(bgLevel, collisionRect.Width, true, rows, columns);
                                AdjustPosition(intLevel, collisionRect.Width, true, rows, columns);
                                
                            }
                            //player is on the left side of the tile, cannot move right
                            else if (rect.X + 100 <= bgLevel[i, j].Rect.X)
                            {
                                touchingRightWall = true;
                                //player is not stuck in the tile
                                AdjustEnemyPosition(-collisionRect.Width, true, enemies);
                                AdjustPosition(bgLevel, -collisionRect.Width, true, rows, columns);
                                AdjustPosition(intLevel, -collisionRect.Width, true, rows, columns);
                                
                            }
                        }
                    }
                }
            }

            //----------interactbale collisions----------

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    //makes sure only near blocks that have collision are taken into account
                    if (Math.Abs(intLevel[i, j].Rect.X - rect.X) < 400 &&
                        Math.Abs(intLevel[i, j].Rect.Y - rect.Y) < 400 && intLevel[i, j].CanCollide)
                    {
                        //creates a rectangle of the overlaping area
                        //collisionRect = Rectangle.Intersect(intLevel[i, j].Rect, rect);
                        isColliding = rect.Intersects(intLevel[i, j].Rect);

                        //player is hiting a left spring
                        if (intLevel[i, j].TypeOfCollision == "leftSpring" && isColliding)
                        {
                            //launches player left and a bit up
                            xVelocity = xSpringXVelocity;
                            yVelocity = xSpringYVelocity;
                            //resets double jump
                            canDoubleJump = true;
                        }

                        //player is hiting a right spring
                        else if (intLevel[i, j].TypeOfCollision == "rightSpring" && isColliding)
                        {
                            //launches player right and a bit up
                            xVelocity = -xSpringXVelocity;
                            yVelocity = xSpringYVelocity;
                            //resets double jump
                            canDoubleJump = true;
                        }

                        //player is hiting an up spring
                        else if (intLevel[i, j].TypeOfCollision == "upSpring" && isColliding)
                        {
                            //lauches player upwards and resets x momentum
                            yVelocity = ySpringYVelocity;
                            xVelocity = 0;
                            //resets double jump
                            canDoubleJump = true;
                        }

                        //player is hiting an up tube
                        else if (intLevel[i, j].TypeOfCollision == "upTube" && isColliding)
                        {
                            //centers x momentum if player is not attempting to leave beam 
                            if (!playerWantsOut)
                            {
                                xVelocity = CenterPlayerInTube(intLevel[i, j].Rect, rect, false);
                            }
                            else
                            {
                                xVelocity = 0;
                            } 

                            //resets double jump
                            canDoubleJump = true;
                            //on the 3rd frame, accelerate by 1
                            if (tubeAccelerationChance % 3 == 0 && Math.Abs(yVelocity) <= maxBeamSpeed)
                            {
                                if (normalTube)
                                {
                                    yVelocity++;
                                }
                                else
                                {
                                    yVelocity--;
                                }
                                
                            }
                            tubeAccelerationChance += 1;
                            hitVTube = true;
                        }

                        //player is hiting a down tube
                        else if (intLevel[i, j].TypeOfCollision == "downTube" && isColliding)
                        {
                            //centers x momentum if player is not attempting to leave beam 
                            if (!playerWantsOut)
                            {
                                xVelocity = CenterPlayerInTube(intLevel[i, j].Rect, rect, false);
                            }
                            else
                            {
                                xVelocity = 0;
                            }
                            //resets double jump
                            canDoubleJump = true;
                            //on the 3rd frame, accelerate by 1
                            if (tubeAccelerationChance % 3 == 0 && Math.Abs(yVelocity) <= maxBeamSpeed)
                            {
                                if (normalTube)
                                {
                                    yVelocity--;
                                }
                                else
                                {
                                    yVelocity++;
                                }
                            }
                            tubeAccelerationChance += 1;
                            hitVTube = true;
                        }

                        //player is hiting a left tube
                        else if (intLevel[i, j].TypeOfCollision == "leftTube" && isColliding
                            && tubeDisableTimer == 60)
                        {
                            //centers y momentum if player is not attempting to leave beam 
                            if (!playerWantsOut)
                            {
                                yVelocity = CenterPlayerInTube(intLevel[i, j].Rect, rect, true);
                            }
                            else
                            {
                                yVelocity = 0;
                            }
                                
                            //resets double jump
                            canDoubleJump = true;
                            //on the 3rd frame, accelerate by 1
                            if (tubeAccelerationChance % 3 == 0 && Math.Abs(xVelocity) <= maxBeamSpeed)
                            {
                                if (normalTube)
                                {
                                    xVelocity++;
                                }
                                else
                                {
                                    xVelocity--;
                                }
                            }
                            tubeAccelerationChance += 1;
                            hitHTube = true;
                        }

                        //player is hiting a right tube
                        else if (intLevel[i, j].TypeOfCollision == "rightTube" && isColliding
                            && tubeDisableTimer == 60)
                        {
                            //centers y momentum if player is not attempting to leave beam 
                            if (!playerWantsOut)
                            {
                                yVelocity = CenterPlayerInTube(intLevel[i, j].Rect, rect, true);
                            }
                            else
                            {
                                yVelocity = 0;
                            }
                                
                            //resets double jump
                            canDoubleJump = true;
                            //on the 3rd frame, accelerate by 1
                            if (tubeAccelerationChance % 3 == 0 && Math.Abs(xVelocity) <= maxBeamSpeed)
                            {
                                if (normalTube)
                                {
                                    xVelocity--;
                                }
                                else
                                {
                                    xVelocity++;
                                }
                            }
                            tubeAccelerationChance += 1;
                            hitHTube = true;
                        }

                        //count up if the timer is not at its final value
                        if (tubeDisableTimer != 60)
                        {
                            tubeDisableTimer++;
                        }
                    }
                        
                }
            }
            //if the player touched a tube at all, inTube = true;
            inHTube = hitHTube;
            inVTube = hitVTube;
            
            //------ enemy / bullet collisions -------
            foreach (Enemy e in enemies)
            {
                isColliding = rect.Intersects(e.Hitbox);
                collisionRect = Rectangle.Intersect(rect, e.Hitbox);
                if (isColliding)
                {
                        double launchAngle = Math.Atan2(e.Hitbox.Center.Y - rect.Center.Y,
                            e.Hitbox.Center.X - rect.Center.X);

                        //update player x and y velocity
                        xVelocity = 30 * Math.Cos(launchAngle);
                        yVelocity = 30 * Math.Sin(launchAngle);
                        //player loses double jump
                        canDoubleJump = false;
                }
                //goes through the list of bullets for each enemy
                foreach (Bullet b in e.Bullets)
                {
                    //collision
                    if (b.Hitbox.Intersects(rect))
                    {
                        //use the width and height in arctan to find the angle
                        double launchAngle = Math.Atan2(b.Hitbox.Center.Y - rect.Center.Y, 
                            b.Hitbox.Center.X - rect.Center.X);

                        //update player x and y velocity
                        xVelocity = 50 * Math.Cos(launchAngle);
                        yVelocity = 50 * Math.Sin(launchAngle);

                        //remove the bullet from its list
                        e.Bullets.Remove(b);
                        return;
                    }
                }
            }
        }
        
        /// <summary>
        /// performs a slight correction when player hits a 
        /// collidable surface to prevent the player getting stuck
        /// </summary>
        /// <param name="level"></param>
        /// <param name="distance"></param>
        /// <param name="isHorizontal"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        private void AdjustPosition(Room[,] level, int distance, bool isHorizontal, 
            int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (isHorizontal && xVelocity > 0)
                    {
                        level[i, j].RectX -= distance + 1;
                        xVelocity = 0;
                    }
                    else if (isHorizontal && xVelocity <= 0)
                    {
                        level[i, j].RectX -= distance - 1;
                        xVelocity = 0;
                    }
                    else
                    {
                        level[i, j].RectY -= distance + 1;
                        yVelocity = 0;
                    }
                }
            }
        }

        private void AdjustEnemyPosition(int distance, bool isHorizontal, List<Enemy> enemies)
        {
            foreach (Enemy e in enemies)
            {
                if (isHorizontal && xVelocity > 0)
                {
                    e.AdjustmentX += distance + 1;
                }
                else if (isHorizontal && xVelocity <= 0)
                {
                    e.AdjustmentX += distance - 1;
                }
                else
                {
                    e.AdjustmentY += distance + 1;
                }
            }
        }

        /// <summary>
        /// slowly moves the player towards the center of the tube in an attempt to center it
        /// </summary>
        /// <param name="tubeRect"></param>
        /// <param name="playerRect"></param>
        /// <param name="isHorizontal"></param>
        /// <returns> a value that will be added to the current x or y velocity </returns>
        private int CenterPlayerInTube(Rectangle tubeRect, Rectangle playerRect, bool isHorizontal)
        {
            //speed that the player will be corrected
            const int correctionStrength = 5;

            //value stays at 0 if player is already in center
            int velocityUpdate = 0;

            //changes the x velocity if in a vertical tube
            if (!isHorizontal)
            {
                //moves left if center is to the right of tube
                if (playerRect.Center.X > tubeRect.Center.X)
                {
                    velocityUpdate = correctionStrength;
                }
                //right if center is to the left
                else if (playerRect.Center.X < tubeRect.Center.X)
                {
                    velocityUpdate = -correctionStrength;
                }
            }
            //changes the y velocity if in a horizontal tube
            else
            {
                //moves up if center is below the tube
                if (playerRect.Center.Y > tubeRect.Center.Y)
                {
                    velocityUpdate = correctionStrength;
                }
                //down if center is above
                else if (playerRect.Center.Y < tubeRect.Center.Y)
                {
                    velocityUpdate = -correctionStrength;
                }
            }
            return velocityUpdate;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Asset, rect, Color.White);
            /*
            sb.DrawString(debugFont, isGrounded + ", " + touchingLeftWall + ", " + touchingRightWall + 
                ", "  + debugText + ", " + xVelocity + ", " + normalTube, 
                new Vector2(100, 100), Color.Red);
            */
        }

    }
}
