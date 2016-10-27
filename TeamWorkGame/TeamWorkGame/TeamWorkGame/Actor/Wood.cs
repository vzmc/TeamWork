//木材クラス
//最終修正時間:2016年10月27日
//by 長谷川修一

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Def;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public class Wood : GameObject
    {
        private Timer timer;
        private bool isToDeath;
        private float scale;

        public Wood(Vector2 pos)
            : base("wood", new Size(64 * 1, 64 * 1), pos, Vector2.Zero, false, "Wood")
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            timer = new Timer(2.0f);
            isToDeath = false;
            scale = 1.0f;
        }

        public void ToDeath()
        {
            if(!isToDeath)
            {
                isToDeath = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (isToDeath)
            {
                timer.Update();
                if(timer.IsTime())
                {
                    IsDead = true;
                }
            }
        }

        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset, scale, alpha);
        }

        public override void EventHandle(GameObject other)
        {
            if(other is Fire)
            {
                other.IsDead = true;
            }
            name = "fire";
            isTrigger = true;
            scale = 1.5f;
            ToDeath();
        }

        public float GetScale()
        {
            return scale;
        }
    }
}
