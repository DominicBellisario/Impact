﻿using Microsoft.Xna.Framework;
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
        private const int speed = 10;

        //bullet hitbox
        private Rectangle hitbox;

        //angle that the bullet was fired at
        private int angle;

        //bullet texture
        private Texture2D bulletSprite;

        public Bullet (int xPos, int yPos, int angle, Texture2D bulletSprite)
        {
            this.angle = angle;
            this.bulletSprite = bulletSprite;
            hitbox = new Rectangle(xPos - 50, yPos - 50, 100, 100);
        }

        /// <summary>
        /// updates the bullets position
        /// </summary>
        public void Update()
        {
            //update the bullet's location
            hitbox.X += (int)(speed * Math.Cos(angle));
            hitbox.Y += (int)(speed * Math.Sin(angle));
        }


        public void Draw (SpriteBatch sb)
        {
            sb.Draw(bulletSprite, hitbox, Color.White);
        }
    }
}