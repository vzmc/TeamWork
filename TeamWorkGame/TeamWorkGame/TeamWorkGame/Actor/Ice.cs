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
    public class Ice : Character
    {
        private Timer timer;
        private bool isToDeath;

        public Ice(Vector2 pos, Vector2 velo) : base("ice", 64, 64, "Ice")
        { 
            Initialize(pos, velo);
        }

        public override void Initialize(Vector2 position, Vector2 velocity)
        {
            timer = new Timer(0.1f);
            this.position = position;
            this.velocity = velocity;
            isDead = false;
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
    }
}
