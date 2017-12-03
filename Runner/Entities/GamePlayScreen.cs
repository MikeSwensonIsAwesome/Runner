using Microsoft.Xna.Framework;
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
    class GamePlayScreen 
    {
        private KeyboardState lastKBoardState;
        GamePadState lastGamePadState = GamePad.GetState(PlayerIndex.One);
        private Rick rick;

        private Vector2 direction = new Vector2(-1, 0);
        private Vector2 speed = new Vector2(140, 0);
        private Sprite background0;
        private Sprite background1;

        private AnimatedSprite spookySkeleton;
        private AnimatedSprite flyer; 
        private SpriteFont timerFont;

        private Texture2D punching, empty, walking;

        //Nice for Calculating screen positions ie screenWidth * .8 positions at 80% of screen
        private const int screenWidth = 800;
        private const int screenHeight = 600;

        public static int collisions;
        private static int player = 6;

        private static float totalTimeStart = 120; //2 Min
        private IList<Sprite> jumpObstacles = new List<Sprite>();
        Random random = new Random();

        //Helps Position certain Draw Methods
        public static int Start_X = 100;
        public static int Start_Y = (int)(screenHeight * .7);
        public static Vector2 RickVector = new Vector2(Start_X, Start_Y);

        public GamePlayScreen()
        {
            Initialize();
        }
        private void Initialize()
        {
            PopulateJumpObs(jumpObstacles);
            background0 = new Sprite();
            background1 = new Sprite();

            flyer = new AnimatedSprite(1, 3)
            {
                Position = new Vector2(815, 200)
            };

            //Pass Rows,Columns so we know how to split up the sprite sheet
            spookySkeleton = new AnimatedSprite(1, 8)
            {
                //This is the starting Position, Skeleton is big so he goes off screen
                Position = new Vector2(-300, 215)
            };
            //Has his own initialization
            rick = new Rick(1, 6);
        }
        private IList<Sprite> PopulateJumpObs(IList<Sprite> jumpers)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 randomXVec = RandomStartPos(514);
                jumpers.Add(new Sprite { Position = randomXVec });
            }
            return jumpers;
        }

        private Vector2 RandomStartPos(int y)
        {
            int randomXPos = random.Next(820, 1800) + random.Next(1, 3) * 40;
            Vector2 randomXVec = new Vector2(randomXPos, y);
            return randomXVec;
        }

        public void LoadContent(ContentManager contentManager)
        {
            //Timer Display
            timerFont = contentManager.Load<SpriteFont>("Alagard"); //Gothic font

            //Different Rick States, still in Progress
            punching = contentManager.Load<Texture2D>("Punching");
            empty = contentManager.Load<Texture2D>("EmptyCheese"); //1x1 pixel for disappearing
            walking = contentManager.Load<Texture2D>("WalkRight");

            //Basic Images, Moved by a Position Vector
            background0.LoadContent(contentManager, "FourTrees");
            background1.LoadContent(contentManager, "FourTrees");

            foreach (Sprite jumper in jumpObstacles)
            {
                jumper.LoadContent(contentManager, "jumpObstacle");
            }

            //Animated Sprites & Rick::AnimatedSprites
            spookySkeleton.LoadContent(contentManager, "spooky512Sheet");
            flyer.LoadContent(contentManager, "flyingObstacle");
            rick.LoadContent(contentManager);
        }
        public void GamePlayStart(GameTime gameTime)
        {
            //Controls Position on screen, 75 is about the skeleton's belly mouth
            rick.Position.X = MathHelper.Clamp(rick.Position.X, 75, 749);

            CollisionChecker();
            //Decrement timer to 0
            TimerHandler(gameTime);

            if (rick.Position.X <= 75)
            {
                collisions++;
            }
            //Move attack jump rick
            rick.Update(gameTime);

            //Animate Skeleton
            spookySkeleton.Update(gameTime, Vector2.Zero, Vector2.Zero);
            flyer.Update(gameTime, Vector2.Zero, Vector2.Zero);

            //Which way to move backgrounds and how fast

            background0.Position += MoveSprite(gameTime, direction, speed);
            background1.Position += MoveSprite(gameTime, direction, speed);
            flyer.Position += MoveSprite(gameTime, direction, speed * new Vector2(2, 0));
            for (int i = 0; i < jumpObstacles.Count; i++)
            {
                jumpObstacles[i].Position += MoveSprite(gameTime, direction, speed);
                TileObstacles(jumpObstacles[i]);
            }

            TileBackground();
            TileObstacles(flyer);

            KeyboardState currentKboardState = Keyboard.GetState();
            GamePadState currentGamePadState = GamePad.GetState(PlayerIndex.One);

            if ((currentKboardState.IsKeyDown(Keys.Z) && lastKBoardState.IsKeyUp(Keys.Z)) ||
                currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed && lastGamePadState.Buttons.RightShoulder == ButtonState.Released)
            {
                Game1.CurrentGameState = Game1.GameState.startMenu;
            }
            lastKBoardState = currentKboardState;
            lastGamePadState = currentGamePadState;
        }

        private void CollisionChecker()
        {
            foreach (Sprite s in jumpObstacles)
            {
                Rectangle rickR = new Rectangle((int)rick.Position.X, (int)rick.Position.Y, rick.Size.Width / 5, rick.Size.Height);
                Rectangle sRect = new Rectangle((int)s.Position.X, (int)s.Position.Y, s.Size.Width, s.Size.Height);
                Rectangle flyerR = new Rectangle((int)flyer.Position.X, (int)flyer.Position.Y, flyer.Size.Width / 3, flyer.Size.Height);
                if (rickR.Intersects(sRect))
                {
                    s.Position.X = 0;
                    collisions++;
                }
                if (rickR.Intersects(flyerR))
                {
                    flyer.Position.X = -25;
                    collisions += 10;
                }
            }
        }

        private void TileObstacles(Sprite sprite)
        {
            if (sprite.Position.X < -screenWidth - 25)
            {
                sprite.Position = RandomStartPos(514);
            }
        }

        private void TileObstacles(AnimatedSprite aniSprite)
        {
            if (aniSprite.Position.X < -screenWidth - 25)
            {
                aniSprite.Position = RandomStartPos(225);
            }
        }

        private static Vector2 MoveSprite(GameTime gameTime, Vector2 direction, Vector2 speed)
        {
            return direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                GameOver();

                Game1.CurrentGameState = Game1.GameState.startMenu;
            }
        }

        private static void GameOver()
        {
            try
            {
                string path = @"Content\ScoreRecords.txt";
                string line = $"{player}, {collisions}";
                using (StreamWriter writer = new StreamWriter(path, true)/*File.AppendText(path)*/)
                {
                    writer.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("gps Reader failure");
                Console.WriteLine(ex.Message);
            }
            HighScore.ReadFile = true;
            collisions = 0;
            totalTimeStart = 120;
            player++;

        }

        private void TileBackground()
        {
            if (background0.Position.X < -background0.Size.Width + screenWidth)
            {
                background1.Position.X = background0.Position.X + background0.Size.Width;
            }
            if (background1.Position.X < -background1.Size.Width + screenWidth)
            {
                background0.Position.X = background1.Position.X + background1.Size.Width;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            KeyboardState currentKboardState = Keyboard.GetState();
            GamePadState currentGamePadState = GamePad.GetState(PlayerIndex.One);

            background0.Draw(spriteBatch);
            background1.Draw(spriteBatch);

            foreach (Sprite jumper in jumpObstacles)
            {
                jumper.Draw(spriteBatch);
            }

            flyer.Draw(spriteBatch, Vector2.Zero);

            spookySkeleton.Draw(spriteBatch, RickVector);

            if (currentKboardState.IsKeyDown(Keys.F) == true || currentGamePadState.Buttons.X == ButtonState.Pressed) //Needs previous keyboard state to prevent button 
            {
                spriteBatch.Draw(punching, new Rectangle(rick.Position.ToPoint(), new Point(70, 70)), Color.White);
            }

            else
            {
                rick.Draw(spriteBatch, RickVector, walking);
            }

            //Timer
            spriteBatch.DrawString(timerFont, $"Time Remaining: {(int)totalTimeStart / 60}:{totalTimeStart % 60:00.000}", new Vector2(10, 70), //70 is inbetween Leaves of trees
               Color.Yellow, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);

            spriteBatch.DrawString(timerFont, $"Collisions: {collisions}", new Vector2(700, 70), //70 is inbetween Leaves of trees
               Color.Yellow, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);


        }

    }
}
