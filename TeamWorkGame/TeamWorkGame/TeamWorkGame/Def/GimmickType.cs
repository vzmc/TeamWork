/////////////////////////////////////////////////////////////////////////////////////
// ギミックの種類列挙型
// 作成者：氷見悠人
// 最終更新日：11月02日
// by 佐瀬拓海
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamWorkGame.Def
{
    public enum GimmickType
    {
        PLAYER = 0,      //草
        GROUND1,        //土1
        GROUND2,        //土2
        ROCK,           //岩
        ICE,            //氷
        IRON,           //鉄
        STRAW,          //藁
        COAL,           //炭
        LIGHT,          //松明
        GOAL,           //ゴール
        WATER,          //水
        WOOD,           //木材
        TREE,           //木
        SAND,           //砂
        BALLOON,        //気球
    }
}
