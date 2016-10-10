using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TeamWorkGame.Actor
{
    public class Goal : Object
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

        public Goal(Vector2 pos) : base("goal", new Size(62, 44), pos, Vector2.Zero, true, "Goal")
        {
        }

        public override void Initialize(Vector2 pos, Vector2 velo, bool isTrigger)
        {
            base.Initialize(pos, velo, isTrigger);
            isOnFire = false;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void EventHandle(Object other)
        {
            other.Velocity = velocity;
            other.Position = position + new Vector2(imageSize.Width / 2 - other.ImageSize.Width / 2, -other.ImageSize.Height);
            other.IsOnGround = true;
            isOnFire = true;
        }
    }
}
