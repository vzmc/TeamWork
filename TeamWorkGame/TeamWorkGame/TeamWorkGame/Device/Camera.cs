/////////////////////////////////////////////////////////////////////////////
// カメラ処理
// 作成時間：2016/9/25
// 作成者：氷見悠人
/////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;  //Vector2
using TeamWorkGame.Def;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Device
{
    public class Camera
    {
        //フィールド 
        public int ViewWidth { get; }
        public int ViewHeight { get; }

        private Vector2 position;
        private Vector2 centerPosition;
        private Vector2 aimPosition;
        private Map map;
        private bool IsLimitView;

        public Vector2 OffSet
        {
            get
            {
                return -position;
            }
        }
        public Vector2 CenterPosition
        {
            get
            {
                return centerPosition;
            }
        }
        public Vector2 AimPosition
        {
            get
            {
                return aimPosition;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="aimPos">カメラ注視位置（中心位置）</param>
        public Camera()
        {
            ViewWidth = Parameter.ScreenWidth;
            ViewHeight = Parameter.ScreenHeight;
            map = MapManager.GetNowMapData();
            IsLimitView = true;
            SetData(new Vector2(ViewWidth / 2, ViewHeight / 2));
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="aimPos">カメラ注視位置（中心位置）</param>
        public Camera(Vector2 aimPos, bool isLimitView = true)
        {
            ViewWidth = Parameter.ScreenWidth;
            ViewHeight = Parameter.ScreenHeight;
            map = MapManager.GetNowMapData();
            IsLimitView = isLimitView;
            SetData(aimPos);
        }

        /// <summary>
        /// カメラの各位置を設定
        /// </summary>
        /// <param name="aimPos">注視する位置</param>
        private void SetData(Vector2 aimPos)
        {
            if (IsLimitView)
            {
                // Cameraの位置を制限する
                if (aimPos.X < ViewWidth / 2)
                {
                    aimPos.X = ViewWidth / 2;
                }
                if (aimPos.Y < ViewHeight / 2)
                {
                    aimPos.Y = ViewHeight / 2;
                }

                if (aimPos.X > map.MapWidth - ViewWidth / 2)
                {
                    aimPos.X = map.MapWidth - ViewWidth / 2;
                }
                if (aimPos.Y > map.MapHeight - ViewHeight / 2)
                {
                    aimPos.Y = map.MapHeight - ViewHeight / 2;
                }
            }

            aimPosition = aimPos;
            centerPosition = aimPosition;
            position = centerPosition - new Vector2(ViewWidth / 2, ViewHeight / 2);
        }

        /// <summary>
        /// カメラの注視位置を設定
        /// </summary>
        /// <param name="aimPos"></param>
        public void SetAimPosition(Vector2 aimPos)
        {
            SetData(aimPos);
        }

        public void MoveAimPosition(Vector2 aimPos)
        {
            Vector2 distance = aimPos - aimPosition;
            //float speed = 0;
            Vector2 aim;

            Vector2 velocity = distance * 0.9f;

            //カメラの位置を整数化
            velocity.X = (float)Math.Floor((velocity.X));
            velocity.Y = (float)Math.Floor((velocity.Y));

            aim = aimPos - velocity;
            //if (distance <= 1)
            //{
            //    aim = aimPos;
            //}
            //else
            //{
            //    Vector2 velocity = aimPos - aimPosition;
            //    velocity *= 0.1f;
            //    aim = aimPosition + velocity;
            //}

            //velocity.Normalize();
            //velocity *= speed;


            SetData(aim);
        }

        /// <summary>
        /// カメラ注視位置と注視物の距離により、カメラの移動速度を計算し、速度を返す
        /// </summary>
        /// <param name="distance">カメラ注視位置と注視物位置の距離</param>
        /// <returns>カメラの移動速度</returns>
        private float GetMoveSpeed(float distance)
        {
            float speed;
            speed = 0.1f * distance;
            return speed;
        }

        public void UpdateMap()
        {
            map = MapManager.GetNowMapData();
        }

        public void SetLimitView(bool flag)
        {
            IsLimitView = flag;
        }
    }
}
