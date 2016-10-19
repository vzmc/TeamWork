/////////////////////////////////////////////////
// 鉄ブロックのクラス
// 作成時間：2016年10月12日
// By 長谷川修一
// 最終修正時間：2016年10月19日
// By 佐瀬拓海
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
    public class Iron : GameObject
    {
        private Timer timer;
        private bool isToDeath;

        public Iron(Vector2 pos, Vector2 velo)
            : base("iron", new Size(64, 64), pos, velo, false, "Iron")
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            timer = new Timer(0.1f);
            isToDeath = false;
        }

        /// <summary>
        /// 死亡する時
        /// </summary>
        public void ToDeath()
        {
            if (!isToDeath)
            {
                isToDeath = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            //if (isToDeath)
            //{
            //    timer.Update();
            //    if (timer.IsTime())
            //    {
            //        isDead = true;
            //    }
            //}
            AliveUpdate();
        }

        public override void EventHandle(GameObject other)
        {
            //火の数が5以上の時に消える
            //if (other is Player && ((Player)other).FireNum > 4)
            //{
            //    ToDeath();
            //}
            AliveEvent(other);
        }
    }
}
