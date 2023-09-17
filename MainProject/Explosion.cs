using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{
    internal class Explosion
    {
        private Texture2D explosion;
        private Rectangle exRect;

        //animation
        private int frame;              // The current animation frame
        private double timeCounter;     // The amount of time that has passed
        private const double fps = 20;             // The speed of the animation
        private double timePerFrame;    // The amount of time (in fractional seconds) per frame
        private const int WalkFrameCount = 4;       // The number of frames in the animation


        public Explosion(Texture2D explosion, Rectangle exRect)
        {
            this.explosion = explosion;
            frame = 0;
            timePerFrame = 1 / fps;
            this.exRect = exRect;
        }

        public bool Update(GameTime gameTime, int playerXVelocity, int playerYVelocity)
        {
            exRect.X += playerXVelocity;
            exRect.Y += playerYVelocity;

            // Handle animation timing
            // - Add to the time counter
            // - Check if we have enough "time" to advance the frame

            // How much time has passed?  
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // If enough time has passed:
            if (timeCounter >= timePerFrame)
            {
                frame += 1;                     // Adjust the frame to the next image

                if (frame >= WalkFrameCount)
                {
                    frame = 0;
                    return true;
                }     

                timeCounter -= timePerFrame;    // Remove the time we "used" - don't reset to 0
            }
            return false;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
            explosion,
            new Vector2(exRect.X, exRect.Y),
                new Rectangle(
                    frame * exRect.Width,
                    0,
                    exRect.Width,
                    exRect.Height),
                 Color.White,
                 0,
                 Vector2.Zero,
                 1.0f,
                 SpriteEffects.None,
                 0
                 );
        }

    }
}
