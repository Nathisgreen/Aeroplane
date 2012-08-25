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

        public Player( Vector2 location, Texture2D  sprite)
        {
            this.Location = location;
            this.Sprite = sprite;
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();

            if (MovementVector.X > MaxVelocity)
                MovementVector.X = MaxVelocity;
            if (MovementVector.X < -MaxVelocity)
                MovementVector.X = -MaxVelocity;
            if (MovementVector.Y > MaxVelocity)
                MovementVector.Y = MaxVelocity;
            if (MovementVector.Y < -MaxVelocity)
                MovementVector.Y = -MaxVelocity;

            Location += MovementVector;
            
            //apply friction
            if (MovementVector.X > 0)
                MovementVector.X -= (float)0.5;
            if (MovementVector.Y > 0)
                MovementVector.Y--;
            if (MovementVector.X < 0)
                MovementVector.X++;
            if (MovementVector.Y < 0)
                MovementVector.Y++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Location, Color.White);
        }

        public void HandleInput()
        {
            if(Keyboard.GetState().IsKeyDown(Keys.W))
            {
                MovementVector.Y -= 5;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.A))
            {
                MovementVector.X -= 5;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.S))
            {
                MovementVector.Y += 5;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.D))
            {
                MovementVector.X += 1;
            }

        }
    }
}
