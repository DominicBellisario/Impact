using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MainProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

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

        //player
        private Texture2D playerSprite;

        //enemies
        private Texture2D enemyWalking;
        private Texture2D enemyShooting;

        //bullets
        private Texture2D bullet;

        //list of sprites needed for level loading
        private Dictionary<string, Texture2D> bgLevelSprites;
        private Dictionary<string, Texture2D> intLevelSprites;

        //width and height of screen
        private int width;
        private int height;

        //levels
        private Level testLevel;

        //player
        private Player player;

        //list of enemies
        private List<Enemy> enemies;

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

            //set screen size to the size of the monitor (3840 x 2160)
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            
            _graphics.ApplyChanges();

            width = _graphics.GraphicsDevice.Viewport.Width;
            height = _graphics.GraphicsDevice.Viewport.Height;

            bgLevelSprites = new Dictionary<string, Texture2D>();
            intLevelSprites = new Dictionary<string, Texture2D>();
            enemies = new List<Enemy>();
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

            //player sprite
            playerSprite = Content.Load<Texture2D>("Player");
            #endregion

            //enemy sprites
            enemyWalking = Content.Load<Texture2D>("EnemyWalking");
            enemyShooting = Content.Load<Texture2D>("EnemyShooting");

            //bullet sprite
            bullet = Content.Load<Texture2D>("Bullet");

            //fonts
            debugFont = Content.Load<SpriteFont>("DebugFont");

            //level loading
            testLevel = new Level(bgLevelSprites, intLevelSprites, 
                width, height, "TestLevel.txt", "TestLevelInteractables.txt");

            //player loading
            player = new Player(width/2, height/2, playerSprite, debugFont);

            //test level enemies
            enemies.Add(new Enemy(5, 1000, 4000, -400, 3500, -400, 4300, -400,
                enemyWalking, enemyShooting, bullet, debugFont, testLevel.BgLevelBlueprint, 50, 50));
            enemies.Add(new Enemy(3, 1000, 4900, -900, 4600, -900, 5200, -900,
                enemyWalking, enemyShooting, bullet, debugFont, testLevel.BgLevelBlueprint, 50, 50));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //checks for player collison
            player.Collisions(testLevel.BgLevelBlueprint, testLevel.IntLevelBlueprint, 
                testLevel.Rows, testLevel.Columns, enemies);
            //determines the new player x and y velocity
            player.Update(gameTime);

            //updates the level position each frame
            testLevel.Update(gameTime, player.XVelocity, player.YVelocity);

            //updates every enemy each frame
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime, (int)player.XVelocity, (int)player.YVelocity);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //draws level first
            testLevel.Draw(_spriteBatch);
            //then player
            player.Draw(_spriteBatch);
            //then enemies
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}