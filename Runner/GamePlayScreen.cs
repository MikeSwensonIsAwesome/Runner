using Microsoft.Xna.Framework;
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
    class GamePlayScreen
    {
        private KeyboardState lastKBoardState;

        private Rick rick;

        private Sprite background0;
        private Sprite background1;

        private AnimatedSprite spookySkeleton;

        private SpriteFont timerFont;

        //Again, works in progress for Rick::Ani Sprite
        private Texture2D punching, empty, walking;

        //Nice for Calculating screen positions ie screenWidth * .8 positions at 80% of screen
        private const int screenWidth = 800;
        private const int screenHeight = 600;

        private static float totalTimeStart = 120; //2 Min

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
            background0 = new Sprite();
            background1 = new Sprite();

            //Pass Rows,Columns so we know how to split up the sprite sheet
            spookySkeleton = new AnimatedSprite(1, 8)
            {
                //This is the starting Position, Skeleton is big so he goes off screen
                Position = new Vector2(-300, 300)
            };

            //Has his own initialization
            rick = new Rick(1, 6);
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

            //Animated Sprites & Rick::AnimatedSprites
            spookySkeleton.LoadContent(contentManager, "spooky512Sheet");
            rick.LoadContent(contentManager);
        }
        public void GamePlayStart(GameTime gameTime)
        {
            //Controls Position on screen, 75 is about the skeleton's belly mouth
            rick.Position.X = MathHelper.Clamp(rick.Position.X, 75, 749);

            //Decrement timer to 0
            TimerHandler(gameTime);

            //Move attack jump rick
            rick.Update(gameTime);

            //Move background0 and background1 when they fall off screen
            TileBackground();

            //Animate Skeleton
            spookySkeleton.Update(gameTime, Vector2.Zero, Vector2.Zero);

            //Which way to move backgrounds and how fast
            Vector2 direction = new Vector2(-1, 0);
            Vector2 speed = new Vector2(140, 0);

            background0.Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            background1.Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState currentKboardState = Keyboard.GetState();
            //Big HEADS UP Might have to cheese it and use a Key Different than AXYB to navigate menus
            //If you assign it B for instance it will close the game, A will start the game etc.
            if (currentKboardState.IsKeyDown(Keys.Z) && lastKBoardState.IsKeyUp(Keys.Z))
            {
                Game1.CurrentGameState = Game1.GameState.startMenu;
            }
            lastKBoardState = currentKboardState;
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
            }
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

            background0.Draw(spriteBatch);
            background1.Draw(spriteBatch);

            spookySkeleton.Draw(spriteBatch, RickVector);
            if (currentKboardState.IsKeyDown(Keys.F) == true) //Needs previous keyboard state to prevent button 
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
        }

    }
}
