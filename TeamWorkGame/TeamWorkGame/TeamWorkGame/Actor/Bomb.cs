//////////////////////////////
//爆弾クラス
//作成者　葉梨竜太
//作成日時　11/30
//////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Def;
using TeamWorkGame.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace TeamWorkGame.Actor
{
    public class Bomb : GameObject
    {
        private bool isToDeath;
        private Map map;
        private float gForce;

        public Bomb(Vector2 pos)
            :base("bomb",pos,Vector2.Zero,false,"Bomb")
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            isToDeath = false;
            isShow = true;
            map = MapManager.GetNowMapData();
            gForce = Parameter.GForce;
        }

        public override void Update(GameTime gameTime)
        {
            velocity.Y += gForce;

            //マップ上の物と障害物判定

            foreach (var m in map.MapThings.FindAll(x => !x.IsTrigger && x.Tag != "Bomb"))
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

            Explosion();
            AliveUpdate();
        }

        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset, float cameraScale)
        {
            renderer.DrawTexture(name, position * cameraScale + offset, cameraScale, alpha);
        }

        public override void EventHandle(GameObject other)
        {
            AliveEvent(other);
        }

        public override void AliveEvent(GameObject other)
        {
            if (other is Fire)
            {
                //other.IsDead = true;
                isShow = false;
            }
            if (other is Player)
            {
                isShow = false;
            }
            spawnTimer.Initialize();
        }

        public void Explosion()
        {
            if (isShow == false)
            {
                localColRect.Offset(-1, -1);
                localColRect.Width += 2;
                localColRect.Height += 2;

                IsDead = true;

                foreach (var m in map.MapThings)
                {
                    if (ColRect.Intersects(m.ColRect))
                        m.EventHandle(this);
                }
            }
        }
    }
}
