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
        void Initialize(int index);
        void Update(GameTime gameTime); //更新
        void Draw(GameTime gameTime, Renderer renderer);   //描画
        void ShutDown();                //終了

        //シーン管理
        bool IsEnd();                   //終了チェック
        NextScene Next();                   //次のシーン
    }
}
