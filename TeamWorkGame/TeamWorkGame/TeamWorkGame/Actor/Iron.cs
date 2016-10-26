/////////////////////////////////////////////////
// 鉄ブロックのクラス
// 作成時間：2016年10月12日
// By 長谷川修一
// 最終修正時間：2016年10月20日
// By 氷見悠人
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
            isShow = true;
            SetTimer(0.5f, 5f);
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

        public override void AliveEvent(GameObject other)
        {
            if (other is Fire)
            {
                other.IsDead = true;
                isShow = false;         //不可視化
            }
            if(other is Player)
            {
                isShow = false;         //Playerの場合は不可視化だけ
            }
            spawnTimer.Initialize(); //Timerを初期化して可視化するのを防ぐ
        }

        /// <summary>
        /// 描画の再定義（透明値を追加）　By　氷見悠人　2016/10/20
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="renderer"></param>
        /// <param name="offset"></param>
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset, alpha);
        }

        public override void EventHandle(GameObject other)
        {
            //火の数が5以上の時に消える
            if (other is Player && ((Player)other).FireNum > 4)
            {
                //ToDeath();
                AliveEvent(other);
            }
            //AliveEvent(other);
        }
    }
}
