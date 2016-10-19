//////////////////////////////////////////////////////////////////////////////
// 氷クラス
// 作成者：氷見悠人
// 最終修正時間：2016/10/19
// By 佐瀬拓海
////////////////////////////////////////////////////////////////////////////

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
    public class Ice : GameObject
    {
        private bool isToDeath;

        public Ice(Vector2 pos, Vector2 velo) : base("ice", new Size(64, 64), pos, velo, false, "Ice")
        { 
        }

        public override void Initialize()
        {
            base.Initialize();
            isToDeath = false;
            isShow = true;      //初期値はtrue by佐瀬拓海
            SetTimer(0.5f, 1.0f);
        }

        /// <summary>
        /// 死亡開始
        /// </summary>
        public void ToDeath()
        {
            if(!isToDeath)
            {
                isToDeath = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            AliveUpdate();
        }

        public override void EventHandle(GameObject other)
        {
            AliveEvent(other);
        }
    }
}
