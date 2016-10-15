////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 松明クラス
// 作成者：氷見悠人
/////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public class Light : GameObject
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

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            isOn = false;
        }

        public override void EventHandle(GameObject other)
        {
            if (other is Fire || other is Player)
            {
                if (!isOn)
                {
                    other.Velocity = velocity;
                    other.Position = position + new Vector2(imageSize.Width / 2 - other.ImageSize.Width / 2, -other.ImageSize.Height + 10);
                    other.IsOnGround = true;
                    isOn = true;
                }
            }
        }
    }
}
