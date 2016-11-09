/////////////////////////////////////////////////
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
using Microsoft.Xna.Framework.Graphics;

namespace TeamWorkGame.Actor
{
    public class Straw : GameObject
    {
        private Timer timer;
        private Timer burnTimer;
        private bool isToDeath;
        //private float scale;
        private Animation animation;
        private AnimationPlayer animationPlayer;
        private Map map;
        private bool IsAnimation = false;

        public Straw(Vector2 pos)
            : base("straw", pos, Vector2.Zero, false, "Straw")
        {
            animationPlayer = new AnimationPlayer();
        }


        public override void Initialize()
        {
            base.Initialize();
            timer = new Timer(Parameter.OneframeStrawAnime * 3);
            burnTimer = new Timer(Parameter.BurnStraw);
            isToDeath = false;
            isShow = true;
            SetTimer(0.01f);
            map = MapManager.GetNowMapData();
            animation = new Animation(Renderer.GetTexture("strawAnime"), Parameter.OneframeStrawAnime, false);

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
            animationPlayer.PlayAnimation(animation);

        }

        /// <summary>
        /// 描画の再定義（Scale追加）　by　氷見悠人　2016/10/20
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="renderer"></param>
        /// <param name="offset"></param>
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset, alpha);
            if (IsAnimation)
            {
                animationPlayer.Draw(gameTime, renderer, position + offset, SpriteEffects.None);
                IsAnimation = animationPlayer.Reset(isShow);
            }
        }

        public override void EventHandle(GameObject other)
        {
            DeathEvent(other);
            IsAnimation = true;
            //if (other is Fire)
            //{
            //    other.IsDead = true;

            //}
            //name = "fire";
            //IsTrigger = true;
            //scale = 1.5f;
            //ToDeath();
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

        public void CheckOhterStraw()
        {
            foreach (var m in map.MapThings)
            {
                if (m is Straw)
                {
                    if (!((Straw)m).IsToDeath)
                    {
                        if (ColRect.Intersects(m.ColRect))
                            m.EventHandle(this);
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
            if (other is Straw)
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
