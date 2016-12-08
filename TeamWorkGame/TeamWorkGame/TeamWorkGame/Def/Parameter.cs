/////////////////////////////////////////////////
// 設定用パラメータのクラス
// 作成時間：2016年9月24日
// By 氷見悠人
// 最終修正時間：2016年12月01日
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
        //ゲーム関連
        public const int ScreenWidth = 1280;      //windowsのSize
        public const int ScreenHeight = 720;      //windowsのSize
        public const int TextWidth = 231;         //text画像の横幅
        public const int TextHeight = 50;         //text画像1つ分の縦幅、切り取り用

        //ゲーム内容関連
        public const float GForce = 0.5f;               //重力の大きさ（1フレームにつきスピードの縦分量の増量）
        public const float GroundFriction = 0.4f;         //地面摩擦
        public const float AirFriction = 0.1f;            //空中摩擦
        public const float PlayerAccelerationX = 1.5f;    //左右操作の時プレーヤーの横方向の加速度
        public const float PlayerJumpPower = 13f;
        public const float MaxPlayerHorizontalSpeed = 5f;   //Player横方向の最大速度
        public const float MaxPlayerVerticalSpeed = 10f;    //Player横方向の最大速度
        public const int FireUpSpeed = 15;              //真上に投げる時のSpeed；
        public const int FireHorizontalSpeedY = 11;     //横い投げる時のY方向Speed分量
        public const int FireHorizontalSpeedX = 6;      //横い投げる時のX方向Speed分量
        public const int FireMaxNum = 5;                //火の総量   
        public const int MoveLightSpeed = 3;        //動く松明の速度
        public const int BalloonDown = 3;           //気球が下降する速度
        public const int BalloonUp = 3;            //気球が上昇する速度  
        public const int BalloonMove = 3;           //気球の移動速度 ２０１６．１２．７   Ｂｙ柏
        public const float StrawColTime = 0.2f;   //藁ブロックの当たり判定が消えるまでの時間
        public const float StrawBurn = 0.3f;       //藁ブロックの火が燃え移り始めるまでの時間        ※11/30現在、当たり判定がなくなってから火が燃え移り始める。
        public const float StrawAnimeTime = 0.6f;  //藁ブロックアニメ―ションの時間
        public const float WoodAnimeTime = 1.0f;   //木材ブロックアニメーションの時間
        public const float WoodSpawnTime = 2.0f;   //木材ブロックの復活時間
        public const float IronAnimeTime = 1.0f;   //鉄ブロックアニメーションの時間
        public const float IronSpawnTime = 4.0f;   //鉄ブロックの復活時間
        public const float IceAnimeTime = 1.0f;    //アイスブロックアニメーションの時間
        public const float IceSpawnTime = 2.0f;    //アイスブロックの復活時間
        public const float WaterFlowSpeed = 0.3f;  //水が下のマスに流れるまでの時間

        //必要な火の量(数字以上で燃える)
        public const int icefire = 2;
        public const int treefire = 3;
        public const int ironfire = 5;
        public const int woodfire = 1;
        public const int strawfire = 1;

        //カメラのスケール
        public const float CameraScale = 1.0f;

        //操作関連
        public const Keys JumpKey = Keys.Z;
        public const Keys TeleportKey = Keys.C;
        public const Keys ThrowKey = Keys.X;
        public const Keys MenuKey = Keys.Enter;

        public const Buttons JumpButton = Buttons.A;
        public const Buttons TeleportButton = Buttons.LeftTrigger;
        public const Buttons ThrowButton = Buttons.RightTrigger;
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
