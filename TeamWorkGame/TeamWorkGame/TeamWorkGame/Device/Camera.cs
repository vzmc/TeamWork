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
        public Camera(Vector2 aimPos, bool isLimitView = false)
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

        public void UpDateMap()
        {
            map = MapManager.GetNowMapData();
        }

        public void SetLimitView(bool flag)
        {
            IsLimitView = flag;
        }
    }
}
