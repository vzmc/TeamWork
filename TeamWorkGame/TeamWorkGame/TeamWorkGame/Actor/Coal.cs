///////////////////////////////
//炭のクラス
//作成時間：１０月１３日
//By　佐瀬　拓海
//最終更新日：１０月１３日
//By　佐瀬　拓海
///////////////////////////////
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamWorkGame.Device;

namespace TeamWorkGame.Actor
{
    public class Coal : GameObject
    {
        private bool isToDeath;
        public Coal(Vector2 pos, Vector2 velo):
            base("coal", new Size(64, 64), pos, velo, true, "coal")
        {
            
        }
        public override void Initialize()
        {
            base.Initialize();
            isToDeath = false;
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
                isDead = true;
            }
        }
        public override void EventHandle(GameObject other)
        {
            if(other is Player)
            {
                ToDeath();
            }
        }
    }
}
