////////////////////////////////////////////////////////////
//マップのゴール
//作成時間：2016/10/1
//作成者：氷見悠人
////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TeamWorkGame.Actor
{
    public class Goal : GameObject
    {
        private bool isOnFire;

        public bool IsOnFire
        {
            get
            {
                return isOnFire;
            }
            set
            {
                isOnFire = value;
            }
        }

        public Goal(Vector2 pos) : base("goal", pos, Vector2.Zero, true, "Goal")
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            isOnFire = false;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void EventHandle(GameObject other)
        {
            other.Velocity = velocity;
            other.Position = new Vector2(ColRect.X + ColRect.Width/2 - other.Width/2, ColRect.Y - other.Height);
            other.IsOnGround = true;
            isOnFire = true;
        }
    }
}
