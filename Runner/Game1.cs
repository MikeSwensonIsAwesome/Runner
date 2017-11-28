/****************************************************************************
 * This class initializes assets, Loads assests, Updates gameplay (60ms timer),
 * Draws assests(60 ms timer) and is the base for classes
 * 
 * Uses the Xna Framework for the built-in texture and sound support
 * 
 * Author Michael Swenson
 * 
 * Last Updated 11/26/2017
 * 
 * Note: Resources are Handled using Monogames Pipeline which converts all
 * resources to .xnb files that can be built and cleaned
 * ***************************************************************************/


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Threading;

namespace Runner
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    public class Game1 : Game
    {
        public enum GameState
        {
            introVideoPlaying, gamePlaying, startMenu
        }

        VideoPlayer player;
        Video intro;

        private KeyboardState currentKboardState;

        //Used to Switch between screens and music Works Great right now, needs to be a screenManager/Sound class 
        private static GameState currentGameState = GameState.introVideoPlaying;
        public static GameState CurrentGameState
        {
            get { return currentGameState; }
            set { currentGameState = value; }
        }
        
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private MenuScreen menu = new MenuScreen();

        //private Song bgMusic, menuMetal; 
        //Extra Note: .mp3's passed through pipeline are automatically recognized as songs not effects

        private SoundEffect bgEffect, introEffect;//16-bit wav
        private SoundEffectInstance introMusic, game1;

        private Sprite background0;
        private Sprite background1;

        private AnimatedSprite spookySkeleton;

        private SpriteFont timerFont;

        //VideoTexture is used for introVid.Draw
        private Texture2D videoTexture = null;

        //Again, works in progress for Rick::Ani Sprite
        private Texture2D punching, empty, walking;

        private static float totalTimeStart = 120; //2 Min

        private Rick rick;

        //Nice for Calculating screen positions ie screenWidth * .8 positions at 80% of screen
        private const int screenWidth = 800;
        private const int screenHeight = 600;

        //Helps Position certain Draw Methods
        public static int Start_X = 100;
        public static int Start_Y = (int)(screenHeight * .7);
        public static Vector2 RickVector = new Vector2(Start_X, Start_Y);

        //Controls Music start/stops
        private bool playIntroMovie = true;
        private bool songHasNotStarted = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenWidth;  
            graphics.PreferredBackBufferHeight = screenHeight;  

            //****IMPORTANT****** Due to the way The graphicsManager displays full screen if this is on you cannot screenshot or screenrecord
            //graphics.ToggleFullScreen();
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            background0 = new Sprite();
            background1 = new Sprite();

            //Pass Rows,Columns so we know how to split up the sprite sheet
            spookySkeleton = new AnimatedSprite(1, 8)
            {
                //This is the starting Position, Skeleton is big so he goes off screen
                Position = new Vector2(-300, 300)
            };

            //Has his own initialization
            rick = new Rick(1, 6);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Video and VideoPlayer 
            intro = Content.Load<Video>("introVid");
            player = new VideoPlayer();

            //Timer Display
            timerFont = Content.Load<SpriteFont>("Alagard"); //Gothic font

            //Different Rick States, still in Progress
            punching = Content.Load<Texture2D>("Punching");
            empty = Content.Load<Texture2D>("EmptyCheese"); //1x1 pixel for disappearing
            walking = Content.Load<Texture2D>("WalkRight");

            //Basic Images, Moved by a Position Vector
            background0.LoadContent(this.Content, "FourTrees");
            background1.LoadContent(this.Content, "FourTrees"); //Tiles funky, placement or image?

            //menu buttons
            menu.LoadContent(this.Content);


            //Animated Sprites & Rick::AnimatedSprites
            spookySkeleton.LoadContent(this.Content, "spooky512Sheet");
            rick.LoadContent(this.Content);

            //SoundEffects used to Create SoundEffectInstances for greater sound control
            bgEffect = Content.Load<SoundEffect>("Stage4");
            introEffect = Content.Load<SoundEffect>("Metal");
            }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {}

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Built in Exit via Esc or Back
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Control Intro Sound and Video start/stops
            if (currentGameState == GameState.introVideoPlaying)
            {
                IntroPlayerAndSound();
            }

            //Control Menu Sound and Video start/stops //Needs Button added along with actions
            if (currentGameState == GameState.startMenu)
            {
                menu.StartMenu(gameTime);
            }

            //Actual Gameplay stuff starts here, function and input
            if (currentGameState == GameState.gamePlaying)
            {
                currentKboardState = Keyboard.GetState();
                if (songHasNotStarted)
                {
                    game1 = bgEffect.CreateInstance();
                    game1.Play();
                    songHasNotStarted = false;
                }
                //Controls Position on screen, 75 is about the skeleton's belly mouth
                rick.Position.X = MathHelper.Clamp(rick.Position.X, 75, 749);

                //Decrement timer to 0
                TimerHandler(gameTime);

                //Move attack jump rick
                rick.Update(gameTime);

                //Move background0 and background1 when they fall off screen
                TileBackground();

                //Animate Skeleton
                spookySkeleton.Update(gameTime, Vector2.Zero, Vector2.Zero);

                //Which way to move backgrounds and how fast
                Vector2 direction = new Vector2(-1, 0);
                Vector2 speed = new Vector2(140, 0);

                background0.Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                background1.Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            base.Update(gameTime);
        }

        private void IntroPlayerAndSound()
        {
            if (playIntroMovie == true)
            {
                introMusic = introEffect.CreateInstance();
                introMusic.Play();
                player.Volume = 0;
                player.Play(intro);
                playIntroMovie = false;
            }

            if (player.State == MediaState.Stopped)
            {
                player.Stop();
                videoTexture = null;
                currentGameState = GameState.startMenu;
                introMusic.Stop();
            }
        }

        private static void TimerHandler(GameTime gameTime)
        {
            if (totalTimeStart > 0)
            {
                totalTimeStart -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                totalTimeStart = 0;
            }
        }

        private void TileBackground()
        {
            if (background0.Position.X < -background0.Size.Width + 900)
            {
                background1.Position.X = background0.Position.X + background0.Size.Width;
            }
            if (background1.Position.X < -background1.Size.Width + 900)
            {
                background0.Position.X = background1.Position.X + background1.Size.Width;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //Videoplayer control sets videoTexture and it is null video stops playing
            if (currentGameState == GameState.introVideoPlaying)
            {
                if (player.State != MediaState.Stopped)
                    videoTexture = player.GetTexture();

                if (videoTexture != null)
                {
                    spriteBatch.Draw(videoTexture, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                }
            }
            
            //Just draws the menu animation, needs buttons here
            if(currentGameState == GameState.startMenu)
            {
                menu.Draw(this.spriteBatch);
            }

            //All gameplay items here needs work on rickPunching
            if(currentGameState == GameState.gamePlaying)
            {
                background0.Draw(this.spriteBatch);
                background1.Draw(this.spriteBatch);

                spookySkeleton.Draw(this.spriteBatch, RickVector);
                if(currentKboardState.IsKeyDown(Keys.F) == true) //Needs previous keyboard state to prevent button 
                {
                    spriteBatch.Draw(punching, new Rectangle(rick.Position.ToPoint(), new Point(70, 70)),Color.White);
                    rick.Draw(this.spriteBatch, RickVector, empty);
                }
                rick.Draw(this.spriteBatch, RickVector, walking);

                //Timer
                spriteBatch.DrawString(timerFont, $"Time Remaining: {(int)totalTimeStart / 60}:{totalTimeStart % 60:00.000}", new Vector2(10, 70), //70 is inbetween Leaves of trees
                    Color.Yellow, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

/********************
 * PEBBLE DISTRIBUTION
 * MICHAEL SWENSON: 50
 * JARRED SROUFE:   50
 * ******************/
