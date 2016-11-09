///////////////////////////////
//砂のクラス
//作成時間：１１月０２日
//By　佐瀬　拓海
//最終更新日：１１月０２日
//By　佐瀬拓海
///////////////////////////////
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamWorkGame.Device;
using TeamWorkGame.Def;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public class Sand : GameObject
    {
        private bool isToDeath;
        
        private float gForce;
        private Map map;

        public Sand(Vector2 pos, Vector2 velo) :
            base("sand", pos, velo, false, "Sand")
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            isToDeath = false;

            gForce = Parameter.GForce;
            map = MapManager.GetNowMapData();
        }

        protected override Rectangle InitLocalColRect()
        {
            //
            return base.InitLocalColRect();
        }

        public void ToDeath()
        {
            if (!isToDeath)
            {
                isToDeath = true;
            }
        }


        public override void Update(GameTime gameTime)
        {
            velocity.Y += gForce;

            //マップ上の物と障害物判定

            foreach (var m in map.MapThings.FindAll(x => !x.IsTrigger))
            {
                if (m is Sand == false)//砂以外
                {
                    ObstacleCheck(m);
                }
            }

            Method.MapObstacleCheck(ref position, localColRect, ref velocity, ref isOnGround, map, new int[] { 1, 2 });


            //地面にいると運動停止
            if (isOnGround)
            {
                velocity = Vector2.Zero;
                //gForce = 0.0f;
            }

            position += velocity;

            //マップ上の物と衝突区域判定
            foreach (var m in map.MapThings.FindAll(x => x.IsTrigger))
            {
                CollisionCheck(m);
            }

            if (isToDeath)
            {
                isDead = true;
            }
        }
        public override void EventHandle(GameObject other)
        {
            if (other is Player)
            {
                //ToDeath();
            }
        }
    }
}
