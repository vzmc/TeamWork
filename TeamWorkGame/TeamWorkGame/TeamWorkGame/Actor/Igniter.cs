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
    class Igniter : GameObject
    {
        private Timer burnTimer;
        private bool isToDeath;
        //private float scale;
        private Map map;

        public Igniter(Vector2 pos)
            : base("igniter", pos, Vector2.Zero, true, "Igniter")
        {
        }

        protected override Rectangle InitLocalColRect()
        {
            return new Rectangle(0, 0, 64, 64);
        }


        public override void Initialize()
        {
            base.Initialize();
            burnTimer = new Timer(0.1f);
            isToDeath = false;
            isShow = true;
            map = MapManager.GetNowMapData();
            SetTimer(0.01f);
        }

        public bool IsToDeath
        {
            get
            {
                return isToDeath;
            }
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
            DeathUpdate();
        }

        /// <summary>
        /// 描画の再定義（Scale追加）　by　氷見悠人　2016/10/20
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="renderer"></param>
        /// <param name="offset"></param>
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset, float cameraScale)
        {
            if (isShow == true)
            {
                Rectangle rect = new Rectangle(0, 0, 64, 64);
                renderer.DrawTexture(name, position * cameraScale + offset, rect, cameraScale, alpha);
            }
        }

        public override void EventHandle(GameObject other)
        {
            //葉梨竜太 11/30
            if (other is Bomb)
            {
                BombEvent(other);
            }
            else
            {
                DeathEvent(other);
            }
        }

        //spawnTimerで復活 by長谷川修一
        public override void DeathUpdate()
        {
            if (isShow == false)
            {
                deathTimer.Update();
                if (deathTimer.IsTime())
                {
                    isTrigger = true;
                    //scale = 1.5f;
                    //name = "fire";
                    ToDeath();
                }
            }
            if (isToDeath)
            {
                burnTimer.Update();
                if (burnTimer.IsTime())
                {
                    burnTimer.Initialize();
                    burnTimer.Stop();
                    localColRect.Offset(-1, -1);
                    localColRect.Width += 2;
                    localColRect.Height += 2;
                    CheckOhter();
                }
            }
        }

        public void CheckOhter()
        {
            foreach (var m in map.MapThings)
            {
                if (m is Igniter)
                {
                    if (!((Igniter)m).IsToDeath)
                    {
                        if (ColRect.Intersects(m.ColRect))
                            m.EventHandle(this);
                    }
                }
                if(m is Bomb)
                {
                    if (!((Bomb)m).IsDead)
                    {
                        if (ColRect.Intersects(m.ColRect))
                        {
                            m.EventHandle(this);
                        }
                    }
                }
            }
        }

        public override void DeathEvent(GameObject other)
        {
            if (other is Fire || other is Player)
            {
                isShow = false;
            }
            if (other is Igniter)
            {
                if (((Igniter)other).IsToDeath)
                {
                    isShow = false;
                }
            }
        }

        //public float GetScale()
        //{
        //    return scale;
        //}
    }
}
