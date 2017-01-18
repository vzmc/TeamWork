////////////////////////////////////////
//石クラス（爆弾のみ破壊可能）
//作成日時：2017/1/13
//作成者：葉梨竜太
///////////////////////////////////////
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
    public class Stone : GameObject
    {

        private Map map;
        public Stone(Vector2 pos)
            :base("stone",pos,Vector2.Zero,false,"Stone")
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            map = MapManager.GetNowMapData();
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsTrigger)
            {
                //マップ上の物と障害物判定
                foreach (var m in map.MapThings.FindAll(x => !x.IsTrigger && x != this))
                {
                    ObstacleCheck(m);
                }
            }
            Method.MapObstacleCheck(ref position, localColRect, ref velocity, ref isOnGround, map, new int[] { 1, 2 });

            foreach (var m in map.MapThings.FindAll(x => x.IsTrigger))
            {
                CollisionCheck(m);
            }
        }
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset, float cameraScale)
        {
            renderer.DrawTexture(name, position * cameraScale + offset, cameraScale, alpha);
        }
        public override void EventHandle(GameObject other)
        {
            if (other is Bomb)
            {
                BombEvent(other);
            }
        }

    }
}
