using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace MiniJamAirPlanes
{
    public class BaseEnemy
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
        private bool xDone = false;
        private bool yDone = false;
        private bool patternDone = false;

<<<<<<< HEAD
        private bool usePattern = false;
        private int patternSpeed = 3;
        private Vector2 finishVelocity = new Vector2(-3, 0);
=======
        public Rectangle ColosionRect;
        public bool destroyed = false;
>>>>>>> origin/master

        public BaseEnemy(Vector2 location, Texture2D sprite, int movementPattern)
        {
            this.Location = location;
            this.Sprite = sprite;
            if (movementPattern != 2)
            {
                this.Velocity.X = -5;
            }
            this.movementPattern = movementPattern;

            if (movementPattern == 2)
            {
                wayPoints.Add(new Vector2(500, 500));
                wayPoints.Add(new Vector2(500,  200));
                wayPoints.Add(new Vector2(700, 200));
                usePattern = true;
            }

            if (movementPattern == 3)
            {
                wayPoints.Add(new Vector2(420, 320));
                wayPoints.Add(new Vector2(350, 250));
                wayPoints.Add(new Vector2(420, 230));
                wayPoints.Add(new Vector2(490, 320));
                usePattern = true;
            }
<<<<<<< HEAD

            if (movementPattern == 6)
            {
                wayPoints.Add(new Vector2(420, 320));
                wayPoints.Add(new Vector2(490, 250));
                wayPoints.Add(new Vector2(420, 230));
                wayPoints.Add(new Vector2(350, 320));
                usePattern = true;
            }

            if (movementPattern == 4)
            {
                usePattern = true;
                this.Velocity.X = -3;
                wayPoints.Add(new Vector2(320, 220));
                wayPoints.Add(new Vector2(520, 420));
            }
=======

            ColosionRect = new Rectangle((int)this.Location.X, (int)this.Location.Y, sprite.Width, sprite.Height);
        }

        public void Update(GameTime gameTime, Rectangle PlayerCollsionRect)
        {
>>>>>>> origin/master

            if (movementPattern == 5)
            {
                usePattern = true;
                this.Velocity.X = -3;
                wayPoints.Add(new Vector2(320, 420));
                wayPoints.Add(new Vector2(520, 220));
            }
        }

        public void Update(GameTime gameTime)
        {
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
            
            if (usePattern)
            {
                //Trace.WriteLine("Distance: " + (Math.Abs(Location.X - wayPoints[wayPointCount].X)));
                if (!patternDone)
                {
                    if (MathHelper.Distance(Location.X, wayPoints[wayPointCount].X) > 5)
                    {
                        if (Location.X < wayPoints[wayPointCount].X)
                        {
                            Velocity.X = patternSpeed;
                        }
                        else
                        {
                            Velocity.X = -patternSpeed;
                        }
                    }
                    else
                    {
                       // Location.X = wayPoints[wayPointCount].X;
                        Velocity.X = 0;
                    }
                    
                    if (MathHelper.Distance(Location.Y, wayPoints[wayPointCount].Y) > 5)
                    {
                        if (Location.Y < wayPoints[wayPointCount].Y)
                        {
                            Velocity.Y = patternSpeed;
                        }
                        else
                        {
                            Velocity.Y = -patternSpeed;
                        }
                    }
                    else
                    {
                        //Location.Y = wayPoints[wayPointCount].Y;
                        Velocity.Y = 0;
                    }
              
                    if ((MathHelper.Distance(Location.X, wayPoints[wayPointCount].X) < 10) &&
                        (MathHelper.Distance(Location.Y, wayPoints[wayPointCount].Y) < 10))
                    {
                        if (wayPointCount < wayPoints.Count-1)
                        {
                            wayPointCount++;

                        }
                        else
                        {
                            //Velocity.X = -5;
                            Velocity = finishVelocity;
                            patternDone = true;
                        }
                    }
                }
            }

            Location += Velocity;

            ColosionRect.X = (int)Location.X;
            ColosionRect.Y = (int)Location.Y;

            if (ColosionRect.Intersects(PlayerCollsionRect))
            {
                Trace.WriteLine("Poosible Collossion");
                destroyed = true;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Location, Color.White);
        }
    }
}
