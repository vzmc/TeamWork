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

        public Title(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState();
            Initialize();
        }

        public void Draw(Renderer renderer)
        {
            Vector2 pos = new Vector2((Parameter.ScreenWidth - 754) / 2, (Parameter.ScreenHeight - 127) / 2);

            renderer.Begin();

            renderer.DrawTexture("title", pos);

            renderer.End();

        }

        public void Initialize()
        {
            isEnd = false;
        }

        public void Initialize(int index)
        {
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
            if (inputState.IsKeyDown(Keys.Enter))
            {
                isEnd = true;
            }
        }

        NextScene IScene.Next()
        {
            //NextScene nextScene = new NextScene(SceneType.PlayScene, 2);
            //return nextScene;

            //ステージセレクト画面へ移行
            //Ｂｙ葉梨竜太
            //２０１６年１０月１２日
            NextScene nextScene = new NextScene(SceneType.Stage, 2);
            return nextScene;


        }
    }
}
