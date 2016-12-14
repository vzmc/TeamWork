﻿/////////////////////////////////////////////////
// 火のクラス
// 作成時間：2016年9月25日
// By 氷見悠人
// 最終修正時間：2016年12月8日
// やまなりから真っすぐに対応　By葉梨竜太
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
        private List<WaterLine> watersList;         //滝のリスト
        Vector2 vlo = Vector2.Zero;


        public Fire(Vector2 position, Vector2 velocity, List<WaterLine> waterline) : base("fire", position, velocity, true, "Fire")
        {
            watersList = waterline;
        }

        public override void Initialize()
        {
            base.Initialize();
            gForce = Parameter.GForce;
            map = MapManager.GetNowMapData();
            //葉梨竜太
            //飛ぶ時間
            timer = new Timer(Parameter.FireFlyTime);
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
                    velocity = new Vector2(0, 10);
                    //相手の処理を実行する
                    other.EventHandle(this);
                }
            }
            return flag;
        }


        public override void Update(GameTime gameTime)
        {
            if(isGoDie)
            {
                isDead = true;
            }
            //velocity.Y += gForce;
            //葉梨竜太
            //一定時間飛んだら落ちる
            timer.Update();
            if (timer.IsTime())
            {
                velocity = new Vector2(0, 10);
                timer.Initialize();
            }

            //マップ上の物と障害物判定
            foreach (var m in map.MapThings.FindAll(x => !x.IsTrigger))
            {
                ObstacleCheck(m);
               
            }

            //葉梨竜太
            //壁に当たるとvelocity = 0
            if(Method.MapObstacleCheck(ref position, localColRect, ref velocity, ref isOnGround, map, new int[] { 1, 2 }))
            {
                velocity = new Vector2(0, 10);
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

            foreach (var wl in watersList)
            {
                foreach (var w in wl.Waters)
                    CollisionCheck(w);
            }

           
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        //public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset, float cameraScale)
        //{
        //    renderer.DrawTexture(name, position * cameraScale + offset, cameraScale, 1.0f);
        //}

        /// <summary>
        /// 事件処理
        /// </summary>
        /// <param name="other"></param>
        public override void EventHandle(GameObject other)
        {
            if(other is Player)
            {
                if (((Player)other).FireNum < Parameter.FireMaxNum)//Fireの数がMax以上にならないよう変更
                {
                    ((Player)other).FireNum++;
                }
            }
            isDead = true;
        }
    }
}
