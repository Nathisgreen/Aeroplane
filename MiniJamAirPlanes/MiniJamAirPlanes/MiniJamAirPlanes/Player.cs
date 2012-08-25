using System;
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
        int MaxVelocity = 10;
        float VelocityChangePerStep = (float)1.0;
        float Friction = (float)0.5;
        bool CanFire = true;
        float ShotDelay = 0.25f;
        float LastShot = 0.0f;


        public Player( Vector2 location, Texture2D  sprite)
        {
            this.Location = location;
            this.Sprite = sprite;
        }

        public void Update(GameTime gameTime)
        {
            HandleInput(gameTime);

            // Limit Speed
            if (MovementVector.X > MaxVelocity)
                MovementVector.X = MaxVelocity;
            if (MovementVector.X < -MaxVelocity)
                MovementVector.X = -MaxVelocity;
            if (MovementVector.Y > MaxVelocity)
                MovementVector.Y = MaxVelocity;
            if (MovementVector.Y < -MaxVelocity)
                MovementVector.Y = -MaxVelocity;

            // move
            Location += MovementVector;

            // Limit to screen
            if (Location.X < 0)
                Location.X = 0;
            if (Location.Y < 0)
                Location.Y = 0;
            if (Location.X > Game1.WindowWidth - Sprite.Width)
                Location.X = Game1.WindowWidth - Sprite.Width;
            if (Location.Y > Game1.WindowHeight - Sprite.Height)
                Location.Y = Game1.WindowHeight - Sprite.Height;

            //apply friction
            if (MovementVector.X > 0)
                MovementVector.X -= Friction;
            if (MovementVector.Y > 0)
                MovementVector.Y -= Friction;
            if (MovementVector.X < 0)
                MovementVector.X += Friction;
            if (MovementVector.Y < 0)
                MovementVector.Y += Friction;

            if (CanFire == false)
            {
                LastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (LastShot > ShotDelay)
                    CanFire = true;
            }
                
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Location, Color.White);
        }

        public void HandleInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                MovementVector.Y -= VelocityChangePerStep;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                MovementVector.X -= VelocityChangePerStep;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                MovementVector.Y += VelocityChangePerStep;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                MovementVector.X += VelocityChangePerStep;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (CanFire == true)
                {
                    Game1.bManager.SpawnBullet(new Vector2(Location.X + Sprite.Width, Location.Y + 16), new Vector2(5, 0), true);
                    CanFire = false;
                    LastShot = (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

        }
    }
}
