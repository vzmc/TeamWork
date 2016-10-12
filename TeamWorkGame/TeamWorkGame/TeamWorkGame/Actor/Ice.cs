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
        private Timer timer;
        private bool isToDeath;
        private float alpha;

        public Ice(Vector2 pos, Vector2 velo) : base("ice", new Size(64, 64), pos, velo, false, "Ice")
        { 
        }

        public override void Initialize()
        {
            base.Initialize();
            timer = new Timer(1.0f);
            isToDeath = false;
            alpha = 1.0f;
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
            if(isToDeath)
            {
                timer.Update();
                alpha -= 0.1f;
                if(timer.IsTime())
                {
                    isDead = true;
                }
            }
        }

        public override void EventHandle(GameObject other)
        {
            if(other is Fire)
            {
                other.IsDead = true;
            }
            ToDeath();
        }
        /// <summary>
        /// 透明値をゲットする
        /// </summary>
        /// <returns></returns>
        public float GetAlpha()
        {
            return alpha;
        }
    }
}
