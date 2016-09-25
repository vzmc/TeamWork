/////////////////////////////////////////////////
// プレーヤーのクラス
// 作成時間：2016年9月24日
// By 氷見悠人
// 最終修正時間：2016年9月25日
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
    class Player : Character
    {
        // フィールド
        private InputState inputState;          //入力管理
        //private Camera camera;
        private Map map;
        private Motion motion;                  //アニメーション管理
        private Timer timer;                    //アニメーションの時間間隔
        private Direction diretion;             //向いている方向
        private float gForce;                   //重力
        private bool isOnGround;                //地上にいるか？
        //private bool isOnLadder;
        private List<Fire> fires;               //投げ出した火
        private int fireNum;                    //火の数
        

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="input"></param>
        /// <param name="position">位置</param>
        /// <param name="velocity">移動量</param>
        /// <param name="fires">投げ出した火のList、書き出す</param>
        public Player(InputState input, Vector2 position, Vector2 velocity, ref List<Fire> fires)
            : base("hero", 64, 64)
        {
            inputState = input;
            this.fires = fires;
            Initialize(position, velocity);
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="velo"></param>
        public override void Initialize(Vector2 pos, Vector2 velo)
        {
            map = MapManager.GetNowMapData();
            position = pos;
            velocity = velo;
            gForce = Parameter.GForce;
            isOnGround = false;
            diretion = Direction.RIGHT;
            fireNum = Parameter.FireNum;
        }

        /// <summary>
        /// 火を投げる
        /// </summary>
        private void ThrowFire()
        {
            if (fireNum > 0)
            {
                if (inputState.IsKeyDown(Keys.X))
                {
                    Vector2 firePos = position;
                    Vector2 fireVelo = new Vector2(1, 1);

                    if (diretion == Direction.LEFT)
                    {
                        fireVelo = new Vector2(-1, -2);
                    }
                    else if (diretion == Direction.RIGHT)
                    {
                        fireVelo = new Vector2(1, -2);
                    }
                    else if (diretion == Direction.UP)
                    {
                        fireVelo = new Vector2(0, -2);
                    }

                    fireVelo.Normalize();
                    fireVelo *= Parameter.FireSpeed;

                    fires.Add(new Fire(firePos, fireVelo + velocity));
                    fireNum--;
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            float speed = 5f;    //移動速度

            velocity.X = inputState.Velocity().X * speed;

            if(velocity.X > 0)
            {
                diretion = Direction.RIGHT;
            }
            else if(velocity.X < 0)
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

            Method.MapObstacleCheck(ref position, width, height, ref velocity, ref isOnGround, map, new int[] { 0, 1, 2 });

            position += velocity;

            ThrowFire();
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
    }
}
