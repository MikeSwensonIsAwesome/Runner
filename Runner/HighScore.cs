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
        private GamePadState lastGamePadState = GamePad.GetState(PlayerIndex.One);

        private SpriteFont highScoreFont;
        internal static bool readFile = true;
        public static bool ReadFile { get { return ReadFile; } set { readFile = value; } }
        static string curFile = @"Content\ScoreRecords.txt";
        string highscores = File.ReadAllText(curFile);
        private IList<string> backupScores = new List<string>();
        private IList<Record> tokenedList = new List<Record>();
        private ScoreDisplays currentDisplay = ScoreDisplays.CollisionDesc;

        private void Initialize()
        {
            ReadScoresToSB();
            RawTxtToRecordsList();
        }

        private void ReadScoresToSB()
        {
            backupScores.Clear();
            tokenedList.Clear();
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
            GamePadState currentGamePadState = GamePad.GetState(PlayerIndex.One);

            if (currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed && lastGamePadState.Buttons.RightShoulder == ButtonState.Released)
            {
                Game1.CurrentGameState = Game1.GameState.startMenu;
            }

            currentGamePadState = ControlDisplaySort(currentGamePadState);

            lastGamePadState = currentGamePadState;
        }

        private GamePadState ControlDisplaySort(GamePadState currentGamePadState)
        {
            if (currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                currentDisplay = ScoreDisplays.CollisionAsc;
            }
            else if (currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                currentDisplay = ScoreDisplays.CollisionDesc;
            }
            else if (currentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                currentDisplay = ScoreDisplays.PlayerAsc;
            }
            else if (currentGamePadState.Buttons.LeftStick == ButtonState.Pressed)
            {
                currentDisplay = ScoreDisplays.PlayerDesc;
            }

            return currentGamePadState;
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