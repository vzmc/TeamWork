/////////////////////////////////////////////////
// 火のクラス
// 作成時間：2016年9月25日
// By 氷見悠人
// 最終修正時間：2016年10月09日
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
    public class Fire : Object
    {
        private Map map;
        private float gForce;

        public Fire(Vector2 position, Vector2 velocity) : base("fire", new Size(33, 44), position, velocity, true, "Fire")
        {
        }

        public override void Initialize(Vector2 position, Vector2 velocity, bool isTrigger)
        {
            base.Initialize(position, velocity, isTrigger);
            gForce = Parameter.GForce;
            map = MapManager.GetNowMapData();
        }

        /// <summary>
        /// 衝突区域判定
        /// </summary>
        /// <param name="other">対象</param>
        /// <returns></returns>
        public override bool CollisionCheck(Object other)
        {
            bool flag = false;

            if (other.IsTrigger)
            {
                flag = base.CollisionCheck(other);

                if (flag)
                {
                    //if (other is Light)
                    //{
                    //    if (((Light)other).IsOn == false)
                    //    {
                    //        velocity = other.Velocity;
                    //        position = other.Position + new Vector2(other.ImageSize.Width / 2 - imageSize.Width / 2, -imageSize.Height);
                    //        isOnGround = true;
                    //        ((Light)other).ChangeSate(true);
                    //    }
                    //}
                    //else if (other is Goal)
                    //{
                    //    ((Goal)other).IsOnFire = true;
                    //    velocity = other.Velocity;
                    //    position = other.Position + new Vector2(other.ImageSize.Width / 2 - imageSize.Width / 2, -imageSize.Height);
                    //    isOnGround = true;
                    //}
                    other.EventHandle(this);
                }
            }

            return flag;
        }

        public override bool ObstacleCheck(Object other)
        {
            bool flag = false;
            if (!other.IsTrigger)
            {
                flag = base.ObstacleCheck(other);
                if (flag)
                {
                    //if (other is Ice)
                    //{
                    //    ((Ice)other).ToDeath();
                    //}
                    other.EventHandle(this);
                }
            }

            return flag;
        }


        public override void Update(GameTime gameTime)
        {
            velocity.Y += gForce;

            Method.MapObstacleCheck(ref position, colSize.Width, colSize.Height, ref velocity, ref isOnGround, map, new int[] { 0, 1, 2 });

            //マップ上の物と障害物判定
            foreach (var m in map.MapThings.FindAll(x => !x.IsTrigger))
            {
                ObstacleCheck(m);
            }

            if (isOnGround)
            {
                velocity = Vector2.Zero;
            }

            position += velocity;

            //マップ上の物と衝突区域判定
            foreach (var m in map.MapThings.FindAll(x => x.IsTrigger))
            {
                CollisionCheck(m);
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset);
        }

        public override void EventHandle(Object other)
        {
            if(other is Player)
            {
                ((Player)other).FireNum++;
            }
            isDead = true;
        }
    }
}
