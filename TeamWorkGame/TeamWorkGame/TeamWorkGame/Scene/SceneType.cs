﻿/////////////////////////////////////////////////
// SceneType
// 最終修正時間：2016年10月13日
// By　葉梨竜太
/////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamWorkGame.Scene
{
    enum SceneType
    {
        Load,
        Title,
        Stage,
        Credit,
        //シーンの追加
        //葉梨竜太
        //２０１６年１０月１３日
        SmallStage,
        StageIn,        //Stageに入る時の表示Scene　By　氷見悠人
        PlayScene,
        Ending
    }
}
