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
        //springs
        private Texture2D leftSpring;
        private Texture2D rightSpring;
        private Texture2D upSpring;
        private Texture2D downSpring;
        private Texture2D upLeftSpring;
        private Texture2D upRightSpring;
        private Texture2D downLeftSpring;
        private Texture2D downRightSpring;
        //player
        private Texture2D playerSprite;

        //list of sprites needed for level loading
        private Dictionary<string, Texture2D> levelSprites;

        //width and height of screen
        private int width;
        private int height;

        //levels
        private Level testLevel;

        //player
        private Player player;

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

            levelSprites = new Dictionary<string, Texture2D>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //sprites
            #region spriteLoading
            background = Content.Load<Texture2D>("Background");
            levelSprites.Add("background", background);

            leftPlat = Content.Load<Texture2D>("LeftPlat");
            levelSprites.Add("leftPlat", leftPlat);

            centerPlat = Content.Load<Texture2D>("CenterPlat");
            levelSprites.Add("centerPlat", centerPlat);

            rightPlat = Content.Load<Texture2D>("RightPlat");
            levelSprites.Add("rightPlat", rightPlat);

            floor = Content.Load<Texture2D>("Floor");
            levelSprites.Add("floor", floor);

            spawn = Content.Load<Texture2D>("Spawn");
            levelSprites.Add("spawn", spawn);

            wall = Content.Load<Texture2D>("Wall");
            levelSprites.Add("wall", wall);

            leftSpring = Content.Load<Texture2D>("LeftSpring");
            levelSprites.Add("leftSpring", leftSpring);

            rightSpring = Content.Load<Texture2D>("RightSpring");
            levelSprites.Add("rightSpring", rightSpring);

            upSpring = Content.Load<Texture2D>("UpSpring");
            levelSprites.Add("upSpring", upSpring);

            downSpring = Content.Load<Texture2D>("DownSpring");
            levelSprites.Add("downSpring", downSpring);

            upLeftSpring = Content.Load<Texture2D>("UpLeftSpring");
            levelSprites.Add("upLeftSpring", upLeftSpring);

            upRightSpring = Content.Load<Texture2D>("UpRightSpring");
            levelSprites.Add("upRightSpring", upRightSpring);

            downLeftSpring = Content.Load<Texture2D>("DownLeftSpring");
            levelSprites.Add("downLeftSpring", downLeftSpring);

            downRightSpring = Content.Load<Texture2D>("DownRightSpring");
            levelSprites.Add("downRightSpring", downRightSpring);

            playerSprite = Content.Load<Texture2D>("Player");
            #endregion

            //fonts
            debugFont = Content.Load<SpriteFont>("DebugFont");

            //level loading
            testLevel = new Level(levelSprites, width, height, "TestLevel.txt");

            //player loading
            player = new Player(width/2, height/2, playerSprite, debugFont);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //checks for player collison with the level
            player.Collisions(testLevel.LevelBlueprint, testLevel.Rows, testLevel.Columns);
            //determines the new player x and y velocity
            player.Update(gameTime);

            //updates the level position each frame
            testLevel.Update(gameTime, player.XVelocity, player.YVelocity);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            testLevel.Draw(_spriteBatch);
            player.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}