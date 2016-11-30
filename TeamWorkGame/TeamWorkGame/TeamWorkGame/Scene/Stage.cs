/////////////////////////////////////////////////
// ステージのクラス
// 最終修正時間：2016年11月30日
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
    class Stage : IScene
    {
        private InputState inputState;
        private Sound sound;
        private bool isEnd;
        private int mapIndex;
        public List<Vector2> herol;
        private StageSever sever;
        private Animation standAnime;
        private AnimationPlayer animePlayer;
        private bool isBack;

        public Stage(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState();
            sound = gameDevice.GetSound();
            sever = gameDevice.GetStageSever();
            herol = new List<Vector2>()
            {
                new Vector2(85,153),
                new Vector2(270,310),
                new Vector2(610,340),
                new Vector2(933,460),
                new Vector2(1110,336),
            };
        }
        public void Initialize()
        {
            isEnd = false;
            mapIndex = 0;
            sever.LoadStageData();
            standAnime = new Animation(Renderer.GetTexture("standAnime"), 0.1f, true);
            sound.PlayBGM("worldmap1");
            isBack = false;
        }

        public void Initialize(int index)
        {
            isEnd = false;
            mapIndex = 0;
            sever.LoadStageData();
            standAnime = new Animation(Renderer.GetTexture("standAnime"), 0.1f, true);
            sound.PlayBGM("worldmap1");
            isBack = false;
        }

        //描画の開始と終了は全部Game1のDrawに移動した
        public void Draw(GameTime gameTime, Renderer renderer)
        {

            //renderer.Begin();

            renderer.DrawTexture("worldmap", Vector2.Zero);
            
            renderer.DrawTexture("Zback", new Vector2(10, 10));

            //renderer.DrawTexture("hero",herol[mapIndex]);


            animePlayer.PlayAnimation(standAnime);
            animePlayer.Draw(gameTime, renderer, herol[mapIndex], SpriteEffects.None, 1);
           
            for(int i = 0;i < herol.Count(); i++)
            {
                if (i> sever.ClearStage / 5)
                {
                    renderer.DrawTexture("lock", herol[i]);
                }
            }

            //renderer.End();
        }

        public void Update(GameTime gametime)
        {

            if (inputState.IsKeyDown(Keys.Right)||inputState.IsKeyDown(Buttons.LeftThumbstickRight))
            {
                mapIndex++;
                if (mapIndex > 4)
                {
                    mapIndex = 4;
                }
                if(mapIndex > sever.ClearStage / 5)
                {
                    mapIndex--;
                }
                
            }
            else if (inputState.IsKeyDown(Keys.Left) || inputState.IsKeyDown(Buttons.LeftThumbstickLeft))
            {

                mapIndex--;
                if(mapIndex < 0)
                {
                    mapIndex = 0;
                }
            }

           
            
            if (inputState.IsKeyDown(Keys.Z) || inputState.IsKeyDown(Keys.Space) || inputState.IsKeyDown(Keys.Enter) || inputState.IsKeyDown(Buttons.A))
            {
                isEnd = true;
            }
            else if(inputState.IsKeyDown(Keys.X)|| inputState.IsKeyDown(Buttons.B))
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
            if (isBack == true)
            {
                nextScene = new NextScene(SceneType.Title);
                sound.PlaySE("cancel1");
            }
            else
            {
                //NextScene nextScene = new NextScene(SceneType.PlayScene, mapIndex);
                 nextScene = new NextScene(SceneType.SmallStage, mapIndex * 6);
                sound.PlaySE("decision1");
            }
            return nextScene;
        }

        public void ShutDown()
        {
            //sound.StopBGM();
        }
    }
}
