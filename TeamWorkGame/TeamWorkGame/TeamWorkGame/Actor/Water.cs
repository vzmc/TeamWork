////////////////////////////////////////////////////////////////
// 水クラス
// 作成者：氷見悠人
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Def;
using TeamWorkGame.Scene;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public class Water : GameObject
    {
        public Water(Vector2 pos, Vector2 velo) : base("water", pos, velo, true, "Water")
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void EventHandle(GameObject other)
        {
            if(other is Fire || other is Player)
            {
                other.IsDead = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset)
        {
            base.Draw(gameTime, renderer, offset);
        }
    }
}
