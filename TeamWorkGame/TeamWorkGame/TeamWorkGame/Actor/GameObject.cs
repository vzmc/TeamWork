﻿////////////////////////////////////////////////////////////////////
//マップ上にある物の親クラス
//作成時間：2016/9/23
//作成者：氷見悠人
//最終修正時間：2016/10/19
//修正者：佐瀬拓海
///////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    /// <summary>
    /// マップ上にある物の親クラス
    /// </summary>
    public abstract class GameObject
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
        protected bool isShow;          //存在判定 true:見える false:見えない
        private Timer deathTimer;       //消えるまでのTimer
        private Timer spawnTimer;       //再度見えるようになるまでのTimer
        protected float alpha;

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
        public GameObject(string name, Size imageSize, Vector2 pos, Vector2 velo, bool isTrigger, string tag = "")
        {
            this.name = name;
            this.imageSize = imageSize;
            position = pos;
            velocity = velo;
            colSize = imageSize;
            colOffset = Vector2.Zero;
            this.tag = tag;
            this.isTrigger = isTrigger;
            deathTimer = new Timer(0.5f);
            spawnTimer = new Timer(0.5f);

            Initialize();
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
        public GameObject(string name, Size imageSize, Vector2 pos, Vector2 velo, Size colSize, Vector2 colOffset, bool isTrigger, string tag = "")
        {
            this.name = name;
            this.imageSize = imageSize;
            position = pos;
            velocity = velo;
            this.colSize = colSize;
            this.colOffset = colOffset;
            this.tag = tag;
            this.isTrigger = isTrigger;
            deathTimer = new Timer(0.5f);
            spawnTimer = new Timer(0.5f);

            Initialize();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public virtual void Initialize()
        {
            isDead = false;
            isOnGround = false;
            isShow = true;      //初期はTrue（見える
            alpha = 1.0f;
            //Timerの初期化
            deathTimer.Initialize();
            spawnTimer.Initialize();
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
        /// <param name="offset">カメラ偏移</param>
        public virtual void Draw(Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset);
        }

        /// <summary>
        /// 描画(透明値を設定する)    作成者：佐瀬　日付：１０/１２
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="offset">カメラ偏移</param>
        /// <param name="alpha">透明値</param>
        public virtual void Draw(Renderer renderer, Vector2 offset, float alpha = 1.0f)
        {
            renderer.DrawTexture(name, position + offset, alpha);
        }

        /// <summary>
        /// 描画（範囲指定する）      BY 氷見悠人   2016/10/13
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="offset">カメラ偏移</param>
        /// <param name="rect">描画範囲</param>
        /// <param name="alpha">透明値</param>
        public virtual void Draw(Renderer renderer, Vector2 offset, Rectangle rect, float alpha = 1.0f)
        {
            renderer.DrawTexture(name, position + offset, rect, alpha);
        }

        /// <summary>
        /// 描画(拡大率) By 長谷川修一 2016/10/13
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="offset">カメラ偏移</param>
        /// <param name="scale">描画範囲</param>
        /// <param name="alpha">透明値</param>
        public virtual void Draw(Renderer renderer, Vector2 offset, float scale, float alpha = 1.0f)
        {
            renderer.DrawTexture(name, position + offset, scale, alpha);
        }

        /// <summary>
        /// 四角同士の衝突区域判定
        /// </summary>
        /// <param name="other">判定する対象</param>
        /// <returns></returns>
        public virtual bool CollisionCheck(GameObject other)
        {
            bool flag = Method.CollisionCheck(position + colOffset, colSize.Width, colSize.Height, other.position + other.colOffset, other.colSize.Width, other.colSize.Height);

            return flag;
        }

        /// <summary>
        /// 障害物判定
        /// </summary>
        /// <param name="other">対象</param>
        /// <returns></returns>
        public virtual bool ObstacleCheck(GameObject other)
        {
            bool flag = Method.ObstacleCheck(this, other);

            return flag;
        }

        /// <summary>
        /// 消える時間と復活までの時間を設定
        /// </summary>
        /// <param name="deathTime">消える時間</param>
        /// <param name="spawnTime">復活する時間</param>
        public void SetTimer(float deathTime, float spawnTime)
        {
            deathTimer.Change(deathTime);
            spawnTimer.Change(spawnTime);
        }

        /// <summary>
        /// 存在判定（そのクラスのUpdateに追加する）
        /// </summary>
        public void AliveUpdate()   
        {
            if (isShow == false)//見えないとき
            {
                deathTimer.Update();
                if (alpha >= 0.0f)       //Alphaが０になるまで減らす
                {
                    alpha -= 0.06f;
                }
                if (deathTimer.IsTime())     //時間になったら当たり判定を消す
                {
                    isTrigger = true;
                }
                spawnTimer.Update();
                if (spawnTimer.IsTime())
                {
                    isShow = true;      //可視化
                    deathTimer.Initialize(); //Timerの初期化
                }
            }
            else//見えるとき
            {
                isTrigger = false; //当たり判定を作る
                if (alpha <= 1.0f) // alphaを増やす
                {
                    alpha += 0.1f;
                }
            }
        }

        /// <summary>
        /// 存在判定（EventHandleに追加する）
        /// </summary>
        /// <param name="other">対象</param>
        public void AliveEvent(GameObject other)
        {
            if (other is Fire || other is Player)
            {
                other.IsDead = true;
                isShow = false;         //不可視化
            }
            spawnTimer.Initialize(); //Timerを初期化して可視化するのを防ぐ
        }

        /// <summary>
        /// 透明値を返す
        /// </summary>
        /// <returns></returns>
        public float GetAlpha()
        {
            return alpha;
        }

        /// <summary>
        /// 衝突事件の処理
        /// </summary>
        /// <param name="other">衝突対象/param>
        public abstract void EventHandle(GameObject other);
        
    }
}
