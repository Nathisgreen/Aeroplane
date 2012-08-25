using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MiniJamAirPlanes
{
    class Bullet
    {
        Vector2 Location;
        Vector2 Velocity;
        Texture2D Sprite;
        public bool FiredByPLayer;
        Rectangle CollosionRect;
        public bool destroyed = false;
        

        public Bullet(Vector2 location, Vector2 velocity, Texture2D texture, bool whofired)
        {
            this.Location = location;
            this.Velocity = velocity;
            this.Sprite = texture;
            this.FiredByPLayer = whofired;
            CollosionRect = new Rectangle((int)this.Location.X, (int)this.Location.Y, Sprite.Width, Sprite.Height);
        }

        public void Update(GameTime gameTime, List<BaseEnemy> enemies)
        {
            Location += Velocity;
            CollosionRect.X = (int)Location.X;
            CollosionRect.Y = (int)Location.Y;

            if (Location.X > Game1.WindowWidth)
            {
                Trace.WriteLine("Off screen being destroyed");
                destroyed = true;
            }
            if( Location.X < 0 )
            {
                Trace.WriteLine("Off screen being destroyed");
                destroyed = true;
            }

            foreach( BaseEnemy enemy in enemies)
            {
                if (CollosionRect.Intersects(enemy.ColosionRect))
                {
                    enemy.destroyed = true;
                    destroyed = true;

                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Location, Color.White);
        }
    }
}
