using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TeamWorkGame.Actor
{
    public class Goal : Character
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

        public Goal(Vector2 position) : base("goal", 62, 44, "Goal", true)
        {

            Initialize(position, Vector2.Zero);
        }

        public override void Initialize(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
            isOnFire = false;
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
