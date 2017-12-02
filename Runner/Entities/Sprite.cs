/********************************************************
 * Used for static/non animated images
 * Can be repositioned by adjusting the Position via
 * Update(), provides a shorthand method for loading 
 * content and drawing. Also gives the ability to scale
 * the sprite.
 * 
 * Author: Mike Swenson
 * *****************************************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    class Sprite
    {
        //Where the sprite is
        public Vector2 Position = new Vector2(0, 0);

        //Used in overridden draw method
        private Texture2D spriteTexture;

        //Passes resource name to create texture
        public string AssetName;

        //How big the drawn sprite is
        public Rectangle Size;

        //How big image is
        Rectangle sourceRectangle;
        public Rectangle Source
        {
            get { return sourceRectangle; }
            set
            {
                sourceRectangle = value;
                //Uses a scalar to stretch/shrink image rectangle, careful using this some images don't scale well, quality wise
                Size = new Rectangle(0, 0, (int)(sourceRectangle.Width * Scale), (int)(sourceRectangle.Height * Scale));
            }
        }

        //The amount to increase/decrease the size of the original sprite. 
        private float scale = 1.0f; //essentially percentage to scale source

        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(Source.Width * Scale), (int)(Source.Height * Scale));
            }
        }

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            spriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Source = new Rectangle(0, 0, spriteTexture.Width, spriteTexture.Height);
            Size = new Rectangle(0, 0, (int)(spriteTexture.Width * Scale), (int)(spriteTexture.Height * Scale));
        }

        //Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        //Draw the sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(spriteTexture, Position, Source,
                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

    }
}




