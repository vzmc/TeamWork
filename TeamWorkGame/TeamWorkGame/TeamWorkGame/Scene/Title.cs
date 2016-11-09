////////////////////////////////////////////////
//TitleScene
//作成時間：2016/9/30
//作成者：氷見悠人
//最終修正時間：2016/10/26
//修正者：氷見悠人
/////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TeamWorkGame.Device;
using TeamWorkGame.Actor;
using TeamWorkGame.Def;
using TeamWorkGame.Scene;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Scene
{
    class Title : IScene
    {
        private InputState inputState;
        private bool isEnd;
        private TitleSelect titleSelect;    //タイトルの選択肢を管理するクラス（柏）

        public Title(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState();
            Initialize();
        }

        //描画の開始と終了は全部Game1のDrawに移動した
        public void Draw(GameTime gameTime, Renderer renderer)
        {
            Vector2 pos = Vector2.Zero;
            //Vector2 pos = new Vector2((Parameter.ScreenWidth - 754) / 2, (Parameter.ScreenHeight - 127) / 2);

            //renderer.Begin();

            renderer.DrawTexture("title", pos);
            titleSelect.Draw(renderer);

            //renderer.End();

        }

        public void Initialize()
        {
            titleSelect = new TitleSelect(inputState);
            titleSelect.Initialize();
            isEnd = false;
        }

        public void Initialize(int index)
        {
            titleSelect = new TitleSelect(inputState);
            titleSelect.Initialize();
            isEnd = false;
        }

        public bool IsEnd()
        {
            return isEnd;
        }
        
        //public SceneType Next()
        //{
        //    return SceneType.PlayScene;
        //}

        public void ShutDown()
        {
            
        }

        public void Update(GameTime gametime)
        {
            if (!titleSelect.GetStarted)
            {
                //keyの変更とpadの対応　By氷見悠人　10/26
                if (inputState.CheckTriggerKey(Parameter.MenuKey, Parameter.MenuButton))
                {
                    //Startを表示するから、他の選択肢の表示を移す
                    titleSelect.GetStarted = true;
                }
                
            }
            else
            {
                //keyの変更とpadの対応　By氷見悠人　10/26
                if (inputState.CheckTriggerKey(Parameter.MenuKey, Parameter.MenuButton))
                {
                    isEnd = true;
                }
            }

            titleSelect.Update();
        }

        NextScene IScene.Next()
        {
            //NextScene nextScene = new NextScene(SceneType.PlayScene, 2);
            //return nextScene;

            //ステージセレクト画面へ移行
            //Ｂｙ葉梨竜太
            //２０１６年１０月１２日
            //NextScene nextScene = new NextScene(SceneType.Stage, 2);

            NextScene nextScene;
            
            //選択肢によって、次のシーンに移す
            if (titleSelect.GetSelect == 1)
            {
                //ステージ選択Sceneに入る
                nextScene = new NextScene(SceneType.Stage, -1);
            }
            else
            {
                //StaffSceneに入る（未完成）、暫定ステージ選択Sceneに入る
                nextScene = new NextScene(SceneType.Stage, -1);
            }

            return nextScene;
        }
    }
}
