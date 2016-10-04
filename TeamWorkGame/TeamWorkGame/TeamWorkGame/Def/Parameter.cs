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

namespace TeamWorkGame.Def
{
    /// <summary>
    /// ゲーム内で使われているパラメータ
    /// </summary>
    public static class Parameter
    {
        public static readonly int ScreenWidth = 1280;      //windowsのSize
        public static readonly int ScreenHeight = 768;      //windowsのSize
        public static readonly float GForce = 1.0f;         //重力の大きさ
        public static readonly float FireSpeed = 15.0f;     //投げ出す火の速度
        public static readonly int FireMaxNum = 10;             //火の総量
    }
}
