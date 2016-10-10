using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public class Light : Object
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

        public Light(Vector2 pos, bool isOn = false) : base("light_off", new Size(21, 33), pos, Vector2.Zero, true, "Light")
        {
            this.isOn = isOn;
        }

        public override void Initialize(Vector2 pos, Vector2 velo, bool isTrigger)
        {
            base.Initialize(pos, velo, isTrigger);
        }

        public override void Update(GameTime gameTime)
        {
            isOn = false;
        }

        public void ChangeSate(bool isOn)
        {
            this.isOn = isOn;
        }

        public override void EventHandle(Object other)
        {
            other.Velocity = velocity;
            other.Position = position + new Vector2(imageSize.Width / 2 - other.ImageSize.Width / 2, -other.ImageSize.Height);
            other.IsOnGround = true;
            isOn = true;
        }
    }
}
