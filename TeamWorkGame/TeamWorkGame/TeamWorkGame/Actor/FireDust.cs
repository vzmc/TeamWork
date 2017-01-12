// 
//火の粉
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TeamWorkGame.Device;    //入力状態クラス
using TeamWorkGame.Def;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public class FireDust : GameObject
    {
        private Animation anime;             //アニメ
        private AnimationPlayer animePlayer;    //アニメ再生器
        private float lifeTime;
        private float frameTime;

        public FireDust(Vector2 pos, Vector2 velo) : base("FireDust", pos, velo, true, "FireDust")
        {
            anime = new Animation(Renderer.GetTexture("FireDust"), 0.1f, true);
            animePlayer.PlayAnimation(anime);
            lifeTime = 0.5f;
        }

        public override void EventHandle(GameObject other)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if(lifeTime < 0)
            {
                isDead = true;
            }
            else
            {
                lifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                position.Y += 0.5f;
            }
        }

        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset, float cameraScale)
        {
            //base.Draw(gameTime, renderer, offset, cameraScale);
            animePlayer.Draw(gameTime, renderer, position * cameraScale + offset, SpriteEffects.None, cameraScale);
            //}
        }
    }
}
