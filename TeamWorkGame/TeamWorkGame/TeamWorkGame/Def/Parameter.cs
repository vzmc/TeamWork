/////////////////////////////////////////////////
// 設定用パラメータのクラス
// 作成時間：2016年9月24日
// By 氷見悠人
// 最終修正時間：2016年9月25日
// By 氷見悠人
/////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
namespace TeamWorkGame.Def
{
    /// <summary>
    /// ゲーム内で使われているパラメータ
    /// </summary>
    public static class Parameter
    {
        public const int ScreenWidth = 1280;      //windowsのSize
        public const int ScreenHeight = 768;      //windowsのSize
        public const float GForce = 0.5f;         //重力の大きさ
        public const float FireSpeed = 10f;     //投げ出す火の速度
        public const int FireMaxNum = 5;         //火の総量

        public const Keys JumpKey = Keys.Z;
        public const Keys TeleportKey = Keys.C;
        public const Keys ThrowKey = Keys.X;

        public const Buttons JumpButton = Buttons.A;
        public const Buttons TeleportButton = Buttons.RightTrigger;
        public const Buttons ThrowButton = Buttons.LeftTrigger;

        public static readonly List<GimmickType> BackGrounds = new List<GimmickType>()
        {
            GimmickType.GROUND1, GimmickType.GROUND2,
        };
    }
}
