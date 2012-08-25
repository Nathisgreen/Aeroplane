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

        private Boolean dir = true;
        private int changeTime = 0;

        private List<Vector2> wayPoints = new List<Vector2>();
        private int wayPointCount = 0;

        public BaseEnemy(Vector2 location, Texture2D sprite, int movementPattern)
        {
            this.Location = location;
            this.Sprite = sprite;
            this.Velocity.X = -5;
            this.movementPattern = movementPattern;

            if (movementPattern == 2)
            {
                wayPoints.Add(new Vector2(500, 500));
                wayPoints.Add(new Vector2(500,  200));
                wayPoints.Add(new Vector2(700, 200));
            }
        }

        public void Update(GameTime gameTime)
        {

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
                    if (Velocity.Y < 3)
                    {
                        Velocity.Y += 0.1f;
                    }
                }
                else
                {
                    if (Velocity.Y > -3)
                    {
                        Velocity.Y -= 0.1f;
                    }
                }
            }

            if (movementPattern == 2)
            {
                if (MathHelper.Distance(Location.X,wayPoints[wayPointCount].X) < 1)
               // if (Math.Abs(Location.X - wayPoints[wayPointCount].X) > 50)
                {
                    if (Location.X > wayPoints[wayPointCount].X)
                    {
                        Location.X -= 5;
                    }
                    else
                    {
                        Location.X += 5;
                    }
                }

                if (MathHelper.Distance(Location.Y, wayPoints[wayPointCount].Y) < 1)
                {
                    if (Location.Y > wayPoints[wayPointCount].Y)
                    {
                        Location.Y -= 5;
                    }
                    else
                    {
                        Location.Y += 5;
                    }
                }

                if ((MathHelper.Distance(Location.X,wayPoints[wayPointCount].X) < 1) && (MathHelper.Distance(Location.Y, wayPoints[wayPointCount].Y) < 1))
                {
                    if (wayPointCount < wayPoints.Count)
                    {
                        wayPointCount++;
                    }
                }

            }

            Location += Velocity;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Location, Color.White);
        }
    }
}
