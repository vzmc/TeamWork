using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public class Light : Character
    {
        private bool isOn;

        public bool IsOn
        {
            get
            {
                return isOn;
            }
            set
            {
                IsOn = value;
            }
        }

        public Light(Vector2 position, bool isOn = false) : base("light_off", 21, 33, "Light", true)
        {
            this.isOn = isOn;
            Initialize(position, Vector2.Zero);
        }

        public override void Initialize(Vector2 position, Vector2 velocity)
        {
            isTrigger = true;

            this.position = position;
            this.velocity = velocity;
        }

        public override void Update(GameTime gameTime)
        {
            isOn = false;
        }

        public override bool CollisionCheck(Character other)
        {
            //bool flag = false;

            //if (!isOn)
            //{
            //    if (other is Fire || other is Player)
            //    {
            //        flag = base.CollisionCheck(other);
            //        if (flag)
            //        {
            //            other.SetVelocity(velocity);
            //            other.SetPosition(position + new Vector2(width / 2 - other.GetWidth() / 2, -other.GetHeight()));
            //            //other.SetIsOnGround(true);
            //        }
            //    }
            //}

            return false;
        }



        public void ChangeSate(bool isOn)
        {
            this.isOn = isOn;
        }

    }
}
