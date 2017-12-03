/******************************************
 * Uses game controller input to navigate 
 * the different screens available in the game
 *  Does so by changing the game state on button press
 *  An instance of this is created and called in Game1
 *  
 *  Author: Michael Swenson
 *  
 *  *******************************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    class MenuScreen 
    {
        private Sprite startButton, exitButton, howToPlayButton, highScoreButton;
        private AnimatedSprite menuBackground;

        //May not need to be public/static Desperate Times
        public static SoundEffect backgroundMenuMusic, gameBgEffect;
        public static SoundEffectInstance backgroundMusicInstance, gameBgInstance;
        //Should be Properties
        public static bool songHasNotStarted = true;

        public static bool menuMusicNotPlaying = true;

        public MenuScreen()
        {
            Initialize();
        }
        
        /// <summary>
        /// Scale them slightly down
        /// Numbers are taken from positioning in
        /// Gimp 
        /// </summary>
        private void Initialize()
        {
            startButton = new Sprite
            {
                Position = new Vector2(157, 3),
                Scale = .8f
            };
            howToPlayButton = new Sprite
            {
                Position = new Vector2(86, 172),
                Scale = .8f
            };
            highScoreButton = new Sprite
            {
                Position = new Vector2(86, 358),
                Scale = .8f
            };
            exitButton = new Sprite
            {
                Position = new Vector2(173, 529),
                Scale = .8f
            };
            menuBackground = new AnimatedSprite(3, 3)
            {
                Position = Vector2.Zero
            };
        }

        public void LoadContent(ContentManager contentManager)
        {
            startButton.LoadContent(contentManager, "tStartButton");
            exitButton.LoadContent(contentManager, "tExitButton");
            highScoreButton.LoadContent(contentManager, "tHighScoreButton");
            howToPlayButton.LoadContent(contentManager, "tHowToPlayButton");

            menuBackground.LoadContent(contentManager, "menuScreen");

            backgroundMenuMusic = contentManager.Load<SoundEffect>("password");
            gameBgEffect = contentManager.Load<SoundEffect>("Stage4");
        }
        /// <summary>
        /// This is where we navigate between all the different screens
        /// I did have some issues with the Soundeffects starting and stopping
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="game"></param>
        public void StartMenu(GameTime gameTime, Game1 game)
        {
            gameBgInstance = gameBgEffect.CreateInstance();
            backgroundMusicInstance = backgroundMenuMusic.CreateInstance();
            if (menuMusicNotPlaying)
            {
                gameBgInstance.Stop();
                gameBgInstance.Volume = 0;
                gameBgEffect.Dispose();

                backgroundMusicInstance.Volume = .4f;
                backgroundMusicInstance.IsLooped = true;
                backgroundMusicInstance.Play();
                menuMusicNotPlaying = false;
            }
            menuBackground.Update(gameTime, Vector2.Zero, Vector2.Zero);

            if (Keyboard.GetState().IsKeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                backgroundMusicInstance.Stop();
                backgroundMenuMusic.Dispose();


                if (songHasNotStarted)
                {
                    gameBgInstance.Volume = .4f;
                    gameBgInstance.IsLooped = true;
                    gameBgInstance.Play();
                    songHasNotStarted = false;
                }
                Game1.CurrentGameState = Game1.GameState.gamePlaying;

            } else if (Keyboard.GetState().IsKeyDown(Keys.X) || GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)
            {
                Game1.CurrentGameState = Game1.GameState.howToPlay;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Y) || GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
            {
                Game1.CurrentGameState = Game1.GameState.highScore;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.B) || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
            {
                game.Exit();
            }
        }
        public void Update(GameTime gameTime)
        {
            menuBackground.Update(gameTime,Vector2.Zero, Vector2.Zero );
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            menuBackground.Draw(spriteBatch, Vector2.Zero);
            startButton.Draw(spriteBatch);
            howToPlayButton.Draw(spriteBatch);
            highScoreButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
        }
    }
}
