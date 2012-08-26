using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MiniJamAirPlanes
{
    class DownShotEnemy : BaseEnemy
    {
        private float shootTime = 0;
        private float ShootTimer;
        static private Random aRandom = new Random();

        public DownShotEnemy(Vector2 location, Texture2D sprite, int movementPattern, int ID = 0)
            : base(location, sprite, movementPattern, ID)
        {
            ShootTimer = aRandom.Next(60) + 60;
        }

        public override void Update(GameTime gameTime, Rectangle PlayerCollsionRect)
        {
            base.Update(gameTime, PlayerCollsionRect);

            if (shootTime < ShootTimer)
            {
                shootTime++;
            }
            else
            {
                ShootTimer = aRandom.Next(60) + 100;
                shootTime = 0;
                if (getVelocity().X < 0)
                {
                    Game1.bManager.SpawnBullet(new Vector2(this.getPosition().X + 4, this.getPosition().Y + 42), new Vector2(getVelocity().X, 5), false);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
