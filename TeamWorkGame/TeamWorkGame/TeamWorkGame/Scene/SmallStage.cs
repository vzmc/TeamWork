/////////////////////////////////////////////////
// ステージ選択のクラス
// 作成時間：2016年10月13日
// By 葉梨竜太
//最終修正時間：2016年11月16日
//By 葉梨竜太
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
        private List<Vector2> mapl;
        private int mapnum;
        private bool isBack;
        private StageSever sever;
        private Vector2 flame;
        private Animation standAnime;
        private AnimationPlayer animePlayer;

        public SmallStage(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState();
            sever = gameDevice.GetStageSever();
            mapIndex = 0;
            mapnum = 0;
            //mapl = new List<Vector2>()
            //{
            //    //new Vector2(33,51),
            //    //new Vector2(33,283),
            //    //new Vector2(33,498),
            //    new Vector2(636,61),
            //    new Vector2(636,293),
            //    new Vector2(636,525),
            //    new Vector2(636,757),
            //    new Vector2(636,989),
            //    new Vector2(636,1221),
            //};
        }

        public void Initialize()
        {
            mapIndex = 0;
            mapnum = 0;
            isBack = false;
            isEnd = false;
            sever.LoadStageData();
            standAnime = new Animation(Renderer.GetTexture("standAnime"), 0.1f, true);
            mapl = new List<Vector2>()
            {
                //new Vector2(33,51),
                //new Vector2(33,283),
                //new Vector2(33,498),
                new Vector2(636,61),
                new Vector2(636,293),
                new Vector2(636,525),
                new Vector2(636,757),
                new Vector2(636,989),
                new Vector2(636,1221),
            };
        }

        public void Initialize(int index)
        {
            mapIndex = 0;
            mapnum = 0;
            isEnd = false;
            isBack = false;
            stageIndex = index;
            sever.LoadStageData();
            standAnime = new Animation(Renderer.GetTexture("standAnime"), 0.1f, true);
            mapl = new List<Vector2>()
            {
                //new Vector2(33,51),
                //new Vector2(33,283),
                //new Vector2(33,498),
                new Vector2(636,61),
                new Vector2(636,293),
                new Vector2(636,525),
                new Vector2(636,757),
                new Vector2(636,989),
                new Vector2(636,1221),
            };
        }
        public void Draw(GameTime gameTime, Renderer renderer)
        {
            //renderer.Begin();
            
            renderer.DrawTexture("backGround", Vector2.Zero);

            renderer.DrawTexture("Zback", new Vector2(10, 10));

            renderer.DrawTexture("frame", flame = new Vector2(mapl[mapIndex].X - 10, mapl[mapIndex].Y - 10));

            //renderer.DrawTexture("smallmap1", new Vector2(43, 61));
            //renderer.DrawTexture("smallmap2", new Vector2(43, 293));
            //renderer.DrawTexture("smallmap3", new Vector2(43, 508));
            renderer.DrawTexture("smallmap1", mapl[0]);
            renderer.DrawTexture("smallmap2", mapl[1]);
            renderer.DrawTexture("smallmap3", mapl[2]);
            renderer.DrawTexture("smallmap4", mapl[3]);
            renderer.DrawTexture("smallmap5", mapl[4]);
            renderer.DrawTexture("smallmap6", mapl[5]);

            if(mapIndex >=3)
            renderer.DrawTexture("uparrow", new Vector2(550, 30));
            else
            renderer.DrawTexture("downarrow", new Vector2(550, 600));

            for (int i = 0; i < mapl.Count(); i++)
            {
                if (i > sever.ClearStage+1 - stageIndex)
                {
                    renderer.DrawTexture("lock", mapl[i]);
                }
            }

            animePlayer.PlayAnimation(standAnime);
            animePlayer.Draw(gameTime, renderer, new Vector2(200,300), SpriteEffects.None,3);

            //renderer.End();
        }

        public void Update(GameTime gametime)
        {
            if (inputState.IsKeyDown(Keys.Right) || inputState.IsKeyDown(Buttons.LeftThumbstickRight))
            {
                //mapnum += 3;
                //if (mapnum >= 6)
                //{
                //    mapnum -= 6;
                //}

            }
            else if (inputState.IsKeyDown(Keys.Left)||inputState.IsKeyDown(Buttons.LeftThumbstickLeft))
            {
                //mapnum -= 3;
                //if (mapnum <= -1)
                //{
                //    mapnum += 6;
                //}
            }
            if (inputState.IsKeyDown(Keys.Up)||inputState.IsKeyDown(Buttons.LeftThumbstickUp))
            {
                mapnum--;
                if(mapnum == -1)
                {
                    mapnum = 5;
                    if (mapnum > sever.ClearStage - stageIndex)
                    {
                        mapnum = 0;
                    }
                }
                
                if (mapnum == 2) 
                {
                    for (int i = 0; i < mapl.Count; i++)
                    {
                        mapl[i] = new Vector2(mapl[i].X, mapl[i].Y + 696);
                    }
                }

                if (mapnum == 5)
                {
                    for (int i = 0; i < mapl.Count; i++)
                    {
                        mapl[i] = new Vector2(mapl[i].X, mapl[i].Y - 696);
                    }
                }
                //mapnum--;
                //if (mapnum == 2 || mapnum == -1) 
                //{
                //    mapnum += 3;
                //}
            }
            else if (inputState.IsKeyDown(Keys.Down)||inputState.IsKeyDown(Buttons.LeftThumbstickDown))
            {
                if(mapnum  > sever.ClearStage-stageIndex)
                {
                    return;
                }
                mapnum++; 
                if(mapnum == 6)
                {
                    mapnum = 0;
                }   
                        
                if(mapnum == 3)
                {
                    for (int i = 0; i < mapl.Count ; i++) 
                    {
                        mapl[i] = new Vector2(mapl[i].X, mapl[i].Y -696);
                    }
                }

                if (mapnum == 0)
                {
                    for (int i = 0; i < mapl.Count; i++)
                    {
                        mapl[i] = new Vector2(mapl[i].X, mapl[i].Y + 696);
                    }
                }
                //mapnum++;
                //if (mapnum ==6|| mapnum == 3)
                //{
                //    mapnum -= 3;
                //}
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
