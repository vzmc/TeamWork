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
        private StageSever sever;

        public SmallStage(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState();
            sever = gameDevice.GetStageSever();
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
            sever.LoadStageData();
        }

        public void Initialize(int index)
        {
            mapIndex = 0;
            mapnum = 0;
            isEnd = false;
            isBack = false;
            stageIndex = index;
            sever.LoadStageData();
        }
        public void Draw(GameTime gameTime, Renderer renderer)
        {
            //renderer.Begin();
            
            renderer.DrawTexture("forestBG", Vector2.Zero);

            renderer.DrawTexture("frame", framel[mapIndex]);

            renderer.DrawTexture("smallmap1", new Vector2(43, 61));
            renderer.DrawTexture("smallmap2", new Vector2(43, 293));
            renderer.DrawTexture("smallmap3", new Vector2(43, 508));
            renderer.DrawTexture("smallmap4", new Vector2(646, 61));
            renderer.DrawTexture("smallmap5", new Vector2(646, 293));
            renderer.DrawTexture("smallmap6", new Vector2(646, 508));

            //renderer.End();
        }

        public void Update(GameTime gametime)
        {
            if (inputState.IsKeyDown(Keys.Right)||inputState.IsKeyDown(Buttons.DPadRight))
            {
                mapnum += 3;
                if (mapnum >= 6) 
                {
                    mapnum -= 6;
                }
               
            }
            else if (inputState.IsKeyDown(Keys.Left)||inputState.IsKeyDown(Buttons.DPadLeft))
            {
                mapnum -= 3;
                if (mapnum <= -1)
                {
                    mapnum += 6;
                }
            }
            else if (inputState.IsKeyDown(Keys.Up)||inputState.IsKeyDown(Buttons.DPadUp))
            {
                mapnum--;
                if (mapnum == 2 || mapnum == -1) 
                {
                    mapnum += 3;
                }
                
            }
            else if (inputState.IsKeyDown(Keys.Down)||inputState.IsKeyDown(Buttons.DPadDown))
            {
                mapnum++;
                if (mapnum ==6|| mapnum == 3)
                {
                    mapnum -= 3;
                }
               
            }
            
            mapIndex = mapnum;

            if (inputState.IsKeyDown(Keys.Enter)||inputState.IsKeyDown(Buttons.A))
            {
                if (stageIndex + mapIndex <= sever.ClearStage+1)
                {
                    isEnd = true;
                }
            }
            else if (inputState.IsKeyDown(Keys.Z)||inputState.IsKeyDown(Buttons.B))
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
