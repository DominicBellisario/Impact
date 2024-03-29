﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainProject
{
    enum AnimationState
    {
        Idle,
        Walking,
        Jumping,
        Hurt,
        Floating,
        Hard
    }
    internal class Player
    {
        //counts to 1000, then repeats
        private int timer;

        //position of the player
        private double xPos;
        private double yPos;

        //player hitboxes
        private Rectangle rect;
        private Rectangle collRect;

        //current speed of the player
        private double xVelocity;
        private double yVelocity;

        //booleans that check if the player is touching or being blocked by a surface
        private bool isGrounded;
        private bool touchingLeftWall;
        private bool touchingRightWall;
        private bool currentlyJumping;
        private bool canDoubleJump;

        //frames that the player is stunned after getting hit
        private const int framesStunned = 35;
        private int currentStunFrame;
        private bool isStunned;

        //the current player mode
        private bool hard;

        //spawn point for the player if they die
        private Vector2 spawnPoint;
        private bool spawning;
        private bool done;

        //list of keys in the level
        private List<Key> keys;
        private List<Key> collectedKeys;

        //when all keys in the list are gone, player can exit
        private bool exitOpen;

        #region general speeds and accelerations
        //initial y jump speed
        private const int jumpSpeedY = 30;

        //initial x jump speed
        private const int jumpSpeedX = 20;

        //simulates gravity
        private const double gravity = -1.5;

        //higher gravity when hard
        private const double hardGravity = -2;

        //very fast horizontal acceleration when player begins to walk
        private const double walkAccel = 2;

        //slower when hard
        private const double hardWalkAccel = 1;

        //less fast horizonal accelertaion while player is in the air
        private const double airAccel = 1;

        //max speeds in the x and y directions
        private const int maxXGroundSpeed = 20;
        private const int maxXAirSpeed = 30;
        private const int maxYSpeed = -50;
        //less x and more y
        private const int maxHardXGroundSpeed = 10;
        private const int maxHardYSpeed = -60;

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

        #region animation stuff
        //player assets
        private Texture2D asset;
        //idle
        private Texture2D idle;
        //walking
        private Texture2D walking;
        //jumping
        private Texture2D jumping;
        //hurt
        private Texture2D hurt;
        //floating
        private Texture2D floating;
        //explosion
        private Texture2D explosion;

        //animation stuff
        AnimationState animState;
        private int frame;
        // The amount of time that has passed
        private double timeCounter;
        // The speed of the animation
        private const double fpsIdle = 4;
        private const double fpsWalk = 12;
        private const double fpsJump = 6;
        private const double fpsHurt = 10;
        private const double fpsFloat = 10;
        // The amount of time (in fractional seconds) per frame
        private double timePerIdleFrame;
        private double timePerWalkFrame;
        private double timePerJumpFrame;
        private double timePerHurtFrame;
        private double timePerFloatFrame;
        // The number of frames in the animation
        private const int IdleFrameCount = 4;
        private const int WalkFrameCount = 8;
        private const int JumpFrameCount = 2;
        private const int HurtFrameCount = 2;
        private const int FloatFrameCount = 8;
        //keeps track of what direction the player is facing
        private bool lookingRight;
        #endregion

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

        public List<Key> CollectedKeys
        {
            get { return collectedKeys; }
        }

        public bool ExitOpen
        {
            get { return exitOpen; }
        }

        public Player(double xPos, double yPos, Texture2D asset, Texture2D idle, Texture2D walking,
            Texture2D jumping, Texture2D hurt, Texture2D floating, Texture2D explosion, List<Key> keys, bool lookingRight, SpriteFont debugFont)
        {
            timer = 0;
            this.xPos = xPos;
            this.yPos = yPos;
            this.asset = asset;
            this.debugFont = debugFont;
            this.idle = idle;
            this.walking = walking;
            this.jumping = jumping;
            this.hurt = hurt;
            this.floating = floating;
            this.explosion = explosion;
            this.keys = keys;
            this.lookingRight = lookingRight;
            collectedKeys = new List<Key>();
            xVelocity = 0;
            yVelocity = 0;
            isGrounded = false;
            touchingLeftWall = false;
            touchingRightWall = false;
            currentlyJumping = false;
            canDoubleJump = false;
            rect = new Rectangle((int)xPos - 100, (int)yPos - 100, 200, 200);
            collRect = new Rectangle((int)xPos - 50, (int)yPos - 75, 100, 175);
            tubeAccelerationChance = 0;
            inHTube = false;
            inVTube = false;
            //60 frames before tube is reactivated
            tubeDisableTimer = 60;
            playerWantsOut = false;
            normalTube = true;
            onIce = false;
            hard = false;
            //starts at idle on the first frame
            animState = AnimationState.Idle;
            frame = 0;
            timePerIdleFrame = 1 / fpsIdle;
            timePerWalkFrame = 1 / fpsWalk;
            timePerJumpFrame = 1 / fpsJump;
            timePerHurtFrame = 1 / fpsHurt;
            timePerFloatFrame = 1 / fpsFloat;
            currentStunFrame = -1;
            isStunned = false;
            spawning = false;
            done = false;
            //exit is closed at first
            exitOpen = false;
        }

        /// <summary>
        /// controls player physics
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, List<Enemy> enemies)
        {
            //timer counting
            timer++;
            if (timer == 1000)
            {
                timer = 0;
            }

            if (done)
            {
                //stops player movement
                xVelocity = 0;
                yVelocity = 0;
                done = false;
            }
            //reset everything if spawning
            if (spawning)
            {
                spawning = false;
                done = true;
                //move player to spawnpoint
                xVelocity = rect.X - spawnPoint.X;
                yVelocity = rect.Y - spawnPoint.Y;
                //remove all bullets from screen
                foreach (Enemy e in enemies)
                {
                    e.Bullets.Clear();
                }
                //normal player
                hard = false;
                //respawn all keys
                foreach (Key k in collectedKeys)
                {
                    keys.Add(k);
                }
                //no collected keys
                collectedKeys.Clear();
                exitOpen = false;
                return;
            }

            //if there are no more keys in the level, the exit opens
            if (keys.Count == 0)
            {
                exitOpen = true;
            }

            KeyboardState kbState = Keyboard.GetState();

            //pressing the E key reverses every tube's direction
            if (kbState.IsKeyDown(Keys.E) && prevKBState.IsKeyUp(Keys.E))
            {
                normalTube = !normalTube;
            }

            //pressing the SHIFT key goes between hard and normal player
            if (kbState.IsKeyDown(Keys.LeftShift) && prevKBState.IsKeyUp(Keys.LeftShift) && !isStunned)
            {
                hard = !hard;
            }

            //---------------------- motion in the Y direction -----------------------

                //DOUBLE JUMP
                //player jumps while airborne without jumping previously
            if (canDoubleJump && kbState.IsKeyDown(Keys.Space) && prevKBState.IsKeyUp(Keys.Space) && !isGrounded)
            {
                //player can not jump again
                canDoubleJump = false;
                //player jumps right
                if (kbState.IsKeyDown(Keys.D) && kbState.IsKeyUp(Keys.A) && !inHTube && !inVTube && !hard)
                {
                    xVelocity = -jumpSpeedX;
                    yVelocity = jumpSpeedY;
                    frame = 0;
                }
                //player jumps left
                else if (kbState.IsKeyUp(Keys.D) && kbState.IsKeyDown(Keys.A) && !inHTube && !inVTube && !hard)
                {
                    xVelocity = jumpSpeedX;
                    yVelocity = jumpSpeedY;
                    frame = 0;

                }
                //player jumps straight up
                else if ((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) ||
                (kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D)) && !inVTube && !hard)
                {
                    //starts the timer to disable the tube
                    if (inHTube)
                    {
                        tubeDisableTimer = 0;
                    }
                    yVelocity = jumpSpeedY;
                    frame = 0;

                }

            }

            //player accelerates downward if not touching the ground, not at max speed, and not in a tube
            if (!isGrounded && yVelocity > maxYSpeed && !inHTube && !inVTube && !hard)
            {
                yVelocity += gravity;
            }
            //faster if hard
            else if (!isGrounded && yVelocity > maxYSpeed && !inHTube && !inVTube && hard)
            {
                yVelocity += hardGravity;
            }
            //player reaches terminal velocity, no acceleration
            else if (!isGrounded && yVelocity <= maxYSpeed && !inHTube && !inVTube && !hard)
            {
                yVelocity = maxYSpeed;
            }
            //higher terminal velocity if hard
            else if (!isGrounded && yVelocity <= maxYSpeed && !inHTube && !inVTube && hard)
            {
                yVelocity = maxHardYSpeed;
            }
            //player is touching the ground
            else
            {
                //remove any possible stun from player if not in a tube
                if (!inHTube && !inVTube)
                {
                    isStunned = false;
                }
                

                //player jumps up if the space key is pressed
                if (kbState.IsKeyDown(Keys.Space) && prevKBState.IsKeyUp(Keys.Space) &&!inHTube && !inVTube && !hard)
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
                !touchingLeftWall && xVelocity <= maxXGroundSpeed && !hard)
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
            //max speed is less when hard
            else if (kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D) &&
                !touchingLeftWall && xVelocity <= maxHardXGroundSpeed && hard)
            {
                if (!inHTube)
                {
                    //player accelerates slightly faster on the ground than in the air
                    //slower when hard
                    if (isGrounded)
                    {
                        xVelocity += hardWalkAccel;
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
                !touchingRightWall && xVelocity >= -maxXGroundSpeed && !hard)
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
            //max speed is less when hard
            else if (kbState.IsKeyDown(Keys.D) && kbState.IsKeyUp(Keys.A) &&
                !touchingRightWall && xVelocity >= -maxHardXGroundSpeed && hard)
            {
                if (!inHTube)
                {
                    //player accelerates slightly faster on the ground than in the air
                    //slower when hard
                    if (isGrounded)
                    {
                        xVelocity -= hardWalkAccel;
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

            //if a is pressed and d is not pressed, flip sprites left
            if (kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D) && !isStunned)
            {
                lookingRight = false;
            }
            //if a is not pressed and d is pressed, flip sprites right
            if (kbState.IsKeyUp(Keys.A) && kbState.IsKeyDown(Keys.D) && !isStunned)
            {
                lookingRight = true;
            }

            //updates prev keyboard state
            prevKBState = kbState;
            //resets ice trigger
            onIce = false;

            //updates animation state
            switch (animState)
            {
                case AnimationState.Idle:
                    //switch to walking if on the ground and moving
                    if (((kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D)) ||
                        (kbState.IsKeyUp(Keys.A) && kbState.IsKeyDown(Keys.D))) && isGrounded)
                    {
                        animState = AnimationState.Walking;
                        frame = 0;
                    }
                    //switch to jumping if off the ground
                    if (!isGrounded)
                    {
                        animState = AnimationState.Jumping;
                        frame = 0;
                    }
                    //switch to hurt if colliding with enemy or bullet
                    if (isStunned)
                    {
                        animState = AnimationState.Hurt;
                    }
                    //switch to floating if touching a tube
                    if (inHTube || inVTube)
                    {
                        animState = AnimationState.Floating;
                    }

                    //switch to hard if key is pressed
                    if (hard)
                    {
                        animState = AnimationState.Hard;
                        frame = 0;
                    }
                    break;

                case AnimationState.Walking:
                    //switch to idle if on the ground and not moving
                    if (((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) ||
                        (kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D))) && isGrounded)
                    {
                        animState = AnimationState.Idle;
                        frame = 0;
                    }
                    //switch to jumping if off the ground
                    if (!isGrounded)
                    {
                        animState = AnimationState.Jumping;
                        frame = 0;
                    }
                    //switch to hurt if colliding with enemy or bullet
                    if (isStunned)
                    {
                        animState = AnimationState.Hurt;
                    }
                    //switch to floating if colliding with a tube
                    if (inHTube || inVTube)
                    {
                        animState = AnimationState.Floating;
                    }
                    //switch to hard if key is pressed
                    if (hard)
                    {
                        animState = AnimationState.Hard;
                        frame = 0;
                    }
                    break;

                case AnimationState.Jumping:
                    //switch to idle if on the ground and not moving
                    if (((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) || 
                        (kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D))) && isGrounded)
                    {
                        animState = AnimationState.Idle;
                        frame = 0;
                    }
                    //switch to walking if on the ground and moving
                    if (((kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D)) ||
                        (kbState.IsKeyUp(Keys.A) && kbState.IsKeyDown(Keys.D))) && isGrounded)
                    {
                        animState = AnimationState.Walking;
                        frame = 0;
                    }
                    //switch to floating if colliding with a tube
                    if (inHTube || inVTube)
                    {
                        animState = AnimationState.Floating;
                    }
                    //switch to hurt if colliding with enemy or bullet
                    if (isStunned)
                    {
                        animState = AnimationState.Hurt;
                    }
                    //switch to hard if key is pressed
                    if (hard)
                    {
                        animState = AnimationState.Hard;
                        frame = 0;
                    }
                    break;

                case AnimationState.Hurt:
                    //switch to idle if on the ground and not moving
                    if (((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) ||
                        (kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D))) && isGrounded && !isStunned)
                    {
                        animState = AnimationState.Idle;
                        frame = 0;
                    }
                    //switch to walking if on the ground and moving
                    if (((kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D)) ||
                        (kbState.IsKeyUp(Keys.A) && kbState.IsKeyDown(Keys.D))) && isGrounded && !isStunned)
                    {
                        animState = AnimationState.Walking;
                        frame = 0;
                    }
                    //switch to jumping if off the ground
                    if (!isGrounded && !isStunned)
                    {
                        animState = AnimationState.Jumping;
                        frame = 0;
                    }
                    //switch to floating if colliding with a tube
                    if (inHTube || inVTube)
                    {
                        //animState = AnimationState.Floating;
                    }
                    break;

                case AnimationState.Floating:
                    //switch to idle if on the ground and not moving
                    if (((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) ||
                        (kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D))) && isGrounded && !inHTube && !inVTube)
                    {
                        animState = AnimationState.Idle;
                        frame = 0;
                    }
                    //switch to jumping if jump key is pressed and can jump
                    if (!isGrounded && !inHTube && !inVTube)
                    {
                        animState = AnimationState.Jumping;
                        frame = 0;
                    }
                    //switch to walking if on the ground and moving
                    if (((kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D)) ||
                        (kbState.IsKeyUp(Keys.A) && kbState.IsKeyDown(Keys.D))) && isGrounded && !inHTube && !inVTube)
                    {
                        animState = AnimationState.Walking;
                        frame = 0;
                    }
                    //switch to hard if key is pressed
                    if (hard)
                    {
                        animState = AnimationState.Hard;
                        frame = 0;
                    }
                    //switch to hurt if colliding with enemy or bullet
                    if (isStunned)
                    {
                        animState = AnimationState.Hurt;
                    }
                    break;

                case AnimationState.Hard:
                    //switch to idle if on the ground and not moving
                    if (((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) ||
                        (kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D))) && isGrounded && !hard)
                    {
                        animState = AnimationState.Idle;
                        frame = 0;
                    }
                    //switch to walking if on the ground and moving
                    if (((kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D)) ||
                        (kbState.IsKeyUp(Keys.A) && kbState.IsKeyDown(Keys.D))) && isGrounded && !hard)
                    {
                        animState = AnimationState.Walking;
                        frame = 0;
                    }
                    //switch to floating if colliding with a tube
                    if ((inHTube || inVTube) && !hard)
                    {
                        animState = AnimationState.Floating;
                    }
                    //switch to jumping if off the ground
                    if (!isGrounded && !hard)
                    {
                        animState = AnimationState.Jumping;
                        frame = 0;
                    }
                    //switch to floating if colliding with a beam
                    break;
            }
            //updates animation
            UpdateAnimation(gameTime);
        }

        /// <summary>
        /// checks tiles to see if they collide with the player
        /// </summary>
        /// <param name="level"></param>
        public bool Collisions(Room[,] bgLevel, Room[,] intLevel, int rows, int columns, 
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
                    if (bgLevel[i, j].Asset.Name == "Spawn")
                    {
                        spawnPoint.X = (float)bgLevel[i, j].RectX;
                        spawnPoint.Y = (float)bgLevel[i, j].RectY;
                    }
                    //makes sure only near blocks that have collision are taken into account
                    if (Math.Abs(bgLevel[i, j].Rect.X - collRect.X) < 400 &&
                        Math.Abs(bgLevel[i, j].Rect.Y - collRect.Y) < 400 && bgLevel[i, j].CanCollide && !spawning && !done)
                    {
                        //creates a rectangle of the overlaping area
                        collisionRect = Rectangle.Intersect(bgLevel[i, j].Rect, collRect);

                        //if player hits the end after collecting all keys, signal the transition to the next level
                        if (bgLevel[i, j].TypeOfCollision == "end" && bgLevel[i, j].Rect.Intersects(collRect) && exitOpen)
                        {
                            return true;
                        }

                        //player is hitting the top or bottom of a tile
                        if (collisionRect.Width > collisionRect.Height && (bgLevel[i, j].TypeOfCollision == "surface" 
                            || bgLevel[i, j].TypeOfCollision == "ice" || bgLevel[i, j].TypeOfCollision == "end"))
                        {
                            //player is landing on a tile
                            if (collRect.Y <= bgLevel[i, j].Rect.Y)
                            {
                                //player is on the ground
                                isGrounded = true;
                                //player is not stuck in the tile
                                AdjustEnemyPosition(-collisionRect.Height, false, enemies, keys);
                                AdjustPosition(bgLevel, -collisionRect.Height, false, rows, columns);
                                AdjustPosition(intLevel, -collisionRect.Height, false, rows, columns);
                                
                            }
                            //player is hitting the bottom of a tile with their head
                            else
                            {
                                //player has a light bounce off of the tile
                                //player is not stuck in the tile
                                AdjustEnemyPosition(collisionRect.Height, false, enemies, keys);
                                AdjustPosition(bgLevel, collisionRect.Height, false, rows, columns);
                                AdjustPosition(intLevel, collisionRect.Height, false, rows, columns);
                                
                            }

                            //triggers if the player is hitting ice
                            if (bgLevel[i, j].TypeOfCollision == "ice")
                            {
                                onIce = true;
                            }
                        }

                        //player is hitting the side of a tile
                        else if (Math.Abs(collisionRect.Height) > Math.Abs(collisionRect.Width) 
                            && (bgLevel[i, j].TypeOfCollision == "surface" || bgLevel[i, j].TypeOfCollision == "end") 
                            && !collidingWithSpring)
                        {
                            //player is on the right side of the tile, cannot move left
                            if (collRect.X + 50 > bgLevel[i, j].Rect.X)
                            {
                                touchingLeftWall = true;
                                //player is not stuck in the tile
                                AdjustEnemyPosition(collisionRect.Width, true, enemies, keys);
                                AdjustPosition(bgLevel, collisionRect.Width, true, rows, columns);
                                AdjustPosition(intLevel, collisionRect.Width, true, rows, columns);
                                
                            }
                            //player is on the left side of the tile, cannot move right
                            else if (collRect.X + 50 <= bgLevel[i, j].Rect.X)
                            {
                                touchingRightWall = true;
                                //player is not stuck in the tile
                                AdjustEnemyPosition(-collisionRect.Width, true, enemies, keys);
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
                    if (Math.Abs(intLevel[i, j].Rect.X - collRect.X) < 400 &&
                        Math.Abs(intLevel[i, j].Rect.Y - collRect.Y) < 400 && intLevel[i, j].CanCollide)
                    {
                        //creates a rectangle of the overlaping area
                        //collisionRect = Rectangle.Intersect(intLevel[i, j].Rect, rect);
                        isColliding = collRect.Intersects(intLevel[i, j].Rect);

                        //player is hiting a left spring
                        if (intLevel[i, j].TypeOfCollision == "leftSpring" && isColliding)
                        {
                            //remove any possible stun from player
                            isStunned = false;

                            //launches player left and a bit up
                            xVelocity = xSpringXVelocity;
                            yVelocity = xSpringYVelocity;
                            //resets double jump
                            canDoubleJump = true;
                        }

                        //player is hiting a right spring
                        else if (intLevel[i, j].TypeOfCollision == "rightSpring" && isColliding)
                        {
                            //remove any possible stun from player
                            isStunned = false;

                            //launches player right and a bit up
                            xVelocity = -xSpringXVelocity;
                            yVelocity = xSpringYVelocity;
                            //resets double jump
                            canDoubleJump = true;
                        }

                        //player is hiting an up spring
                        else if (intLevel[i, j].TypeOfCollision == "upSpring" && isColliding)
                        {
                            //remove any possible stun from player
                            isStunned = false;

                            //lauches player upwards and resets x momentum
                            yVelocity = ySpringYVelocity;
                            xVelocity = 0;
                            //resets double jump
                            canDoubleJump = true;
                        }

                        //player is hiting an up tube
                        else if (intLevel[i, j].TypeOfCollision == "upTube" && isColliding && !isStunned)
                        {
                            //centers x momentum if player is not attempting to leave beam 
                            if (!playerWantsOut)
                            {
                                xVelocity = CenterPlayerInTube(intLevel[i, j].Rect, collRect, false);
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
                        else if (intLevel[i, j].TypeOfCollision == "downTube" && isColliding && !isStunned)
                        {
                            //centers x momentum if player is not attempting to leave beam 
                            if (!playerWantsOut)
                            {
                                xVelocity = CenterPlayerInTube(intLevel[i, j].Rect, collRect, false);
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
                            && tubeDisableTimer == 60 && !isStunned)
                        {
                            //centers y momentum if player is not attempting to leave beam 
                            if (!playerWantsOut)
                            {
                                yVelocity = CenterPlayerInTube(intLevel[i, j].Rect, collRect, true);
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
                            && tubeDisableTimer == 60 && !isStunned)
                        {
                            //centers y momentum if player is not attempting to leave beam 
                            if (!playerWantsOut)
                            {
                                yVelocity = CenterPlayerInTube(intLevel[i, j].Rect, collRect, true);
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

                        //player is hitting a spike
                        if (intLevel[i, j].TypeOfCollision == "spikes" && isColliding)
                        {
                            spawning = true;

                            return false;
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

            //------ key collisions -------
            foreach (Key k in keys)
            {
                if (rect.Intersects(k.Hitbox))
                {
                    collectedKeys.Add(k);
                    keys.Remove(k);
                    return false;
                }
            }

            //------ enemy / bullet collisions -------
            foreach (Enemy e in enemies)
            {
                if (collRect.Intersects(e.Hitbox))
                {
                    double launchAngle = Math.Atan2(e.Hitbox.Center.Y - collRect.Center.Y,
                    e.Hitbox.Center.X - collRect.Center.X);
                    if (!hard)
                    {
                        //update player x and y velocity
                        xVelocity = 30 * Math.Cos(launchAngle);
                        yVelocity = 30 * Math.Sin(launchAngle);
                    }
                    //less knockback if hard
                    else
                    {
                        //update player x and y velocity
                        xVelocity = 10 * Math.Cos(launchAngle);
                        yVelocity = 10 * Math.Sin(launchAngle);
                    }
                    
                    //player loses double jump
                    canDoubleJump = false;
                    //player is stunned
                    isStunned = true;
                    currentStunFrame = 0;
                }
                //goes through the list of bullets for each enemy
                foreach (Bullet b in e.Bullets)
                {
                    //collision
                    if (b.Hitbox.Intersects(collRect))
                    {
                        //use the width and height in arctan to find the angle
                        double launchAngle = Math.Atan2(b.Hitbox.Center.Y - collRect.Center.Y, 
                            b.Hitbox.Center.X - collRect.Center.X);

                        //moves player less if hard
                        if (!hard)
                        {
                            //update player x and y velocity
                            xVelocity = 50 * Math.Cos(launchAngle);
                            yVelocity = 50 * Math.Sin(launchAngle);
                        }
                        else
                        {
                            xVelocity = 20 * Math.Cos(launchAngle);
                            yVelocity = 20 * Math.Sin(launchAngle);
                        }

                        //add an explosions and remove the bullet from its list
                        e.Explosions.Add(new Explosion(explosion, new Rectangle(b.Hitbox.X - 50, b.Hitbox.Y - 50, 200, 200)));
                        e.Bullets.Remove(b);
                        isStunned = true;

                        currentStunFrame = 0;
                        return false;
                    }
                }
            }

            //actions while player is stunned
            if (currentStunFrame >= 0 && currentStunFrame < framesStunned)
            {
                //increment timer
                currentStunFrame++;
                
            }
            else if (currentStunFrame >= framesStunned)
            {
                isStunned = false;
            }
            //stop timer and when not stunned
            if (!isStunned && currentStunFrame != -1)
            {
                currentStunFrame = -1;
                canDoubleJump = true;
            }

            //end condition not met, game continues
            return false;
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
                    if (isHorizontal && xVelocity > 0 && !inHTube)
                    {
                        level[i, j].RectX -= distance + 1;
                        xVelocity = 0;
                    }
                    else if (isHorizontal && xVelocity > 0 && inHTube)
                    {
                        level[i, j].RectX -= distance + 1;
                        xVelocity = 1;
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

        private void AdjustEnemyPosition(int distance, bool isHorizontal, List<Enemy> enemies, List<Key> keys)
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

            foreach (Key k in keys)
            {
                if (isHorizontal && xVelocity > 0)
                {
                    k.AdjustmentX += distance + 1;
                }
                else if (isHorizontal && xVelocity <= 0)
                {
                    k.AdjustmentX += distance - 1;
                }
                else
                {
                    k.AdjustmentY += distance + 1;
                }
            }

            foreach (Key k in collectedKeys)
            {
                if (isHorizontal && xVelocity > 0)
                {
                    k.AdjustmentX += distance + 1;
                }
                else if (isHorizontal && xVelocity <= 0)
                {
                    k.AdjustmentX += distance - 1;
                }
                else
                {
                    k.AdjustmentY += distance + 1;
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
            const int margin = 5;

            //value stays at 0 if player is already in center
            int velocityUpdate = 0;

            //changes the x velocity if in a vertical tube
            if (!isHorizontal)
            {
                //moves left if center is to the right of tube
                if (playerRect.Center.X > tubeRect.Center.X + margin)
                {
                    velocityUpdate = correctionStrength;
                }
                //right if center is to the left
                else if (playerRect.Center.X < tubeRect.Center.X - margin)
                {
                    velocityUpdate = -correctionStrength;
                }
            }
            //changes the y velocity if in a horizontal tube
            else
            {
                //moves up if center is below the tube
                if (playerRect.Center.Y > tubeRect.Center.Y + margin)
                {
                    velocityUpdate = correctionStrength;
                }
                //down if center is above
                else if (playerRect.Center.Y < tubeRect.Center.Y - margin)
                {
                    velocityUpdate = -correctionStrength;
                }
            }
            return velocityUpdate;
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            // Handle animation timing
            // - Add to the time counter
            // - Check if we have enough "time" to advance the frame

            // How much time has passed?  
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            switch (animState)
            {
                case AnimationState.Idle:
                    Anim(timePerIdleFrame, IdleFrameCount);
                    break;
                case AnimationState.Walking:
                    Anim(timePerWalkFrame, WalkFrameCount);
                    break;
                case AnimationState.Jumping:
                    Anim(timePerJumpFrame, JumpFrameCount);
                    break;
                case AnimationState.Hurt:
                    Anim(timePerHurtFrame, HurtFrameCount);
                    break;
                case AnimationState.Floating:
                    Anim(timePerFloatFrame, FloatFrameCount);
                    break;
                case AnimationState.Hard:
                    timeCounter = 0;
                    break;
            }
        }

        private void Anim(double timePerFrame, int frameCount)
        {
            // If enough time has passed:
            if (timeCounter >= timePerFrame)
            {
                frame += 1;                     // Adjust the frame to the next image

                if (frame >= frameCount)     // Check the bounds - have we reached the end of walk cycle?
                    frame = 0;                  // Back to 1 (since 0 is the "standing" frame)

                timeCounter -= timePerFrame;    // Remove the time we "used" - don't reset to 0
                                                    // This keeps the time passed 
            }
        }

        public void Draw(SpriteBatch sb)
        {
            switch (animState)
            {
                case AnimationState.Idle:
                    DrawPlayer(idle, sb);
                    break;
                case AnimationState.Walking:
                    DrawPlayer(walking, sb);
                    break;
                case AnimationState.Jumping:
                    DrawPlayer(jumping, sb);
                    break;
                case AnimationState.Hurt:
                    DrawPlayer(hurt, sb);
                    break;
                case AnimationState.Floating:
                    DrawPlayer(floating, sb);
                    break;
                case AnimationState.Hard:
                    sb.Draw(Asset, rect, Color.White);
                    break;

            }
            int test1 = (int)spawnPoint.X - rect.X;
            int test2 = (int)spawnPoint.Y - rect.Y;

            //sb.DrawString(debugFont, isStunned + ", " + isGrounded + ", " + touchingLeftWall + ", " + touchingRightWall + 
               // ", "  + debugText + ", " + xVelocity + ", " + test1 + ", " + test2,
               // new Vector2(100, 100), Color.Red);

            sb.DrawString(debugFont, "Keys Left: " + keys.Count, new Vector2(3400, 100), Color.Red);

            //draw keys
            foreach (Key k in keys)
            {
                k.Draw(sb);
            }

        }

        private void DrawPlayer(Texture2D spriteSheet, SpriteBatch sb)
        {
            //flips the sprite if nessisary
            SpriteEffects effect;
            if (lookingRight)
            {
                effect = SpriteEffects.None;
            }
            else
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            sb.Draw(
                    spriteSheet,
                    new Vector2(rect.X, rect.Y),
                    new Rectangle(
                        frame * rect.Width,
                        0,
                        rect.Width,
                        rect.Height),
                    Color.White,
                    0,
                    Vector2.Zero,
                    1.0f,
                    effect,
                    0);
        }

    }
}
