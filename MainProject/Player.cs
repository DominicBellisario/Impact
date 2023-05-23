using Microsoft.Xna.Framework;
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

        //current speed of the player
        private double xVelocity;
        private double yVelocity;

        //wether or not the player is standing on a surface
        private bool isGrounded;

        //simulates gravity
        private const double gravity = -.3;

        //very fast horizontal acceleration when player begins to walk
        private const double walkAccel = 20;

        //less fast horizonal accelertaion while player is in the air
        private const double airAccel = 5;

        //max speeds in the x and y directions
        private const int maxXSpeed = 10;
        private const int maxYSpeed = -30;

        public double XVelocity
        {
            get { return xVelocity; }
        }

        public double YVelocity
        {
            get { return yVelocity; }
        }

        public Player(double xPos, double yPos)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            xVelocity = 0;
            yVelocity = 0;
            isGrounded = false;
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

    }
}
