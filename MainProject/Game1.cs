using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace MainProject
{
    //keeps track of the current level
    enum CurrentLevel
    {
        LevelSelect,
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
        L16,
        L17,
        L18,
        L19,
        L20
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
        private Texture2D exitClosed;
        private Texture2D exitOpen;

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

        //level select
        private int textX;
        private int textY;
        private SpriteFont levelFont;
        private string finalInput;
        private KeyboardState prevKB;
        private string instructions;
        private string instructions2;

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
        private Level level11;
        private Level level12;
        private Level level13;
        private Level level14;
        private Level level15;
        private Level level16;
        private Level level17;
        private Level level18;
        private Level level19;
        private Level level20;
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
        private Player player11;
        private Player player12;
        private Player player13;
        private Player player14;
        private Player player15;
        private Player player16;
        private Player player17;
        private Player player18;
        private Player player19;
        private Player player20;
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
        private List<Enemy> enemies11;
        private List<Enemy> enemies12;
        private List<Enemy> enemies13;
        private List<Enemy> enemies14;
        private List<Enemy> enemies15;
        private List<Enemy> enemies16;
        private List<Enemy> enemies17;
        private List<Enemy> enemies18;
        private List<Enemy> enemies19;
        private List<Enemy> enemies20;
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
        private List<Key> keys11;
        private List<Key> keys12;
        private List<Key> keys13;
        private List<Key> keys14;
        private List<Key> keys15;
        private List<Key> keys16;
        private List<Key> keys17;
        private List<Key> keys18;
        private List<Key> keys19;
        private List<Key> keys20;
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
            //game starts on the level select screen
            currentLevel = CurrentLevel.LevelSelect;

            //set screen size to the size of the monitor (3840 x 2160)
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            
            _graphics.ApplyChanges();

            width = _graphics.GraphicsDevice.Viewport.Width;
            height = _graphics.GraphicsDevice.Viewport.Height;

            //level select
            textX = _graphics.GraphicsDevice.Viewport.Width / 2;
            textY = _graphics.GraphicsDevice.Viewport.Height / 2;

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
            enemies11 = new List<Enemy>();
            enemies12 = new List<Enemy>();
            enemies13 = new List<Enemy>();
            enemies14 = new List<Enemy>();
            enemies15 = new List<Enemy>();
            enemies16 = new List<Enemy>();
            enemies17 = new List<Enemy>();
            enemies18 = new List<Enemy>();
            enemies19 = new List<Enemy>();
            enemies20 = new List<Enemy>();


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
            keys11 = new List<Key>();
            keys12 = new List<Key>();
            keys13 = new List<Key>();
            keys14 = new List<Key>();
            keys15 = new List<Key>();
            keys16 = new List<Key>();
            keys17 = new List<Key>();
            keys18 = new List<Key>();
            keys19 = new List<Key>();
            keys20 = new List<Key>();

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

            exitClosed = Content.Load<Texture2D>("ExitClosed");
            bgLevelSprites.Add("exitClosed", exitClosed);

            exitOpen = Content.Load<Texture2D>("ExitOpen");
            bgLevelSprites.Add("exitOpen", exitOpen);

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
            levelFont = Content.Load<SpriteFont>("LevelFont");

            //instructions
            instructions = "Type the level you want to play! Ex: type 1 to play level 1";
            finalInput = "Level: ";
            instructions2 = "Press \"Enter\" to begin!";

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
            level7 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level7Bg.txt", "Level7Int.txt");
            level8 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level8Bg.txt", "Level8Int.txt");
            level9 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level9Bg.txt", "Level9Int.txt");
            level10 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level10Bg.txt", "Level10Int.txt");
            level11 = new Level(bgLevelSprites, intLevelSprites,
                width, height, "Level11Bg.txt", "Level11Int.txt");
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
            player11 = new Player(width / 2, height / 2, playerSprite, playerIdle, playerWalk, playerJump,
                playerHurt, playerFloat, explosion, keys11, debugFont);
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
            enemies7.Add(new Enemy(10, 1300, 3000, 1900, 2100, 1900, 6900, 1900,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level7.BgLevelBlueprint, level7.Rows, level7.Columns));
            enemies7.Add(new Enemy(12, 1300, 3500, 1900, 2100, 1900, 6900, 1900,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level7.BgLevelBlueprint, level7.Rows, level7.Columns));
            enemies7.Add(new Enemy(7, 1300, 4200, 1900, 2100, 1900, 6900, 1900,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level7.BgLevelBlueprint, level7.Rows, level7.Columns));
            enemies7.Add(new Enemy(-8, 1300, 4600, 1900, 2100, 1900, 6900, 1900,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level7.BgLevelBlueprint, level7.Rows, level7.Columns));
            enemies7.Add(new Enemy(15, 1300, 5600, 1900, 2100, 1900, 6900, 1900,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level7.BgLevelBlueprint, level7.Rows, level7.Columns));
            enemies7.Add(new Enemy(-12, 1300, 6000, 1900, 2100, 1900, 6900, 1900,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level7.BgLevelBlueprint, level7.Rows, level7.Columns));
            enemies7.Add(new Enemy(20, 1300, 6500, 1900, 2100, 1900, 6900, 1900,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level7.BgLevelBlueprint, level7.Rows, level7.Columns));
            enemies7.Add(new Enemy(-11, 1300, 4600, 1900, 2100, 1900, 6900, 1900,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level7.BgLevelBlueprint, level7.Rows, level7.Columns));
            enemies7.Add(new Enemy(-18, 1300, 5000, 1900, 2100, 1900, 6900, 1900,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level7.BgLevelBlueprint, level7.Rows, level7.Columns));
            enemies7.Add(new Enemy(5, 1000, 4600, 1200, 4100, 1200, 5400, 1200,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level7.BgLevelBlueprint, level7.Rows, level7.Columns));


            //level 8 enemies
            //(none)

            //level 9 enemies
            enemies9.Add(new Enemy(8, 1000, 4600, 1100, 4200, 1100, 6400, 1100,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level9.BgLevelBlueprint, level9.Rows, level9.Columns));

            //level 10 enemies
            //(none)

            //level 11 enemies
            enemies11.Add(new Enemy(3, 1500, 3300, 1500, 3100, 1500, 3700, 1500,
                enemyWalking, enemyShooting, bullet, explosion, debugFont, level11.BgLevelBlueprint, level11.Rows, level11.Columns));

            //level 12 enemies
            //(none)

            //level 13 enemies
            //(none)

            //level 14 enemies
            //(none)

            //level 15 enemies
            //(none)

            //level 16 enemies
            //(none)

            //level 17 enemies
            //(none)

            //level 18 enemies
            //(none)

            //level 19 enemies
            //(none)

            //level 20 enemies
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
            keys9.Add(new Key(1500, 1900, playerHurt));
            keys9.Add(new Key(1800, -600, playerHurt));
            keys9.Add(new Key(1800, -1100, playerHurt));
            keys9.Add(new Key(6800, -1300, playerHurt));
            keys9.Add(new Key(6700, 2100, playerHurt));

            //level 10 keys
            keys10.Add(new Key(5600, 500, playerHurt));
            keys10.Add(new Key(6000, 1500, playerHurt));
            keys10.Add(new Key(6700, 1700, playerHurt));
            keys10.Add(new Key(8000, 2500, playerHurt));

            //level 11 keys
            keys11.Add(new Key(3400, 0, playerHurt));

            //level 12 keys
            //(none)

            //level 13 keys
            //(none)

            //level 14 keys
            //(none)

            //level 15 keys
            //(none)

            //level 16 keys
            //(none)

            //level 17 keys
            //(none)

            //level 18 keys
            //(none)

            //level 19 keys
            //(none)

            //level 20 keys
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
                //while player is in level select
                case CurrentLevel.LevelSelect:

                    //get input from player
                    KeyboardState kbState = Keyboard.GetState();

                    //if a valid key has just been clicked, add it to their input
                    //valid keys are 0-9 and enter
                    if (kbState.IsKeyDown(Keys.D0) && prevKB.IsKeyUp(Keys.D0))
                    {
                        finalInput += "0";
                    }
                    else if (kbState.IsKeyDown(Keys.D1) && prevKB.IsKeyUp(Keys.D1))
                    {
                        finalInput += "1";
                    }
                    else if (kbState.IsKeyDown(Keys.D2) && prevKB.IsKeyUp(Keys.D2))
                    {
                        finalInput += "2";
                    }
                    else if (kbState.IsKeyDown(Keys.D3) && prevKB.IsKeyUp(Keys.D3))
                    {
                        finalInput += "3";
                    }
                    else if (kbState.IsKeyDown(Keys.D4) && prevKB.IsKeyUp(Keys.D4))
                    {
                        finalInput += "4";
                    }
                    else if (kbState.IsKeyDown(Keys.D5) && prevKB.IsKeyUp(Keys.D5))
                    {
                        finalInput += "5";
                    }
                    else if (kbState.IsKeyDown(Keys.D6) && prevKB.IsKeyUp(Keys.D6))
                    {
                        finalInput += "6";
                    }
                    else if (kbState.IsKeyDown(Keys.D7) && prevKB.IsKeyUp(Keys.D7))
                    {
                        finalInput += "7";
                    }
                    else if (kbState.IsKeyDown(Keys.D8) && prevKB.IsKeyUp(Keys.D8))
                    {
                        finalInput += "8";
                    }
                    else if (kbState.IsKeyDown(Keys.D9) && prevKB.IsKeyUp(Keys.D9))
                    {
                        finalInput += "9";
                    }
                    //player presses enter
                    else if (kbState.IsKeyDown(Keys.Enter) && prevKB.IsKeyUp(Keys.Enter))
                    {
                        if (finalInput == "Level: 0")
                        {
                            currentLevel = CurrentLevel.Test;
                        }
                        else if (finalInput == "Level: 1")
                        {
                            currentLevel = CurrentLevel.L1;
                        }
                        else if (finalInput == "Level: 2")
                        {
                            currentLevel = CurrentLevel.L2;
                        }
                        else if (finalInput == "Level: 3")
                        {
                            currentLevel = CurrentLevel.L3;
                        }
                        else if (finalInput == "Level: 4")
                        {
                            currentLevel = CurrentLevel.L4;
                        }
                        else if (finalInput == "Level: 5")
                        {
                            currentLevel = CurrentLevel.L5;
                        }
                        else if (finalInput == "Level: 6")
                        {
                            currentLevel = CurrentLevel.L6;
                        }
                        else if (finalInput == "Level: 7")
                        {
                            currentLevel = CurrentLevel.L7;
                        }
                        else if (finalInput == "Level: 8")
                        {
                            currentLevel = CurrentLevel.L8;
                        }
                        else if (finalInput == "Level: 9")
                        {
                            currentLevel = CurrentLevel.L9;
                        }
                        else if (finalInput == "Level: 10")
                        {
                            currentLevel = CurrentLevel.L10;
                        }
                        else if (finalInput == "Level: 11")
                        {
                            currentLevel = CurrentLevel.L11;
                        }
                        else if (finalInput == "Level: 12")
                        {
                            currentLevel = CurrentLevel.L12;
                        }
                        else if (finalInput == "Level: 13")
                        {
                            currentLevel = CurrentLevel.L13;
                        }
                        else if (finalInput == "Level: 14")
                        {
                            currentLevel = CurrentLevel.L14;
                        }
                        else if (finalInput == "Level: 15")
                        {
                            currentLevel = CurrentLevel.L15;
                        }
                        else if (finalInput == "Level: 16")
                        {
                            currentLevel = CurrentLevel.L16;
                        }
                        //any other value
                        else
                        {
                            instructions = "Can not find level, try again! Ex: type 1 for level 1";
                            finalInput = "Level: ";
                        }
                    }

                    //update prev kb
                    prevKB = kbState;
                    break;

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
                    LevelUpdate(level7, enemies7, player7, keys7, CurrentLevel.L8, gameTime);
                    break;

                //while player is in level 8 
                case CurrentLevel.L8:
                    LevelUpdate(level8, enemies8, player8, keys8, CurrentLevel.L9, gameTime);
                    break;

                //while player is in level 9
                case CurrentLevel.L9:
                    LevelUpdate(level9, enemies9, player9, keys9, CurrentLevel.L10, gameTime);
                    break;

                //while player is in level 10
                case CurrentLevel.L10:
                    LevelUpdate(level10, enemies10, player10, keys10, CurrentLevel.L11, gameTime);
                    break;

                //while player is in level 11
                case CurrentLevel.L11:
                    LevelUpdate(level11, enemies11, player11, keys11, CurrentLevel.L12, gameTime);
                    break;

                //while player is in level 12
                case CurrentLevel.L12:
                    //LevelUpdate(level12, enemies12, player12, keys12, CurrentLevel.L13, gameTime);
                    break;

                //while player is in level 13
                case CurrentLevel.L13:
                    //LevelUpdate(level13, enemies13, player13, keys13, CurrentLevel.L14, gameTime);
                    break;

                //while player is in level 14
                case CurrentLevel.L14:
                    //LevelUpdate(level14, enemies14, player14, keys14, CurrentLevel.L15, gameTime);
                    break;

                //while player is in level 15
                case CurrentLevel.L15:
                    //LevelUpdate(level15, enemies15, player15, keys15, CurrentLevel.L16, gameTime);
                    break;

                //while player is in level 16
                case CurrentLevel.L16:
                    //LevelUpdate(level16, enemies16, player16, keys16, CurrentLevel.L17, gameTime);
                    break;

                //while player is in level 17
                case CurrentLevel.L17:
                    //LevelUpdate(level17, enemies17, player17, keys17, CurrentLevel.L18, gameTime);
                    break;

                //while player is in level 18
                case CurrentLevel.L18:
                    //LevelUpdate(level18, enemies18, player18, keys18, CurrentLevel.L19, gameTime);
                    break;

                //while player is in level 19
                case CurrentLevel.L19:
                    //LevelUpdate(level19, enemies19, player19, keys19, CurrentLevel.L20, gameTime);
                    break;

                //while player is in level 20
                case CurrentLevel.L20:
                    //LevelUpdate(level20, enemies20, player20, keys20, CurrentLevel.L20, gameTime);
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
                //while player is in level select
                case CurrentLevel.LevelSelect:
                    _spriteBatch.DrawString(levelFont, instructions, new Vector2(textX - 1000, textY + 100), Color.White);
                    _spriteBatch.DrawString(levelFont, finalInput, new Vector2(textX - 1000, textY), Color.White);
                    _spriteBatch.DrawString(levelFont, instructions2, new Vector2(textX - 1000, textY - 100), Color.White);
                    break;
                //while player is in test level
                case CurrentLevel.Test:
                    DrawUpdate(testLevel, player, enemies);
                    break;

                //while player is in level 1
                case CurrentLevel.L1:
                    DrawUpdate(level1, player1, enemies1);
                    break;

                //while player is in level 2
                case CurrentLevel.L2:
                    DrawUpdate(level2, player2, enemies2);
                    break;

                //while player is in level 3
                case CurrentLevel.L3:
                    DrawUpdate(level3, player3, enemies3);
                    break;

                //while player is in level 4
                case CurrentLevel.L4:
                    DrawUpdate(level4, player4, enemies4);
                    break;

                //while player is in level 5
                case CurrentLevel.L5:
                    DrawUpdate(level5, player5, enemies5);
                    break;

                //while player is in level 6
                case CurrentLevel.L6:
                    DrawUpdate(level6, player6, enemies6);
                    break;

                //while player is in level 7
                case CurrentLevel.L7:
                    DrawUpdate(level7, player7, enemies7);
                    break;

                //while player is in level 8
                case CurrentLevel.L8:
                    DrawUpdate(level8, player8, enemies8);
                    break;

                //while player is in level 9
                case CurrentLevel.L9:
                    DrawUpdate(level9, player9, enemies9);
                    break;

                //while player is in level 10
                case CurrentLevel.L10:
                    DrawUpdate(level10, player10, enemies10);
                    break;

                //while player is in level 11
                case CurrentLevel.L11:
                    DrawUpdate(level11, player11, enemies11);
                    break;

                //while player is in level 12
                case CurrentLevel.L12:
                    //DrawUpdate(level12, player12, enemies12);
                    break;

                //while player is in level 13
                case CurrentLevel.L13:
                    //DrawUpdate(level13, player13, enemies13);
                    break;

                //while player is in level 14
                case CurrentLevel.L14:
                    //DrawUpdate(level14, player14, enemies14);
                    break;

                //while player is in level 15
                case CurrentLevel.L15:
                    //DrawUpdate(level15, player15, enemies15);
                    break;

                //while player is in level 16
                case CurrentLevel.L16:
                    //DrawUpdate(level16, player16, enemies16);
                    break;

                //while player is in level 17
                case CurrentLevel.L17:
                    //DrawUpdate(level17, player17, enemies17);
                    break;

                //while player is in level 18
                case CurrentLevel.L18:
                    //DrawUpdate(level18, player18, enemies18);
                    break;

                //while player is in level 19
                case CurrentLevel.L19:
                    //DrawUpdate(level19, player19, enemies19);
                    break;

                //while player is in level 20
                case CurrentLevel.L20:
                    //DrawUpdate(level20, player20, enemies20);
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

        private void DrawUpdate(Level level, Player player, List<Enemy> enemies)
        {
            //draws level first
            level.Draw(_spriteBatch, player.ExitOpen);
            //then player
            player.Draw(_spriteBatch);
            //then enemies
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(_spriteBatch);
            }
        }
    }
}