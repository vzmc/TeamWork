/////////////////////////////////////////////////
// 火のクラス
// 作成時間：2016年9月25日
// By 氷見悠人
// 最終修正時間：2016年10月13日
// アニメーションの準備　By 氷見悠人　
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
    public class Fire : GameObject
    {
        private Map map;        //マップ情報
        private float gForce;   //重力
        private Motion motion;  //アニメーションの動作
        private Timer timer;                    //アニメーションの時間間隔

        public Fire(Vector2 position, Vector2 velocity) : base("fire", new Size(33, 44), position, velocity, true, "Fire")
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            gForce = Parameter.GForce;
            map = MapManager.GetNowMapData();
        }

        /// <summary>
        /// 衝突区域判定
        /// </summary>
        /// <param name="other">対象</param>
        /// <returns></returns>
        public override bool CollisionCheck(GameObject other)
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

                    //相手の処理を実行する
                    other.EventHandle(this);
                }
            }

            return flag;
        }

        public override bool ObstacleCheck(GameObject other)
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

                    //相手の処理を実行する
                    other.EventHandle(this);
                }
            }

            return flag;
        }


        public override void Update(GameTime gameTime)
        {
            velocity.Y += gForce;

            Method.MapObstacleCheck(ref position, colSize.Width, colSize.Height, ref velocity, ref isOnGround, map, new int[] { 1, 2 });

            //マップ上の物と障害物判定
            foreach (var m in map.MapThings.FindAll(x => !x.IsTrigger))
            {
                ObstacleCheck(m);
            }

            //地面にいると運動停止
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

        /// <summary>
        /// 事件処理
        /// </summary>
        /// <param name="other"></param>
        public override void EventHandle(GameObject other)
        {
            if(other is Player)
            {
                ((Player)other).FireNum++;
            }
            isDead = true;
        }
    }
}
