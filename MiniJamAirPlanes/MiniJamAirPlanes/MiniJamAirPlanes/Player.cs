﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MiniJamAirPlanes
{
    class Player
    {
        Vector2 Location;
        Texture2D Sprite;
        Vector2 MovementVector = new Vector2(0, 0);
        int MaxMaxVelocity = 10;
        int CurrentMaxVelocity = 2;
        int VelocityChange = 2;
        float CurrentVelocityChangePerStep = 1.0f;
        float Friction = 0.5f;
        bool CanFire = true;
        int Shots = 1;
        float DefaultShotDelay = 0.5f;
        float ShotDelay;
        float LastShot = 0.0f;
        float MinShotDelay = 0.1f;
        float ShotDelayChange = 0.1f;
        Rectangle CollosionRect;
        int HasShield = 0;
        bool Dead = false;
        bool Hit = false;
        float HitTime = 0.0f;
        float HitTimer = 0.5f;
        public int PowerupsCollected = 1;
        KeyboardState previousState;
        float Depth = 0.2f;
        float ShieldDepth = 0.19f;
        Vector2 ShieldLocation;
        Color ShieldColor = Color.Red;
        Vector2 defaultLocation;
        int deadTime = 0;
        int deadtimer = 90;


        public Player( Vector2 location, Texture2D  sprite)
        {
            this.Location = location;
            defaultLocation = location;
            this.Sprite = sprite;
            ShieldLocation = new Vector2(Location.X - 14, Location.Y - 32);
            CollosionRect = new Rectangle((int)this.Location.X, (int)this.Location.Y, sprite.Width, sprite.Height);
            ShotDelay = DefaultShotDelay;
        }

        public Vector2 GetLocation
        {
            get { return Location; }
        }

        public Rectangle GetCollosionRect
        {
            get { return CollosionRect; }
        }

        public int PlayerShield
        {
            get { return HasShield; }
        }

        public int PlayerShots
        {
            get { return Shots; }
        }

        public void Update(GameTime gameTime, List<BaseEnemy> enemies, List<PowerUpgrade> powerups, List<Bullet> bullets)
        {
            if (Dead == false)
            {
                HandleInput(gameTime);

                // Limit Speed
                if (MovementVector.X > CurrentMaxVelocity)
                    MovementVector.X = CurrentMaxVelocity;
                if (MovementVector.X < -CurrentMaxVelocity)
                    MovementVector.X = -CurrentMaxVelocity;
                if (MovementVector.Y > CurrentMaxVelocity)
                    MovementVector.Y = CurrentMaxVelocity;
                if (MovementVector.Y < -CurrentMaxVelocity)
                    MovementVector.Y = -CurrentMaxVelocity;

                // move
                Location += MovementVector;

                // Limit to screen
                if (Location.X < 0)
                    Location.X = 0;
                if (Location.Y < 0)
                    Location.Y = 0;
                if (Location.X > Game1.WindowWidth - Sprite.Width)
                    Location.X = Game1.WindowWidth - Sprite.Width;
                if (Location.Y > (Game1.WindowHeight - Sprite.Height) - 32)
                    Location.Y = (Game1.WindowHeight - Sprite.Height) - 32;

                // keep collosion rect in sync
                CollosionRect.X = (int)Location.X;
                CollosionRect.Y = (int)Location.Y;

                //apply friction
                if (MovementVector.X > 0)
                    MovementVector.X -= Friction;
                if (MovementVector.Y > 0)
                    MovementVector.Y -= Friction;
                if (MovementVector.X < 0)
                    MovementVector.X += Friction;
                if (MovementVector.Y < 0)
                    MovementVector.Y += Friction;

                foreach (BaseEnemy enemy in enemies)
                {
                    if (Hit == false)
                    {
                        if (CollosionRect.Intersects(enemy.ColosionRect))
                        {
                            enemy.hit();
                            //enemy.destroyed = true;
                            if (HasShield > 0)
                            {
                                HasShield--;

                            }
                            else
                            {
                                Hit = true;
                                HitTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                            }

                        }
                    }
                }

                foreach (PowerUpgrade powup in powerups)
                {
                    if (CollosionRect.Intersects(powup.CollosionRect))
                    {
                        powup.Collected = true;
                        PowerupsCollected++;
                    }
                }

                foreach (Bullet theBullet in bullets)
                {
                    if (theBullet.FiredByPLayer == false)
                    {
                        if (theBullet.CollosionRect.Intersects(CollosionRect))
                        {
                            if (HasShield > 0)
                            {
                                HasShield--;
                            }
                            else
                            {
                                Hit = true;
                                HitTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                            }
                            theBullet.destroyed = true;

                        }
                    }
                }

                if (CanFire == false)
                {
                    LastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (LastShot > ShotDelay)
                        CanFire = true;
                }

                if (Hit == true)
                {
                    HitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (HitTime > HitTimer)
                        Hit = false;

                    Dead = true;
                }
            }

            if (Dead)
            {
                if (deadTime < deadtimer)
                {
                    deadTime++;
                }
                else
                {
                    
                    resetPlayer();
                }
            }

            ShieldLocation.X = Location.X - 10;
            ShieldLocation.Y = Location.Y - 32;
            switch (HasShield)
            {
                case 1: ShieldColor = Color.Red;
                    break;
                case 2: ShieldColor = Color.Yellow;
                    break;
                case 3: ShieldColor = Color.Green;
                    break;
                default: ShieldColor = Color.White;
                    break;
            }
            previousState = Keyboard.GetState();
                
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Dead == false)
            {
                if (Hit)
                {

                }
                else
                {
                    spriteBatch.Draw(Sprite, Location, null, Color.White, 0f, Game1.vectorZero, 1f, SpriteEffects.None, Depth);

                    if (HasShield > 0)
                    {
                        spriteBatch.Draw(Game1.sheildText, ShieldLocation, null, ShieldColor, 0f, Game1.vectorZero, 1f, SpriteEffects.None, ShieldDepth);
                    }
                }
            }
            else
            {
                    spriteBatch.DrawString(Game1.Size8, "N00B!!!!!", new Vector2(140, 200), Color.Red, 0f, Game1.vectorZero, 15.0f, SpriteEffects.None, 0);
            }
        }

        public void HandleInput(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                MovementVector.Y -= CurrentVelocityChangePerStep;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                MovementVector.X -= CurrentVelocityChangePerStep;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                MovementVector.Y += CurrentVelocityChangePerStep;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                MovementVector.X += CurrentVelocityChangePerStep;
            }

            if (KeyPressed(Keys.PageUp))
            {
                if (PowerupsCollected < 5)
                {
                    PowerupsCollected++;
                }
                else
                {
                    PowerupsCollected = 0;
                }
            }

            if (KeyPressed(Keys.Space))
            {
                if (CanFire == true)
                {
                    if (Shots == 1)
                    {
                        Game1.bManager.SpawnBullet(new Vector2(Location.X + Sprite.Width, Location.Y + 16), new Vector2(6, 0), true);
                        CanFire = false;
                        LastShot = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    if (Shots == 2)
                    {
                        Game1.bManager.SpawnBullet(new Vector2(Location.X + Sprite.Width, Location.Y + 8), new Vector2(6, 0), true);
                        Game1.bManager.SpawnBullet(new Vector2(Location.X + Sprite.Width, Location.Y + 32), new Vector2(6, 0), true);
                        CanFire = false;
                        LastShot = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    if (Shots == 3)
                    {
                        Game1.bManager.SpawnBullet(new Vector2(Location.X + Sprite.Width, Location.Y + 8), new Vector2(6, -0.05f), true);
                        Game1.bManager.SpawnBullet(new Vector2(Location.X + Sprite.Width, Location.Y + 20), new Vector2(6, 0), true);
                        Game1.bManager.SpawnBullet(new Vector2(Location.X + Sprite.Width, Location.Y + 32), new Vector2(6, 0.05f), true);
                        CanFire = false;
                        LastShot = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
            }

            if (KeyPressed(Keys.Enter))
            {
                if (PowerupsCollected > 0)
                {
                    switch (PowerupsCollected)
                    {
                        case 1:
                            CurrentMaxVelocity += VelocityChange;
                            if (CurrentMaxVelocity > MaxMaxVelocity)
                                CurrentMaxVelocity = MaxMaxVelocity;
                            PowerupsCollected = 0;
                            break;

                        case 2:
                            ShotDelay -= ShotDelayChange;
                            if (ShotDelay < MinShotDelay)
                                ShotDelay = MinShotDelay;
                            PowerupsCollected = 0;
                            break;
                        
                        case 3:
                            if (HasShield < 3)
                            {
                                HasShield++;
                                PowerupsCollected = 0;
                            }
                            break;

                        case 4:
                            if (Shots < 3)
                            {
                                Shots++;
                                PowerupsCollected = 0;
                            }
                            break;
                    }
                }
            }

        }

        private bool KeyPressed(Keys key)
        {
            if (Keyboard.GetState().IsKeyDown(key) && previousState.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void resetPlayer()
        {
            HasShield = 0;
            PowerupsCollected = 0;
            Shots = 1;
            ShotDelay = DefaultShotDelay;
            CurrentMaxVelocity = 2;
            Dead = false;
            Location = defaultLocation;
            Game1.CheckPoint();
            deadTime = 0;
            Hit = false;

        }
    }
}
