using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MiniJamAirPlanes
{
    public class Explosion
    {
        int count = 0;
        int timer;
        Vector2 size;
        Vector2 position;
        List<Bang> bangList = new List<Bang>();
        Texture2D tex;
        public bool active = true;

       int livetime = 0;
        int liveTimer = 45;

        public Explosion(Vector2 aPos, Vector2 size, Texture2D bangTex)
        {
            timer = Game1.aRandom.Next(5) + 5;
            this.size = size;
            position = aPos;
            tex = bangTex;
        }

        public void Update(GameTime gameTime)
        {
            if (count < timer)
            {
                count++;
            }
            else
            {
                count = 0;
                bangList.Add(new Bang(new Vector2
                    (position.X + Game1.aRandom.Next((int)size.X),position.Y +Game1.aRandom.Next((int)size.Y)),
                    tex));
                timer = Game1.aRandom.Next(5) + 5;
            }

            if (livetime < liveTimer)
            {
                livetime++;
            }
            else
            {
                active = false;
            }

            foreach (Bang aBang in bangList)
            {
                aBang.update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bang aBang in bangList)
            {
                aBang.draw(spriteBatch);
            }
        }
    }
}
