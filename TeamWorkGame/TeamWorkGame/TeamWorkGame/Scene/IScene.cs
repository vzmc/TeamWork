using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;

namespace TeamWorkGame.Scene
{
    interface IScene
    {
        void Initialize();              //初期化
        void Update(GameTime gametime); //更新
        void Draw(Renderer renderer);   //描画
        void ShutDown();                //終了

        //シーン管理
        bool IsEnd();                   //終了チェック
        Scene Next();                   //次のシーン番号
    }
}
