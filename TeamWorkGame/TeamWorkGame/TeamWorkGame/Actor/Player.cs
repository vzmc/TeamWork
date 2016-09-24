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
        private Camera camera;
        private Map map;
        private Motion motion;                  //アニメーション管理
        private Timer timer;                    //アニメーションの時間間隔
        private float gForce;
        private bool isOnGround;                //地上にいるか？
        private bool isOnLadder;
        

        public Player(InputState input, Camera camera, Vector2 position, Vector2 velocity)
            : base("hero", 64, 64)
        {
            inputState = input;
            this.camera = camera;
            Initialize(position, velocity);
        }

        public override void Initialize(Vector2 pos, Vector2 velo)
        {
            map = MapManager.GetNowMapData();
            position = pos;
            velocity = velo;
            gForce = 1.0f;
            isOnGround = false;
        }

        public override void Update(GameTime gameTime)
        {
            float speed = 5f;    //移動速度

            velocity.X = inputState.Velocity().X * speed;

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
        }

        public bool GetState()
        {
            return isDead;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position + camera.OffSet);
        }
    }
}
