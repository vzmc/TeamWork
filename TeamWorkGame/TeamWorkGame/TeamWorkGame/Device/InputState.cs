///////////////////////////////////////////////////////////////
// キーボード入力処理
// 作成時間：2016/9/22
// 作成者：氷見悠人
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TeamWorkGame.Device
{
    /// <summary>
    /// キーボード操作を処理するクラス
    /// </summary>
    class InputState
    {
        //フィールド
        private Vector2 velocity;   // 移動量の宣言

        //キー
        private KeyboardState currentKey;   //現在のキー
        private KeyboardState previousKey;  //１フレーム前のキー

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputState()
        {
            velocity = Vector2.Zero;
        }

        /// <summary>
        /// 移動量の取得
        /// </summary>
        /// <returns>移動量</returns>
        public Vector2 Velocity()
        {
            return velocity;
        }

        /// <summary>
        /// 移動量の更新
        /// </summary>
        /// <param name="keyState">キーボードの状態</param>
        private void UpdateVelocity(KeyboardState keyState)
        {
            velocity = Vector2.Zero;    // 移動量をゼロで初期化

            // 十字キーの操作処理
            if (keyState.IsKeyDown(Keys.Right))
            {
                velocity.X = 1.0f;
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                velocity.X = -1.0f;
            }

            if (keyState.IsKeyDown(Keys.Up))
            {
                velocity.Y = -1.0f;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                velocity.Y = 1.0f;
            }

            // 正規化する（長さ１の単位ベクトルに）
            //if (velocity.Length() != 0.0f)
            //{
            //    velocity.Normalize();   // 正規化メソッド
            //}
        }

        /// <summary>
        /// キー情報の更新
        /// </summary>
        /// <param name="keyState"></param>
        private void UpdateKey(KeyboardState keyState)
        {
            //現在登録されているキーを１フレーム前のキーに
            previousKey = currentKey;
            //現在のキーを最新のキーに
            currentKey = keyState;
        }

        /// <summary>
        /// キーが押されているか？
        /// </summary>
        /// <param name="key">調べたいキー</param>
        /// <returns>現在押されていて、１フレーム前に押されていなければ true</returns>
        public bool IsKeyDown(Keys key)
        {
            //現在チェックしたいキーが押されたか
            bool current = currentKey.IsKeyDown(key);
            //１フレーム前に押されていたか
            bool previous = previousKey.IsKeyDown(key);

            //現在押されていて、１フレーム前に押されていなければ true
            return current && !previous;
        }

        /// <summary>
        /// キー入力のトリガー判定
        /// </summary>
        /// <param name="key"></param>
        /// <returns>１フレーム前に押されていたらfalse</returns>
        public bool GetKeyTrigger(Keys key)
        {
            return IsKeyDown(key);
        }

        /// <summary>
        /// キー入力の状態判定
        /// </summary>
        /// <param name="key"></param>
        /// <returns>押されていたらtrue</returns>
        public bool GetKeyState(Keys key)
        {
            return currentKey.IsKeyDown(key);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            // 現在のキーボードの状態を取得
            var keyState = Keyboard.GetState();
            UpdateKey(keyState);

            // 移動量の更新
            UpdateVelocity(keyState);
        }
    }
}
