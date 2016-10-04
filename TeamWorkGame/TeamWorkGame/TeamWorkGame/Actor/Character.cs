using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public abstract class Character
    {
        protected string name;      //アセット名
        protected Vector2 position; //位置
        protected Vector2 velocity; //移動量
        protected int width;      
        protected int height;
        protected bool isDead;
        protected bool isOnGround;  //地上にいるか？
        protected string tag;
        protected bool isTrigger;   

        public bool IsDead
        {
            get
            {
                return isDead;
            }
            set
            {
                isDead = value;
            }
        }

        public bool IsTrigger
        {
            get
            {
                return isTrigger;
            }
            set
            {
                isTrigger = value;
            }
        }

        public string Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        public float PositionX
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }
        }

        public float PositionY
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="radius">半径</param>
        public Character(string name, int width, int height, string tag, bool isTrigger = false)
        {
            this.name = name;
            this.width = width;
            this.height = height;
            isDead = false;
            this.tag = tag;
            this.isTrigger = isTrigger;
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
        public virtual void Draw(Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset);
        }

        /// <summary>
        /// 四角同士の衝突判定
        /// </summary>
        /// <param name="other">判定する対象</param>
        /// <returns></returns>
        public virtual bool CollisionCheck(Character other)
        {
            bool flag = Method.CollisionCheck(position+velocity, width, height, other.GetPosition(), other.GetWidth(), other.GetHeight());

            return flag;
        }

        public virtual bool ObstacleCheck(Character other)
        {
            bool flag = Method.ObstacleCheck(this, other);

            return flag;
        }

        public void SetPosition(Vector2 pos)
        {
            position = pos;
        }

        public void SetPosition(float x, float y)
        {
            position = new Vector2(x, y);
        }

        public void SetVelocity(Vector2 velo)
        {
            velocity = velo;
        }

        public void SetVelocity(float x, float y)
        {
            velocity = new Vector2(x, y);
        }

        public void SetIsOnGround(bool flag)
        {
            isOnGround = flag;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public bool GetIsOnGround()
        {
            return isOnGround;
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }
    }
}
