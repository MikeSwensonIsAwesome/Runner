/****************************************
 *  Uses the animation tools defined by
 *  AnimatedSprite.cs and adds additional update
 *  functionality allowing "rick" to move,jump, and attack
 *  based on keyboard input.
 *  
 *  Author: Michael Swenson
 *  *************************************/

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
    class Rick : AnimatedSprite, IEntity
    {   
        //AnimatedSprite Parameters
        public Rick(int rows, int columns) : base(rows, columns){ }

        //Default Is walking
        public string RICK_SPRITE = "WalkRight";

        //Defines Position found in base class
        const int START_POSITION_X = 300;
        const int START_POSITION_Y = (int)(600 * .8);

        //How fast he moves back and forth
        const int RICK_SPEED = 100;

        //Properties for IEntity collision
        public Team Team { get; }
        Vector2 IEntity.Position
        {
            get
            {
                return Position;
            }
        }


        //Jump speed vector modifiers
        const int MOVE_UP = -2;
        const int MOVE_DOWN = 2;

        //Vector directions R/L
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;


       public enum State
        {
            Walking, Jumping, Attacking
        }

        //Default State walking
        public static State currentState = State.Walking;

        Vector2 direction = Vector2.Zero;
        Vector2 speed = Vector2.Zero;

        //Used for preventing Holding button
        KeyboardState lastKBoardState;

        Vector2 startingPos = Vector2.Zero;

        

        //Always starts at startposx and startposy
        public void LoadContent(ContentManager contentManager)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(contentManager, RICK_SPRITE);
        }
        
        //Gets called in Game1 update, moves the player, jumps, attacks
        public void Update(GameTime gameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement(aCurrentKeyboardState);

            UpdateJump(aCurrentKeyboardState);

            UpdateAttack(aCurrentKeyboardState);

            lastKBoardState = aCurrentKeyboardState;

            base.Update(gameTime, speed, direction);
        }

        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            if (currentState == State.Walking)
            {
                speed = Vector2.Zero;
                direction = Vector2.Zero;

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
                {
                    speed.X = RICK_SPEED + 60;
                    direction.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {
                    speed.X = RICK_SPEED;
                    direction.X = MOVE_RIGHT;
                }
            }
        }
        private void UpdateAttack(KeyboardState currentKeyboardState)
        {
            if (currentState == State.Walking)
            {
                if (currentKeyboardState.IsKeyDown(Keys.F) == true && lastKBoardState.IsKeyDown(Keys.F) == false)
                {
                    Attack();
                }
            }
                if (currentState == State.Attacking)
                {
                    currentState = State.Walking;
                }
        }
        private void UpdateJump(KeyboardState currentKeyboardState)
        {
            if (currentState == State.Walking)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Space) == true && lastKBoardState.IsKeyDown(Keys.Space) == false)
                {
                    Jump(); // Loooks like /\ instead of ∩
                }
            }
            if (currentState == State.Jumping)
            {
                if (startingPos.Y - Position.Y > 100)
                {
                    direction.Y = MOVE_DOWN;
                }

                if (Position.Y > startingPos.Y)
                {
                    Position.Y = startingPos.Y;
                    currentState = State.Walking;
                    direction = Vector2.Zero;
                }
            }
        }

        private void Attack()
        {
            if (currentState != State.Jumping)
            {
                currentState = State.Attacking;
                startingPos = Position;
            }
        }

        private void Jump()
        {
            if (currentState != State.Jumping)
            {
                currentState = State.Jumping;
                startingPos = Position;
                direction.Y =  MOVE_UP;
                speed = new Vector2(RICK_SPEED, RICK_SPEED);
            }
        }

        public void OnCollision(IEntity entity)
        {
            // Ignore collisions with your own arm, projectiles if we decide on doing those, etc
            if (entity.Team == this.Team)
                return;
            else
            {

            }
        }
    }
}

