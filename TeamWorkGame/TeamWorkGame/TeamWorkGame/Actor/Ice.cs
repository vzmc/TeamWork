//////////////////////////////////////////////////////////////////////////////
// 氷クラス
// 作成者：氷見悠人
// 最終修正時間：2016/10/26
// By 長谷川修一
////////////////////////////////////////////////////////////////////////////

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
    public class Ice : GameObject
    {
        private bool isToDeath;
        private List<WaterLine> waters;
        private Animation animation;
        private AnimationPlayer animationPlayer;
        private bool IsAnimation = false;

        public Ice(Vector2 pos, Vector2 velo) : base("ice", pos, velo, false, "Ice")
        {
            animationPlayer = new AnimationPlayer();
        }

        public override void Initialize()
        {
            base.Initialize();
            isToDeath = false;
            isShow = true;      //初期値はtrue by佐瀬拓海
            SetTimer(1f, 2f);
            animation = new Animation(Renderer.GetTexture("iceAnime"), 0.5f, false);
        }

        public void SetWaters(List<WaterLine> waters)
        {
            this.waters = waters;
        }

        /// <summary>
        /// 死亡開始
        /// </summary>
        public void ToDeath()
        {
            if (!isToDeath)
            {
                isToDeath = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            AliveUpdate();
            animationPlayer.PlayAnimation(animation);
        }

        public override void AliveEvent(GameObject other)
        {
            if (other is Fire)
            {
                other.IsDead = true;
                isShow = false;         //不可視化
            }
            if (other is Player)
            {
                isShow = false;         //Playerの場合は不可視化だけ
            }
            spawnTimer.Initialize(); //Timerを初期化して可視化するのを防ぐ
        }

        /// <summary>
        /// 描画の再定義（透明値を追加）　By　氷見悠人　2016/10/20
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="renderer"></param>
        /// <param name="offset"></param>
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset, alpha);
            //アニメーションの追加 by長谷川修一
            if(IsAnimation)
            {
                animationPlayer.Draw(gameTime, renderer, position + offset, SpriteEffects.None);
                IsAnimation = animationPlayer.Reset(isShow);
            }
        }

        public override void EventHandle(GameObject other)
        {
            AliveEvent(other);
            IsAnimation = true;
            WaterLine waterLine = new WaterLine(position, animation);
            if (waters != null)
                waters.Add(waterLine);
        }
    }
}
