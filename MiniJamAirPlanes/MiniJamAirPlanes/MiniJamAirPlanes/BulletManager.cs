using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniJamAirPlanes
{
    public class BulletManager
    {
        List<Bullet> bullets;
        Texture2D BulletTexture;

        public BulletManager(Texture2D texture)
        {
            bullets = new List<Bullet>();
            BulletTexture = texture;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Bullet thebullet in bullets)
            {
                thebullet.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet thebullet in bullets)
            {
                thebullet.Draw(spriteBatch);
            }
        }

        public void SpawnBullet(Vector2 Location, Vector2 velocity, bool playerfired)
        {
            bullets.Add( new Bullet(Location, velocity, BulletTexture, playerfired));
        }
    }
}