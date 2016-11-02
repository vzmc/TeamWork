//最終修正日
//by 長谷川修一 11/2

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

        //数値変更
        //by長谷川修一
        public void Draw(Renderer renderer, GameObject other)
        {
            renderer.DrawTexture("FireMeter", new Vector2(1 * 64, 3 * 64), new Rectangle((((Player)other).FireNum - 1) * 64, 0,64,192));
            renderer.DrawNumber("number", new Vector2(1 * 64 + 16, 6 * 64), ((Player)other).FireNum);
        }
    }
}
