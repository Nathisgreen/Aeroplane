using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MiniJamAirPlanes
{
    class Bang
    {
        Texture2D Sprite;
        Vector2 Location;

        Boolean visible = true;
        int time = 0;
        int timer = 10;
        float scale;

        public Bang(Vector2 location, Texture2D texture)
        {
            this.Location = location;
            this.Sprite = texture;
            scale = Game1.aRandom.Next(3);

        }

        public void update(GameTime gameTime)
        {
            if (time < timer)
            {
                time++;
            }
            else
            {
                visible = false;    
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                //spriteBatch.Draw(Sprite, Location, Color.White);
                spriteBatch.Draw(Sprite, Location, null, Color.White, 0f, new Vector2(Sprite.Width/2,Sprite.Height/2), scale, SpriteEffects.None, 0.1f);
            }
        }

    }
}
