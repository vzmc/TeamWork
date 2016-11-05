/////////////////////////////////////////////////
// プレーヤーのクラス
// 作成時間：2016年9月24日
// By 氷見悠人
// 最終修正時間：2016年11月3日
// アニメーションの変更 By 長谷川修一
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
    class Player : GameObject
    {
        // フィールド
        private InputState inputState;          //入力管理
        //private Camera camera;
        private Map map;
        private float gForce;
        //private Motion motion;                  //アニメーション管理
        //private Timer timer;                    //アニメーションの時間間隔
        private Direction diretion;             //向いている方向
        private List<Fire> firesList;               //投げ出した火
        private List<WaterLine> watersList;         //滝のリスト
        private int fireMaxNum;                    //火の総数
        private int fireNum;                        //持ているひの数
        private Animation runAnime;             //走るアニメ

        private AnimationPlayer animePlayer;    //アニメ再生器
        private SpriteEffects flip = SpriteEffects.None;

        private bool isOnBalloon;   //気球に乗ってるかどうか

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

        //new Size(50,52)の中身を(64,64)に修正
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="input"></param>
        /// <param name="position">位置</param>
        /// <param name="velocity">移動量</param>
        /// <param name="fires">投げ出した火のList、書き出す</param>
        public Player(InputState input, Vector2 position, Vector2 velocity, ref List<Fire> firesList, ref List<WaterLine> watersList)
            : base("hero", position, velocity, true, "Player")
        {
            //InitLocalColRect();
            inputState = input;
            this.firesList = firesList;
            this.watersList = watersList;
        }

        //当たり判定変更by長谷川修一
        protected override Rectangle InitLocalColRect()
        {
            return new Rectangle(8, 22, 49, 42);
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            map = MapManager.GetNowMapData();
            gForce = Parameter.GForce;

            diretion = Direction.RIGHT;
            fireMaxNum = Parameter.FireMaxNum;
            fireNum = fireMaxNum;
            runAnime = new Animation(Renderer.GetTexture("playerAnime"), 0.1f, true);
            isOnBalloon = false;
        }

        /// <summary>
        /// 火を投げる
        /// </summary>
        private void ThrowFire()
        {
            if (fireNum > 1)
            {
                //inputState.IsKeyDown(Keys.X)
                if (inputState.CheckTriggerKey(Parameter.ThrowKey, Parameter.ThrowButton))
                {
                    Vector2 firePos = Vector2.Zero;
                    Vector2 fireVelo = Vector2.Zero;
                    Fire fire = new Fire(firePos, fireVelo);

                    //投げ出した火の位置と速度を計算（初期位置は自身とぶつからないように）
                    if (diretion == Direction.UP)
                    {
                        fireVelo = new Vector2(0, -1f);
                        firePos = new Vector2(position.X + ColRect.Width / 2 - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
                    }
                    else if (diretion == Direction.LEFT)
                    {
                        fireVelo = new Vector2(-1f, -2f);
                        firePos = new Vector2(position.X - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
                    }
                    else if (diretion == Direction.RIGHT)
                    {
                        fireVelo = new Vector2(1f, -2f);
                        firePos = new Vector2(position.X + ColRect.Width - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
                    }

                    fireVelo.Normalize();
                    fireVelo *= Parameter.FireSpeed;

                    fire.Position = firePos;
                    fire.Velocity = fireVelo + velocity;

                    firesList.Insert(0, fire);
                    fireNum--;
                }
            }
        }

        /// <summary>
        /// マップ上いある火と位置交換（最後に投げ出した火は最初に交換）
        /// </summary>
        public void Teleport()
        {
            if (firesList.Count > 0)
            {
                if (inputState.CheckTriggerKey(Parameter.TeleportKey, Parameter.TeleportButton))
                {
                    //firesList.Add(new Fire(position, velocity));
                    Vector2 tempPos = firesList[0].Position;
                    Vector2 tempVelo = firesList[0].Velocity;

                    firesList[0].Position = position;
                    firesList[0].Velocity = velocity;

                    position = tempPos;
                    velocity = tempVelo;

                    Fire tempfire = firesList[0];
                    firesList[0] = firesList[firesList.Count - 1];
                    firesList[firesList.Count - 1] = tempfire;
                }
            }
        }

        /// <summary>
        /// 衝突区域判定
        /// </summary>
        /// <param name="other">対象</param>
        /// <returns></returns>
        public override bool CollisionCheck(GameObject other)
        {
            bool flag = false;

            if (other.IsTrigger)
            {
                flag = base.CollisionCheck(other);

                if (flag)
                {
                    other.EventHandle(this);
                }
            }

            return flag;
        }

        /// <summary>
        /// 障害物判定
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool ObstacleCheck(GameObject other)
        {
            bool flag = false;
            if (!other.IsTrigger)
            {
                flag = base.ObstacleCheck(other);
                if (flag)
                {
                    other.EventHandle(this);
                }
                else {
                    if (other is Balloon) {
                        ((Balloon)other).IsPlayerOn = false;
                    }
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


            //気球と衝突判定
            foreach (var m in map.MapThings.FindAll(x => x is Balloon))
            {
                if (base.CollisionCheck(m))
                {
                    if (inputState.GetKeyState(Keys.Left)) { diretion = Direction.LEFT; }
                    if (inputState.CheckTriggerKey(Parameter.JumpKey, Parameter.JumpButton))
                    {
                        isOnBalloon = false;
                        isOnGround = false;
                    }
                }
            }


            velocity.X = inputState.Velocity().X * speed;

            if (velocity.X > 0)
            {
                diretion = Direction.RIGHT;
            }
            else if (velocity.X < 0)
            {
                diretion = Direction.LEFT;
            }
            else if(inputState.Velocity().Y < 0)
            {
                diretion = Direction.UP;
            }

            if (isOnGround)
            {
                if (inputState.CheckTriggerKey(Parameter.JumpKey, Parameter.JumpButton))
                {
                    velocity.Y = -13;
                    isOnGround = false;
                    isOnBalloon = false;    //気球から離れる
                }
            }

            velocity.Y += gForce;

            if (velocity.Y > 0 && velocity.Y < 1)
            {
                velocity.Y = 1;
            }

            if (velocity.Y > 10)
            {
                velocity.Y = 10;
            }

            //マップ上の物と障害物判定
            foreach (var m in map.MapThings.FindAll(x => !x.IsTrigger))
            {
                ObstacleCheck(m);
            }

            Method.MapObstacleCheck(ref position, localColRect, ref velocity, ref isOnGround, map, new int[] { 1, 2 });


            //Console.WriteLine(velocity);

            position += velocity;

            //位置を整数にする　By氷見悠人
            position.X = (float)Math.Round(position.X);
            position.Y = (float)Math.Round(position.Y);

            previousBottom = ColRect.Bottom;


            //マップ上の物と衝突区域判定
            foreach (var m in map.MapThings.FindAll(x => x.IsTrigger))
            {
                CollisionCheck(m);
            }


            //火と衝突判定
            foreach (var f in firesList)
            {
                CollisionCheck(f);
            }

            


            //滝との衝突判定
            foreach (var wl in watersList)
            {
                foreach (var w in wl.Waters)
                    CollisionCheck(w);
            }

            ThrowFire();
            Teleport();

            if (velocity.X != 0)
            {
                animePlayer.PlayAnimation(runAnime);
            }

            //Console.WriteLine("isOnGround: " + isOnGround);
        }

        public bool GetState()
        {
            return isDead;
        }

        public bool IsOnBalloon
        {
            get { return isOnBalloon; }
            set { isOnBalloon = value; }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset)
        {
            if (velocity.X == 0)
            {
                renderer.DrawTexture(name, position + offset);
            }
            else
            {
                if (Velocity.X > 0)
                    flip = SpriteEffects.FlipHorizontally;
                else if (Velocity.X < 0)
                    flip = SpriteEffects.None;
                animePlayer.Draw(gameTime, renderer, position + offset, flip);
            }
        }

        public override void EventHandle(GameObject other)
        {
            
        }
    }
}
