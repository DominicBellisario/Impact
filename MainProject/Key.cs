using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{
    internal class Key
    {
        //hitbox
        private Rectangle hitbox;

        //spritesheet
        private Texture2D spriteSheet;

        //animation
        private int frame;              // The current animation frame
        private double timeCounter;     // The amount of time that has passed
        private const double fps = 6;             // The speed of the animation
        private double timePerFrame;    // The amount of time (in fractional seconds) per frame
        private const int WalkFrameCount = 3;       // The number of frames in the animation

        private int adjustmentX;
        private int adjustmentY;

        public int AdjustmentX
        {
            get { return adjustmentX; }
            set { adjustmentX = value; }
        }

        public int AdjustmentY
        {
            get { return adjustmentY; }
            set { adjustmentY = value; }
        }

        /// <summary>
        /// used by player to calculate collisions
        /// </summary>
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        public Key(int xPos, int yPos, Texture2D spriteSheet)
        {
            this.spriteSheet = spriteSheet;
            hitbox = new Rectangle(xPos, yPos, 100, 100);
            frame = 0;
            timePerFrame = 1 / fps;
        }

        public void UpdateAnimation(GameTime gameTime, int xVelocity, int yVelocity)
        {
            hitbox.X += xVelocity - adjustmentX;
            hitbox.Y += yVelocity - adjustmentY;
            adjustmentX = 0;
            adjustmentY = 0;


            // Handle animation timing
            // - Add to the time counter
            // - Check if we have enough "time" to advance the frame

            // How much time has passed?  
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // If enough time has passed:
            if (timeCounter >= timePerFrame)
            {
                frame += 1;                     // Adjust the frame to the next image

                if (frame >= WalkFrameCount)     // Check the bounds - have we reached the end of walk cycle?
                    frame = 0;                  // Back to 1 (since 0 is the "standing" frame)

                timeCounter -= timePerFrame;    // Remove the time we "used" - don't reset to 0
                                                // This keeps the time passed 
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spriteSheet,
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
    }
}
