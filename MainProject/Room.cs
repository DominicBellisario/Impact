using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MainProject
{
    internal class Room
    {
        private Rectangle rect;
        private Texture2D asset;
        private Texture2D asset2;
        private string typeOfCollision;
        private bool canCollide;
        //how many frames need to pass for the next animation frame (larger # = slower speed)
        private int animationSpeed;
        //numnber of frames that this tile has in its animation 
        private int numberOfFrames;
        //the current frame of the animation
        private int currentFrame;
        //the direction of the spike
        private string spikeDirection;

        /// <summary>
        /// returns the room object's Rectangle
        /// </summary>
        public Rectangle Rect
        {
            get { return rect; }
        }

        /// <summary>
        /// returns and sets the tile's Rectangle's X position
        /// </summary>
        public double RectX
        {
            get { return rect.X; }
            set { rect.X = (int)value; }
        }

        /// <summary>
        /// returns and sets the tile's Rectangle's Y position
        /// </summary>
        public double RectY
        {
            get { return rect.Y; }
            set { rect.Y = (int)value; }
        }

        /// <summary>
        /// returns the texture of the asset
        /// </summary>
        public Texture2D Asset
        {
            get { return asset; }
        }

        /// <summary>
        /// returns the texture of the asset
        /// </summary>
        public Texture2D Asset2
        {
            get { return asset2; }
        }

        /// <summary>
        /// returns the type of collision of the tile
        /// </summary>
        public string TypeOfCollision
        {
            get { return typeOfCollision; }
        }

        /// <summary>
        /// returns wether or not the tile needs to be checked for collisions
        /// </summary>
        public bool CanCollide
        {
            get { return canCollide; }
        }

        /// <summary>
        /// returns how fast the tile switches animation frames
        /// </summary>
        public int AnimationSpeed
        {
            get { return animationSpeed; }
        }

        /// <summary>
        /// returns the number of unique frames in the tile's animation
        /// </summary>
        public int NumberOfFrames
        {
            get { return numberOfFrames; }
        }

        /// <summary>
        /// returns or updates the current frame of the tile
        /// </summary>
        public int CurrentFrame 
        { 
            get { return currentFrame; } 
            set { currentFrame = value; }
        }

        public string SpikeDirection
        {
            get { return spikeDirection; }
        }

        /// <summary>
        /// room constructor
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="asset"></param>
        /// <param name="canCollide"></param>
        public Room(Rectangle rect, Texture2D asset, Texture2D asset2, bool canCollide, 
            string typeOfCollision, int animationSpeed, int numberOfFrames, string spikeDirection)
        {
            this.rect = rect;
            this.asset = asset;
            this.asset2 = asset2;
            this.canCollide = canCollide;
            this.typeOfCollision = typeOfCollision;
            this.animationSpeed = animationSpeed;
            this.numberOfFrames = numberOfFrames;
            this.spikeDirection = spikeDirection;
            currentFrame = 1;
        }

        /// <summary>
        /// Draws the room object
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb, bool normalTube, bool exitOpen)
        {
            if (spikeDirection == "none")
            {
                if(asset.Name != "ExitClosed")
                {
                    if (normalTube)
                    {
                        sb.Draw(Asset,
                        new Vector2((float)RectX, (float)RectY),
                        null,
                        Color.White);
                    }
                    else
                    {
                        sb.Draw(Asset2,
                        new Vector2((float)RectX, (float)RectY),
                        null,
                        Color.White);
                    }
                }
                else
                {
                    if (!exitOpen)
                    {
                        sb.Draw(Asset,
                        new Vector2((float)RectX, (float)RectY),
                        null,
                        Color.White);
                    }
                    else
                    {
                        sb.Draw(Asset2,
                        new Vector2((float)RectX, (float)RectY),
                        null,
                        Color.White);
                    }

                }

            }
            else if (spikeDirection == "down")
            {
                
                sb.Draw(
                    asset, 
                    new Rectangle((int)RectX, (int)RectY, rect.Width, rect.Height),
                    null,
                    Color.White, 
                    0, 
                    Vector2.Zero, 
                    SpriteEffects.None, 
                    0
                    );
            }
            else if (spikeDirection == "up")
            {
                sb.Draw(
                    asset,
                    new Rectangle((int)RectX, (int)RectY, rect.Width, rect.Height),
                    null,
                    Color.White,
                    0,
                    Vector2.Zero,
                    SpriteEffects.FlipVertically,
                    0
                    );
            }
            else if (spikeDirection == "left")
            {
                sb.Draw(
                    asset,
                    new Rectangle((int)RectX, (int)RectY + 25, rect.Height, rect.Width),
                    null,
                    Color.White,
                    (float)Math.PI/2,
                    new Vector2(rect.Width / 2, rect.Height / 2),
                    SpriteEffects.None,
                    0
                    );
            }
            else if (spikeDirection == "right")
            {
                sb.Draw(
                    asset,
                    new Rectangle((int)RectX, (int)RectY + 25, rect.Height, rect.Width),
                    null,
                    Color.White,
                    (float)Math.PI / 2,
                    new Vector2(rect.Width / 2, rect.Height / 2),
                    SpriteEffects.FlipHorizontally,
                    0
                    );
            }
            
        }
    }
}
