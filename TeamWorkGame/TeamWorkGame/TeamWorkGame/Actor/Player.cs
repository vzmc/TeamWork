/////////////////////////////////////////////////
// プレーヤーのクラス
// 作成時間：2016年9月24日
// By 氷見悠人
// 最終修正時間：2016年11月16日
// アニメーションに関する処理 By 長谷川修一
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
        private int fireNum;                        //持っている火の数
        private Animation standAnime;           //待機アニメ
        private Animation runAnime;             //走るアニメ
        private Animation throwAnime;           //投げるアニメ


        private AnimationPlayer animePlayer;    //アニメ再生器
        private SpriteEffects flip = SpriteEffects.FlipHorizontally;
        private PlayerMotion playerMotion;

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
            playerMotion = PlayerMotion.STAND;
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
            standAnime = new Animation(Renderer.GetTexture("standAnime"), 0.1f, true);
            throwAnime = new Animation(Renderer.GetTexture("throwAnime"), 0.1f, false);
            isOnBalloon = false;
            playerMotion = PlayerMotion.STAND;
        }

        /// <summary>
        /// 立つ
        /// </summary>
        public void Stand()
        {
            if(velocity.X == 0 && !IsThrowing())
            {
                playerMotion = PlayerMotion.STAND;
            }
        }

        /// <summary>
        /// 走る
        /// </summary>
        public void Run()
        {
            if(velocity.X != 0 && !IsThrowing())
            {
                animePlayer.PlayAnimation(runAnime);
                playerMotion = PlayerMotion.RUN;
            }
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
                    Fire fire = new Fire(firePos, fireVelo, watersList);

                    //投げ出した火の位置と速度を計算（初期位置は自身とぶつからないように）
                    //Speedを固定にした
                    if (diretion == Direction.UP)
                    {
                        fireVelo = new Vector2(0, -Parameter.FireUpSpeed);
                        firePos = new Vector2(position.X + ColRect.Width / 2 - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
                    }
                    else if (diretion == Direction.LEFT)
                    {
                        fireVelo = new Vector2(-Parameter.FireHerizonSpeed, -Parameter.FireHerizoUpSpeed);
                        firePos = new Vector2(position.X - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
                    }
                    else if (diretion == Direction.RIGHT)
                    {
                        fireVelo = new Vector2(Parameter.FireHerizonSpeed, -Parameter.FireHerizoUpSpeed);
                        firePos = new Vector2(position.X + ColRect.Width - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
                    }

                    fire.Position = firePos;
                    fire.Velocity = fireVelo;
                    firesList.Insert(0, fire);
                    fireNum--;

                    animePlayer.PlayAnimation(throwAnime);
                    playerMotion = PlayerMotion.THROW;
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
                    Vector2 tempPos = firesList[0].Position;
                    Vector2 tempVelo = firesList[0].Velocity;

                    firesList[0].Position = position;
                    firesList[0].Velocity = velocity;

                    position = tempPos;
                    velocity = tempVelo;

                    Fire tempfire = firesList[0];
                    firesList.RemoveAt(0);
                    firesList.Add(tempfire);
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
                    if (other is Balloon)
                    {
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
                    if (inputState.CheckTriggerKey(Parameter.JumpKey, Parameter.JumpButton))
                    {
                        if (!(inputState.CheckDownKey(Keys.Right, Buttons.DPadRight) || inputState.CheckDownKey(Keys.Left, Buttons.DPadLeft)))
                        {
                            isOnBalloon = false;     //気球から降りる
                        }
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
            else if (inputState.Velocity().Y < 0)
            {
                diretion = Direction.UP;
            }

            if (isOnGround)
            {
                if (inputState.CheckTriggerKey(Parameter.JumpKey, Parameter.JumpButton))
                {
                    velocity.Y = -13;
                    isOnGround = false;
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

            Stand();

            Run();

            Teleport();

            if(!isOnBalloon)
            {
                ThrowFire();
            }
            if(playerMotion == PlayerMotion.THROW)
            {
                ResetAnimation(throwAnime);
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
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset, float cameraScale)
        {
            //状態で描画方法が変わる
            if(IsStanding())
            {
                animePlayer.PlayAnimation(standAnime);
                animePlayer.Draw(gameTime, renderer, position * cameraScale + offset, SpriteEffects.None, cameraScale);
                //renderer.DrawTexture(name, position * cameraScale + offset, cameraScale, alpha);
            }
            if(IsRunning() || IsThrowing())
            {
                if (Velocity.X > 0)
                    flip = SpriteEffects.FlipHorizontally;
                else if (Velocity.X < 0)
                    flip = SpriteEffects.None;
                animePlayer.Draw(gameTime, renderer, position * cameraScale + offset, flip, cameraScale);
            }

            //if (velocity.X == 0)
            //{
            //    animePlayer.PlayAnimation(standAnime);
            //    animePlayer.Draw(gameTime, renderer, position * cameraScale + offset, flip, cameraScale);
            //    //renderer.DrawTexture(name, position * cameraScale + offset, cameraScale, alpha);
            //}
            //else
            //{
            //    if (Velocity.X > 0)
            //        flip = SpriteEffects.FlipHorizontally;
            //    else if (Velocity.X < 0)
            //        flip = SpriteEffects.None;
            //    animePlayer.Draw(gameTime, renderer, position * cameraScale + offset, flip, cameraScale);
            //}
        }

        public override void EventHandle(GameObject other)
        {
            //
        }

        /// <summary>
        /// 立っている状態ならtrue
        /// </summary>
        /// <returns></returns>
        public bool IsStanding()
        {
            return playerMotion == PlayerMotion.STAND;
        }

        /// <summary>
        /// 移動している状態ならtrue
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            return playerMotion == PlayerMotion.RUN;
        }

        /// <summary>
        /// 投げている状態ならtrue
        /// </summary>
        /// <returns></returns>
        public bool IsThrowing()
        {
            return playerMotion == PlayerMotion.THROW;
        }

        /// <summary>
        /// ループしないアニメーションのリセット
        /// </summary>
        /// <param name="animation">リセットしたいアニメーション</param>
        public void ResetAnimation(Animation animation)
        {
            if (animePlayer.FrameNow() == animePlayer.Animation.FrameCount - 1)
            {
                if (velocity.X != 0)
                {
                    playerMotion = PlayerMotion.RUN;
                }
                if (velocity.X == 0)
                {
                    playerMotion = PlayerMotion.STAND;
                }
                animePlayer.ResetAnimation(animation);
            }
        }
    }
}
