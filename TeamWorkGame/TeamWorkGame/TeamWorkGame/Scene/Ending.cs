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
    class Ending : IScene
    {
        private InputState inputState;
        private bool isEnd;

        public Ending(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState();
            Initialize();
        }

        //描画の開始と終了は全部Game1のDrawに移動した
        public void Draw(GameTime gameTime, Renderer renderer)
        {
            Vector2 pos = new Vector2((Parameter.ScreenWidth - 473) / 2, (Parameter.ScreenHeight - 127) / 2);

            //renderer.Begin();

            renderer.DrawTexture("clear", pos);

            //renderer.End();
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
        //    return SceneType.Title;
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
            NextScene nextScene = new NextScene(SceneType.Title, -1);
            return nextScene;
        }
    }
}
