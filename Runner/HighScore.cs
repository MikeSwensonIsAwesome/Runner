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
        public enum ScoreDisplays
        {
            PlayerAsc, PlayerDesc, CollisionAsc, CollisionDesc
        }
        private Sprite highScore = new Sprite
        {
            Position = Vector2.Zero
        };
        private KeyboardState lastKBoardState;
        private SpriteFont highScoreFont;
        private bool readFile = true;
        static string curFile = @"Content\ScoreRecords.txt";
        string highscores = File.ReadAllText(curFile);
        private IList<string> backupScores = new List<string>();
        private IList<Record> tokenedList = new List<Record>();
        private ScoreDisplays currentDisplay;

        private void Initialize()
        {
            ReadScoresToSB();
            RawTxtToRecordsList();
        }

        private void ReadScoresToSB()
        {
            try
            {
                using (StreamReader reader = new StreamReader(@"Content\ScoreRecords.txt"))
                {
                    do
                    {
                        string line = reader.ReadLine();
                        backupScores.Add(line + Environment.NewLine);
                    } while (!reader.EndOfStream || reader.ReadLine() != null);
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
            if (readFile)
            {
                Initialize();
                readFile = false;
            }

            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            if (aCurrentKeyboardState.IsKeyDown(Keys.Z) && lastKBoardState.IsKeyUp(Keys.Z))
            {
                Game1.CurrentGameState = Game1.GameState.startMenu;
            }

            aCurrentKeyboardState = ControlDisplaySort(aCurrentKeyboardState);

            lastKBoardState = aCurrentKeyboardState;
        }

        private KeyboardState ControlDisplaySort(KeyboardState aCurrentKeyboardState)
        {
            if (aCurrentKeyboardState.IsKeyDown(Keys.P) == true)
            {
                currentDisplay = ScoreDisplays.CollisionAsc;
            }
            else if (aCurrentKeyboardState.IsKeyDown(Keys.O) == true)
            {
                currentDisplay = ScoreDisplays.CollisionDesc;
            }
            else if (aCurrentKeyboardState.IsKeyDown(Keys.I) == true)
            {
                currentDisplay = ScoreDisplays.PlayerAsc;
            }
            else if (aCurrentKeyboardState.IsKeyDown(Keys.U) == true)
            {
                currentDisplay = ScoreDisplays.PlayerDesc;
            }

            return aCurrentKeyboardState;
        }

        public void LoadContent(ContentManager contentManager)
        {
            highScoreFont = contentManager.Load<SpriteFont>("Alagard"); //Gothic font
            highScore.LoadContent(contentManager, "scoreScreenBg");
        }
        private string TextToDisplay()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in backupScores)
            {
                sb.Append(s);
            }
            return sb.ToString();
        }
        private string DisplayCollisDesc()
        {
            IEnumerable<Record> results =
                (from a in tokenedList
                 select a).OrderByDescending(x => x.Collisions);
            StringBuilder sb = new StringBuilder();
            foreach (Record s in results)
            {
                sb.AppendLine(s.ToString());
            }
            return sb.ToString();
        }

        private string DisplayCollisAsc()
        {
            IEnumerable<Record> results =
                (from a in tokenedList
                 select a).OrderBy(x => x.Collisions);
            StringBuilder sb = new StringBuilder();
            foreach (Record s in results)
            {
                sb.AppendLine(s.ToString());
            }
            return sb.ToString();
        }

        private string DisplayPlayerDesc()
        {
            IEnumerable<Record> results =
                (from a in tokenedList
                 select a).OrderByDescending(x => x.Player);
            StringBuilder sb = new StringBuilder();
            foreach (Record s in results)
            {
                sb.AppendLine(s.ToString());
            }
            return sb.ToString();
        }
        private string DisplayPlayer()
        {
            IEnumerable<Record> results =
                (from a in tokenedList
                 select a).OrderBy(x => x.Player);
            StringBuilder sb = new StringBuilder();
            foreach (Record s in results)
            {
                sb.AppendLine(s.ToString());
            }
            return sb.ToString();
        }


        private void RawTxtToRecordsList()
        {
            foreach (string s in backupScores)
            {
                string[] tokenHolder = s.Split(',');
                tokenedList.Add(new Record(Convert.ToInt32(tokenHolder[0]), Convert.ToInt32(tokenHolder[1])));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            KeyboardState currentState = Keyboard.GetState();
            highScore.Draw(spriteBatch);
            if (currentDisplay == ScoreDisplays.CollisionDesc)
            {
                spriteBatch.DrawString(highScoreFont, $"High Scores\n\n{DisplayCollisDesc()}", new Vector2(208, 59),
                    Color.Gold, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
            } else if (currentDisplay == ScoreDisplays.CollisionAsc)
            {
                spriteBatch.DrawString(highScoreFont, $"High Scores\n\n{DisplayCollisAsc()}", new Vector2(208, 59),
                    Color.Gold, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
            }
            else if (currentDisplay == ScoreDisplays.PlayerDesc)
            {
                spriteBatch.DrawString(highScoreFont, $"High Scores\n\n{DisplayPlayerDesc()}", new Vector2(208, 59),
                    Color.Gold, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
            }
            else if (currentDisplay == ScoreDisplays.PlayerAsc)
            {
                spriteBatch.DrawString(highScoreFont, $"High Scores\n\n{DisplayPlayer()}", new Vector2(208, 59),
                    Color.Gold, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
            }

        }
    }
}

//sexy sexy test code
//{(File.Exists(curFile)? "File exists." : "File does not exist.")}