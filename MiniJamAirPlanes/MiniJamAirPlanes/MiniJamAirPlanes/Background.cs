using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace MiniJamAirPlanes
{
    class Background
    {
        private Texture2D sprite;
        private Vector2 position;
        private float speed;
        private Boolean dir = false;
        private int change = 0;
        public Background(Vector2 position, Texture2D tex, float aSpeed, bool dir)
        {
            sprite = tex;
            this.position = position;
            speed = aSpeed;
            this.dir = dir;
        }

        public void update(GameTime gameTime)
        {
            if (position.X + sprite.Width < 20)
            {
                position.X = Game1.WindowWidth;
            }
            else
            {
                position.X -= speed;
            }

            if (change < 50)
            {
                change++;
            }
            else
            {
                change = 0;
                dir = !dir;
            }
            
            if (dir)
            {
                position.Y += speed / 40;
            }
            else
            {
                position.Y -= speed / 40;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
