/////////////////////////////////////////////////
// ステージ選択のクラス
// 作成時間：2016年10月13日
// By 葉梨竜太
//最終修正時間：2016年10月27日
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
        private int stageIndex;
        private int mapIndex;
        private List<Vector2> framel;
        private int mapnum;
        private bool isBack;

        public SmallStage(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState();
            mapIndex = 0;
            mapnum = 0;
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
            mapIndex = 0;
            mapnum = 0;
            isBack = false;
            isEnd = false;
        }

        public void Initialize(int index)
        {
            mapIndex = 0;
            mapnum = 0;
            isEnd = false;
            isBack = false;
            stageIndex = index;
        }
        public void Draw(GameTime gameTime, Renderer renderer)
        {
            //renderer.Begin();

            renderer.DrawTexture("forestBG", Vector2.Zero);

            renderer.DrawTexture("frame", framel[mapIndex]);

            renderer.DrawTexture("smallmap", new Vector2(43, 61));
            renderer.DrawTexture("smallmap", new Vector2(43, 293));
            renderer.DrawTexture("smallmap", new Vector2(43, 508));
            renderer.DrawTexture("smallmap", new Vector2(646, 61));
            renderer.DrawTexture("smallmap", new Vector2(646, 293));
            renderer.DrawTexture("smallmap", new Vector2(646, 508));

            //renderer.End();
        }

        public void Update(GameTime gametime)
        {
            if (inputState.IsKeyDown(Keys.Right))
            {
                mapnum += 3;
                if(mapnum >= 6)
                {
                    mapnum -= 6;
                }
            }
            else if (inputState.IsKeyDown(Keys.Left))
            {
                mapnum -= 3;
                if (mapnum <= -1)
                {
                    mapnum += 6;
                }
            }
            else if (inputState.IsKeyDown(Keys.Up))
            {
                mapnum--;
                if (mapnum == 2)
                {
                    mapnum = 5;
                }
                else if(mapnum == -1)
                {
                    mapnum = 2;
                }
                
            }
            else if (inputState.IsKeyDown(Keys.Down))
            {
                mapnum++;
                if (mapnum ==6)
                {
                    mapnum = 3;
                }
                else if (mapnum == 3)
                {
                    mapnum = 0;
                }
            }
            
            mapIndex = mapnum;

            if (inputState.IsKeyDown(Keys.Enter))
            {
                isEnd = true;
            }
            else if (inputState.IsKeyDown(Keys.Z))
            {
                isBack = true;
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
            if (isBack) 
            {
                nextScene = new NextScene(SceneType.Stage, stageIndex);
            }
            else
            {
                nextScene = new NextScene(SceneType.PlayScene, mapIndex + stageIndex);
            }
            return nextScene;
        }

        public void ShutDown()
        {
        }

        
    }
}
