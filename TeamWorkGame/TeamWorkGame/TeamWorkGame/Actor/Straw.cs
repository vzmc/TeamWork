﻿/////////////////////////////////////////////////
// 藁のクラス
// 作成時間：2016年10月12日
// By 長谷川修一
// 最終修正時間：2016年11月2日
// By 長谷川修一
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
    public class Straw : GameObject
    {
        private Timer timer;
        private Timer burnTimer;
        private bool isToDeath;
        private float scale;
        private Map map;

        public Straw(Vector2 pos)
            : base("straw", pos, Vector2.Zero, false, "Straw")
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            timer = new Timer(2.0f);
            burnTimer = new Timer(1.0f);
            isToDeath = false;
            isShow = true;
            SetTimer(0.01f);
            scale = 1.0f;
            map = MapManager.GetNowMapData();
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
            if (isToDeath)
            {
                burnTimer.Update();
                timer.Update();

                if (burnTimer.IsTime())
                {
                    burnTimer.Stop();
                    localColRect.Offset(-1, -1);
                    localColRect.Width += 2;
                    localColRect.Height += 2;
                    CheckOhterStraw();
                }

                if (timer.IsTime())
                {
                    IsDead = true;
                }
            }
        }

        /// <summary>
        /// 描画の再定義（Scale追加）　by　氷見悠人　2016/10/20
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="renderer"></param>
        /// <param name="offset"></param>
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset, scale, alpha);
        }

        public override void EventHandle(GameObject other)
        {
            AliveEvent(other);
            //if (other is Fire)
            //{
            //    other.IsDead = true;

            //}
            //name = "fire";
            //IsTrigger = true;
            //scale = 1.5f;
            //ToDeath();
        }

        //見えていないときは火になって当たり判定が消える
        //spawnTimerで復活 by長谷川修一
        public override void DeathUpdate()
        {
            if (isShow == false)
            {
                deathTimer.Update();
                if (!isToDeath && deathTimer.IsTime())
                {
                    isTrigger = true;
                    //scale = 1.5f;
                    name = "fire";
                    ToDeath();
                    
                }
            }
        }

        public void CheckOhterStraw()
        {
            foreach(var m in map.MapThings)
            {
                if(m is Straw)
                {
                    if (!((Straw)m).IsToDeath)
                    {
                        if(ColRect.Intersects(m.ColRect))
                            m.EventHandle(this);
                    }
                }
            }
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
            if(other is Straw)
            {
                if (((Straw)other).IsToDeath)
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
