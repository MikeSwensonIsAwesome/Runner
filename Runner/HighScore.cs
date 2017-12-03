/***********************************************
 * This class reads in two ints from a file in the form
 *  #,# then converts it to a struct Record List for 
 *  the ability to sort these records based on player#
 *  or collision# 
 * 
 * Author: Michael Swenson, Jerrad Sroufe
 * 
 * ************************************************/

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
        //Used to Control what is Displayed, without it only appears on key press
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
        //Needed different access so that any time you leave HowToPlay it will refresh when entered again
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

        /// <summary>
        /// Reads text file from path into a 
        /// string List, this happens during initialize.
        /// List has to be cleared otherwise going in and out of the 
        /// Menu leads to duplicates, could've been a set maybe?
        /// </summary>
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

        /// <summary>
        /// Read Everything into a string list, then convert to a Record list
        /// String list is nice for debugging file reading
        /// </summary>
        /// <param name="gameTime"></param>
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

        /// <summary>
        /// Different button Presses Call different LINQ 
        /// sortings and displays
        /// </summary>
        /// <param name="currentGamePadState"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// All LoadContents are called in Game1 class
        /// </summary>
        /// <param name="contentManager"></param>
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
            //After breaking other Draw method with switch just stuck with if/elseif
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