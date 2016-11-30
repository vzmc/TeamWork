/////////////////////////////////////////////////
// 看板クラス
// 作成時間：2016年11月30日
// By 佐瀬拓海
// 最終修正時間：2016年11月30日
// By 佐瀬拓海
/////////////////////////////////////////////////
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamWorkGame.Device;

namespace TeamWorkGame.Actor
{
    class Sign : GameObject
    {
        public Sign(Vector2 pos)
            : base("sign", pos, Vector2.Zero, true, "Sign")
        {
        }
        public override void Initialize()
        {
            base.Initialize();

        }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override void EventHandle(GameObject other)
        {
            isDead = true;
        }

    }
}
