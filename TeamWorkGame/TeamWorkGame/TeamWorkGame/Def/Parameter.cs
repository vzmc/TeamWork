/////////////////////////////////////////////////
// 設定用パラメータのクラス
// 作成時間：2016年9月24日
// By 氷見悠人
// 最終修正時間：2016年11月16日
// By 葉梨竜太
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
        public const int ScreenHeight = 720;      //windowsのSize
        public const int TextWidth = 231;         //text画像の横幅
        public const int TextHeight = 50;         //text画像1つ分の縦幅、切り取り用
        public const float GForce = 0.5f;         //重力の大きさ
        //public const float FireSpeed = 10f;     //投げ出す火の速度

        public const int FireUpSpeed = 15;          //真上に投げる時のSpeed；

        public const int FireHerizoUpSpeed = 10;    //横い投げる時のY方向Speed分量
        public const int FireHerizonSpeed = 6;      //横い投げる時のX方向Speed分量
        public const int FireMaxNum = 5;         //火の総量
        public const float BurnStraw = 0.2f;     //藁の火が燃え移る時間
        public const float OneframeStrawAnime = 0.2f;  //藁アニメ―ションの1フレームあたりの時間
        public const float OneframeWoodAnime = 0.2f;  //木アニメーションの1フレームあたりの時間

        //必要な火の量(数字以上で燃える)
        public const int icefire = 2;
        public const int treefire = 3;
        public const int ironfire = 5;
        public const int woodfire = 1;
        public const int strawfire = 1;

        public const float CameraScale = 1.0f;

        public const Keys JumpKey = Keys.Z;
        public const Keys TeleportKey = Keys.C;
        public const Keys ThrowKey = Keys.X;
        public const Keys MenuKey = Keys.Enter;

        public const Buttons JumpButton = Buttons.A;
        public const Buttons TeleportButton = Buttons.RightTrigger;
        public const Buttons ThrowButton = Buttons.LeftTrigger;
        public const Buttons MenuButton = Buttons.Start;

        public static readonly List<GimmickType> ImpassableTiles = new List<GimmickType>()
        {
            GimmickType.GROUND1, GimmickType.GROUND2,
        };

        public static readonly List<GimmickType> PlatformTiles = new List<GimmickType>()
        {
            
        };

        public static readonly List<GimmickType> PassableTiles = new List<GimmickType>()
        {
            
        };
    }
}
