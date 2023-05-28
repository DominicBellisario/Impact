using System;
using System.Collections.Generic;
using System.Linq;
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
        private string typeOfCollision;
        private bool canCollide;

        /// <summary>
        /// returns the room object's Rectangle
        /// </summary>
        public Rectangle Rect
        {
            get { return rect; }
        }

        /// <summary>
        /// returns and sets the room's Rectangle's X position
        /// </summary>
        public double RectX
        {
            get { return rect.X; }
            set { rect.X = (int)value; }
        }

        /// <summary>
        /// returns and sets the room's Rectangle's Y position
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
        /// returns the type of collision of the tile
        /// </summary>
        public string TypeOfCollision
        {
            get { return typeOfCollision; }
        }

        /// <summary>
        /// returns wether or not the tile needs to be checkewd for collisions
        /// </summary>
        public bool CanCollide
        {
            get { return canCollide; }
        }

        /// <summary>
        /// room constructor
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="asset"></param>
        /// <param name="canCollide"></param>
        public Room(Rectangle rect, Texture2D asset, bool canCollide, string typeOfCollision)
        {
            this.rect = rect;
            this.asset = asset;
            this.canCollide = canCollide;
            this.typeOfCollision = typeOfCollision;
        }

        /// <summary>
        /// Draws the room object
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Asset, Rect, Color.White);
        }
    }
}
