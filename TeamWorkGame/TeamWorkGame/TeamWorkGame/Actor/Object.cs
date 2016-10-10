using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public abstract class Object
    {
        //フィールド
        protected string name;          //アセット名
        protected Size imageSize;       //画像のサイズ
        protected Size colSize;         //衝突判定用のサイズ
        protected Vector2 position;     //位置
        protected Vector2 colOffset;    //衝突判定区域の偏移量。自身位置との相対位置
        protected Vector2 velocity;     //移動量
        protected bool isDead;          //生きているか？
        protected bool isOnGround;      //地上にいるか？
        protected string tag;           //タグ
        protected bool isTrigger;       //衝突判定の種類 true: 衝突区域  false: 障害物
        
        //プロパティ
        public Size ImageSize
        {
            get
            {
                return imageSize;
            }
        }

        public Size ColSize
        {
            get
            {
                return colSize;
            }
            set
            {
                colSize = value;
            }
        }

        public Vector2 ColOffset
        {
            get
            {
                return colOffset;
            }
            set
            {
                colOffset = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
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

        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        public float VelocityX
        {
            get
            {
                return velocity.X;
            }
            set
            {
                velocity.X = value;
            }
        }

        public float VelocityY
        {
            get
            {
                return velocity.Y;
            }
            set
            {
                velocity.Y = value;
            }
        }

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

        public bool IsOnGround
        {
            get
            {
                return isOnGround;
            }
            set
            {
                isOnGround = value;
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

        /// <summary>
        /// コンストラクタ（衝突判定区域は自身サイズと同じ）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="imageSize">画像サイズ</param>
        /// <param name="pos">位置</param>
        /// <param name="velo">移動量</param>
        /// <param name="isTrigger">区域ですか？</param>
        /// <param name="tag">タグ</param>
        public Object(string name, Size imageSize, Vector2 pos, Vector2 velo, bool isTrigger, string tag = "")
        {
            this.name = name;
            this.imageSize = imageSize;
            colSize = imageSize;
            colOffset = Vector2.Zero;
            this.tag = tag;
            this.isTrigger = isTrigger;
            
            Initialize(pos, velo, isTrigger);
        }

        /// <summary>
        /// コンストラクタ（衝突区域指定）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="imageSize">画像サイズ</param>
        /// <param name="pos">位置</param>
        /// <param name="velo">移動量</param>
        /// <param name="colSize">衝突区域サイズ</param>
        /// <param name="colOffset">衝突区域と自身位置の相対位置</param>
        /// <param name="isTrigger">区域ですか？</param>
        /// <param name="tag">タグ</param>
        public Object(string name, Size imageSize, Vector2 pos, Vector2 velo, Size colSize, Vector2 colOffset, bool isTrigger, string tag = "")
        {
            this.name = name;
            this.imageSize = imageSize;
            this.colSize = colSize;
            this.colOffset = colOffset;
            this.tag = tag;
            this.isTrigger = isTrigger;

            Initialize(pos, velo, isTrigger);
        }

        /// <summary>
        /// 位置などの初期化処理
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="velo">移動量</param>
        /// <param name="isTrigger">区域か？</param>
        public virtual void Initialize(Vector2 pos, Vector2 velo, bool isTrigger)
        {
            position = pos;
            velocity = velo;
            this.isTrigger = isTrigger;
            isDead = false;
            isOnGround = false;
        }

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
        /// 四角同士の衝突区域判定
        /// </summary>
        /// <param name="other">判定する対象</param>
        /// <returns></returns>
        public virtual bool CollisionCheck(Object other)
        {
            bool flag = Method.CollisionCheck(position + colOffset, colSize.Width, colSize.Height, other.position + other.colOffset, other.colSize.Width, other.colSize.Height);

            return flag;
        }

        /// <summary>
        /// 障害物判定
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool ObstacleCheck(Object other)
        {
            bool flag = Method.ObstacleCheck(this, other);

            return flag;
        }

        /// <summary>
        /// 衝突事件の処理
        /// </summary>
        /// <param name="other">衝突対象/param>
        public abstract void EventHandle(Object other);
        
    }
}
