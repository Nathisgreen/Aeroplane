using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniJamAirPlanes
{
    class Bullet
    {
        Vector2 Location;
        Vector2 Velocity;
        Texture2D Sprite;
        bool FiredByPLayer;
        

        public Bullet(Vector2 location, Vector2 velocity, Texture2D texture, bool whofired)
        {
            this.Location = location;
            this.Velocity = velocity;
            this.Sprite = texture;
            this.FiredByPLayer = whofired;
        }

        public void Update(GameTime gameTime)
        {
            Location += Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Location, Color.White);
        }
    }
}
