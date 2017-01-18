/////////////////////////////////////////////////
// ステージのクラス
// 最終修正時間：2016年12月14日
// by 柏 ＳＥ実装
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
        private StageSaver sever;
        private Animation animation;
        private Animation standAnime;
        private AnimationPlayer animePlayer;
        private bool isBack;
        private Vector2 position;
        private bool isMove;
        private Vector2 startpos;
        private Vector2 endpos;
        private Timer time;
        private Timer totaltime;

        public Stage(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState();
            sound = gameDevice.GetSound();
            sever = gameDevice.GetStageSaver();
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
            animation = standAnime;
            position = herol[mapIndex];
        }

        public void Initialize(int index)
        {
            isEnd = false;
            mapIndex = 0;
            sever.LoadStageData();
            standAnime = new Animation(Renderer.GetTexture("standAnime"), 0.1f, true);
            sound.PlayBGM("worldmap1");
            isBack = false;
            animation = standAnime;
            position = herol[mapIndex];
            isMove = false;
        }

        //描画の開始と終了は全部Game1のDrawに移動した
        public void Draw(GameTime gameTime, Renderer renderer)
        {
            renderer.DrawTexture("worldmap", Vector2.Zero);

            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                renderer.DrawTexture("stageSelect_UI", new Vector2(0, 720 - 64));
            }
            else
            {
                renderer.DrawTexture("stageSelect_UI2", new Vector2(0, 720 - 64));
            }

            animePlayer.PlayAnimation(animation);
            animePlayer.Draw(gameTime, renderer, position, SpriteEffects.None, 1);

            for (int i = 0; i < herol.Count(); i++)
            {
                if (i > (sever.ClearStage + 1) / 6)
                {
                    renderer.DrawTexture("lock", herol[i]);
                }
            }
        }

        //public void MapAnimation(GameTime gametime)
        //{
        //    if (isMove)
        //    {
        //        try
        //        {
        //            startpos = herol[mapIndex-1];
        //            endpos = herol[mapIndex];
        //        }
        //        catch(ArgumentOutOfRangeException iore)
        //        {
        //            return;
        //        }
        //
        //        Vector2 velocity = endpos - startpos;
        //        velocity.Normalize();
        //        position = position + velocity * 3;
        //
        //        if () 
        //        {
        //            isMove = false;
        //        }
        //    }
        //}

        public void Update(GameTime gametime)
        {
            //MapAnimation(gametime);

            //if (isMove == false)
            //{
            if (inputState.IsKeyDown(Keys.Right) || inputState.IsKeyDown(Buttons.LeftThumbstickRight))
            {
                sound.PlaySE("cursor");    //by 柏　2016.12.14 ＳＥ実装
                if (mapIndex >= (sever.ClearStage + 1) / 6)
                {
                    return;
                }
                if (mapIndex >= 4)
                {
                    return;
                }
                //isMove = true;

                mapIndex++;


            }
            else if (inputState.IsKeyDown(Keys.Left) || inputState.IsKeyDown(Buttons.LeftThumbstickLeft))
            {
                sound.PlaySE("cursor");    //by 柏　2016.12.14 ＳＥ実装
                if (mapIndex <= 0)
                {
                    return;
                }

                //isMove = true;
                mapIndex--;
            }
        
            
            position = herol[mapIndex];

            if (inputState.IsKeyDown(Keys.Z) || inputState.IsKeyDown(Keys.Space) || inputState.IsKeyDown(Keys.Enter) || inputState.IsKeyDown(Buttons.A))
            {
                //sound.PlaySE("decision1");    //by 柏　2016.12.14 ＳＥ実装  (Next()のところにもう再生した　By　氷見悠人)
                isEnd = true;
            }
            else if (inputState.IsKeyDown(Keys.X) || inputState.IsKeyDown(Buttons.B))
            {
                //sound.PlaySE("cancel1");    //by 柏　2016.12.14 ＳＥ実装    (Next()のところにもう再生した　By　氷見悠人)
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
                nextScene = new NextScene(SceneType.SmallStage, mapIndex * 6);
                sound.PlaySE("decision1");
            }
            return nextScene;
        }

        public NextScene GetNext() {
            NextScene nextScene;
            if (isBack == true)
            {
                nextScene = new NextScene(SceneType.Title);
            }
            else
            {
                nextScene = new NextScene(SceneType.SmallStage, mapIndex * 6);
            }
            return nextScene;
        }

        public void ShutDown()
        {
        }
    }
}
