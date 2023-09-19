using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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

        //levels
        private Level testLevel;
        private Level level1;
        private Level level2;
        private Level level3;

        //player
        private Player player;
        private Player player1;
        private Player player2;
        private Player player3;

        //lists of enemies
        private List<Enemy> enemies;
        private List<Enemy> enemies1;
        private List<Enemy> enemies2;
        private List<Enemy> enemies3;

        //lists of keys
        private List<Key> keys;
        private List<Key> keys1;
        private List<Key> keys2;
        private List<Key> keys3;

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
            currentLevel = CurrentLevel.L3;

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

            //keys
            keys = new List<Key>();
            keys1 = new List<Key>();
            keys2 = new List<Key>();
            keys3 = new List<Key>();

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

            //level loading
            testLevel = new Level(bgLevelSprites, intLevelSprites, 
                width, height, "TestLevel.txt", "TestLevelInteractables.txt");
            level1 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level1Bg.txt", "Level1Int.txt");
            level2 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level2Bg.txt", "Level2Int.txt");
            level3 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level3Bg.txt", "Level3Int.txt");

            //player loading
            player = new Player(width/2, height/2, playerSprite, playerIdle, playerWalk, playerJump, 
                playerHurt, playerFloat, explosion, keys, debugFont);
            player1 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys1, debugFont);
            player2 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys2, debugFont);
            player3 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys3, debugFont);

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
                    LevelUpdate(testLevel, enemies, player, CurrentLevel.L1, gameTime);
                    break;

                //while player is in level 1
                case CurrentLevel.L1:
                    LevelUpdate(level1, enemies1, player1, CurrentLevel.L2, gameTime);
                    break;

                //while player is in level 2
                case CurrentLevel.L2:
                    LevelUpdate(level2, enemies2, player2, CurrentLevel.L3, gameTime);
                    break;

                //while player is in level 3
                case CurrentLevel.L3:
                    LevelUpdate(level3, enemies3, player3, CurrentLevel.L4, gameTime);
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
            Player player, CurrentLevel nextLevel, GameTime gameTime)
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