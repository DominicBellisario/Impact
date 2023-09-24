using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MainProject
{
    //keeps track of the current level
    enum CurrentLevel
    {
        Test,
        L1,
        L2,
        L3,
        L4,
        L5,
        L6,
        L7,
        L8,
        L9,
        L10,
        L11,
        L12,
        L13,
        L14,
        L15,
        L16
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        CurrentLevel currentLevel;

        //sprites
        //non collidables
        private Texture2D background;
        private Texture2D spawn;

        //collidables
        private Texture2D floor;
        private Texture2D leftPlat;
        private Texture2D centerPlat;
        private Texture2D rightPlat;
        private Texture2D wall;
        private Texture2D ice;

        //transparent tile
        private Texture2D nullTile;

        //springs
        private Texture2D leftSpring;
        private Texture2D rightSpring;
        private Texture2D upSpring;

        //tubes
        private Texture2D leftTube;
        private Texture2D rightTube;
        private Texture2D upTube;
        private Texture2D downTube;

        //spike
        private Texture2D spikes;

        //player
        private Texture2D playerSprite;
        private Texture2D playerIdle;
        private Texture2D playerWalk;
        private Texture2D playerJump;
        private Texture2D playerHurt;
        private Texture2D playerFloat;

        //enemies
        private Texture2D enemyWalking;
        private Texture2D enemyShooting;

        //bullets
        private Texture2D bullet;
        private Texture2D explosion;

        //list of sprites needed for level loading
        private Dictionary<string, Texture2D> bgLevelSprites;
        private Dictionary<string, Texture2D> intLevelSprites;

        //width and height of screen
        private int width;
        private int height;

        #region levels
        //levels
        private Level testLevel;
        private Level level1;
        private Level level2;
        private Level level3;
        private Level level4;
        private Level level5;
        private Level level6;
        private Level level7;
        private Level level8;
        private Level level9;
        private Level level10;
        #endregion

        #region player
        //player
        private Player player;
        private Player player1;
        private Player player2;
        private Player player3;
        private Player player4;
        private Player player5;
        private Player player6;
        private Player player7;
        private Player player8;
        private Player player9;
        private Player player10;
        #endregion

        #region enemies
        //lists of enemies
        private List<Enemy> enemies;
        private List<Enemy> enemies1;
        private List<Enemy> enemies2;
        private List<Enemy> enemies3;
        private List<Enemy> enemies4;
        private List<Enemy> enemies5;
        private List<Enemy> enemies6;
        private List<Enemy> enemies7;
        private List<Enemy> enemies8;
        private List<Enemy> enemies9;
        private List<Enemy> enemies10;
        #endregion

        #region keys
        //lists of keys
        private List<Key> keys;
        private List<Key> keys1;
        private List<Key> keys2;
        private List<Key> keys3;
        private List<Key> keys4;
        private List<Key> keys5;
        private List<Key> keys6;
        private List<Key> keys7;
        private List<Key> keys8;
        private List<Key> keys9;
        private List<Key> keys10;
        #endregion

        //fonts
        private SpriteFont debugFont;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //game starts at level 1
            currentLevel = CurrentLevel.L6;

            //set screen size to the size of the monitor (3840 x 2160)
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            
            _graphics.ApplyChanges();

            width = _graphics.GraphicsDevice.Viewport.Width;
            height = _graphics.GraphicsDevice.Viewport.Height;

            //level template
            bgLevelSprites = new Dictionary<string, Texture2D>();
            intLevelSprites = new Dictionary<string, Texture2D>();
            
            //enemies
            enemies = new List<Enemy>();
            enemies1 = new List<Enemy>();
            enemies2 = new List<Enemy>();
            enemies3 = new List<Enemy>();
            enemies4 = new List<Enemy>();
            enemies5 = new List<Enemy>();
            enemies6 = new List<Enemy>();
            enemies7 = new List<Enemy>();
            enemies8 = new List<Enemy>();
            enemies9 = new List<Enemy>();
            enemies10 = new List<Enemy>();

            //keys
            keys = new List<Key>();
            keys1 = new List<Key>();
            keys2 = new List<Key>();
            keys3 = new List<Key>();
            keys4 = new List<Key>();
            keys5 = new List<Key>();
            keys6 = new List<Key>();
            keys7 = new List<Key>();
            keys8 = new List<Key>();
            keys9 = new List<Key>();
            keys10 = new List<Key>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            #region spriteLoading
            //background sprites
            background = Content.Load<Texture2D>("Background");
            bgLevelSprites.Add("background", background);

            leftPlat = Content.Load<Texture2D>("LeftPlat");
            bgLevelSprites.Add("leftPlat", leftPlat);

            centerPlat = Content.Load<Texture2D>("CenterPlat");
            bgLevelSprites.Add("centerPlat", centerPlat);

            rightPlat = Content.Load<Texture2D>("RightPlat");
            bgLevelSprites.Add("rightPlat", rightPlat);

            floor = Content.Load<Texture2D>("Floor");
            bgLevelSprites.Add("floor", floor);

            spawn = Content.Load<Texture2D>("Spawn");
            bgLevelSprites.Add("spawn", spawn);

            wall = Content.Load<Texture2D>("Wall");
            bgLevelSprites.Add("wall", wall);

            ice = Content.Load<Texture2D>("Ice");
            bgLevelSprites.Add("ice", ice);

            //interactable sprites

            nullTile = Content.Load<Texture2D>("Null");
            intLevelSprites.Add("null", nullTile);

            leftSpring = Content.Load<Texture2D>("LeftSpring");
            intLevelSprites.Add("leftSpring", leftSpring);

            rightSpring = Content.Load<Texture2D>("RightSpring");
            intLevelSprites.Add("rightSpring", rightSpring);

            upSpring = Content.Load<Texture2D>("UpSpring");
            intLevelSprites.Add("upSpring", upSpring);

            upTube = Content.Load<Texture2D>("UpTube");
            intLevelSprites.Add("upTube", upTube);

            downTube = Content.Load<Texture2D>("DownTube");
            intLevelSprites.Add("downTube", downTube);

            leftTube = Content.Load<Texture2D>("LeftTube");
            intLevelSprites.Add("leftTube", leftTube);

            rightTube = Content.Load<Texture2D>("RightTube");
            intLevelSprites.Add("rightTube", rightTube);

            spikes = Content.Load<Texture2D>("Spikes");
            intLevelSprites.Add("spikes", spikes);

            //player sprites
            playerSprite = Content.Load<Texture2D>("Player");
            playerIdle = Content.Load<Texture2D>("PlayerIdle");
            playerWalk = Content.Load<Texture2D>("PlayerWalk");
            playerJump = Content.Load<Texture2D>("PlayerJump");
            playerHurt = Content.Load<Texture2D>("PlayerHurt");
            playerFloat = Content.Load<Texture2D>("PlayerFloat");
            
            //enemy sprites
            enemyWalking = Content.Load<Texture2D>("EnemyWalking");
            enemyShooting = Content.Load<Texture2D>("EnemyShooting");

            //bullet sprite
            bullet = Content.Load<Texture2D>("Bullet");
            explosion = Content.Load<Texture2D>("Explosion");
            #endregion

            //fonts
            debugFont = Content.Load<SpriteFont>("DebugFont");

            #region levels
            //level loading
            testLevel = new Level(bgLevelSprites, intLevelSprites, 
                width, height, "TestLevel.txt", "TestLevelInteractables.txt");
            level1 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level1Bg.txt", "Level1Int.txt");
            level2 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level2Bg.txt", "Level2Int.txt");
            level3 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level3Bg.txt", "Level3Int.txt");
            level4 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level4Bg.txt", "Level4Int.txt");
            level5 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level5Bg.txt", "Level5Int.txt");
            level6 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level6Bg.txt", "Level6Int.txt");
            #endregion

            #region players
            //player loading
            player = new Player(width/2, height/2, playerSprite, playerIdle, playerWalk, playerJump, 
                playerHurt, playerFloat, explosion, keys, debugFont);
            player1 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys1, debugFont);
            player2 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys2, debugFont);
            player3 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys3, debugFont);
            player4 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys4, debugFont);
            player5 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys5, debugFont);
            player6 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys6, debugFont);
            player7 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys7, debugFont);
            player8 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys8, debugFont);
            player9 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys9, debugFont);
            player10 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys10, debugFont);
            #endregion

            #region enemies
            //test level enemies
            enemies.Add(new Enemy(5, 1000, 4000, -400, 3500, -400, 4300, -400,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, testLevel.BgLevelBlueprint, 50, 50));
            enemies.Add(new Enemy(3, 1000, 4900, -900, 4600, -900, 5200, -900,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, testLevel.BgLevelBlueprint, 50, 50));

            //level 1 enemies
            //(none)

            //level 2 enemies
            //(none)

            //level 3 enemies
            enemies3.Add(new Enemy(7, 1000, 3500, 1300, 2600, 1300, 4400, 1300,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level3.BgLevelBlueprint, level3.Rows, level3.Columns));
            enemies3.Add(new Enemy(4, 1000, 6800, 1500, 6500, 1500, 7100, 1500,
               enemyWalking, enemyShooting, bullet, explosion, debugFont, level3.BgLevelBlueprint, level3.Rows, level3.Columns));

            //level 4 enemies
            enemies4.Add(new Enemy(5, 1000, 4400, -800, 3800, -800, 4900, -800,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level4.BgLevelBlueprint, level4.Rows, level4.Columns));

            //level 5 enemies
            //(none)

            //level 6 enemies
            enemies6.Add(new Enemy(1, 1000, 2900, 300, 2800, 300, 3250, 300,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level6.BgLevelBlueprint, level6.Rows, level6.Columns));
            enemies6.Add(new Enemy(2, 1000, 3750, 200, 3600, 200, 4200, 200,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level6.BgLevelBlueprint, level6.Rows, level6.Columns));

            //level 7 enemies
            //(none)

            //level 8 enemies
            //(none)

            //level 9 enemies
            //(none)

            //level 10 enemies
            //(none)
            #endregion

            #region keys
            //test keys
            //(none)

            //level 1 keys
            //(none)

            //level 2 keys
            //(none)

            //level 3 keys
            //(none)

            //level 4 keys
            keys4.Add(new Key(1950, -2650, playerHurt));
            keys4.Add(new Key(2900, -800, playerHurt));
            keys4.Add(new Key(4400, 800, playerHurt));

            //level 5 keys
            //(none)

            //level 6 keys
            //(none)

            //level 7 keys
            //(none)

            //level 8 keys
            //(none)

            //level 9 keys
            //(none)

            //level 10 keys
            //(none)
            #endregion
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (currentLevel)
            {
                //while player is in the test level
                case CurrentLevel.Test:
                    LevelUpdate(testLevel, enemies, player, keys, CurrentLevel.L1, gameTime);
                    break;

                //while player is in level 1
                case CurrentLevel.L1:
                    LevelUpdate(level1, enemies1, player1, keys1, CurrentLevel.L2, gameTime);
                    break;

                //while player is in level 2
                case CurrentLevel.L2:
                    LevelUpdate(level2, enemies2, player2, keys2, CurrentLevel.L3, gameTime);
                    break;

                //while player is in level 3
                case CurrentLevel.L3:
                    LevelUpdate(level3, enemies3, player3, keys3, CurrentLevel.L4, gameTime);
                    break;

                //while player is in level 4
                case CurrentLevel.L4:
                    LevelUpdate(level4, enemies4, player4, keys4, CurrentLevel.L5, gameTime);
                    break;

                //while player is in level 5
                case CurrentLevel.L5:
                    LevelUpdate(level5, enemies5, player5, keys5, CurrentLevel.L6, gameTime);
                    break;

                //while player is in level 6
                case CurrentLevel.L6:
                    LevelUpdate(level6, enemies6, player6, keys6, CurrentLevel.L7, gameTime);
                    break;

                //while player is in level 7
                case CurrentLevel.L7:
                    //LevelUpdate(level7, enemies7, player7, keys7, CurrentLevel.L8, gameTime);
                    break;

                //while player is in level 8 
                case CurrentLevel.L8:
                    //LevelUpdate(level8, enemies8, player8, keys8, CurrentLevel.L9, gameTime);
                    break;

                //while player is in level 9
                case CurrentLevel.L9:
                    //LevelUpdate(level9, enemies9, player9, keys9, CurrentLevel.L10, gameTime);
                    break;

                //while player is in level 10
                case CurrentLevel.L10:
                    //LevelUpdate(level10, enemies10, player10, keys10, CurrentLevel.L11, gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            switch (currentLevel)
            {
                //while player is in test level
                case CurrentLevel.Test:
                    //draws level first
                    testLevel.Draw(_spriteBatch);
                    //then player
                    player.Draw(_spriteBatch);
                    //then enemies
                    foreach (Enemy enemy in enemies)
                    {
                        enemy.Draw(_spriteBatch);
                    }
                    break;

                //while player is in level 1
                case CurrentLevel.L1:
                    //draws level first
                    level1.Draw(_spriteBatch);
                    //then player
                    player1.Draw(_spriteBatch);
                    //then enemies (none)
                    break;

                //while player is in level 2
                case CurrentLevel.L2:
                    //draws level first
                    level2.Draw(_spriteBatch);
                    //then player
                    player2.Draw(_spriteBatch);
                    //then enemies (none)
                    break;

                //while player is in level 3
                case CurrentLevel.L3:
                    //draws level first
                    level3.Draw(_spriteBatch);
                    //then player
                    player3.Draw(_spriteBatch);
                    //then enemies
                    foreach (Enemy enemy in enemies3)
                    {
                        enemy.Draw(_spriteBatch);
                    }
                    break;

                //while player is in level 4
                case CurrentLevel.L4:
                    //draws level first
                    level4.Draw(_spriteBatch);
                    //then player
                    player4.Draw(_spriteBatch);
                    //then enemies
                    foreach (Enemy enemy in enemies4)
                    {
                        enemy.Draw(_spriteBatch);
                    }
                    break;

                //while player is in level 5
                case CurrentLevel.L5:
                    //draws level first
                    level5.Draw(_spriteBatch);
                    //then player
                    player5.Draw(_spriteBatch);
                    //then enemies (none)
                    break;

                //while player is in level 6
                case CurrentLevel.L6:
                    //draws level first
                    level6.Draw(_spriteBatch);
                    //then player
                    player6.Draw(_spriteBatch);
                    //then enemies
                    foreach (Enemy enemy in enemies6)
                    {
                        enemy.Draw(_spriteBatch);
                    }
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// template for level update
        /// </summary>
        /// <param name="level"></param>
        /// <param name="enemies"></param>
        /// <param name="player"></param>
        /// <param name="nextLevel"></param>
        /// <param name="gameTime"></param>
        private void LevelUpdate(Level level, List<Enemy> enemies, 
            Player player, List<Key> keys, CurrentLevel nextLevel, GameTime gameTime)
        {
            //determines the new player x and y velocity
            player.Update(gameTime, enemies);

            //updates the level position each frame
            level.Update(gameTime, player.XVelocity, player.YVelocity);

            //updates every enemy each frame
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime, (int)player.XVelocity, (int)player.YVelocity);
            }

            //update keys and collected keys
            foreach (Key k in keys)
            {
                k.UpdateAnimation(gameTime, (int)player.XVelocity, (int)player.YVelocity);
            }
            foreach (Key k in player.CollectedKeys)
            {
                k.UpdateAnimation(gameTime, (int)player.XVelocity, (int)player.YVelocity);
            }

            //checks for player collison and for if the next level should be loaded
            if (player.Collisions(level.BgLevelBlueprint, level.IntLevelBlueprint,
                level.Rows, level.Columns, enemies))
            {
                currentLevel = nextLevel;
                return;
            }
        }
    }
}