/////////////////////////////////////////////////
// プレーヤーのクラス
// 作成時間：2016年9月24日
// By 氷見悠人
// 最終修正時間：2016年10月09日
// By 氷見悠人
/////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;  //Vector2
using Microsoft.Xna.Framework.Graphics; //spriteBatch
using Microsoft.Xna.Framework.Input;
using TeamWorkGame.Device;    //入力状態クラス
using TeamWorkGame.Def;
using TeamWorkGame.Utility;


namespace TeamWorkGame.Actor
{
    class Player : Object
    {
        // フィールド
        private InputState inputState;          //入力管理
        //private Camera camera;
        private Map map;
        private float gForce;
        private Motion motion;                  //アニメーション管理
        private Timer timer;                    //アニメーションの時間間隔
        private Direction diretion;             //向いている方向
        private List<Fire> fires;               //投げ出した火
        private int fireMaxNum;                    //火の総数
        private int fireNum;                        //持ているひの数

        public int FireNum
        {
            get
            {
                return fireNum;
            }
            set
            {
                fireNum = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="input"></param>
        /// <param name="position">位置</param>
        /// <param name="velocity">移動量</param>
        /// <param name="fires">投げ出した火のList、書き出す</param>
        public Player(InputState input, Vector2 position, Vector2 velocity, ref List<Fire> fires)
            : base("hero", new Size(50, 52), position, velocity, true, "Player")
        {
            inputState = input;
            this.fires = fires;
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="velo"></param>
        public override void Initialize(Vector2 pos, Vector2 velo, bool isTrigger)
        {
            base.Initialize(pos, velo, isTrigger);
            map = MapManager.GetNowMapData();
            gForce = Parameter.GForce;

            diretion = Direction.RIGHT;
            fireMaxNum = Parameter.FireMaxNum;
            fireNum = fireMaxNum;
        }

        /// <summary>
        /// 火を投げる
        /// </summary>
        private void ThrowFire()
        {
            if (fireNum > 1)
            {
                if (inputState.IsKeyDown(Keys.X))
                {
                    Vector2 firePos = Vector2.Zero;
                    Vector2 fireVelo = Vector2.Zero;
                    Fire fire = new Fire(firePos, fireVelo);

                    if (diretion == Direction.LEFT)
                    {
                        fireVelo = new Vector2(-1, -2);
                        firePos = new Vector2(position.X - fire.ImageSize.Width / 2, position.Y - fire.ImageSize.Height);
                    }
                    else if (diretion == Direction.RIGHT)
                    {
                        fireVelo = new Vector2(1, -2);
                        firePos = new Vector2(position.X + imageSize.Width - fire.ImageSize.Width / 2, position.Y - fire.ImageSize.Height);

                    }
                    else if (diretion == Direction.UP)
                    {
                        fireVelo = new Vector2(0, -2);
                        firePos = new Vector2((position.X + imageSize.Width) / 2 - fire.ImageSize.Width / 2, position.Y - fire.ImageSize.Height);
                    }

                    fireVelo.Normalize();
                    fireVelo *= Parameter.FireSpeed;

                    fire.Position = firePos;
                    fire.Velocity = fireVelo + velocity;

                    fires.Insert(0, fire);
                    fireNum--;
                }
            }
        }

        public void Teleport()
        {
            if (fires.Count > 0)
            {
                if (inputState.IsKeyDown(Keys.C))
                {
                    fires.Add(new Fire(position, velocity));
                    position = fires[0].Position;
                    velocity = fires[0].Velocity;
                    fires.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// 衝突区域判定
        /// </summary>
        /// <param name="other">対象</param>
        /// <returns></returns>
        public override bool CollisionCheck(Object other)
        {
            bool flag = false;

            if (other.IsTrigger)
            {
                flag = base.CollisionCheck(other);

                if (flag)
                {
                    //if (other is Fire)
                    //{
                    //    ((Fire)other).IsDead = true;
                    //    fireNum++;
                    //}
                    //else if (other is Light)
                    //{
                    //    if (((Light)other).IsOn == false)
                    //    {
                    //        velocity = other.Velocity;
                    //        position = other.Position + new Vector2(other.ImageSize.Width / 2 - imageSize.Width / 2, -imageSize.Height);
                    //        isOnGround = true;
                    //        ((Light)other).ChangeSate(true);
                    //    }
                    //}
                    //else if (other is Goal)
                    //{
                    //    ((Goal)other).IsOnFire = true;
                    //    velocity = other.Velocity;
                    //    position = other.Position + new Vector2(other.ImageSize.Width / 2 - imageSize.Width / 2, -imageSize.Height);
                    //    isOnGround = true;
                    //}
                    other.EventHandle(this);
                }
            }

            return flag;
        }

        public override bool ObstacleCheck(Object other)
        {
            bool flag = false;
            if (!other.IsTrigger)
            {
                flag = base.ObstacleCheck(other);
                if (flag)
                {
                    other.EventHandle(this);
                }
            }

            return flag;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            float speed = 5f;    //移動速度

            velocity.X = inputState.Velocity().X * speed;

            if (velocity.X > 0)
            {
                diretion = Direction.RIGHT;
            }
            else if (velocity.X < 0)
            {
                diretion = Direction.LEFT;
            }

            if (isOnGround)
            {
                if (inputState.IsKeyDown(Keys.Z))
                {
                    velocity.Y = -18;
                    isOnGround = false;
                }
            }

            velocity.Y += gForce;

            Method.MapObstacleCheck(ref position, colSize.Width, colSize.Height, ref velocity, ref isOnGround, map, new int[] { 0, 1, 2 });

            //マップ上の物と障害物判定
            foreach (var m in map.MapThings.FindAll(x => !x.IsTrigger))
            {
                ObstacleCheck(m);
            }

            position += velocity;

            //マップ上の物と衝突区域判定
            foreach (var m in map.MapThings.FindAll(x => x.IsTrigger))
            {
                CollisionCheck(m);
            }

            //火と衝突判定
            foreach (var f in fires)
            {
                CollisionCheck(f);
            }

            ThrowFire();
            Teleport();
        }


        public bool GetState()
        {
            return isDead;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset);
        }

        public override void EventHandle(Object other)
        {
            
        }
    }
}
