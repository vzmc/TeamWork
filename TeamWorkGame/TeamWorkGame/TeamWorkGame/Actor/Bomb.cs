//////////////////////////////
//爆弾クラス
//作成者　葉梨竜太
//作成日時　11/30
//最後修正日　2016.12.14
//修正者と内容　柏、ＳＥ実装
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
        private Map map;
        protected Sound sound;       //2016.12.14、柏
        private float gForce;       //2016.12.21　佐瀬
        private Animation bombEffect;
        private AnimationPlayer animationPlayer;
        private bool IsAnimation = false;
        private Vector2 effectPosition; //エフェクト用の位置

        public Bomb(Vector2 pos, Sound sound, Vector2 velo)
            :base("bomb",pos, velo, false,"Bomb")
        {
            this.sound = sound;
            animationPlayer = new AnimationPlayer();
        }

        public override void Initialize()
        {
            base.Initialize();
            isShow = true;
            map = MapManager.GetNowMapData();
            gForce = Parameter.GForce;
            bombEffect = new Animation(Renderer.GetTexture("bombEffect"), Parameter.BombAnimeTime / 7, false);
            effectPosition = Vector2.Zero;
            SetTimer(Parameter.BombColTime);
        }

        public override void Update(GameTime gameTime)
        {
            //重力を付ける
            velocity.Y += gForce;

            //マップ上の物と障害物判定
            foreach (var m in map.MapThings.FindAll(x => !x.IsTrigger && x.Tag != "Bomb"))
            {
                ObstacleCheck(m);
            }

            Method.MapObstacleCheck(ref position, localColRect, ref velocity, ref isOnGround, map, new int[] { 1, 2 });

            //地面にいると運動停止 //アニメーションが始まったら運動停止by長谷川
            if (isOnGround || IsAnimation)
            {
                velocity = Vector2.Zero;
            }

            position += velocity;

            //エフェクト用のポジション取得 by長谷川
            effectPosition = new Vector2(position.X - 64, position.Y - 64);

            //マップ上の物と衝突区域判定
            foreach (var m in map.MapThings.FindAll(x => x.IsTrigger))
            {
                CollisionCheck(m);
            }
            
            AliveUpdate();
            DeathUpdate();
            animationPlayer.PlayAnimation(bombEffect);
        }
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset, float cameraScale)
        {
            renderer.DrawTexture(name, position * cameraScale + offset, cameraScale, alpha);
            if(IsAnimation)
            {
                animationPlayer.Draw(gameTime, renderer, effectPosition * cameraScale + offset, SpriteEffects.None, cameraScale);
                IsAnimation = animationPlayer.Reset(isShow);
            }
        }

        public override void EventHandle(GameObject other)
        {
            DeathEvent(other);
        }

        public override void DeathEvent(GameObject other)
        {
            if (other is Fire)
            {
                ////other.IsDead = true;
                sound.PlaySE("bomb1");
                isShow = false;
            }
            if (other is Player)
            {
                sound.PlaySE("bomb1");
                isShow = false;
            }
            if(other is Igniter)
            {
                sound.PlaySE("bomb1");
                isShow = false;
            }
        }

        //by長谷川 1/11
        public override void DeathUpdate()
        {
            //DeaathEventが発生したら
            if (isShow == false)
            {
                //タイマー更新
                deathTimer.Update();
                if (deathTimer.IsTime())
                {
                    //当たり判定消滅
                    isTrigger = true;
                    //爆発
                    Explosion();
                    //爆弾画像を透明にして
                    alpha = 0.0f;
                    //アニメーションを開始
                    IsAnimation = true;
                }
            }
        }

        public void Explosion()
        {
            if (isShow == false)
            {

                if (alpha != 0.0f)
                {
                    localColRect.Offset(-1, -1);
                    localColRect.Width += 2;
                    localColRect.Height += 2;
                }

                foreach (var m in map.MapThings)
                {
                    if (ColRect.Intersects(m.ColRect))
                        m.EventHandle(this);
                }
            }
        }
    }
}
