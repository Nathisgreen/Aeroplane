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

        public Bang(Vector2 location, Texture2D texture)
        {
            this.Location = location;
            this.Sprite = texture;
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
                spriteBatch.Draw(Sprite, Location, Color.White);
            }
        }

    }
}
