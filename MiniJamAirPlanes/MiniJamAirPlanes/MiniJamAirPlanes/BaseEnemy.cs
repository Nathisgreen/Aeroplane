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


        private bool usePattern = false;
        private int patternSpeed = 3;
        private Vector2 finishVelocity = new Vector2(-3, 0);

        public Rectangle ColosionRect;
        public bool destroyed = false;

        public int ID = 0;

        public BaseEnemy(Vector2 location, Texture2D sprite, int movementPattern, int ID = 0)
        {
            this.Location = location;
            this.Sprite = sprite;
            this.ID = ID;
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
                wayPoints.Add(new Vector2(300+420, 320));
                wayPoints.Add(new Vector2(300+350, 250));
                wayPoints.Add(new Vector2(300+420, 230));
                wayPoints.Add(new Vector2(300+490, 320));
                usePattern = true;
            }

            if (movementPattern == 5)
            {
                wayPoints.Add(new Vector2(300+420, 320));
                wayPoints.Add(new Vector2(300+490, 250));
                wayPoints.Add(new Vector2(300+420, 280));
                wayPoints.Add(new Vector2(300+350, 320));
                usePattern = true;
            }

            if (movementPattern == 6)
            {
                wayPoints.Add(new Vector2(300+420, 320));
                wayPoints.Add(new Vector2(300+490, 250));
                wayPoints.Add(new Vector2(300+420, 230));
                wayPoints.Add(new Vector2(300+350, 320));
                usePattern = true;
            }

            if (movementPattern == 7)
            {
                wayPoints.Add(new Vector2(300+700, 0));
                wayPoints.Add(new Vector2(300+600, 100));
                wayPoints.Add(new Vector2(300+500, 200));
                wayPoints.Add(new Vector2(300+400, 300));
                wayPoints.Add(new Vector2(300+300, 400));
                wayPoints.Add(new Vector2(300+200, 300));
                usePattern = true;
                patternSpeed = 3;
            }

            if (movementPattern == 8)
            {
                wayPoints.Add(new Vector2(300+200, 500));
                wayPoints.Add(new Vector2(300+300, 400));
                wayPoints.Add(new Vector2(300+400, 300));
                wayPoints.Add(new Vector2(300+500, 200));
                wayPoints.Add(new Vector2(300+600, 100));
                wayPoints.Add(new Vector2(300+700, 0));
                usePattern = true;
                patternSpeed = 3;
            }

            if (movementPattern == 4)
            {
                usePattern = true;
                this.Velocity.X = -3;
                wayPoints.Add(new Vector2(300+320, 220));
                wayPoints.Add(new Vector2(300+520, 420));
            }

            ColosionRect = new Rectangle((int)this.Location.X, (int)this.Location.Y, sprite.Width, sprite.Height);
        }

        public virtual void Update(GameTime gameTime, Rectangle PlayerCollsionRect)
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

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (ID == 0)
            {
                spriteBatch.Draw(Sprite, Location, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);
            }
            else
            {
                spriteBatch.Draw(Sprite, Location, null, Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);
            }
        }

        public void hit()
        {
            destroyed = true;

            if (ID != 0)
            {
                bool aBool = true;
                foreach (BaseEnemy aEnemy in Game1.enemyArray)
                {
                    if (aEnemy != this)
                    {
                        if (aEnemy.ID == ID)
                        {
                            aBool = false;
                        }
                    }
                }

                if (aBool)
                {
                    Trace.WriteLine("CREATE");
                    //create power up part
                    Game1.addPowerUp(this.Location);
                }
            }
        }

        public Vector2 getPosition()
        {
            return Location;
        }

        public Texture2D getSprite()
        {
            return Sprite;
        }

        public Vector2 getVelocity()
        {
            return Velocity;
        }
    }
}
