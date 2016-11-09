//木材クラス
//最終修正時間:2016年11月2日
//by 長谷川修一

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
    public class Wood : GameObject
    {
        //private Timer timer;
        //private bool isToDeath;
        //private float scale;
        private Animation animation;
        private AnimationPlayer animationPlayer;
        private bool IsAnimation = false;

        public Wood(Vector2 pos)
            : base("wood", pos, Vector2.Zero, false, "Wood")
        {
            animationPlayer = new AnimationPlayer();
        }

        public override void Initialize()
        {
            base.Initialize();
            //timer = new Timer(2.0f);
            //isToDeath = false;
            isShow = true;
            SetTimer(0.6f, 3f);
            animation = new Animation(Renderer.GetTexture("woodAnime"), Parameter.OneframeWoodAnime, false);
            //scale = 1.0f;
        }

        //public void ToDeath()
        //{
        //    if (!isToDeath)
        //    {
        //        isToDeath = true;
        //    }
        //}

        public override void Update(GameTime gameTime)
        {
            AliveUpdate();
            animationPlayer.PlayAnimation(animation);
            //if (isToDeath)
            //{
            //    timer.Update();
            //    if (timer.IsTime())
            //    {
            //        IsDead = true;
            //    }
            //}
        }

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
            AliveEvent(other);
            IsAnimation = true;
            //if (other is Fire)
            //{
            //    other.IsDead = true;
            //}
            //ToDeath();
        }

        //見えていないときは火になって当たり判定が消える
        //spawnTimerで復活 by長谷川修一
        //public override void AliveUpdate()
        //{
        //    if (isShow == false)
        //    {
        //        deathTimer.Update();
        //        if (deathTimer.IsTime())
        //        {
        //            isTrigger = true;
        //            scale = 1.5f;
        //            name = "fire";
        //        }
        //        spawnTimer.Update();
        //        if (spawnTimer.IsTime())
        //        {
        //            isShow = true;
        //            scale = 1.0f;
        //            name = "wood";
        //            deathTimer.Initialize();      
        //        }
        //    }
        //    else
        //    {
        //        isTrigger = false;
        //    }
       // }

        public override void AliveEvent(GameObject other)
        {
            if (other is Fire)
            {
                other.IsDead = true;
                isShow = false; 
            }
            if (other is Player)
            {
                isShow = false;
            }
            spawnTimer.Initialize();
        }

        //public float GetScale()
        //{
        //    return scale;
        //}
    }
}
