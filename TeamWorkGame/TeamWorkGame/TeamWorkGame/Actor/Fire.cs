/////////////////////////////////////////////////
// 火のクラス
// 作成時間：2016年9月25日
// By 氷見悠人
// 最終修正時間：2016年9月25日
// By 氷見悠人
/////////////////////////////////////////////////

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
    public class Fire : Character
    {
        private Map map;
        private float gForce;
        private Character onThings;


        public Character OnThings
        {
            set
            {
                onThings = value;
            }
            get
            {
                return onThings;
            }
        }

        public bool IsOnGround
        {
            get
            {
                return isOnGround;
            }
            set
            {
                isOnGround = value;
            }
        }
        public bool IsReturn { get; set; }

        public Fire(Vector2 position, Vector2 velocity) : base("fire", 33, 44, "Fire", true)
        {
            Initialize(position, velocity);
        }

        public override void Initialize(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
            gForce = Parameter.GForce;
            isOnGround = false;
            isDead = false;
            isTrigger = true;
            onThings = null;
            map = MapManager.GetNowMapData();
        }

        public override void Update(GameTime gameTime)
        {
            if (onThings == null)
            {
                velocity.Y += gForce;
                Method.MapObstacleCheck(ref position, width, height, ref velocity, ref isOnGround, map, new int[] { 0, 1, 2 });
                if (isOnGround)
                {
                    velocity = Vector2.Zero;
                }
                position += velocity;
            }
            else
            {
                velocity = onThings.GetVelocity();
                position = onThings.GetPosition() + new Vector2(onThings.GetWidth()/2 - width/2, -height);
            }
            
        }

        public override bool CollisionCheck(Character other)
        {
            bool flag = false;

            if (other.IsTrigger)
            {
                flag = base.CollisionCheck(other);
                if (flag)
                {
                    if (other is Player)
                    {
                        //IsReturn = true;
                        //if(onThings != null)
                        //{
                        //    ((Light)onThings).ChangeSate(false);
                        //}
                    }
                    else if (other is Light)
                    {
                        if (((Light)other).IsOn == false)
                        {
                            onThings = other;
                            velocity = onThings.GetVelocity();
                            position = onThings.GetPosition() + new Vector2(onThings.GetWidth() / 2 - width / 2, -height);
                            isOnGround = true;
                            ((Light)other).ChangeSate(true);
                        }

                    }
                    else if (other is Goal)
                    {
                        onThings = other;
                        ((Goal)other).IsOnFire = true;
                        velocity = onThings.GetVelocity();
                        position = onThings.GetPosition() + new Vector2(onThings.GetWidth() / 2 - width / 2, -height);
                        isOnGround = true;
                    }

                }
            }
            else
            {
                flag = base.ObstacleCheck(other);
                if (flag)
                {
                    if (other is Ice)
                    {
                        ((Ice)other).ToDeath();
                        isDead = true;
                    }
                }
            }

            return flag;
        }

        public override bool ObstacleCheck(Character other)
        {
            bool flag = false;
            if (!other.IsTrigger)
            {
                flag = base.ObstacleCheck(other);
                if (flag)
                {
                    if (other is Ice)
                    {
                        ((Ice)other).ToDeath();
                    }
                }
            }

            return flag;
        }


        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset);
        }
    }
}
