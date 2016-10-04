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

        public void Draw(Renderer renderer)
        {
            Vector2 pos = new Vector2((Parameter.ScreenWidth - 473) / 2, (Parameter.ScreenHeight - 127) / 2);

            renderer.Begin();

            renderer.DrawTexture("clear", pos);

            renderer.End();
        }

        public void Initialize()
        {
            isEnd = false;
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public Scene Next()
        {
            return Scene.Title;
        }

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
    }
}
