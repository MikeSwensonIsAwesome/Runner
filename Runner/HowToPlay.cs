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
        private KeyboardState lastKBoardState;

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

        public void HowToPlayMenu(GameTime gameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            //Big HEADS UP Might have to cheese it and use a Key Different then AXYB to navigate menus
            //If you assign it B for instance it will close the game, A will start the game etc.
            if (aCurrentKeyboardState.IsKeyDown(Keys.Z) && lastKBoardState.IsKeyUp(Keys.Z))
            {
                Game1.CurrentGameState = Game1.GameState.startMenu;
            }
            lastKBoardState = aCurrentKeyboardState;
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
