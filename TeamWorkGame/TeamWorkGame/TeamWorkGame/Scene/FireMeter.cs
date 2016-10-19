//最終修正日
//by 長谷川修一 10/19

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Actor;

namespace TeamWorkGame.Scene
{
    class FireMeter
    {

        public FireMeter()
        {

        }

        public void Initialize()
        {

        }

        public void Update()
        {

        }

        public void Draw(Renderer renderer, GameObject other)
        {
            renderer.DrawTexture("FireMeter", new Vector2(1 * 64, 5 * 64), new Rectangle((((Player)other).FireNum - 1) * 64, 0,64,64));
            renderer.DrawNumber("number", new Vector2(1 * 64 + 16, 6 * 64), ((Player)other).FireNum);
        }
    }
}
