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
        public Enemy(int speed, int aggroRadius, Rectangle hitbox, int leftX, int leftY, int rightX, int rightY)
        {
            this.speed = speed;
            this.aggroRadius = aggroRadius;
            this.hitbox = hitbox;
            //enemy begins by walking
            isWalking = true;
            //creates edge rectangles
            leftRect = new Rectangle(leftX, leftY, 100, 100);
            rightRect = new Rectangle(rightX, rightY, 100, 100);
        }

        /// <summary>
        /// updates enemy position
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="xVelocity"></param>
        /// <param name="yVelocity"></param>
        public void Update(GameTime gametime, int xVelocity, int yVelocity)
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
