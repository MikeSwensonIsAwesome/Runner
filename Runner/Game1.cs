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
 * Note: Resources that need to be built are Handled using Monogames 
 * Pipeline which converts all resources to .xnb files that can be built and cleaned
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
            introVideoPlaying, gamePlaying, startMenu, howToPlay,
            highScore
        }

        private VideoPlayer player;
        private Video intro;

        //Used to Switch between screens and music Works Great right now, needs to be a screenManager/Sound class 
        private static GameState currentGameState = GameState.startMenu;
        public static GameState CurrentGameState
        {
            get { return currentGameState; }
            set { currentGameState = value; }
        }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private MenuScreen menu = new MenuScreen();
        private HowToPlay hToP = new HowToPlay();
        private HighScore hScore = new HighScore();
        private GamePlayScreen gameStarter = new GamePlayScreen();

        private SoundEffect bgEffect, introEffect;//16-bit wav
        private SoundEffectInstance introMusic, game1;

        //VideoTexture is used for introVid.Draw
        private Texture2D videoTexture = null;

        //Controls Music start/stops
        private bool playIntroMovie = true;
        private bool songHasNotStarted = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            //****IMPORTANT****** Due to the way The graphicsManager displays full screen if this is on you cannot screenshot or screenrecord
            graphics.ToggleFullScreen();
            graphics.ApplyChanges();
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

            //SoundEffects used to Create SoundEffectInstances for greater sound control
            bgEffect = Content.Load<SoundEffect>("Stage4");
            introEffect = Content.Load<SoundEffect>("Metal");

            //menu bg and btns
            menu.LoadContent(this.Content);

            //HowToPlay sprite
            hToP.LoadContent(this.Content);

            //HighScore Font Writer
            hScore.LoadContent(this.Content);

            //GameStart
            gameStarter.LoadContent(this.Content);

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            game1 = bgEffect.CreateInstance();
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
                game1.Volume = 0;
                game1.Stop();
                game1.Pause();
                menu.StartMenu(gameTime, this);
                game1.Volume = 0;

            }
            if (currentGameState == GameState.howToPlay)
            {
                hToP.HowToPlayMenu(gameTime);
            }
            if (currentGameState == GameState.highScore)
            {
                hScore.HighScoreMenu(gameTime);
            }

            //Actual Gameplay stuff starts here, function and input
            if (currentGameState == GameState.gamePlaying)
            {
                if (songHasNotStarted)
                {
                    //game1 = bgEffect.CreateInstance();
                    game1.Volume = .4f;
                    game1.Play();
                    songHasNotStarted = false;
                }
                gameStarter.GamePlayStart(gameTime);
            }
            base.Update(gameTime);
        }

        //Refactored Video and Sound could probably be it's own class
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
            //Could maybe turn these into dictionaries dictionary<State, Method>
            //Tried to turn this into a switch, it ruined animations
            if (currentGameState == GameState.startMenu)
            {
                menu.Draw(this.spriteBatch);
            }

            //How to Play Screen
            if (currentGameState == GameState.howToPlay)
            {
                hToP.Draw(this.spriteBatch);
            }

            //High Score Screen
            if (currentGameState == GameState.highScore)
            {
                hScore.Draw(this.spriteBatch);
            }

            //All gameplay items
            if (currentGameState == GameState.gamePlaying)
            {
                gameStarter.Draw(this.spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

