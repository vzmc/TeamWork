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
    public class Ice : Object
    {
        private Timer timer;
        private bool isToDeath;

        public Ice(Vector2 pos, Vector2 velo) : base("ice", new Size(64, 64), pos, velo, false, "Ice")
        { 
        }

        public override void Initialize(Vector2 pos, Vector2 velo, bool isTrigger)
        {
            base.Initialize(pos, velo, isTrigger);
            timer = new Timer(0.1f);
            isToDeath = false;
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
            if(isToDeath)
            {
                timer.Update();
                if(timer.IsTime())
                {
                    isDead = true;
                }
            }
        }

        public override void EventHandle(Object other)
        {
            if(other is Fire)
            {
                other.IsDead = true;
            }
            ToDeath();
        }
    }
}
