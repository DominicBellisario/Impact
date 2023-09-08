using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{
    internal class Bullet
    {
        //speed of the bullet
        private const int speed = 15;

        //bullet hitbox
        private Rectangle hitbox;

        //angle that the bullet was fired at
        private double angle;

        //bullet texture
        private Texture2D bulletSprite;

        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        public Bullet (int xPos, int yPos, double angle, Texture2D bulletSprite)
        {
            this.angle = angle;
            this.bulletSprite = bulletSprite;
            hitbox = new Rectangle(xPos - 50, yPos - 50, 100, 100);
        }

        /// <summary>
        /// updates the bullets position
        /// </summary>
        public void Update(int playerXVelocity, int playerYVelocity)
        {
            //update the bullet's location
            hitbox.X += (int)(speed * Math.Cos(angle) + playerXVelocity);
            hitbox.Y += (int)(speed * Math.Sin(angle) + playerYVelocity);
        }


        public void Draw (SpriteBatch sb)
        {
            sb.Draw(bulletSprite, new Rectangle(hitbox.X + 50, hitbox.Y + 50, 100, 100), new Rectangle(0, 0, 100, 100), 
                Color.White, (float)angle, new Vector2(50, 50), SpriteEffects.None, 0);
        }
    }
}
