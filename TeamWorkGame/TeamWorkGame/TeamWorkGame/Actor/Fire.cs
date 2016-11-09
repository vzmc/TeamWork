/////////////////////////////////////////////////
// 火のクラス
// 作成時間：2016年9月25日
// By 氷見悠人
// 最終修正時間：2016年11月9日
// アニメーションの準備　By 長谷川修一　
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

        public Fire(Vector2 position, Vector2 velocity) : base("fire", position, velocity, true, "Fire")
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            gForce = Parameter.GForce;
            map = MapManager.GetNowMapData();
        }

        protected override Rectangle InitLocalColRect()
        {
            return new Rectangle(8, 22, 49, 42);
            //return new Rectangle(15, 10, 32, 44);
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
                    //相手の処理を実行する
                    other.EventHandle(this);
                }
            }

            return flag;
        }


        public override void Update(GameTime gameTime)
        {
            velocity.Y += gForce;

            //マップ上の物と障害物判定
            foreach (var m in map.MapThings.FindAll(x => !x.IsTrigger))
            {
                ObstacleCheck(m);
            }

            Method.MapObstacleCheck(ref position, localColRect, ref velocity, ref isOnGround, map, new int[] { 1, 2 });

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
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset)
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
                if(((Player)other).FireNum < Parameter.FireMaxNum)//Fireの数がMax以上にならないよう変更
                ((Player)other).FireNum++;
            }
            isDead = true;
        }
    }
}
