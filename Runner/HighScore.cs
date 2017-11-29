using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Runner
{
    class HighScore
    {
        private Sprite highScore = new Sprite
        {
            Position = Vector2.Zero
        };
        private KeyboardState lastKBoardState;
        private StringBuilder scores;
        private SpriteFont highScoreFont;
        private bool readFile = true; //needs to be reinstated


        public HighScore()
        {
            Initialize();
        }

        private void Initialize()
        {
          // ReadScoresToSB();
        }

        private void ReadScoresToSB()
        {
            try
            {
                using (StreamReader reader = new StreamReader("Content\\ScoreRecords.txt"))
                {
                    do
                    {
                        scores.Append(reader.ReadToEnd());

                    } while (!reader.EndOfStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("High Score Reader failure");
               Console.WriteLine(ex.Message);
            }
        }

        public void HighScoreMenu(GameTime gameTime)
        {
                //Everytime we enter the Score Menu the builder scores should be updated
                //Initialize();

            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            //Big HEADS UP Might have to cheese it and use a Key Different than AXYB to navigate menus
            //If you assign it B for instance it will close the game, A will start the game etc.
            if (aCurrentKeyboardState.IsKeyDown(Keys.Z) && lastKBoardState.IsKeyUp(Keys.Z))
            {
                Game1.CurrentGameState = Game1.GameState.startMenu;
            }
            lastKBoardState = aCurrentKeyboardState;
        }

        public void LoadContent(ContentManager contentManager)
        {
            highScoreFont = contentManager.Load<SpriteFont>("Alagard"); //Gothic font
            highScore.LoadContent(contentManager, "scoreScreenBg");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            highScore.Draw(spriteBatch);
            spriteBatch.DrawString(highScoreFont, $"High Scores\n\n {DateTime.Now}", new Vector2(200, 59), 
                Color.Gold, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);

        }
    }
}

