﻿////////////////////////
//   動く松明クラス
//   作成者　葉梨竜太
//   2016年11月9日
////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public class MoveLight : GameObject
    {
        private bool isOn;
        private Vector2 startpos;
        private Vector2 endpos;
        private int speed;

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
        public MoveLight(Vector2 startpos, Vector2 endpos, int speed, bool isOn = false) : base("light_off", startpos, Vector2.Zero, true, "MoveLight")
        {
            this.startpos = startpos;
            this.endpos = endpos;
            this.speed = speed;
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        protected override Rectangle InitLocalColRect()
        {
            return new Rectangle(25, 34, 14, 30);
        }

        public override void Update(GameTime gameTime)
        {
            Move();
            isOn = false;
        }

        public override void EventHandle(GameObject other)
        {
            if (other is Fire || other is Player)
            {
                if (!isOn)
                {
                    other.Velocity = Vector2.Zero;
                    other.Position = new Vector2(ColRect.Left + ColRect.Width / 2 - other.Width / 2, ColRect.Top - other.ColRect.Height - other.LocalColRect.Top);
                    other.IsOnGround = true;
                    isOn = true;
                }
            }
        }
        public void Move()
        {
            velocity = endpos - startpos;
            velocity.Normalize();
            velocity *= speed;
            position += velocity;

            if (position.X > endpos.X || position.X < startpos.X)
            {
                speed = speed * -1;
            }
            if (position.Y < startpos.Y || position.Y > endpos.Y)
            {
                speed = speed * -1;
            }
        }
    }
}
