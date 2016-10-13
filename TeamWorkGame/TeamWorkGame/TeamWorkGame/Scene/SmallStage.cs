/////////////////////////////////////////////////
// ステージ選択のクラス
// 作成時間：2016年10月13日
// By 葉梨竜太
/////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TeamWorkGame.Device;
using TeamWorkGame.Actor;
using TeamWorkGame.Def;
using TeamWorkGame.Scene;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Scene
{
    class SmallStage : IScene
    {
        private InputState inputState;
        private bool isEnd;
        private int mapIndex;
        private List<Vector2> framel;

        public SmallStage(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState();
            mapIndex = 0;
            framel = new List<Vector2>()
            {
                new Vector2(33,51),
                new Vector2(33,283),
                new Vector2(33,498),
                new Vector2(636,51),
                new Vector2(636,283),
                new Vector2(636,498),
            };
        }

        public void Initialize()
        {
            isEnd = false;
        }

        public void Initialize(int index)
        {
            isEnd = false;
        }
        public void Draw(Renderer renderer)
        {
            //renderer.Begin();

            renderer.DrawTexture("forestBG", Vector2.Zero);

            renderer.DrawTexture("frame", framel[mapIndex]);

            renderer.DrawTexture("smallmap", new Vector2(43, 61));
            renderer.DrawTexture("smallmap", new Vector2(43, 293));
            renderer.DrawTexture("smallmap", new Vector2(43, 508));
            renderer.DrawTexture("smallmap", new Vector2(646, 61));
            renderer.DrawTexture("smallmap", new Vector2(646, 293));

            //renderer.End();
        }

        public void Update(GameTime gametime)
        {
            if (inputState.IsKeyDown(Keys.Right))
            {
                mapIndex++;
                if (mapIndex >= 5)
                {
                    mapIndex = 5;
                }
            }
            else if (inputState.IsKeyDown(Keys.Left))
            {
                mapIndex--;
                if (mapIndex <= 0)
                {
                    mapIndex = 0;
                }
            }
            if (inputState.IsKeyDown(Keys.Enter))
            {
                isEnd = true;
            }
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public NextScene Next()
        {
            NextScene nextScene;
            if (mapIndex == 5)
            {
                nextScene = new NextScene(SceneType.Stage, mapIndex);
            }
            else
            {
                nextScene = new NextScene(SceneType.PlayScene, mapIndex);
            }
            return nextScene;
        }

        public void ShutDown()
        {
        }

        
    }
}
