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
    class Lake : GameObject
    {
        public Lake(Vector2 pos)
            : base("water", pos, Vector2.Zero, true, "Lake")
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
            if(other is Fire ||other is Player)
            {
                other.IsDead = true;
            }
        }
    }
}
