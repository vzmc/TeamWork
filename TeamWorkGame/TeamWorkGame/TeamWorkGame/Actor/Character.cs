using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;

namespace TeamWorkGame.Actor
{
    abstract class Character
    {
        protected string name;      //アセット名
        protected Vector2 position; //位置
        protected Vector2 velocity; //移動量
        protected int width;      
        protected int height;
        protected bool isDead;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="radius">半径</param>
        public Character(string name, int width, int height)
        {
            this.name = name;
            this.width = width;
            this.height = height;
            isDead = false;
            position = Vector2.Zero;
            velocity = Vector2.Zero;
        }

        /// <summary>
        /// 抽象初期化メソッド
        /// </summary>
        public abstract void Initialize(Vector2 position, Vector2 velocity);

        /// <summary>
        /// 抽象更新メソッド
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public float GetWidth()
        {
            return width;
        }

        public float GetHeight()
        {
            return height;
        }
    }

}
