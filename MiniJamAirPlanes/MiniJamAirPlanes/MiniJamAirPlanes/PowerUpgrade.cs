using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MiniJamAirPlanes
{
    class PowerUpgrade 
    {
        Vector2 pos;
        Texture2D Sprite;
        public Rectangle CollosionRect;
        public bool Collected = false;

        public PowerUpgrade(Vector2 position, Texture2D aSprite)
        {
            pos = position;
            Sprite = aSprite;
            CollosionRect = new Rectangle((int)pos.X, (int)pos.Y, Sprite.Width, Sprite.Height);
        }

        public void Update(GameTime gameTime)
        {
            pos.X -= 1;
            CollosionRect.X = (int)pos.X;
            CollosionRect.Y = (int)pos.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(Sprite,pos, Color.White);
        }
    }
}
