///////////////////////////////
//炭のクラス
//作成時間：１０月１３日
//By　佐瀬　拓海
//最終更新日：１２月２２日
//炭取得時のエフェクト追加
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
    public class Coal : GameObject
    {
        private bool isToDeath;

        //追加　葉梨竜太
        private float gForce;
        private Map map;
        //追加　佐瀬拓海
        private bool isMove;
        private bool prevIsMove;
        private Vector2 coalPos;

        public Coal(Vector2 pos, Vector2 velo):
            base("coal", pos, velo, true, "coal")
        {
            
        }
        public override void Initialize()
        {
            base.Initialize();
            isToDeath = false;
            //追加　佐瀬拓海
            isMove = false;
            prevIsMove = false;
            coalPos = new Vector2(1024, 16);

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
            if (isMove == false)//Playerとまだ触れてないとき
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
            else//Playerと触れた後
            {
                Move();
            }
        }
        public void Move()//画面右上のCoal表示まで移動させる
        {
            if (isMove)
            {
                Vector2 distance = position - coalPos;
                velocity = distance;
                velocity.Normalize();
                velocity *= -20;
                //if (position.X > coalPos.X)
                //{
                //    velocity.X = -10.0f;
                //}
                //if(position.X < coalPos.X)
                //{
                //    velocity.X = 10.0f;
                //}
                //if (position.Y > coalPos.Y)
                //{
                //    velocity.Y = -10.0f;
                //}
                //if (position.Y < coalPos.Y)
                //{
                //    velocity.Y = 10.0f;
                //}
                position += velocity;

                if(Math.Abs(distance.X) < 10 || Math.Abs(distance.Y) < 10)
                {
                    isDead = true;
                }
                prevIsMove = isMove;
            }
        }
        public override void EventHandle(GameObject other)
        {
            if(other is Player)
            {
                if(((Player)other).FireNum < Parameter.FireMaxNum)//Fireの数を回復
                {
                    if (!isMove)
                    {
                        ((Player)other).FireNum = ((Player)other).FireNum + 1;
                    }
                }
                prevIsMove = isMove;
                isMove = true;
            }
        }
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset, float cameraScale)//Playerと触れる前と触れる後の描画処理
        {
            if (!isMove)
            {
                base.Draw(gameTime, renderer, offset, cameraScale);
            }
            if (isMove)
            {
                if(prevIsMove == false && isMove == true)
                {
                    position += offset;
                }
                renderer.DrawTexture(name, position);
            }
        }
    }
}
