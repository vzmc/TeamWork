///////////////////////////////
//炭のクラス
//作成時間：１０月１３日
//By　佐瀬　拓海
//最終更新日：１０月１９日
//重力、あたり判定の追加
//By　葉梨竜太
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
    public class Coal : GameObject
    {
        private bool isToDeath;

        //追加　葉梨竜太
        private float gForce;
        private Map map;

        public Coal(Vector2 pos, Vector2 velo):
            base("coal", pos, velo, true, "coal")
        {
            
        }
        public override void Initialize()
        {
            base.Initialize();
            isToDeath = false;

            //追加　葉梨竜太
            gForce = Parameter.GForce;
            map = MapManager.GetNowMapData();
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

            //重力とあたり判定の追加
            //葉梨竜太
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
        public override void EventHandle(GameObject other)
        {
            if(other is Player)
            {
                if(((Player)other).FireNum < Parameter.FireMaxNum)//Fireの数を回復
                {
                    ((Player)other).FireNum = ((Player)other).FireNum + 1;
                }
                isDead = true;
            }
        }
    }
}
