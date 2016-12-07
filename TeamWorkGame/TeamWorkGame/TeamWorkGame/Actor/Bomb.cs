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
        }

        public override void Update(GameTime gameTime)
        {
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
            if(other is Igniter)
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
