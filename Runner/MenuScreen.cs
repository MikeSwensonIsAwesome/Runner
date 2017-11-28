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
        private SoundEffect backgroundMenuMusic;
        private SoundEffectInstance backgroundMusicInstance;

        public bool menuMusicNotPlaying = true;

        public MenuScreen()
        {
            Initialize();
        }

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
                Position = new Vector2(0, 0)
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
        }

        public void StartMenu(GameTime gameTime)
        {
            backgroundMusicInstance = backgroundMenuMusic.CreateInstance();
            if (menuMusicNotPlaying)
            {
                backgroundMusicInstance.Volume = .4f;
                backgroundMusicInstance.Play();
                menuMusicNotPlaying = false;
            }

            menuBackground.Update(gameTime, Vector2.Zero, Vector2.Zero);
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                backgroundMusicInstance.Stop();
                backgroundMenuMusic.Dispose();
                Game1.CurrentGameState = Game1.GameState.gamePlaying;
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
