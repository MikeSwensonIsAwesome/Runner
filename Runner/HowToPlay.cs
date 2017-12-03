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
    class HowToPlay
    {
        private Sprite howToPlay;
        private GamePadState lastGamePadState = GamePad.GetState(PlayerIndex.One);


        public HowToPlay()
        {
            Initialize();
        }

        private void Initialize()
        {
            howToPlay = new Sprite
            {
                Position = Vector2.Zero
            };
        }
        /// <summary>
        /// All Right Button Actions Navigate back to Menu
        /// </summary>
        /// <param name="gameTime"></param>
        public void HowToPlayMenu(GameTime gameTime)
        {
            GamePadState currentGamePadState = GamePad.GetState(PlayerIndex.One);
            if (currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed && lastGamePadState.Buttons.RightShoulder == ButtonState.Released)
            {
                Game1.CurrentGameState = Game1.GameState.startMenu;
            }
            lastGamePadState = currentGamePadState;
        }

        public void LoadContent(ContentManager contentManager)
        {
            howToPlay.LoadContent(contentManager, "howToPlay");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            howToPlay.Draw(spriteBatch);
        }
    }
}
