/////////////////////////////////////////////////////////////////////////
// Timerクラス
// 作成者：氷見悠人
/////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamWorkGame.Utility
{
    class Timer
    {
        //フィールド
        private float currentTime;  //現在時間
        private float limitTime;    //制限時間

        public Timer()
        {
            limitTime = 60;
            Initialize();
        }

        public Timer(float seconds)
        {
            limitTime = seconds * 60;
            Initialize();
        }

        public void Initialize()
        {
            currentTime = limitTime;
        }

        /// <summary>
        /// Timer状態の更新
        /// </summary>
        public void Update()
        {
            currentTime--;
            if (currentTime < 0.0f)
            {
                currentTime = 0.0f;
            }
        }

        /// <summary>
        /// Get今の時刻
        /// </summary>
        /// <returns>今の時間</returns>
        public float Now()
        {
            return currentTime;
        }

        /// <summary>
        /// 規定時間になったか？
        /// </summary>
        /// <returns>true or false</returns>
        public bool IsTime()
        {
            return currentTime <= 0;
        }

        /// <summary>
        /// 規定時間の変更
        /// </summary>
        /// <param name="limitTime"></param>
        public void Change(float seconds)
        {
            this.limitTime = seconds * 60;
            Initialize();
        }
    }
}
