using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniJamAirPlanes
{
    class Boss : BaseEnemy
    {
        public int HP = 5;

        public Boss(Vector2 location, Texture2D theTexture)
            : base(location, theTexture, 100, 0)
        {
        }

        public override void Update(GameTime gameTime, Rectangle PlayerCollsionRect)
        {
            base.Update(gameTime, PlayerCollsionRect);
        }
    }
}
