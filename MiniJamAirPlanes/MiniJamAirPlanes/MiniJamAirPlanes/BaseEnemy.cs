using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MiniJamAirPlanes
{
    class BaseEnemy
    {
        Vector2 Location;
        Texture2D Sprite;
        Vector2 Velocity = new Vector2(0, 0);
        int MaxVelocity = 10;
        private int movementPattern;

        private Boolean dir = false;
        private int changeTime = 0;

        public BaseEnemy(Vector2 location, Texture2D sprite, int movementPattern)
        {
            this.Location = location;
            this.Sprite = sprite;
            this.Velocity.X = -5;
            this.movementPattern = movementPattern;
        }

        public void Update(GameTime gameTime)
        {
            Location += Velocity;

            if (Location.X < -100)
            {
                Location.X = 800;
            }

            if (movementPattern == 1)
            {
                if (changeTime < 60)
                {
                    changeTime++;
                }
                else
                {
                    dir = !dir;
                    changeTime = 0;
                }

                if (dir)
                {
                    if (Velocity.Y < 1)
                    {
                        Velocity.Y += 0.1f;
                    }
                }
                else
                {
                    if (Velocity.Y > -1)
                    {
                        Velocity.Y -= 0.1f;
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Location, Color.White);
        }
    }
}
