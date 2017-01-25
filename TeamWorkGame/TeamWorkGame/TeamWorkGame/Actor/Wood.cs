﻿/////////////////////////////
//木材クラス
//最終修正時間:2016年11月30日
//by 葉梨竜太
/////////////////////////////
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
            isShow = true;
            animation = new Animation(Renderer.GetTexture("woodAnime"), Parameter.WoodAnimeTime / 3, false);
            SetTimer(Parameter.WoodAnimeTime, Parameter.WoodSpawnTime);
        }

        public override void Update(GameTime gameTime)
        {
            AliveUpdate();
            animationPlayer.PlayAnimation(animation);
        }

        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset, float cameraScale)
        {
            renderer.DrawTexture(name, position * cameraScale + offset, cameraScale, alpha);
            if (IsAnimation)
            {
                animationPlayer.Draw(gameTime, renderer, position * cameraScale + offset, SpriteEffects.None, cameraScale);
                IsAnimation = animationPlayer.Reset(isShow);
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
                AliveEvent(other);
                IsAnimation = true;
            }
        }

        public override void AliveEvent(GameObject other)
        {
            if (other is Fire)
            {
                //other.IsDead = true;
                isShow = false; 
            }
            if (other is Player && ((Player)other).FireNum >= Parameter.woodfire)
            {
                isShow = false;
            }
            spawnTimer.Initialize();
        }
    }
}
