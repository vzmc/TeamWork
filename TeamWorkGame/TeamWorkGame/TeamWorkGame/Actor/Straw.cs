/////////////////////////////////////////////////
// 藁のクラス
// 作成時間：2016年10月12日
// By 長谷川修一
// 最終修正時間：2016年10月19日
// By 長谷川修一
/////////////////////////////////////////////////

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
    public class Straw : GameObject
    {
        private Timer timer;
        private bool isToDeath;
        private float scale;
        public Straw(Vector2 pos)
            : base("straw", new Size(64 * 1, 64 * 1), pos, Vector2.Zero,false, "Straw")
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
            if (!isToDeath)
            {
                isToDeath = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (isToDeath)
            {
                timer.Update();
                if (timer.IsTime())
                {
                    IsDead = true;
                }

            }
        }

        public override void EventHandle(GameObject other)
        {
            if (other is Fire)
            {
                other.IsDead = true;

            }
            name = "fire";
            IsTrigger = true;
            scale = 1.5f;
            ToDeath();
        }

        public float GetScale()
        {
            return scale;
        }
    }
}
