/*************************************************
 * Divides up a large image into equal size sections
 * these are the frames.  These frames are then moved
 * incrementally one after the other to make the image appear
 * as if it is moving.  It requires how many rows and columns
 * the large image should be broken into.
 * 
 * Author: Mike Swenson
 * ****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Runner
{
    class AnimatedSprite
    {
        /***********************************/
        private Vector2 Velocity;

        public Vector2 Position;
        /********************************/

        //All these are used to Divy up a sprite sheet and move which image is drawn
        /********************************/
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;

        private int totalFrames;

        private int timeSinceLastFrame = 0;

        private int milliscondsPerFrame = 100;

        //Used in all sprite stuff
        /*********************************/
        private Texture2D spriteTexture;

        public Rectangle Size;

        private float spriteScalar = 1.0f;
        /********************************/

        //Initializes all the fields needed to animate
        public AnimatedSprite(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            Velocity = Vector2.Zero;
        }

        //Changes Source size
        public float SpriteScalar
        {
            get { return spriteScalar; }
            set
            {
                spriteScalar = value;
                Size = new Rectangle(0, 0, (int)(spriteTexture.Width * SpriteScalar), (int)(spriteTexture.Height * SpriteScalar));
            }
        }

        //Loads in assest by name and creates a rectangle to hold sprite
        public void LoadContent(ContentManager contentManager, string artName)
        {
            spriteTexture = contentManager.Load<Texture2D>(artName);
            Size = new Rectangle(0, 0, (int)(spriteTexture.Width * SpriteScalar), (int)(spriteTexture.Height * SpriteScalar));
        }

        //Updates Position AND changes the frame based on time between frames, increments the frame and then loops the animation
        public void Update(GameTime gameTime, Vector2 speed, Vector2 theDirection)
        {
            Position += theDirection * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > milliscondsPerFrame)
            {
                timeSinceLastFrame -= milliscondsPerFrame;

                currentFrame++;
                timeSinceLastFrame = 0;

                if (currentFrame == totalFrames)
                {
                    currentFrame = 0;
                }
            }
        }

        //Defines Position and uses the size of the texture to build a rectangle and location to define a rectangle destination
        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = spriteTexture.Width / Columns;
            int height = spriteTexture.Height / Rows;
            int row = (int)(currentFrame / (float)Columns);
            int column = currentFrame % Columns;
            location = Position;

             Rectangle sourceRectangle = new Rectangle(width * column, height * row, width - 4, height); //Minus four because apparently I am bad at image editing
             Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(spriteTexture, destinationRectangle, sourceRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
        }

        //Experimenting to try to get rick to Punch by passing different textures to this draw
        public void Draw(SpriteBatch spriteBatch, Vector2 location, Texture2D skeletonSpecial)
        {
            spriteTexture = skeletonSpecial;
            int width = spriteTexture.Width / Columns;
            int height = spriteTexture.Height / Rows;
            int row = (int)(currentFrame / (float)Columns);
            int column = currentFrame % Columns;
            location = Position;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width - 4, height); //Minus four because apparently I am bad at image editing
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(spriteTexture, destinationRectangle, sourceRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
