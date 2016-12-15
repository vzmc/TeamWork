/////////////////////////////////////////////////
// プレーヤーのクラス
// 作成時間：2016年9月24日
// By 氷見悠人
// 最終修正時間：2016年12月15日
// エイムつけた By 葉梨竜太
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
        private GameDevice gameDevice;
        private InputState inputState;          //入力管理
        private Sound sound;
        private Map map;
        private float gForce;
        private Direction diretion;             //向いている方向
        private List<Fire> firesList;               //投げ出した火
        private List<WaterLine> watersList;         //滝のリスト

        private int firstFireNum;                    //火の総数
        private int fireNum;                        //持っている火の数
        private Animation standAnime;           //待機アニメ、正面
        private Animation sidewaysAnime;            //待機アニメ、横
        private Animation runAnime;             //走るアニメ
        private Animation throwAnime;           //投げるアニメ
        private Animation deathAnime;           //死ぬアニメ
        private Animation lowRunAnime;     //待機アニメ、横向き、弱い

        private Timer jumpEffectTimer;
        private Timer fallEffectTimer;
        private Vector2 jumpEffectPos;
        private Vector2 fallEffectPos;
        private bool previousIsOnGround;

        private AnimationPlayer animePlayer;    //アニメ再生器
        private SpriteEffects flip = SpriteEffects.FlipHorizontally;
        private PlayerMotion playerMotion;

        private Vector2 aim; //エイムの向き
        private Vector2 aimpos;
        Fire fire;

        private bool isOnBalloon;   //気球に乗ってるかどうか

        private bool isView;                        //カメラ操作中か？
        public bool IsView
        {
            get
            {
                return isView;
            }
            set
            {
                isView = value;
            }
        }

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
        public Player(GameDevice gameDevice, Vector2 position, Vector2 velocity, ref List<Fire> firesList, ref List<WaterLine> watersList, bool isView)
            : base("hero", position, velocity, true, "Player")
        {
            this.gameDevice = gameDevice;
            inputState = gameDevice.GetInputState();
            sound = gameDevice.GetSound();
            this.firesList = firesList;
            this.watersList = watersList;
            this.isView = isView;
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
            firstFireNum = Parameter.FirstFireNum;
            fireNum = firstFireNum;
            runAnime = new Animation(Renderer.GetTexture("playerAnime"), 0.1f, true);
            standAnime = new Animation(Renderer.GetTexture("standAnime"), 0.1f, true);
            sidewaysAnime = new Animation(Renderer.GetTexture("sideAnime"), 0.1f, true);
            throwAnime = new Animation(Renderer.GetTexture("throwAnime"), 0.1f, false);
            deathAnime = new Animation(Renderer.GetTexture("deathAnime"), 0.1f, false);
            lowRunAnime = new Animation(Renderer.GetTexture("lowRunAnime"), 0.1f, true);
            isOnBalloon = false;
            playerMotion = PlayerMotion.STAND;

            jumpEffectTimer = new Timer(0.15f);
            jumpEffectTimer.CurrentTime = 0;
            fallEffectTimer = new Timer(0.15f);
            fallEffectTimer.CurrentTime = 0;
        }

        /// <summary>
        /// 立つ状態の判断
        /// </summary>
        private void Stand()
        {
            if (diretion == Direction.UP)// && !IsThrowing())
            {
                playerMotion = PlayerMotion.STAND;
            }
        }

        /// <summary>
        /// 横向きの状態の判断
        /// </summary>
        private void StandSideWays()
        {
            if ((diretion == Direction.LEFT || diretion == Direction.RIGHT))// && !IsThrowing())
            {
                animePlayer.PlayAnimation(sidewaysAnime);
                playerMotion = PlayerMotion.SIDEWAYS;
            }
        }

        /// <summary>
        /// 横向きの状態の判断（弱い火）
        /// </summary>
        private void LowRun()
        {
            if ((diretion == Direction.LEFT || diretion == Direction.RIGHT))// && !IsThrowing())
            {
                animePlayer.PlayAnimation(lowRunAnime);
                playerMotion = PlayerMotion.LOWRUN;
            }
        }

        /// <summary>
        /// 走る状態の判断
        /// </summary>
        private void Run()
        {
            if (diretion == Direction.LEFT || diretion == Direction.RIGHT)// && !IsThrowing())
            {
                animePlayer.PlayAnimation(runAnime);
                playerMotion = PlayerMotion.RUN;
            }
        }
        public void Death()
        {
            sound.PlaySE("dead");   //by柏　2016.12.14 ＳＥ実装
            animePlayer.PlayAnimation(deathAnime);
            playerMotion = PlayerMotion.DEATH;
        }

        /// <summary>
        /// 火を投げる
        /// </summary>
        private void ThrowFire()
        {
            if (fireNum > 0)
            {
                if (inputState.CheckTriggerKey(Parameter.ThrowKey, Parameter.ThrowButton))
                {
                    Vector2 firePos = Vector2.Zero;
                    Vector2 fireVelo = Vector2.Zero;
                    fire = new Fire(firePos, fireVelo, watersList);

                    //投げ出した火の位置と速度を計算（初期位置は自身とぶつからないように）
                    //Speedを固定にした
                    //葉梨竜太
                    //８方向に投げ分け対応                   

                    if (diretion == Direction.UP || inputState.CheckDownKey(Keys.Up, Buttons.LeftThumbstickUp))
                    {
                        //右上
                        if (inputState.CheckDownKey(Keys.Right, Buttons.LeftThumbstickRight))
                            fireVelo = new Vector2(Parameter.FireSpeed, -Parameter.FireSpeed);
                        //左上
                        else if (inputState.CheckDownKey(Keys.Left, Buttons.LeftThumbstickLeft))
                            fireVelo = new Vector2(-Parameter.FireSpeed, -Parameter.FireSpeed);
                        //真上
                        else
                            fireVelo = new Vector2(0, -Parameter.FireSpeed);

                        firePos = new Vector2(position.X + ColRect.Width / 2 - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
                    }

                    else if (diretion == Direction.LEFT || inputState.CheckDownKey(Keys.Left, Buttons.LeftThumbstickLeft))
                    {
                        //左上
                        if (inputState.CheckDownKey(Keys.Up, Buttons.LeftThumbstickUp))
                        {
                            fireVelo = new Vector2(-Parameter.FireSpeed, -Parameter.FireSpeed);
                            firePos = new Vector2(position.X - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
                        }
                        //左下
                        else if (inputState.CheckDownKey(Keys.Down, Buttons.LeftThumbstickDown))
                        {
                            fireVelo = new Vector2(-Parameter.FireSpeed, Parameter.FireSpeed);
                            firePos = new Vector2(position.X - ColRect.Width, position.Y);
                        }
                        //左
                        else
                        {
                            fireVelo = new Vector2(-Parameter.FireSpeed, 0);
                            firePos = isOnGround ? new Vector2(position.X - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height) : new Vector2(position.X - ColRect.Width, position.Y);
                        }
                    }

                    else if (diretion == Direction.RIGHT || inputState.CheckDownKey(Keys.Right, Buttons.LeftThumbstickRight))
                    {
                        //右上
                        if (inputState.CheckDownKey(Keys.Up, Buttons.LeftThumbstickUp))
                        {
                            fireVelo = new Vector2(Parameter.FireSpeed, -Parameter.FireSpeed);
                            firePos = new Vector2(position.X + ColRect.Width, position.Y - fire.ColRect.Height);
                        }
                        //右下
                        else if (inputState.CheckDownKey(Keys.Down, Buttons.LeftThumbstickDown))
                        {
                            fireVelo = new Vector2(Parameter.FireSpeed, Parameter.FireSpeed);
                            firePos = new Vector2(position.X + ColRect.Width, position.Y);
                        }
                        //右
                        else
                        {
                            fireVelo = new Vector2(Parameter.FireSpeed, 0);
                            firePos = isOnGround ? new Vector2(position.X + ColRect.Width - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height) : new Vector2(position.X + ColRect.Width, position.Y);
                        }

                    }

                    //if (diretion == Direction.UP || inputState.CheckDownKey(Keys.Up, Buttons.RightThumbstickUp))
                    //{
                    //    fireVelo = new Vector2(0, -Parameter.FireUpSpeed);
                    //    firePos = new Vector2(position.X + ColRect.Width / 2 - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
                    //}
                    //else if (diretion == Direction.LEFT)
                    //{
                    //    fireVelo = new Vector2(-Parameter.FireHorizontalSpeedX, -Parameter.FireHorizontalSpeedY);
                    //    firePos = new Vector2(position.X - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
                    //}
                    //else if (diretion == Direction.RIGHT)
                    //{
                    //    fireVelo = new Vector2(Parameter.FireHorizontalSpeedX, -Parameter.FireHorizontalSpeedY);
                    //    firePos = new Vector2(position.X + ColRect.Width - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
                    //}

                    fire.Position = firePos;

                    //葉梨竜太
                    //単位ベクトル化
                    fireVelo.Normalize();
                    fire.Velocity = fireVelo * Parameter.FireSpeed;
                    //葉梨竜太
                    fire.SetStartPos();

                    firesList.Insert(0, fire);
                    fireNum--;

                    //投げる状態に入る
                    animePlayer.PlayAnimation(throwAnime);
                    playerMotion = PlayerMotion.THROW;
                    sound.PlaySE("fire1");
                }
            }
        }

        /// <summary>
        /// エイムの向きと位置
        /// </summary>
        public void Aim()
        {
            fire = new Fire(Vector2.Zero, Vector2.Zero, watersList);
            if (diretion == Direction.UP || inputState.CheckDownKey(Keys.Up, Buttons.LeftThumbstickUp))
            {
                //右上
                if (inputState.CheckDownKey(Keys.Right, Buttons.LeftThumbstickRight))
                    aim = new Vector2(Parameter.FireSpeed, -Parameter.FireSpeed);
                //左上
                else if (inputState.CheckDownKey(Keys.Left, Buttons.LeftThumbstickLeft))
                    aim = new Vector2(-Parameter.FireSpeed, -Parameter.FireSpeed);
                //真上
                else
                   aim = new Vector2(0, -Parameter.FireSpeed);

                aimpos = new Vector2(position.X + ColRect.Width / 2 - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
            }

            else if (diretion == Direction.LEFT || inputState.CheckDownKey(Keys.Left, Buttons.LeftThumbstickLeft))
            {
                //左上
                if (inputState.CheckDownKey(Keys.Up, Buttons.LeftThumbstickUp))
                {
                    aim = new Vector2(-Parameter.FireSpeed, -Parameter.FireSpeed);
                    aimpos = new Vector2(position.X - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height);
                }
                //左下
                else if (inputState.CheckDownKey(Keys.Down, Buttons.LeftThumbstickDown))
                {
                    aim = new Vector2(-Parameter.FireSpeed, Parameter.FireSpeed);
                    aimpos = new Vector2(position.X - ColRect.Width, position.Y);
                }
                //左
                else
                {
                    aim = new Vector2(-Parameter.FireSpeed, 0);
                    aimpos = isOnGround ? new Vector2(position.X - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height) : new Vector2(position.X - ColRect.Width, position.Y);
                }
            }

            else if (diretion == Direction.RIGHT || inputState.CheckDownKey(Keys.Right, Buttons.LeftThumbstickRight))
            {
                //右上
                if (inputState.CheckDownKey(Keys.Up, Buttons.LeftThumbstickUp))
                {
                    aim = new Vector2(Parameter.FireSpeed, -Parameter.FireSpeed);
                    aimpos = new Vector2(position.X + ColRect.Width, position.Y - fire.ColRect.Height);
                }
                //右下
                else if (inputState.CheckDownKey(Keys.Down, Buttons.LeftThumbstickDown))
                {
                    aim = new Vector2(Parameter.FireSpeed, Parameter.FireSpeed);
                    aimpos = new Vector2(position.X + ColRect.Width, position.Y);
                }
                //右
                else
                {
                    aim = new Vector2(Parameter.FireSpeed, 0);
                    aimpos = isOnGround ? new Vector2(position.X + ColRect.Width - fire.ColRect.Width / 2, position.Y - fire.ColRect.Height) : new Vector2(position.X + ColRect.Width, position.Y);
                }
            }
            aim.Normalize();
            aimpos.X = aimpos.X + (Parameter.FireFly * 64 * aim.X);
            aimpos.Y = aimpos.Y + (Parameter.FireFly * 64 * aim.Y);
            AimCheck();
        }
        /// <summary>
        /// エイムがマップ内か？
        /// </summary>
        public void AimCheck()
        {
            if (aimpos.Y  > map.MapHeight)
            {
                aimpos.Y = map.MapHeight - 64;
            }
            if (aimpos.X+64  > map.MapWidth)
            {
                aimpos.X = map.MapWidth - 64;
            }
            if (aimpos.Y < 0)
            {
                aimpos.Y = 0;
            }
            if (aimpos.X < 0)
            {
                aimpos.X = 0;
            }
        }

        /// <summary>
        /// マップ上いある火と位置交換（最後に投げ出した火は最初に交換）
        /// </summary>
        private void Teleport()
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
                    //葉梨竜太
                    tempfire.Velocity = new Vector2(0,Parameter.FireFall);
                }
            }
        }

        /// <summary>
        /// プレイヤーの移動系の処理全般
        /// </summary>
        private void MoveMotion()
        {
            //立つ状態の切り替え判断
            Stand();
            if (velocity.X != 0)
            {
                if (FireNum != 0)
                {
                    //走る状態の切り替え判断
                    Run();
                }
                else
                {
                    LowRun();
                }
            }
            else
            {
                StandSideWays();
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
        /// 地図の下に落ちたか？
        /// </summary>
        private void CheckIsOut()
        {
            if (position.Y > map.MapHeight + 64)
            {
                isDead = true;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (isGoDie)
            {
                if (playerMotion != PlayerMotion.DEATH)
                    Death();
                if (animePlayer.FrameIndex >= deathAnime.FrameCount - 1)
                {
                    isDead = true;
                }
                return;
            }

            if (isView)
            {
                inputState = new InputState();
            }
            else
            {
                inputState = gameDevice.GetInputState();
            }

            fallEffectTimer.Update();
            jumpEffectTimer.Update();

            //普通は空中摩擦
            float friction = Parameter.AirFriction;

            if (isOnGround)
            {
                //カメラ操作中は主人公に対する操作不可
                if (inputState.CheckTriggerKey(Parameter.JumpKey, Parameter.JumpButton))
                {
                    velocity.Y = -Parameter.PlayerJumpPower;
                    //isOnGround = false;
                    jumpEffectTimer.Initialize();
                    jumpEffectPos = position;
                }
                //地上にいると、摩擦は地面摩擦
                friction = Parameter.GroundFriction;

                //isOnGroundをReset
                isOnGround = false;
            }

            //横方向のスピード計算
            //velocity.X = inputState.Velocity().X * Parameter.MaxPlayerHorizontalSpeed;
            velocity.X += inputState.Velocity().X * Parameter.PlayerAccelerationX;

            //摩擦計算
            friction = Math.Abs(velocity.X) > friction ? friction : velocity.X;
            friction = friction * velocity.X > 0 ? -friction : friction;

            velocity.X += friction;

            //速度制限
            if (velocity.X > Parameter.MaxPlayerHorizontalSpeed)
            {
                velocity.X = Parameter.MaxPlayerHorizontalSpeed;
            }
            else if (velocity.X < -Parameter.MaxPlayerHorizontalSpeed)
            {
                velocity.X = -Parameter.MaxPlayerHorizontalSpeed;
            }

            //方向判断
            if (velocity.X > 0 || inputState.Velocity().X > 0)
            {
                diretion = Direction.RIGHT;
            }
            else if (velocity.X < 0 || inputState.Velocity().X < 0)
            {
                diretion = Direction.LEFT;
            }
            else if (inputState.Velocity().Y < 0)
            {
                diretion = Direction.UP;
            }

            //縦スピード計算
            velocity.Y += gForce;

            if (velocity.Y > 0 && velocity.Y < 1)
            {
                velocity.Y = 1;
            }

            if (velocity.Y > Parameter.MaxPlayerVerticalSpeed)
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

            //位置計算
            position += velocity;

            //位置を整数にする　By氷見悠人
            position.X = (float)Math.Round(position.X);
            position.Y = (float)Math.Round(position.Y);

            previousBottom = ColRect.Bottom;

            //マップ上の物と衝突区域判定
            foreach (var m in map.MapThings.FindAll(x => x.IsTrigger))
            {
                //if (m is Sign)      //Signの場合のみ上キーを押したときのみ判定
                //{
                //    if (inputState.GetKeyTrigger(Keys.Up))
                //    {
                //        CollisionCheck(m);
                //    }
                //}
                //else
                //{
                //葉梨竜太
                CollisionCheck(m);
                //}
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

            CheckIsOut();
            if (!previousIsOnGround && isOnGround)
            {
                fallEffectTimer.Initialize();
                fallEffectPos = position;
            }
            previousIsOnGround = isOnGround;
            //Console.WriteLine("isOnGround: " + isOnGround);

            if (!IsThrowing())
            {
                //移動モーション関係はメソッド化by長谷川 12/15
                MoveMotion();
            }
            //火と位置交換処理
            Teleport();

            //if (!isOnBalloon)     //2016.12.7仕様書に合わせて変更 By柏
            //{
            //火を投げる処理
            ThrowFire();
            //}
            Aim();

            if (playerMotion == PlayerMotion.THROW)
            {
                ResetAnimation(throwAnime);
            }

            //地図外か？
            CheckIsOut();

            if(!previousIsOnGround && isOnGround)
            {
                fallEffectTimer.Initialize();
                fallEffectPos = position;
            }
            previousIsOnGround = isOnGround;
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
            //状態によって描画方法が変わる
            if (IsStanding())
            {
                animePlayer.PlayAnimation(standAnime);
                animePlayer.Draw(gameTime, renderer, position * cameraScale + offset, flip, cameraScale);
            }
            if (IsRunning() || IsThrowing() || IsDeath() || IsSideWays() || IsLowRun())
            {
                if (diretion == Direction.RIGHT)
                    flip = SpriteEffects.FlipHorizontally;
                else if (diretion == Direction.LEFT)
                    flip = SpriteEffects.None;
                animePlayer.Draw(gameTime, renderer, position * cameraScale + offset, flip, cameraScale);
            }

            //JumpのEffect描画
            if (!fallEffectTimer.IsTime())
            {
                renderer.DrawTexture("JumpEffect", fallEffectPos * cameraScale + offset, new Rectangle(0, 0, 64, 64), cameraScale, 1.0f);
            }
            if (!jumpEffectTimer.IsTime())
            {
                Rectangle rect;
                if (velocity.X >= 0)
                {
                    rect = new Rectangle(64 * 2, 0, 64, 64);
                }
                else
                {
                    rect = new Rectangle(64, 0, 64, 64);
                }
                renderer.DrawTexture("JumpEffect", jumpEffectPos * cameraScale + offset, rect, cameraScale, 1.0f);
            }
            if(!IsDeath())
            renderer.DrawTexture("aiming",aimpos * cameraScale + offset);
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
        /// 横向き状態ならtrue
        /// </summary>
        /// <returns></returns>
        public bool IsSideWays()
        {
            return playerMotion == PlayerMotion.SIDEWAYS;
        }

        public bool IsLowRun()
        {
            return playerMotion == PlayerMotion.LOWRUN;
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
        /// 死んだ状態ならtrue
        /// </summary>
        /// <returns></returns>
        public bool IsDeath()
        {
            return playerMotion == PlayerMotion.DEATH;
        }

        /// <summary>
        /// ループしないアニメーションのリセット
        /// </summary>
        /// <param name="animation">リセットしたいアニメーション</param>
        public void ResetAnimation(Animation animation)
        {
            if (animePlayer.FrameNow() == animePlayer.Animation.FrameCount - 1)
            {
                MoveMotion();
                animePlayer.ResetAnimation(animation);
            }
        }
    }
}
