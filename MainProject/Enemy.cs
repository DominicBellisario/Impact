using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{
    enum enemyState
    {
        Walking,
        Shooting
    }
    internal class Enemy
    {
        //how fast the enemy moves
        private int speed;

        //the aggro radius for the enemy
        private int aggroRadius;

        //enemy hitbox
        private Rectangle hitbox;

        //coordinates for the edges of the enemies movement
        private int leftX;
        private int leftY;
        private int rightX;
        private int rightY;
    }
}
